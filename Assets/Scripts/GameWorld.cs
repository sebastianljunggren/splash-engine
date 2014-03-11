using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using WaypointGeneration;

public delegate void WaypointGraphUpdatedEvent();

[RequireComponent(typeof(WaypointGraphGeneration))]
public class GameWorld : MonoBehaviour {

    private WaypointGraph graph;
    public event WaypointGraphUpdatedEvent WaypointGraphUpdated;

    // Use this for initialization
    void Start() {
        Debug.Log("GameWorld Start");

        //UpdateWaypointGraph();

        InvokeRepeating("UpdateWaypointGraph", 0f, 3f);
    }

    void Update() {
        if (Input.GetKey(KeyCode.G)) {
            Debug.Log("GenerateGraph");
            UpdateWaypointGraph();

            WaypointGraphUpdated();
        }
    }

    public void UpdateWaypointGraph() {
        graph = GetComponent<WaypointGraphGeneration>().GenerateGraph();

        foreach (var edge in graph.Edges) {
            edge.Calculate(graph.Nodes);

        }
    }

    public RoomInformation[] GetRooms() {
        return GetComponentsInChildren<RoomInformation>();
    }

    // Dijkstra's algorithm for shortest path
    public WaypointNode[] ShortestPath(Vector3 source, Vector3 target) {
        WaypointNode[] nodes = graph.Nodes;
        
        // Find the closest waypoints
        int sourceIndex = 0, targetIndex = 0;
        float sourceDist = float.MaxValue, targetDist = float.MaxValue;

        for (int i = 0; i < nodes.Length; i++) {
            float distance = Vector3.Distance(source, nodes[i].position);
            if (distance < sourceDist) {
                sourceDist = distance;
                sourceIndex = i;
            }
            distance = Vector3.Distance(target, nodes[i].position);
            if (distance < targetDist) {
                targetDist = distance;
                targetIndex = i;
            }
        }

        // Initializations
        float[] dist = new float[nodes.Length];
        int[] previous = new int[nodes.Length];

        for (int v = 0; v < nodes.Length; v++) {
            dist[v] = float.MaxValue;
            previous[v] = -1;
        }

        dist[sourceIndex] = 0;
        var Q = new Collection<int>();
        for (int i = 0; i < dist.Length; i++) {
            Q.Add(i);
        }

        // Main loop
        while (Q.Count > 0) {
            int u = Q[0];
            for (int i = 0; i < Q.Count; i++) {
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
            List<WaypointEdge> neighbours = GetConnectedEdges(u, theRest);

            for (int j = 0; j < neighbours.Count; j++) {
                WaypointEdge e = neighbours[j];
                int neighbour = (e.FirstIndex == u ? e.SecondIndex : e.FirstIndex);
                float alt = dist[u] + e.Cost;
                if (alt < dist[neighbour]) {
                    dist[neighbour] = alt;
                    previous[neighbour] = u;
                }
            }
        }


        return nodes;
    }

    // Returns a list of all edges connected to the current node
    private List<WaypointEdge> GetConnectedEdges(int current, int[] theRest) {

        List<WaypointEdge> neighbours = new List<WaypointEdge>();
        WaypointEdge[] edges = graph.Edges;

        for (int i = 0; i < edges.Length; i++) {
            int theOther = 0;
            if (edges[i].FirstIndex == current) {
                theOther = edges[i].SecondIndex;
            }
            else if (edges[i].SecondIndex == current) {
                theOther = edges[i].FirstIndex;
            }
            else {
                continue;
            }
            // Is theOther in theRest
            bool ok = false;
            for (int v = 0; v < theRest.Length; v++) {
                if (theOther == theRest[v]) {
                    ok = true;
                }
            }
            if (ok) {
                neighbours.Add(edges[i]);
            }
        }
        return neighbours;
    }

    
}
