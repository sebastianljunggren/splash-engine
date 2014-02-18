using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();
    public FireCell fireCell;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public int FULL_FLAMMABLE_HP = 50;
    public int FULL_FIRE_HP = 80;

    public float cellSize = 0.3f;
    public float radius = 0.6f;
    public bool grid2D = true;

    void Start() {
        Bounds meshBounds = GetComponent<MeshCollider>().bounds;
        Vector3 min = meshBounds.min;
        Vector3 max = meshBounds.max;

        int i = 0;

        for (float x = min.x; x <= max.x; x = x + cellSize) {
            for (float y = min.y; y <= max.y; y = y + cellSize) {
                for (float z = min.z; z <= max.z; z = z + cellSize) {
                    Vector3 pos = new Vector3(x, y, z);
                    i++;

                    //if (meshBounds.Contains(pos)) {
                    if (positionInsideThis(pos)) {
                        FireCell cell = (FireCell)Instantiate(fireCell, pos, transform.rotation);
                        cell.instantiate(this);
                        fireGrid.Add(cell);

                        if (i % 2 == 0) {
                            cell.active = false;
                        }

                        if (i == 251) {
                            cell.StartFire();
                        }
                    }
                }
            }
        }

        InvokeRepeating("UpdateSpread", 0f, 1.0f);
    }

    private bool positionInsideThis(Vector3 pos) {
        Collider[] closeObjects = Physics.OverlapSphere(pos, cellSize/2);

        foreach (Collider obj in closeObjects) {
            if (obj.collider == transform.collider) {
                return true;
            }
        }

        return false;
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
}
