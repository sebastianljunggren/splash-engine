using UnityEngine;
using System.Collections;

public class AgentManager : MonoBehaviour {


    private AgentBehaviour[] agents;

	// Use this for initialization
	void Start () {
        agents = GetComponentsInChildren<AgentBehaviour>();

        foreach (var agent in agents) {
            agent.Pathfinding.TargetReached += new TargetReachedEvent(Pathfinding_TargetReached);
        }

        agents[0].Pathfinding.StartJourney("Room4");
        agents[1].Pathfinding.StartJourney("Room2");
        agents[2].Pathfinding.StartJourney("Room3");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Pathfinding_TargetReached(object source, string arg) {
        Debug.Log(source + " " + arg);

        PathfindingBehaviour pathfinding = source as PathfindingBehaviour;
        pathfinding.StartJourney("Room" + Random.Range(1, 5));
    }
}
