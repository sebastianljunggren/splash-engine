using UnityEngine;
using System.Collections;

public class SteeringBehaviour : MonoBehaviour {

	public Transform target;
	public float speed;
	private Transform transformCache;
	private CharacterController charController;

	// Use this for initialization
	void Start () {
		transformCache = transform;
		charController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target.position);

		Vector3 forward = transformCache.TransformDirection(Vector3.forward);
		charController.SimpleMove(forward * speed);
	}
}
