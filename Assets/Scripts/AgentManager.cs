using UnityEngine;
using System.Collections;

public class AgentManager : MonoBehaviour {


    private AgentBehaviour[] agents;
    private RoomInformation[] rooms;

    public GameWorld world;
    public enum AgentState { SEARCH, DIVIDE }
    public AgentState agentState;

	// Use this for initialization
	void Start () {
        agents = GetComponentsInChildren<AgentBehaviour>();
        rooms = world.GetRooms();

        foreach (var agent in agents) {
            agent.Pathfinding.TargetReached += new TargetReachedEvent(Pathfinding_TargetReached);

        }
	}
	
	// Update is called once per frame
	void Update () {
        
        switch (agentState) {
        
            case AgentState.SEARCH:
                stateSearch();
                break;
            case AgentState.DIVIDE:
                stateDivide();
                break;
                
        }

        
        
	}

    private bool allRoomsDiscovered() {
        // If all rooms discovered
        for (int i=0; i<rooms.Length; i++) {
            if (!rooms[i].IsDiscovered) return false;

        }
        return true;
    }

    private void stateSearch() {
        // Greedy algorithm to search through the whole house before trying to extinguish the fire
        for (int i = 0; i < rooms.Length; i++) {
            if (!rooms[i].IsDiscovered && !isRoomTargeted(rooms[i].name)) {

                AgentBehaviour nearestAgent = findNearestAgent(rooms[i].transform.position);

                if (nearestAgent != null) {
                    nearestAgent.Pathfinding.StartJourney(rooms[i].name);
                    //Debug.Log(nearestAgent.name + ": " + rooms[i].name);
                }
            }
        }


        if (allRoomsDiscovered()) {
            agentState = AgentState.DIVIDE;
            divideAgents();
            Debug.Log("Dividing the agents");
        }
    }

    private AgentBehaviour findNearestAgent(Vector3 position) {
        AgentBehaviour nearestAgent = null;
        var minDistance = float.MaxValue;
        for (int j = 0; j < agents.Length; j++) {
            var distance = Vector3.Distance(agents[j].transform.position, position);
            if (!agents[j].Pathfinding.IsBusy && distance < minDistance) {
                minDistance = distance;
                nearestAgent = agents[j];
            }
        }
        return nearestAgent;
    }

    private void divideAgents() {
        int totalFire = 0;
        for (int i=0; i<rooms.Length; i++) {
            totalFire += rooms[i].FireCount;
        }

        // Divide based on most fire
        for (int i=0; i<rooms.Length; i++) {
            float firePercentage = rooms[i].FireCount / (float)totalFire;
            int numberOfAgents = Mathf.RoundToInt(firePercentage * agents.Length);
            Debug.Log(rooms[i].name + ": " + firePercentage);
            sendNearestAgentsToRoom(rooms[i], numberOfAgents);
        }
    }

    private void sendNearestAgentsToRoom(RoomInformation room, int numberOfAgents) {
        for (int n=0; n<numberOfAgents; n++) {
            AgentBehaviour nearestAgent = findNearestAgent(room.transform.position);
            
            if (nearestAgent != null) {
                nearestAgent.Pathfinding.StartJourney(room.name);
                Debug.Log(nearestAgent.name + " to " + room.name);
            }
        }

    }

    private void stateDivide() {
        
    }

    public void Pathfinding_TargetReached(object source, string arg) {
        //Debug.Log(source + " " + arg);


        for (int i = 0; i < rooms.Length; i++) {
            if (rooms[i].name.Equals(arg)) {
                rooms[i].IsDiscovered = true;
            }
            //Debug.Log(rooms[i].name + ": " + rooms[i].IsDiscovered);
        }

        
    }

    private bool isRoomTargeted(string room) {
        for (int i = 0; i < agents.Length; i++) {
            var agent = agents[i];
            if (agent.Pathfinding.Target.Equals(room)) return true;
        }
        return false;
    }
}
