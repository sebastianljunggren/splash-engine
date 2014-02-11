using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();
    public FireCell fireCell;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    void Start() {
        FireEventManager.FireSpread += RespondToFire;

        //for (int i = 0; i < 100; i++) {
        //    FireCell foo = new FireCell();
        //    fireGrid.Add(foo);

        //    if (i == 50) {
        //        foo.startFire();
        //    }
        //}

        Vector3 size = renderer.bounds.size;

        for (int y = 0; y < 50; y++) {
            for (int x = 0; x < 50; x++) {
                Vector3 pos = new Vector3(x, 0, y);
                FireCell cell = (FireCell)Instantiate(fireCell, pos, Quaternion.identity);
                cell.AddParentReference(this);
                fireGrid.Add(cell);

                if (y == 2 && x == 2) {
                    cell.StartFire();
                }
            }
        }

        Vector3 foo = new Vector3(2, 1, 2);
        FireCell bar = (FireCell)Instantiate(fireCell, foo, Quaternion.identity);
        bar.AddParentReference(this);

        //InvokeRepeating("GridOutput", 0f, 2.5f);
        InvokeRepeating("UpdateSpread", 0f, 2.0f);
    }

	void Update () {

	}

    void UpdateSpread() {
        //FireEventManager.spreadFire();
        Debug.Log("UpdateSpread");
        if (OnFire != null) {
            OnFire();
        }
    }

    public void RespondToFire(Vector3 position, float radius, int damage) {
        if (true) { //Collide with object?
            if (fireGrid.Count == 0) {
                //for (int i = 0; i < 100; i++) {
                //    fireGrid.Add(new FireCell());
                //}
            }
            else {
                DamageCellAt(position, radius, damage);
            }
        }
    }

    private void DamageCellAt(Vector3 position, float radius, int damage) {
        //foreach (FireCell cell in fireGrid) {
        //    if(cell.HitBy(position, radius)) {
        //        cell.Damage(damage);
        //    }
        //}
    }

    private void GridOutput() {
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
