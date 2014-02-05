using UnityEngine;
using System.Collections.Generic;

public class FluidBehaviour : MonoBehaviour {

	public List<FluidParticle> Particles;
	private List<Vector3> particleRow;
	private const float LAYER_OFFSET = 0.01f;
	public int LayersInShot = 10;

	public FluidBehaviour() {
		this.Particles = new List<FluidParticle>();
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

	void FixedUpdate () {
		// TODO: apply gravity and update positions based on speed.
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		foreach (FluidParticle p in Particles) {
			Gizmos.DrawSphere(p.Position, 0.01f);
		}
	}

	public void ShootFluid (Transform transform) {
		Vector3 v = particleRow[0];
		FluidParticle p = new FluidParticle(transform.TransformPoint(v), transform.forward);
		Particles.Add(p);
		Debug.Log("New particle added. Position: " + p.Position + ", Velocity: " + p.Velocity + ".");

	}
}
