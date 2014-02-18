using UnityEngine;
using System.Collections;

public class FireGrid {
    private FireCell[,] grid;

    private int cellSize = 1;
    public int x;
    public int y;

    public FireGrid(float x, float y, Vector3 position){
        this.x = (int) x / cellSize;
        this.y = (int) y / cellSize;

        grid = new FireCell[this.x, this.y];

        //TODO Use jagged array? http://stackoverflow.com/questions/7154592/initializing-two-dimensional-array-of-objects
        for (int i = 0; i < this.y; i++) {
            for (int j = 0; j < this.x; j++) {
                grid[i, j] = new FireCell();
            }
        }
    }

    public FireCell[,] getGrid() {
        return grid;
    }
}
