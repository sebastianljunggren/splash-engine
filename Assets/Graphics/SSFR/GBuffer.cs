using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GBuffer : MonoBehaviour {

	public Shader shader;
	private Material mat;

	// Use this for initialization
	void Start () {
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
		mat = new Material (shader);
	}
	
	// Update is called once per frame
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, mat);
	}
}
