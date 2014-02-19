using UnityEngine;
using System.Collections;

public class WaterToTexture : MonoBehaviour {	

	int size = 16;
	public Texture2D data;

	// Use this for initialization
	void Start () {
		data = new Texture2D (size, size, TextureFormat.RGB24, false);
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;

		var particles = GetComponent<FluidBehaviour>().Particles;
		Color[] positions = new Color[size*size];

		foreach (var particle in particles) {
			positions[i++] = new Color(particle.Position.x, particle.Position.y, particle.Position.z);
			if (i > size*size)
				break;
		}

		data.SetPixels(positions);
		data.Apply ();
	}
}
