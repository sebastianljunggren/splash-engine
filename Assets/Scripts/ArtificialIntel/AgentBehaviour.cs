using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PathfindingBehaviour))]
public class AgentBehaviour : MonoBehaviour {


    public FluidBehaviour Water;
    public PathfindingBehaviour Pathfinding { get; set; }

    private float timeBetweenShots = .1f;
    private float timeLeft;

    private FireCell currentTarget;
    private GameObject room;
    private RoomInformation roomInfo;
    private Flammable roomFlammable;

	// Use this for initialization
	void Start () {
	    Pathfinding = GetComponent<PathfindingBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        //Debug.Log(timeLeft);

        if (Pathfinding.Target.Equals(string.Empty) && timeLeft < 0) {
            if (Pathfinding.Target != null) {

                if (room == null || !Pathfinding.CurrentRoom.Equals(room.name)) {
                    room = GameObject.Find(Pathfinding.CurrentRoom);
                    roomInfo = room.GetComponent<RoomInformation>();
                    roomFlammable = room.GetComponent<Flammable>();
                }
                if (roomInfo.FireCount > 0) {
                    if (currentTarget == null || !currentTarget.IsBurning) {
                        
                        FireCell[] fires = roomFlammable.GetComponentsInChildren<FireCell>();
                        if (currentTarget == null) {
                            for (int i = 0; i < fires.Length; i++) {
                                if (fires[i].IsBurning) {
                                    currentTarget = fires[i];
                                    break;
                                }
                            }
                        }
                        else {
                            Collider[] colliders = Physics.OverlapSphere(currentTarget.transform.position, roomFlammable.cellSize);
                            Debug.Log(colliders.Length);
                            for (int i = 0; i < colliders.Length; i++) {
                                FireCell cell = colliders[i].GetComponent<FireCell>();
                                if (cell != null && cell.IsBurning) {
                                    currentTarget = cell;
                                    break;
                                }
                            }
                            if (!currentTarget.IsBurning) {
                                currentTarget = null;
                            }
                        }
                        
                    }

                    if (currentTarget != null) {
                        transform.LookAt(currentTarget.transform);
                        //Water.ShootFluidParticle(this.transform);
                        Water.ShootFluidParticle(this.transform);
                    }
                }

                
            }
            
            timeLeft = timeBetweenShots;
        }
	}


    


}


