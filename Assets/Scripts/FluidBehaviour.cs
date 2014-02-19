using UnityEngine;
using System.Collections.Generic;

public class FluidBehaviour : MonoBehaviour {

	public List<FluidParticle> Particles;
    public List<FluidParticle> NewParticles;
	private List<Vector3> ParticleShot;
	private const float LAYER_OFFSET = 0.5f;
	private const float GRAVITY = 0.005f;
    private const int PARTICLE_DIAMETER_IN_SHOT = 5;
    private const float REST_DENSITY = 50f;
    private const float RELAXATION_CONSTANT = 0.1f;
	private int ParticleCount = 0;
    private const float NEIGHBOUR_DISTANCE = LAYER_OFFSET;
	private const float ENERGY_LOSS_ON_BOUNCE = 0.01f;
    private const int SOLVER_ITERATIONS = 4;

	public FluidBehaviour() {
		this.Particles = new List<FluidParticle>();
        this.NewParticles = new List<FluidParticle>();
		this.ParticleShot = new List<Vector3>();
        float radius = 0.5f * LAYER_OFFSET * PARTICLE_DIAMETER_IN_SHOT;
        //  Create cube with points
		for (int x = 0; x < PARTICLE_DIAMETER_IN_SHOT; x++) {
            for (int y = 0; y < PARTICLE_DIAMETER_IN_SHOT; y++)
            {
                for (int z = 0; z < PARTICLE_DIAMETER_IN_SHOT; z++)
                {
                    Vector3 v = new Vector3(x * LAYER_OFFSET - radius, y * LAYER_OFFSET - radius, z * LAYER_OFFSET);
                    this.ParticleShot.Add(v);
                }

            }
		}
        Vector3 center = new Vector3(0, 0, radius);
        // Create sphere shape for shot by removing points
       this.ParticleShot.RemoveAll(point => Vector3.Distance(point, center) > (radius + 0.1f * LAYER_OFFSET) );
    }

	void FixedUpdate () {
		// Remove all particles that have fallen below the level.
		Particles.RemoveAll(p => p.Position.y < -1);

        // Add new particles
        Particles.AddRange(NewParticles);
        NewParticles.Clear();

		int newParticleCount = Particles.Count;
		if (newParticleCount != ParticleCount) {
			ParticleCount = newParticleCount;
            Debug.Log(ParticleCount + " fluid particles.");
		}

		// Predict the positions of the particles
        // Lines 1-4 from the algorithm in the paper
		foreach (FluidParticle p in Particles) {
			// Apply gravity
			p.Velocity.y -= GRAVITY;

			Vector3 newPosition = p.Position + p.Velocity;
            RaycastHit hit = new RaycastHit();
			if (Physics.Linecast(p.Position, newPosition + LAYER_OFFSET * (newPosition - p.Position).normalized, out hit)) {
				if (hit.normal.y != 0) {
					p.Velocity.y += GRAVITY;
				}
				p.Velocity = Vector3.Reflect(p.Velocity * ENERGY_LOSS_ON_BOUNCE, hit.normal);
				newPosition = p.Position + p.Velocity;
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
                if (potentialNeighbour != p && Vector3.Distance(potentialNeighbour.Position, p.Position) < NEIGHBOUR_DISTANCE)
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
        for (int iteration = 0; iteration < SOLVER_ITERATIONS; iteration++)
        {
            // Find lambda for each particle
            // Lines 9-11 from the algorithm in the paper
            for (int i = 0; i < Particles.Count; i++)
            {
                FluidParticle p = Particles[i];
                List<FluidParticle> foundNeighbours = neighbours[i];
                float pi = StdSPHDensityEstimator(p, foundNeighbours);
                float c = pi / REST_DENSITY - 1;
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
                    deltaPos[i] += (lambdas[i] + lambdas[neigbour.Index]) * Vector3.Normalize(p.PredictedPosition - neigbour.PredictedPosition) * SpikyDerivative(p, neigbour);
                }
                deltaPos[i] = 1 / REST_DENSITY * deltaPos[i] ;
                Vector3 newPosition = p.PredictedPosition + deltaPos[i];
                if (Physics.Linecast(p.PredictedPosition, newPosition + LAYER_OFFSET * (newPosition - p.Position).normalized)) {
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

        // Actually set the new positions
        foreach (FluidParticle p in Particles)
        {
            p.Velocity = p.PredictedPosition - p.Position;
            p.Position = p.PredictedPosition;
        }        
	}

    private float StdSPHDensityEstimator(FluidParticle p, List<FluidParticle> neighbours)
    {
        float pi = 0;
        foreach (FluidParticle neighbour in neighbours)
        {
            // Use the Poly6 kernel
            pi += (315f / 64 * Mathf.PI * Mathf.Pow(NEIGHBOUR_DISTANCE, 4))
                * Mathf.Pow(Mathf.Pow(NEIGHBOUR_DISTANCE,2) - Mathf.Pow(Vector3.Distance(p.PredictedPosition, neighbour.PredictedPosition), 2), 3); 
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
        return sum + RELAXATION_CONSTANT;
    }

    private float SpikyDerivative(FluidParticle p1, FluidParticle p2)
    {
        return 45 * Mathf.Pow(NEIGHBOUR_DISTANCE, 6) / Mathf.PI
                * Mathf.Pow(NEIGHBOUR_DISTANCE - Vector3.Distance(p1.PredictedPosition, p2.PredictedPosition), 2);
        
    }

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		foreach (FluidParticle p in Particles) {
			Gizmos.DrawSphere(p.Position, LAYER_OFFSET / 2);
		}
	}

	public void ShootFluidParticles (Transform transform) {
		foreach(Vector3 v in ParticleShot) {
            FluidParticle p = new FluidParticle(
				transform.TransformPoint(v),
				transform.forward / 2);
			NewParticles.Add(p);
		}
	}

    public void ShootFluidParticle(Transform transform)
    {
        FluidParticle p = new FluidParticle(
            transform.TransformPoint(Vector3.zero),
            transform.forward / 2);
        NewParticles.Add(p);
    }
}
