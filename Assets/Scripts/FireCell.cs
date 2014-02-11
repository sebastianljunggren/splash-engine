using UnityEngine;
using System.Collections;

public class FireCell : MonoBehaviour {
    public float radius = 0.5f;
    private bool active = true;
    public bool isBurning = false;
    public int hp = 50;
    Flammable parent;

    void Start() {

    }

    void Update() {

    }

    public void Damage(int damage) {
        if (!isBurning) {
            hp -= damage;

            isBurning = hp <= 0;

            if (isBurning) {
                StartFire();
            }
        }
    }

    public void AddParentReference(Flammable parent) {
        this.parent = parent;
    }

    public void StartFire() {
        hp = 0;
        isBurning = true;

        parent.OnFire += Burning;
    }

    public void Burning() {
        Collider[] closeObjects = Physics.OverlapSphere(transform.position, 1.1f);

        Debug.Log(closeObjects.Length);

        foreach (Collider obj in closeObjects) {
            obj.GetComponent<FireCell>().Damage(10);
        }

        //FireEventManager.FireAt(transform.position, radius);
    }

    public void RedrawGizmos() {
        //OnDrawGizmos();
    }

    void OnDrawGizmos() {
        if (isBurning) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1.1f);
        }

        if (hp < 50 && hp != 0) {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
        Gizmos.color = Color.white;
    }

    private bool foo = false;

    void OnDrawGizmosSelected() {
        if (isBurning && !foo) {
            Burning();
            foo = true;
        }

        //Debug.Log(transform.position);
    }
}