using UnityEngine;
using System.Collections;

public class FireSpread : MonoBehaviour {
    private FireGrid fireGrid;

    public delegate void FireEvent(Vector3 position, int radius, int damage);
    public static event FireEvent TakeDamage;

    public delegate void BurningEvent();
    public static event BurningEvent OnFire;

	void Start () {
        Vector3 size = renderer.bounds.size;
        fireGrid = new FireGrid(size.x, size.z, this.transform.position);

        fireGrid.getGrid()[3, 4].startFire();

        InvokeRepeating("gridOutput", 0f, 2.5f);
        InvokeRepeating("UpdateSpread", 0f, 2.0f);
	}

	void Update () {

	}

    void UpdateSpread() {
        if (OnFire != null) {
            OnFire();
        }
    }

    public static void fireAt(Vector3 position, int radius) {
        if (TakeDamage != null) {
            TakeDamage(position, radius, 2);
        }
    }

    private void gridOutput() {
        string foo = "";
        FireCell[,] grid = fireGrid.getGrid();

        for (int i = 0; i < fireGrid.y; i++) {
            for (int j = 0; j < fireGrid.x; j++) {
                foo += grid[i, j].hp + ", ";
            }
            Debug.Log(foo);
            foo = "";
        }

        Debug.Log("");
        Debug.Log("");
    }
}
