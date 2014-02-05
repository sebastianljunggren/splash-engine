using UnityEngine;
using System.Collections.Generic;

public class FluidBehaviour : MonoBehaviour {

	public List<FluidParticle> particles;
	private List<Vector3> particleRow;
	private const float LAYER_OFFSET = 0.01f;
	public int LayersInShot = 10;

	public FluidBehaviour() {
		int layerCount = 4;
		int layerMultiplier = 3;
		int particlesInLayer = 1;
		particleRow = new List<Vector3>();
		particlesInLayer = 1;
		int particle = 0;
		for (int layer = 0; layer < layerCount; layer++) {
			for (int i = 0; i < particlesInLayer; i++, particle++) {
				float rotation = (2 * Mathf.PI / particlesInLayer) * i;
				particleRow.Add(new Vector3(
					Mathf.Cos(rotation) * LAYER_OFFSET * layer,
					Mathf.Sin(rotation) * LAYER_OFFSET * layer,
					0
				));
			}
			particlesInLayer *= layerMultiplier;
		}

	}

	void Start () {
		this.particles = new List<FluidParticle>();
	}

	void FixedUpdate () {
		// TODO: apply gravity and update positions based on speed.
	}

	public void ShootFluid (Transform transform) {
		Vector3 v = particleRow[0];
		FluidParticle p = new FluidParticle(transform.TransformPoint(v), transform.forward);
		particles.Add(p);
		Debug.Log("New particle added. Position: " + p.Position + ", Velocity: " + p.Velocity + ".");

	}
}
