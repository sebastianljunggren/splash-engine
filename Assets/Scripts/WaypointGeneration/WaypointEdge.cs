using UnityEngine;
using System.Collections;

namespace WaypointGeneration {

    public class WaypointEdge {
        public WaypointNode first;
        public WaypointNode second;

        private float cost;
        private int firstIndex, secondIndex;
        private static float accuracy = 0.0001f;

        public WaypointEdge(WaypointNode f, WaypointNode s) {
            first = f;
            second = s;
        }

        public void Calculate(WaypointNode[] nodes) {
            cost = (second.position - first.position).magnitude;

            for (int i = 0; i < nodes.Length; i++) {
                float dist = Vector3.Distance(first.position, nodes[i].position);
                if (dist < accuracy) {
                    firstIndex = i;
                }
                dist = Vector3.Distance(second.position, nodes[i].position);
                if (dist < accuracy) {
                    secondIndex = i;
                }
            }
        }

        public float Cost {
            get { return cost; }
        }

        public int FirstIndex {
            get { return firstIndex; }
        }

        public int SecondIndex {
            get { return secondIndex; }
        }
    }

}