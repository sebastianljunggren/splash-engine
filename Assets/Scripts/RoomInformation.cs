using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomInformation : MonoBehaviour {

    public int FireCount;
    public bool IsDiscovered;
    public AgentManager agentManager;

    private FireCell[] fireCells;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        fireCells = GetComponentsInChildren<FireCell>();

        int oldFireCount = FireCount;
        FireCount = 0;
        for (int i = 0; i < fireCells.Length; i++) {
            if (fireCells[i].IsBurning) {
                FireCount++;
            }
        }
        if (FireCount != oldFireCount) {
            agentManager.Pathfinding_TargetReached(this, name);
        }

	}

    void OnTriggerEnter(Collider other) {
        Debug.Log(name + " colliding with " + other.name);
        if (other.GetComponent<AgentBehaviour>() != null) {
            other.GetComponent<AgentBehaviour>().Pathfinding.RoomReached(name);
        }
        else {
            other.GetComponent<PlayerBehaviour>().RoomReached(name);
        }

        
    }
}
