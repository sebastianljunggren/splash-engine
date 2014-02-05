using UnityEngine;
using System.Collections;

public class FireSpread : MonoBehaviour {
    private FireGrid fireGrid;
    //public GameObject test;

	void Start () {
        Vector3 size = renderer.bounds.size;
        fireGrid = new FireGrid(size.x, size.z, this.transform.position);

        fireGrid.getGrid()[3, 4].damage(50); //Start a fire
        InvokeRepeating("gridOutput", 0f, 2.5f);
        InvokeRepeating("testUpdate", 0f, 2.0f);

        //GameObject.Instantiate(test, this.fireGrid.position, this.transform.rotation);
	}

	void Update () {

	}

    void testUpdate() {
        FireCell[,] grid = fireGrid.getGrid();

        for (int i = 0; i < fireGrid.y; i++) {
            for (int j = 0; j < fireGrid.x; j++) {
                if (grid[i, j].isBurning()) {
                    if (i - 1 >= 0) {
                        grid[i - 1, j].damage(2);
                    }

                    if (i + 1 < fireGrid.y) {
                        grid[i + 1, j].damage(2);
                    }

                    if (j - 1 >= 0) {
                        grid[i, j - 1].damage(2);
                    }

                    if (j + 1 < fireGrid.x) {
                        grid[i, j + 1].damage(2);
                    }
                }
            }
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
