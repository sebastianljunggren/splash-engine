using UnityEngine;
using System.Collections;

public class PathfindingBehaviour : MonoBehaviour {

	public float speed;
	public float rotationSpeed;
	public GameWorld gameWorld;

	private WaypointNode[] currentWaypoints;
	private Vector3 waypointTarget;
	private int waypointIndex;
	private Transform cachedTransform;

	// Use this for initialization
	void Start () {
		cachedTransform = transform;

		currentWaypoints = gameWorld.ShortestPath(new Vector3(4f, 1f, -8f), new Vector3(19, 1f, -16f));

		waypointIndex = 0;
		MoveTo(currentWaypoints[waypointIndex].transform.position);
	}
	
	// Update is called once per frame
	void Update () {


		Vector3 distance = waypointTarget - cachedTransform.position;
		if (distance.magnitude > 0.5) {
			distance.Normalize ();

			cachedTransform.rotation = Quaternion.Slerp (cachedTransform.rotation,
			                                             Quaternion.LookRotation(distance), rotationSpeed * Time.deltaTime);
			
			cachedTransform.position += cachedTransform.forward * speed * Time.deltaTime;
		}
        else if (waypointIndex < currentWaypoints.Length - 1) {
            waypointIndex++;
            MoveTo(currentWaypoints[waypointIndex].transform.position);
        }
        else {
            currentWaypoints = gameWorld.ShortestPath(cachedTransform.transform.position, new Vector3(10, 1f, 12f));

            if (currentWaypoints.Length > 0) {
                waypointIndex = 0;
                MoveTo(currentWaypoints[waypointIndex].transform.position);
            }
        }
	}

	void MoveTo(Vector3 waypoint) {
		this.waypointTarget = waypoint;
	}
}
