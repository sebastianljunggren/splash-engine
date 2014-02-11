using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PathfindingBehaviour))]
public class AgentBehaviour : MonoBehaviour {

    public PathfindingBehaviour Pathfinding { get; set; }

	// Use this for initialization
	void Start () {
	    Pathfinding = GetComponent<PathfindingBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
}


