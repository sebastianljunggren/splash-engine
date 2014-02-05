using UnityEngine;
using System.Collections;

public class FluidShooterBehaviour : MonoBehaviour {

	public FluidBehaviour water;

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown ("Fire1")) {
			water.ShootFluid(this.transform);
		}
	}
}
