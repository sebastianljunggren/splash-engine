using UnityEngine;
using System.Collections;

public class DrawDepthTest : MonoBehaviour {

	public Texture texture;

	// Use this for initialization
	void Start () {
	}

	void OnGUI () {
		if(Event.current.type.Equals(EventType.Repaint))
			Graphics.DrawTexture(new Rect(0, 0, 256, 256), texture);
	}
}
