using UnityEngine;
using System.Collections;

public class FluidParticle {
	public Vector3 Position;
    public Vector3 PredictedPosition;
	public Vector3 Velocity;
    public bool ToBeRemoved = false;
    public int Expiry = 5 * 50;
    public int Index;
    private const int LEAST_EXPIRY_SECONDS = 5;
    private const int MOST_EXPIRY_SECONDS = 10;


	public FluidParticle(Vector3 Position, Vector3 Velocity, System.Random random) {
		this.Position = Position;
		this.Velocity = Velocity;
        Expiry = random.Next(LEAST_EXPIRY_SECONDS * 50, MOST_EXPIRY_SECONDS * 50 + 1);
	}
}
