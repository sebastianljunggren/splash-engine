using UnityEngine;
using System.Collections;

public class FluidParticle {
	public Vector3 Position;
    public Vector3 PredictedPosition;
	public Vector3 Velocity;
    public int Index;

	public FluidParticle(Vector3 Position, Vector3 Velocity) {
		this.Position = Position;
		this.Velocity = Velocity;
	}
}
