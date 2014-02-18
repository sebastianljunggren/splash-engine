using UnityEngine;
using System.Collections.Generic;

public class FluidBehaviour : MonoBehaviour {

	public List<FluidParticle> Particles;
    public List<FluidParticle> NewParticles;
	private List<Vector3> ParticleRow;
	private const float LAYER_OFFSET = 0.5f;
	private const float GRAVITY = 0.005f;
	private const int LAYERS_IN_SHOT = 5;
	private const int CIRCLES_IN_SHOT = 3;
    private const float NEIGHBOUR_DISTANCE = LAYER_OFFSET * 0.8f;
	private const float ENERGY_LOSS_ON_BOUNCE = 0.05f;
    private const int SOLVER_ITERATIONS = 5;
    private const float REST_DENSITY = 100f;
    private const float RELAXATION_CONSTANT = 0.5f;
	private int ParticleCount = 0;

	public FluidBehaviour() {
		this.Particles = new List<FluidParticle>();
        this.NewParticles = new List<FluidParticle>();
		int layerMultiplier = 3;
		int particlesInLayer = 1;
		ParticleRow = new List<Vector3>();
		particlesInLayer = 1;
		int particle = 0;
		for (int layer = 0; layer < CIRCLES_IN_SHOT; layer++) {
			for (int i = 0; i < particlesInLayer; i++, particle++) {
				float rotation = (2 * Mathf.PI / particlesInLayer) * i;
				ParticleRow.Add(new Vector3(
					Mathf.Cos(rotation) * LAYER_OFFSET * layer,
					Mathf.Sin(rotation) * LAYER_OFFSET * layer + 0.5f,
					0
				));
			}
			particlesInLayer *= layerMultiplier;
		}
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
			if (Physics.Linecast(p.Position, newPosition, out hit)) {
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
                if (Physics.Linecast(p.PredictedPosition, p.PredictedPosition + deltaPos[i])) {
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
            sum += Mathf.Pow(derivative, 2) + RELAXATION_CONSTANT;

        }
        return sum;
    }

    private float SpikyDerivative(FluidParticle p1, FluidParticle p2)
    {
        return 45 * Mathf.Pow(NEIGHBOUR_DISTANCE, 6) / Mathf.PI
                * Mathf.Pow(Vector3.Distance(p1.PredictedPosition, p2.PredictedPosition), 2);
        
    }

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		foreach (FluidParticle p in Particles) {
			Gizmos.DrawSphere(p.Position, LAYER_OFFSET / 2);
		}
	}

	public void ShootFluid (Transform transform) {
		//for (int i = 0; i < LAYERS_IN_SHOT; i++) {
			//foreach(Vector3 v in ParticleRow) {
                Vector3 v = ParticleRow[0];
                int i = 0;
				FluidParticle p = new FluidParticle(
					transform.TransformPoint(v + new Vector3(0, 0 , LAYER_OFFSET * i)),
					transform.forward / 2);
				NewParticles.Add(p);
			//}
		//}
	}
}
