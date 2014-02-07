using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();

	void Start () {
        FireEventManager.FireSpread += respondToFire;

        for (int i = 0; i < 100; i++) {
            FireCell foo = new FireCell();
            fireGrid.Add(foo);

            if (i == 50) {
                foo.startFire();
            }
        }

        InvokeRepeating("gridOutput", 0f, 2.5f);
        InvokeRepeating("UpdateSpread", 0f, 2.0f);
	}

	void Update () {

	}

    void UpdateSpread() {
        FireEventManager.spreadFire();
    }

    public void respondToFire(Vector3 position, int radius, int damage) {
        if (true) { //Collide with object?
            if (fireGrid.Count == 0) {
                //for (int i = 0; i < 100; i++) {
                //    fireGrid.Add(new FireCell());
                //}
            }
            else {
                damageCellAt(position, radius, damage);
            }
        }
    }

    private void damageCellAt(Vector3 position, int radius, int damage) {
        foreach (FireCell cell in fireGrid) {
            if(cell.hitBy(position, radius)) {
                cell.damage(damage);
            }
        }
    }

    private void gridOutput() {
        string foo = "";
        int i = 0;

        foreach (FireCell cell in fireGrid) {
            if (i < 10) {
                foo += cell.hp + ", ";
                i++;
            }
            else {
                Debug.Log(foo);
                foo = "";
            }
        }

        Debug.Log("");
        Debug.Log("");
    }
}
