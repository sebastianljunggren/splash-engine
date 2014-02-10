using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	Vector2? mouse_start;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1"))
		{
			mouse_start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
		if (Input.GetButtonUp ("Fire1"))
		{
			mouse_start = null;
		}

		if (mouse_start.HasValue)
		{
			var dX = Input.mousePosition.x - mouse_start.Value.x;
			var dY = Input.mousePosition.y - mouse_start.Value.y;

			var transform = GetComponent<Transform>();

			var lookAt = GameObject.Find("Water").GetComponent<Transform>().position;

			transform.RotateAround(lookAt, transform.right,
			                       dY * -.1f);
			transform.RotateAround(lookAt, Vector3.up, 
			                       dX * .1f);

			mouse_start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
	
	}
}
