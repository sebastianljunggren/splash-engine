using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class GameWorld : MonoBehaviour {

	public WaypointNode[] nodes;
	public WaypointEdge[] edges;

	// Use this for initialization
	void Start () {
		foreach (var edge in edges) {
			edge.Calculate(nodes);

		}
	}

	// Dijkstra's algorithm for shortest path
	public WaypointNode[] ShortestPath(Vector3 source, Vector3 target) {
		// Find the closest waypoints
		int sourceIndex = 0, targetIndex = 0;
		float sourceDist = float.MaxValue, targetDist = float.MaxValue;

		for (int i=0; i<nodes.Length; i++) {
			float distance = Vector3.Distance(source, nodes[i].transform.position);
			if (distance < sourceDist) {
				sourceDist = distance;
				sourceIndex = i;
			}
			distance = Vector3.Distance(target, nodes[i].transform.position);
			if (distance < targetDist) {
				targetDist = distance;
				targetIndex = i;
			}
		}

		// Initializations
		float[] dist = new float[nodes.Length];
		int[] previous = new int[nodes.Length];

		for (int v=0; v<nodes.Length; v++) {
			dist[v] = float.MaxValue;
			previous[v] = -1;
		}

		dist[sourceIndex] = 0;
		var Q = new Collection<int>();
		for (int i=0; i<dist.Length; i++) {
			Q.Add (i);
		}

		// Main loop
		while (Q.Count > 0) {
			int u = Q[0];
			for (int i=0; i<Q.Count; i++) {
				if (dist[Q[i]] < dist[u]) u = Q[i];
			}
			Q.Remove(u);

			// We found our target
			if (u == targetIndex) {
				Stack<WaypointNode> sp = new Stack<WaypointNode>();
				while (previous[u] >= 0) {
					sp.Push(nodes[u]);
					u = previous[u];
				}
				WaypointNode[] result = new WaypointNode[sp.Count];
				sp.CopyTo(result, 0);
				return result;
			}

			if (dist[u] > float.MaxValue - 1) {
				break;
			}

			int[] theRest = new int[Q.Count];
			Q.CopyTo(theRest, 0);
			List<WaypointEdge> neighbours = GetNeighbours(u, theRest);
            
			for (int j=0; j<neighbours.Count; j++) {
				WaypointEdge e = neighbours[j];
				int neighbour = (e.GetFirst() == u ? e.GetSecond() : e.GetFirst());
				float alt = dist[u] + e.GetCost();
				if (alt < dist[neighbour]) {
					dist[neighbour] = alt;
					previous[neighbour] = u;
				}
			}
		}


		return nodes;
	}

	private List<WaypointEdge> GetNeighbours(int current, int[] theRest) {

		List<WaypointEdge> neighbours = new List<WaypointEdge>();


		for (int i=0; i<edges.Length; i++) {
			int theOther = 0;
			if (edges[i].GetFirst() == current) {
				theOther = edges[i].GetSecond();
			}
            else if (edges[i].GetSecond() == current) {
                theOther = edges[i].GetFirst();
            }
            else {
                continue;
            }
			// Is theOther in theRest
			bool ok = false;
			for (int v=0; v<theRest.Length; v++) {
				if (theOther == theRest[v]) {
					ok = true;
				}
			}
			if (ok) {
				neighbours.Add (edges[i]);
			}
		}
		return neighbours;
	}


	[System.Serializable]
	public class WaypointEdge {
		public WaypointNode first;
		public WaypointNode second;

		private float cost;
		private int firstIndex, secondIndex;
		private static float accuracy = 0.0001f;

		public void Calculate(WaypointNode[] nodes) {
			cost = (second.transform.position - first.transform.position).magnitude;
            
			for (int i=0; i<nodes.Length; i++) {
				float dist = Vector3.Distance(first.transform.position, nodes[i].transform.position);
				if (dist < accuracy) {
					firstIndex = i;
				}
				dist = Vector3.Distance(second.transform.position, nodes[i].transform.position);
				if (dist < accuracy) {
					secondIndex = i;
				}
			}
			Debug.DrawLine (first.transform.position, second.transform.position, Color.red, 100);
            
		}

		public float GetCost() {
			return cost;
		}

		public int GetFirst() {
			return firstIndex;
		}

		public int GetSecond() {
			return secondIndex;
		}
	}
}
