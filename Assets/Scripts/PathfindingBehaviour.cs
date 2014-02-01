using UnityEngine;
using System.Collections;

public class PathfindingBehaviour : MonoBehaviour {
	
	public Vector3[] waypoints;
	public float speed;
	public float rotationSpeed;

	private Vector3 waypoint;
	private int wpIndex;
	private Transform cachedTransform;

	// Use this for initialization
	void Start () {
		cachedTransform = transform;

		wpIndex = 0;
		MoveTo(waypoints[wpIndex]);
	}
	
	// Update is called once per frame
	void Update () {


		Vector3 distance = waypoint - cachedTransform.position;
		if (distance.magnitude > 0.5) {
			distance.Normalize ();

			cachedTransform.rotation = Quaternion.Slerp (cachedTransform.rotation,
			                                             Quaternion.LookRotation(distance), rotationSpeed * Time.deltaTime);
			
			cachedTransform.position += cachedTransform.forward * speed * Time.deltaTime;
			Debug.Log (distance * speed * Time.deltaTime);
		}
		else if (wpIndex < waypoints.Length - 1) {
			wpIndex++;
			Debug.Log (wpIndex);
			MoveTo (waypoints[wpIndex]);
		}
	}

	void MoveTo(Vector3 waypoint) {
		this.waypoint = waypoint;
	}
}
