using UnityEngine;
using System.Collections;

public class TextureRenderer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<MeshRenderer> ().material.mainTexture = 
			GameObject.Find ("Water4").
				GetComponent<WaterToTexture> ().data;
	}
}
