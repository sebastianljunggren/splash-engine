using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();
    public FireCell fireCell;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public float cellSize = 0.5f;
    public bool grid2D = true;

    void Start() {
        //FireEventManager.FireSpread += RespondToFire;

        Bounds meshBounds = GetComponent<MeshCollider>().bounds;
        Vector3 min = meshBounds.min;
        Vector3 max = meshBounds.max;

        int i = 0;

        //for (int y = 0; y < 50; y++) {
        //    for (int x = 0; x < 50; x++) {
        //        Vector3 pos = new Vector3(x, 0, y);
        //        FireCell cell = (FireCell)Instantiate(fireCell, pos, Quaternion.identity);
        //        cell.AddParentReference(this);
        //        fireGrid.Add(cell);

        //        if (y == 2 && x == 2) {
        //            cell.StartFire();
        //        }
        //    }
        //}

        for (float x = min.x; x <= max.x; x = x + cellSize) {
            for (float y = min.y; y <= max.y; y = y + cellSize) {
                for (float z = min.z; z <= max.z; z = z + cellSize) {
                    Vector3 pos = new Vector3(x, y, z);
                    i++;

                    if (meshBounds.Contains(pos)) {
                        FireCell cell = (FireCell)Instantiate(fireCell, pos, Quaternion.identity);
                        cell.AddParentReference(this);
                        fireGrid.Add(cell);

                        if (i % 2 == 0) {
                            cell.active = false;
                        }

                        if (i == 691) {
                            cell.StartFire();
                        }
                    }
                }
            }
        }

        InvokeRepeating("UpdateSpread", 0f, 1.0f);
    }

	void Update () {

	}

    void UpdateSpread() {
        if (OnFire != null) {
            OnFire();
        }
    }

    public void RespondToFire(Vector3 position, float radius, int damage) {
        if (fireGrid.Count == 0) {
            // Create cell and find closest cell
        }
    }

    private void DamageCellAt(Vector3 position, float radius, int damage) {
        //foreach (FireCell cell in fireGrid) {
        //    if(cell.HitBy(position, radius)) {
        //        cell.Damage(damage);
        //    }
        //}
    }
}
