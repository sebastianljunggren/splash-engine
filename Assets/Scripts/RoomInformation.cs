using UnityEngine;
using System.Collections;

public class RoomInformation : MonoBehaviour {

    public int FireCount;
    public bool IsDiscovered;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        Debug.Log(name + " colliding with " + other.name);
        other.GetComponent<AgentBehaviour>().Pathfinding.RoomReached(name);
    }
}
