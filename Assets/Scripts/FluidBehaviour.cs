using UnityEngine;
using System.Collections.Generic;

public class FluidBehaviour : MonoBehaviour
{

    public List<FluidParticle> Particles { get; set; }
    private List<FluidParticle> newParticles;
    private List<Vector3> particleShot;
    private const float ParticleDistance = 0.25f;
    private readonly Vector3 Gravity = new Vector3(0, 0.005f, 0);
    private const int ShotParticleDiameter = 5;
    private const float RestDensity = 100f;
    private const float RelaxationConstant = 0.0002f;
    private int particleCount = 0;
    private const float NeighbourDistance = ParticleDistance;
    private const float ParticleElasticity = 0.001f;
    private const int SolverIterations = 4;
    private System.Random random = new System.Random();
    private const float ParticleVelocityScaling = 0.25f;
    private const bool RemoveExpiredParticles = true;

    public FluidBehaviour()
    {
        this.Particles = new List<FluidParticle>();
        this.newParticles = new List<FluidParticle>();
        this.particleShot = new List<Vector3>();
        float radius = 0.5f * ParticleDistance * ShotParticleDiameter;
        //  Create cube with points
        for (int x = 0; x < ShotParticleDiameter; x++)
        {
            for (int y = 0; y < ShotParticleDiameter; y++)
            {
                for (int z = 0; z < ShotParticleDiameter; z++)
                {
                    Vector3 v = new Vector3(x * ParticleDistance - radius, y * ParticleDistance - radius, z * ParticleDistance);
                    this.particleShot.Add(v);
                }

            }
        }
        Vector3 center = new Vector3(0, 0, radius);
        // Create sphere shape for shot by removing points
        this.particleShot.RemoveAll(point => Vector3.Distance(point, center) > (radius + 0.1f * ParticleDistance));
    }

    void Start()
    {
        Screen.showCursor = false;
    }

    void FixedUpdate()
    {
        // Remove all particles that have fallen too low, have expired or hit fire
        if (RemoveExpiredParticles)
        {
            Particles.RemoveAll(p => p.ToBeRemoved || (p.Position.y < -1) || p.Expiry <= 0);
        }
        else
        {
            Particles.RemoveAll(p => p.ToBeRemoved || (p.Position.y < -1));
        }

        // Add new particles
        Particles.AddRange(newParticles);
        newParticles.Clear();

        int newParticleCount = Particles.Count;
        if (newParticleCount != particleCount)
        {
            particleCount = newParticleCount;
            Debug.Log(particleCount + " fluid particles.");
        }

        // Predict the positions of the particles
        // Lines 1-4 from the algorithm in the paper
        foreach (FluidParticle p in Particles)
        {
            // Apply gravity
            p.Velocity -= Gravity;

            Vector3 newPosition = p.Position + p.Velocity;
            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(p.Position, newPosition + ParticleDistance * (newPosition - p.Position).normalized, out hit))
            {
                if (hit.normal.y != 0)
                {
                    p.Velocity += Gravity;
                }
                p.Velocity = Vector3.Reflect(p.Velocity * ParticleElasticity, hit.normal);
                newPosition = p.Position + p.Velocity;

                FireCell fireCell = hit.collider.GetComponent<FireCell>();
                if (fireCell != null)
                {
                    fireCell.Water();
                    p.ToBeRemoved = true;
                }
            }
            p.PredictedPosition = newPosition;
        }

        // Find neighbours of each particle
        // Lines 5-7 from the algorithm in the paper
        List<FluidParticle>[] neighbours = new List<FluidParticle>[Particles.Count];
        for (int i = 0; i < Particles.Count; i++)
        {
            FluidParticle p = Particles[i];
            p.Index = i;
            List<FluidParticle> foundNeighbours = new List<FluidParticle>();
            foreach (FluidParticle potentialNeighbour in Particles)
            {
                if (potentialNeighbour != p && Vector3.Distance(potentialNeighbour.Position, p.Position) < NeighbourDistance)
                {
                    foundNeighbours.Add(potentialNeighbour);
                }
            }
            neighbours[i] = foundNeighbours;
        }

        float[] lambdas = new float[Particles.Count];
        Vector3[] deltaPos = new Vector3[Particles.Count];

