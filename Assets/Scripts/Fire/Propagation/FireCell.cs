using UnityEngine;
using System.Collections;

public class FireCell : MonoBehaviour {
    private float radius = 0.6f;
    public bool active = true;
    private bool isBurning = false;
    
    Flammable parent;
    private FireBehaviour fire;

    public FireBehaviour firePrefab;
    public int hp = 50;

    void Start() {
        transform.localScale = new Vector3(parent.cellSize, parent.cellSize, parent.cellSize);
    }

    void Update() {

    }

    public void Damage(int damage) {
        if (!isBurning && active) {
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
        if (active) {
            hp = 0;
            isBurning = true;

            // Spawn fire at the current position
            fire = (FireBehaviour)Instantiate(firePrefab, transform.position, Quaternion.identity);

            parent.OnFire += Burning;
        }
    }

    public void Burning() {
        if (active) {
            //fire.IncreaseIntensity();

            Collider[] closeObjects = Physics.OverlapSphere(transform.position * Random.Range(0.6f, 1.3f), radius * Random.Range(0.2f, 1.3f));

            foreach (Collider obj in closeObjects) {
                // Check if it collides with itself
                if (obj.collider != transform.collider) {
                    Flammable flammable = obj.GetComponent<Flammable>();
                    FireCell cell = obj.GetComponent<FireCell>();

                    if (cell != null) {
                        cell.Damage(20);
                    }
                    else if (flammable != null) {
                        flammable.RespondToFire(transform.position, radius, 20);
                    }
                }
            }
        }
    }

    void OnDrawGizmos() {
        if (active) {
            if (isBurning) {
                Gizmos.color = Color.red;
                //Gizmos.DrawWireSphere(transform.position, 2f);
            }

            if (hp < 50 && hp != 0) {
                //Gizmos.color = Color.green;
            }

            //Gizmos.DrawWireCube(transform.position, new Vector3(parent.cellSize, parent.cellSize, parent.cellSize));
            Gizmos.color = Color.white;
        }
    }

    void OnDrawGizmosSelected() {

    }
}