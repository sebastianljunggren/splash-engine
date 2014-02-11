using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WaypointGeneration {

    public class WaypointGraphGeneration : MonoBehaviour {

        //private List<WaypointNode> nodes;
        private List<WaypointEdge> edges;
        private WaypointNode[,] grid;
        public int gridSize;
        public int cellSize;
        public int range;
        public Vector3 gridOffset;


        // Use this for initialization
        public WaypointGraph GenerateGraph() {
            Debug.Log("Generating graph...");

            grid = new WaypointNode[gridSize, gridSize];
            List<WaypointNode> nodes = new List<WaypointNode>();
            edges = new List<WaypointEdge>();

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    WaypointNode node = new WaypointNode(new Vector3(gridOffset.x + i * cellSize, gridOffset.y, gridOffset.z + j * cellSize));
                    grid[i, j] = node;
                    nodes.Add(node);
                }
            }

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {

                    CheckNeighbours(i, j);

                }
            }

            for (int i = 0; i < edges.Count; i++) {
                Debug.DrawLine(edges[i].first.position, edges[i].second.position, Color.green, 100);

            }

            Debug.Log("Generation done!");

            return new WaypointGraph(nodes.ToArray(), edges.ToArray());

        }

        void CheckNeighbours(int x, int y) {
            for (int a = -range; a <= range; a++) {
                for (int b = -range; b <= range; b++) {
                    if (x + a >= 0 && y + b >= 0 && x + a < gridSize && y + b < gridSize) {
                        RaycastHit hit;
                        WaypointNode current = grid[x, y];
                        WaypointNode target = grid[x + a, y + b];
                        Vector3 direction = target.position - current.position;
                        if (Physics.Raycast(current.position, direction, out hit, 100.0F)) {
                            float distance = hit.distance;
                            if (distance > direction.magnitude) {
                                edges.Add(new WaypointEdge(current, target));
                            }
                        }
                    }
                }
            }
        }

    }

}