using UnityEngine;
using System.Collections;

namespace WaypointGeneration {

    public class WaypointGraph {
        private WaypointNode[] nodes;
        private WaypointEdge[] edges;

        public WaypointGraph(WaypointNode[] nodes, WaypointEdge[] edges) {

            this.nodes = nodes;
            this.edges = edges;
        }

        public WaypointNode[] Nodes {
            get { return nodes; }
        }

        public WaypointEdge[] Edges {
            get { return edges; }
        }
    }
}