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
            //Calculate();
        }

        //public void Calculate(WaypointNode[] nodes) {
        //    cost = (second.transform.position - first.transform.position).magnitude;

        //    for (int i = 0; i < nodes.Length; i++) {
        //        float dist = Vector3.Distance(first.transform.position, nodes[i].transform.position);
        //        if (dist < accuracy) {
        //            firstIndex = i;
        //        }
        //        dist = Vector3.Distance(second.transform.position, nodes[i].transform.position);
        //        if (dist < accuracy) {
        //            secondIndex = i;
        //        }
        //    }
        //    Debug.DrawLine(first.transform.position, second.transform.position, Color.red, 1000);

        //}

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