using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WaypointGeneration {

    public class WaypointGraphGeneration : MonoBehaviour {

        //private List<WaypointNode> nodes;
        private List<WaypointEdge> edges;
        private WaypointNode[,] grid;
        public int gridSizeX;
        public int gridSizeZ;
        public int cellSize;
        public int range;
        public Vector3 gridOffset;


        // Use this for initialization
        public WaypointGraph GenerateGraph() {
            Debug.Log("Generating graph...");

            grid = new WaypointNode[gridSizeX, gridSizeZ];
            List<WaypointNode> nodes = new List<WaypointNode>();
            edges = new List<WaypointEdge>();

            for (int i = 0; i < gridSizeX; i++) {
                for (int j = 0; j < gridSizeZ; j++) {
                    WaypointNode node = new WaypointNode(new Vector3(gridOffset.x + i * cellSize, gridOffset.y, gridOffset.z + j * cellSize));
                    grid[i, j] = node;
                    nodes.Add(node);
                }
            }

            for (int i = 0; i < gridSizeX; i++) {
                for (int j = 0; j < gridSizeZ; j++) {

                    CheckNeighbours(i, j);

                }
            }

            

            Debug.Log("Generation done! N: " + nodes.Count + ", E: " + edges.Count);

            return new WaypointGraph(nodes.ToArray(), edges.ToArray());

        }

        void CheckNeighbours(int x, int y) {
            for (int a = -range; a <= range; a++) {
                for (int b = -range; b <= range; b++) {
                    if (x + a >= 0 && y + b >= 0 && x + a < gridSizeX && y + b < gridSizeZ) {
                        WaypointNode current = grid[x, y];
                        WaypointNode target = grid[x + a, y + b];
                        float halfWidth = .4f;

                        Vector3 perp = Vector3.Cross(current.position - target.position, Vector3.up);
                        perp.Normalize();
                        if (!IsRaycastColliding(current.position + perp * halfWidth, target.position + perp * halfWidth)
                                && !IsRaycastColliding(current.position, target.position)
                                && !IsRaycastColliding(current.position - perp * halfWidth, target.position - perp * halfWidth)) {
                            edges.Add(new WaypointEdge(current, target));
                        }

                    }
                }
            }
        }

        bool IsRaycastColliding(Vector3 current, Vector3 target) {
            RaycastHit hit;
            
            Vector3 direction = target - current;
            if (Physics.Raycast(current, direction, out hit, 100.0F, ~(1 << 2))) {
                float distance = hit.distance;

                if (distance < direction.magnitude) {
                    return true;
                }
            }
            // The other way around to get around the problem of sending a raycast from inside a collider
            if (Physics.Raycast(target, -direction, out hit, 100.0F, ~(1 << 2))) {
                float distance = hit.distance;

                if (distance < direction.magnitude) {
                    return true;
                }
            }
            return false;
        }

        void OnDrawGizmos() {
            if (edges != null) {
                for (int i = 0; i < edges.Count; i++) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(edges[i].first.position, edges[i].second.position);

                }
            }
        }
    }

}