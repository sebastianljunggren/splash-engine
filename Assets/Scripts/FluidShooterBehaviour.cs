using UnityEngine;
using System.Collections;

public class FluidShooterBehaviour : MonoBehaviour {

	public FluidBehaviour water;
    private bool shootStream = false;

	// Update is called once per frame
	void FixedUpdate() {
        shootStream = !shootStream;
		if (Input.GetButtonDown ("Fire1")) {
			water.ShootFluidParticles(this.transform);
		}
        if (Input.GetButton("Fire2") && shootStream)
        {
            water.ShootFluidParticle(this.transform);
        }
	}
}
