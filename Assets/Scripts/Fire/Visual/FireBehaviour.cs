using UnityEngine;
using System.Collections;

public class FireBehaviour : MonoBehaviour {
	
	private Component[] fireComponentsArray;

	void Start() {
		fireComponentsArray = GetComponentsInChildren<ParticleEmitter>();
	}

	void update () {
	
	}

	//Called when cells are damaged enough
	void IncreaseIntensity() {
		foreach (ParticleEmitter pe in fireComponentsArray) {
			pe.maxEmission = pe.maxEmission * 1.1f;
			pe.minEmission = pe.minEmission * 1.1f;
			pe.maxEnergy = pe.maxEnergy * 1.1f;
			pe.minEnergy = pe.minEnergy * 1.1f;
		}
	}

	//Called when cells are healed enough
	void LowerIntensity() {
		foreach (ParticleEmitter pe in fireComponentsArray) {
			pe.maxEmission = pe.maxEmission / 1.1f;
			pe.minEmission = pe.minEmission / 1.1f;
			pe.maxEnergy = pe.maxEnergy / 1.1f;
			pe.minEnergy = pe.minEnergy / 1.1f;
		}
	}	
}
