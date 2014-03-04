using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PathfindingBehaviour))]
public class AgentBehaviour : MonoBehaviour {


    public FluidBehaviour Water;
    public PathfindingBehaviour Pathfinding { get; set; }

    private float timeBetweenShots = .01f;
    private float timeLeft;

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

                GameObject room = GameObject.Find(Pathfinding.CurrentRoom);
                if (room.GetComponent<RoomInformation>().FireCount > 0) {
                    FireCell[] fires = room.GetComponent<Flammable>().GetComponentsInChildren<FireCell>();
                    for (int i = 0; i < fires.Length; i++) {
                        if (fires[i].IsBurning) {
                            transform.LookAt(fires[i].transform);
                            break;
                        }
                    }

                    Water.ShootFluidParticle(this.transform);
                }

                
            }
            
            timeLeft = timeBetweenShots;
        }
	}


    


}


