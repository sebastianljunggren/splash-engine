using UnityEngine;
using System.Collections;

using WaypointGeneration;

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

        waypointTarget = cachedTransform.position;
        StartJourney("Node1");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Alpha1)) {
            StartJourney("Node1");
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            StartJourney("Node3");
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            StartJourney("Node5");
        }
        if (Input.GetKey(KeyCode.Alpha4)) {
            StartJourney("Node8");
        }

		Vector3 distance = waypointTarget - cachedTransform.position;
		if (distance.magnitude > 0.5) {
			distance.Normalize();

			cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation,
			                                             Quaternion.LookRotation(distance), rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = new Vector3(0, 0, 1);
            moveDirection = cachedTransform.TransformDirection(moveDirection) * speed;

			GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
		}
        //else if (waypointIndex < currentWaypoints.Length - 1) {
        //    waypointIndex++;
        //    //MoveTo(currentWaypoints[waypointIndex].transform.position);
        //}
	}

    void StartJourney(string objectName) {
        //currentWaypoints = gameWorld.ShortestPath(cachedTransform.position, GameObject.Find(objectName).transform.position);

        //if (currentWaypoints.Length > 0) {
            waypointIndex = 0;
            //MoveTo(currentWaypoints[waypointIndex].transform.position);
        //}
    }

	void MoveTo(Vector3 waypoint) {
		this.waypointTarget = waypoint;
	}
}
