using UnityEngine;
using System.Collections;

public class CubeBehaviour : MonoBehaviour {

	public float speed;
	
	void Start () {
	
	}

	void Update() {
		if (Input.GetKey(KeyCode.Space)) {
			rigidbody.AddForce(Vector3.up * speed);
		}
		else if (Input.GetKey(KeyCode.W)) {
			rigidbody.AddForce(Vector3.forward * speed);
		}
		else if (Input.GetKey(KeyCode.S)) {
			rigidbody.AddForce(Vector3.back * speed);
		}
		else if (Input.GetKey(KeyCode.A)) {
			rigidbody.AddForce(Vector3.left * speed);
		}
		else if (Input.GetKey(KeyCode.D)) {
			rigidbody.AddForce(Vector3.right * speed);
		}
		
	}

	void FixedUpdate () {

	}
}
