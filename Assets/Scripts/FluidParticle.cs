using UnityEngine;
using System.Collections;

public class FluidParticle
{
    public Vector3 Position{get; set;}
    public Vector3 PredictedPosition{get; set;}
    public Vector3 Velocity{get; set;}
    public bool ToBeRemoved{get; set;}
    public int Expiry{get; set;}
    public int Index{get; set;}
    private const int LEAST_EXPIRY_SECONDS = 5;
    private const int MOST_EXPIRY_SECONDS = 10;


    public FluidParticle(Vector3 Position, Vector3 Velocity, System.Random random)
    {
        this.Position = Position;
        this.Velocity = Velocity;
        Expiry = random.Next(LEAST_EXPIRY_SECONDS * 50, MOST_EXPIRY_SECONDS * 50 + 1);
        this.ToBeRemoved = false;
    }
}
