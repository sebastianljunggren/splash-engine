using UnityEngine;
using System.Collections;

public class FireEventManager {
    //public delegate void FireSpreadEvent(Vector3 position, float radius, int damage);
    //public static event FireSpreadEvent FireSpread;

    public delegate void SpawnFireEvent(Vector3 position);
    public static event SpawnFireEvent SpawnFire;

    public FireEventManager() {

    }

    //public static void FireAt(Vector3 position, float radius) {
    //    if (FireSpread != null) {
    //        FireSpread(position, radius, 2);
    //    }
    //}

    public static void SpawnFireAt(Vector3 position) {
        if (SpawnFire != null) {
            SpawnFire(position);
        }
    }
}