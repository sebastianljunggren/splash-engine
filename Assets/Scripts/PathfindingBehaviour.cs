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
    private CharacterController charController;

    public string Target;
    public event TargetReachedEvent TargetReached;
    
    // TODO: Should be replaced by state machine
    public bool IsBusy = false;

    public string CurrentRoom;

	// Use this for initialization
	void Start () {
		cachedTransform = transform;
        charController = GetComponent<CharacterController>();

        waypointTarget = cachedTransform.position;

        gameWorld.WaypointGraphUpdated += new WaypointGraphUpdatedEvent(gameWorld_WaypointGraphUpdated);
	}

    public void gameWorld_WaypointGraphUpdated() {
        if (Target.Length > 0) {
            Debug.Log("WaypointGraphUpdated: " + Target);
            StartJourney(Target);
        }
    }

	// Update is called once per frame
	void Update () {
		Vector3 distance = waypointTarget - cachedTransform.position;
		if (distance.magnitude > 1) {
			distance.Normalize();

			cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation,
			                                             Quaternion.LookRotation(distance), rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = new Vector3(0, 0, 1);
            moveDirection = cachedTransform.TransformDirection(moveDirection) * speed;

			charController.Move(moveDirection * Time.deltaTime);
		}
        else if (waypointIndex < currentWaypoints.Length - 1) {
            waypointIndex++;
            MoveTo(currentWaypoints[waypointIndex].position);
        }
        
        RoomReached(CurrentRoom);
        
	}

    public void RoomReached(string room) {
        CurrentRoom = room;

        if (room.Equals(Target)) {
            TargetReached(this, Target);
            IsBusy = false;
            Target = string.Empty;
        }
    }

    public void StartJourney(string objectName) {
        this.Target = objectName;

        // Set busy even if the agent is already there, is removed next frame.
        // Used when dividing the agents based on fire data.
        IsBusy = true;

        if (CurrentRoom != objectName) {

            currentWaypoints = gameWorld.ShortestPath(cachedTransform.position, GameObject.Find(objectName).transform.position);
            Debug.Log("currentWaypoints: " + currentWaypoints.Length);
            if (currentWaypoints.Length > 0) {
                for (int i = 0; i < currentWaypoints.Length - 1; i++) {
                    Debug.DrawLine(currentWaypoints[i].position, currentWaypoints[i + 1].position, Color.red, 5);
                }

                waypointIndex = 0;
                MoveTo(currentWaypoints[waypointIndex].position);


            }
        }

    }

	void MoveTo(Vector3 waypoint) {
		this.waypointTarget = waypoint;
	}
}
