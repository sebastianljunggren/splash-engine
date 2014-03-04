using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();
    public FireCell fireCell;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public int fullFlammableHp = 50;
    public int fullFireHp = 80;

    public float cellSize = 0.3f;
    public float radius = 0.6f;

    public bool ignite = false;

    void Start() {
        InvokeRepeating("UpdateSpread", 0f, 1.0f);

        // Control if the object should ignite
        if (ignite) {
            GenerateGrid();

            // Start a fire at a random position
            fireGrid[Random.Range(0, fireGrid.Count - 1)].StartFire();
            fireGrid[Random.Range(0, fireGrid.Count - 1)].StartFire();
            fireGrid[Random.Range(0, fireGrid.Count - 1)].StartFire();
        }
    }

	void Update () {

	}

    void UpdateSpread() {
        // Trigger event to all burning cell
        if (OnFire != null) {
            OnFire();
        }
    }

    public void RespondToFire() {
        if (fireGrid.Count == 0) {
            GenerateGrid();
        }
    }

    private void GenerateGrid() {
        Bounds meshBounds = transform.collider.bounds;
        Vector3 min = meshBounds.min;
        Vector3 max = meshBounds.max;

        for (float x = min.x; x <= max.x; x = x + cellSize) {
            for (float y = min.y; y <= max.y; y = y + cellSize) {
                for (float z = min.z; z <= max.z; z = z + cellSize) {
                    Vector3 pos = new Vector3(x, y, z);

                    if (positionInsideThis(pos)) {
                        FireCell cell = (FireCell) Instantiate(fireCell, pos, transform.rotation);
                        cell.Instantiate(this);
                        fireGrid.Add(cell);
                    }
                }
            }
        }
    }

    private bool positionInsideThis(Vector3 pos) {
        Collider[] closeObjects = Physics.OverlapSphere(pos, cellSize / 2);

        foreach (Collider obj in closeObjects) {
            if (obj.collider == transform.collider) {
                return true;
            }
        }

        return false;
    }
}
