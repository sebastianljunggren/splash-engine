using UnityEngine;
using System.Collections;

public class QuadRender : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		MeshFilter filter = GetComponent<MeshFilter>();
		var mainCam = Camera.main;
		foreach (var particle in GetComponent<FluidBehaviour>().Particles)
		{
			Graphics.DrawMesh(filter.mesh, particle.Position,
			                  mainCam.transform.rotation,
			                  filter.renderer.material, 0);
		}
	}


}
