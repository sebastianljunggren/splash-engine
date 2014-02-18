using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flammable : MonoBehaviour {
    private List<FireCell> fireGrid = new List<FireCell>();
    public FireCell fireCell;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public float cellSize = 0.1f;
    public bool grid2D = true;

    // Temp
    public Bounds meshBounds;
    //public Bounds boxBounds;

    void Start() {
        FireEventManager.FireSpread += RespondToFire;

        //boxBounds = GetComponent<BoxCollider>().bounds;
        //Vector3 min = boxBounds.min;
        //Vector3 max = boxBounds.max;

        //meshBounds = GetComponent<MeshCollider>().bounds;
        meshBounds = GetComponent<BoxCollider>().bounds;
        Vector3 min = meshBounds.min;
        Vector3 max = meshBounds.max;

        Debug.Log(min);
        Debug.Log(max);

        int i = 0;

        //Vector3 pos = new Vector3(6.18f, 1.7f, -0.5f);
        //FireCell cell = (FireCell)Instantiate(fireCell, pos, Quaternion.identity);
        //cell.AddParentReference(this);
        //fireGrid.Add(cell);

        for (float x = min.x; x <= max.x; x = x + cellSize) {
            for (float y = min.y; y <= max.y; y = y + cellSize) {
                for (float z = min.z; z <= max.z; z = z + cellSize) {

                    Vector3 pos = new Vector3(x, y, z);
                    i++;

                    //if (MeshContains(cell)) {
                    if (meshBounds.Contains(pos)) {
                        //if (meshBounds.Intersects(cell.collider.bounds)) {
                        FireCell cell = (FireCell)Instantiate(fireCell, pos, Quaternion.identity);
                        cell.AddParentReference(this);
                        fireGrid.Add(cell);
                    }
                    //else {
                    //    Destroy(cell);
                    //}
                }
            }
        }

        int a = 0;

        foreach (FireCell cell in fireGrid) {
            //Debug.Log(meshBounds.Contains(cell.transform.position));

            if(meshBounds.Contains(cell.transform.position)){
                a++;
                //fireGrid.Remove(cell);
            }
        }

        Debug.Log("Number of created cells " + i);
        Debug.Log("Number of existing cells " + fireGrid.Count);
        Debug.Log("Number of containing cells " + a);

        //InvokeRepeating("UpdateSpread", 0f, 2.0f);
    }

    //public bool MeshContains(FireCell cell) {
    //    Mesh mesh = GetComponent<MeshFilter>().mesh;

    //    foreach (Vector3 vertex in mesh.vertices) {
    //        if(cell.collider.bounds.Contains(vertex)) {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

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
}
