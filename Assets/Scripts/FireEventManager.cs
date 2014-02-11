using UnityEngine;
using System.Collections;

public class FireEventManager {
    //public delegate void OnFireEvent();
    //public static event OnFireEvent OnFire;

    public delegate void FireSpreadEvent(Vector3 position, float radius, int damage);
    public static event FireSpreadEvent FireSpread;

    public FireEventManager() {

    }

    public static void FireAt(Vector3 position, float radius) {
        if (FireSpread != null) {
            FireSpread(position, radius, 2);
        }
    }

    //public static void spreadFire() {
    //    if (OnFire != null) {
    //        OnFire();
    //    }
    //}
}
