using UnityEngine;
using System.Collections;

using WaypointGeneration;

public delegate void TargetReachedEvent(object source, string arg);

public class PathfindingBehaviour : MonoBehaviour {

	public float speed;
	public float rotationSpeed;
	public GameWorld gameWorld;

	private WaypointNode[] currentWaypoints = new WaypointNode[0];
	private Vector3 waypointTarget;
	private int waypointIndex = 0;
	private Transform cachedTransform;
    //private string targetName;

    public string Target;
    public event TargetReachedEvent TargetReached;
    
    // TODO: Should be replaced by state machine
    public bool IsBusy = false;

	// Use this for initialization
	void Start () {
		cachedTransform = transform;

        waypointTarget = cachedTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 distance = waypointTarget - cachedTransform.position;
		if (distance.magnitude > 0.5) {
			distance.Normalize();

			cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation,
			                                             Quaternion.LookRotation(distance), rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = new Vector3(0, 0, 1);
            moveDirection = cachedTransform.TransformDirection(moveDirection) * speed;

			GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
		}
        else if (waypointIndex < currentWaypoints.Length - 1) {
            waypointIndex++;
            MoveTo(currentWaypoints[waypointIndex].position);
        }
        else {
            TargetReached(this, Target);
            IsBusy = false;
            Target = string.Empty;
        }
	}

    public void StartJourney(string objectName) {
        this.Target = objectName;
        currentWaypoints = gameWorld.ShortestPath(cachedTransform.position, GameObject.Find(objectName).transform.position);

        if (currentWaypoints.Length > 0) {
            for (int i = 0; i < currentWaypoints.Length - 1; i++) {
                Debug.DrawLine(currentWaypoints[i].position, currentWaypoints[i + 1].position, Color.red, 5);
            }

            waypointIndex = 0;
            MoveTo(currentWaypoints[waypointIndex].position);

            IsBusy = true;
        }
    }

	void MoveTo(Vector3 waypoint) {
		this.waypointTarget = waypoint;
	}
}