        // Refine the predicted positions by taking incompressibilty into account
        // Lines 8-19 from the algorithm in the paper
        for (int iteration = 0; iteration < SolverIterations; iteration++)
        {
            // Find lambda for each particle
            // Lines 9-11 from the algorithm in the paper
            for (int i = 0; i < Particles.Count; i++)
            {
                FluidParticle p = Particles[i];
                List<FluidParticle> foundNeighbours = neighbours[i];
                float pi = StdSPHDensityEstimator(p, foundNeighbours);
                float c = pi / RestDensity - 1;
                float deltaCSum = DerivativeCSum(p, foundNeighbours);
                lambdas[i] = -c / deltaCSum;
            }

            // Find more finely predicted positions
            // Lines 12-15 from the algorithm in the paper
            for (int i = 0; i < Particles.Count; i++)
            {
                FluidParticle p = Particles[i];
                List<FluidParticle> foundNeighbours = neighbours[i];
                deltaPos[i] = Vector3.zero;
                foreach (FluidParticle neigbour in foundNeighbours)
                {
                    deltaPos[i] += (lambdas[i] + lambdas[neigbour.Index]) * Vector3.Normalize(p.PredictedPosition - neigbour.Position) * SpikyDerivative(p, neigbour);
                }
                deltaPos[i] = 1 / RestDensity * deltaPos[i];
                Vector3 newPosition = p.PredictedPosition + deltaPos[i];
                if (Physics.Linecast(p.PredictedPosition, newPosition + ParticleDistance * (newPosition - p.Position).normalized))
                {
                    deltaPos[i] = Vector3.zero;
                }
            }

            // Use the new positions
            // Lines 16-18 from the algorithm in the paper
            for (int i = 0; i < Particles.Count; i++)
            {
                FluidParticle p = Particles[i];
                p.PredictedPosition += deltaPos[i];
            }
        }

        // Actually set the new positions and count down the expiry of the particles.
        foreach (FluidParticle p in Particles)
        {
            p.Velocity = p.PredictedPosition - p.Position;
            p.Position = p.PredictedPosition;
            p.Expiry--;
        }
    }

    private float StdSPHDensityEstimator(FluidParticle p, List<FluidParticle> neighbours)
    {
        float pi = 0;
        foreach (FluidParticle neighbour in neighbours)
        {
            // Use the Poly6 kernel
            pi += (315f / 64 * Mathf.PI * Mathf.Pow(NeighbourDistance, 4))
                * Mathf.Pow(Mathf.Pow(NeighbourDistance, 2) - Mathf.Pow(Vector3.Distance(p.PredictedPosition, neighbour.PredictedPosition), 2), 3);
        }
        return pi;
    }

    private float DerivativeCSum(FluidParticle p, List<FluidParticle> neigbours)
    {
        float sum = 0;
        foreach (FluidParticle neighbour in neigbours)
        {
            // Use  the derivative of the spiky kernel.
            float derivative = SpikyDerivative(p, neighbour);
            sum += Mathf.Pow(derivative, 2);

        }
        return sum + RelaxationConstant;
    }

    private float SpikyDerivative(FluidParticle p1, FluidParticle p2)
    {
        return 45 * Mathf.Pow(NeighbourDistance, 6) / Mathf.PI
                * Mathf.Pow(NeighbourDistance - Vector3.Distance(p1.PredictedPosition, p2.PredictedPosition), 2);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (FluidParticle p in Particles)
        {
            Gizmos.DrawSphere(p.Position, ParticleDistance / 2);
        }
    }

    public void ShootFluidParticles(Transform transform)
    {
        foreach (Vector3 v in particleShot)
        {
            FluidParticle p = new FluidParticle(
                transform.TransformPoint(v + new Vector3(0, 0, 0.5f)) + new Vector3(0, 0.5f, 0),
                transform.forward * ParticleVelocityScaling,
                random);
            newParticles.Add(p);
        }
    }

    public void ShootFluidParticle(Transform transform)
    {
        FluidParticle p = new FluidParticle(
            transform.TransformPoint(new Vector3(0, 0, 0.5f)) + new Vector3(0, 0.5f, 0),
            transform.forward * ParticleVelocityScaling,
            random);
        newParticles.Add(p);
    }
}
