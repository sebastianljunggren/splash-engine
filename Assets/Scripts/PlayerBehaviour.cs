using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public string CurrentRoom;

    public event TargetReachedEvent TargetReached;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RoomReached(string room) {
        CurrentRoom = room;

        TargetReached(this, CurrentRoom);
    }


}
