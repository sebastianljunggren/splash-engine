using UnityEngine;
using System.Collections;

public class FireCell : MonoBehaviour {
    private bool isBurning = false;
    private Flammable parent;
    private FireBehaviour fire;

    public FireBehaviour firePrefab;
    public bool active = true;

    private bool drawGizmos = false;
    private const int damage = 20;

    private int flammableHp;
    private int fireHp;

    void Start() {
        transform.localScale = new Vector3(parent.cellSize, parent.cellSize, parent.cellSize);
    }

    void Update() {
        //Debug.DrawRay(transform.position, Vector3.forward * 1, Color.green);
    }

    public void FireDamage() {
        if (active && !isBurning) {
            flammableHp -= 20;

            isBurning = flammableHp <= 0;

            if (isBurning) {
                StartFire();
            }
        }
    }

    public void WaterDamage() {
        if (active && isBurning) {
            fireHp -= 20;

            // Decrease fire intensity

            if (fireHp <= 0) {
                ExtinguishFire();
            }
        }
    }

    public void Instantiate(Flammable parent) {
        this.parent = parent;

        transform.parent = parent.transform;

        flammableHp = Flammable.FULL_FLAMMABLE_HP;
        fireHp = Flammable.FULL_FIRE_HP;
    }

    public void StartFire() {
        if (active) {
            flammableHp = 0;
            isBurning = true;

            if (!drawGizmos) {
                // Spawn fire at the current position
                //* Random.Range(0.8f, 1.2f)
                fire = (FireBehaviour)Instantiate(firePrefab, transform.position, Quaternion.identity);
                fire.transform.parent = transform;
            }

            parent.OnFire += Burning;
        }
    }

    public void ExtinguishFire() {
        if (active) {
            fireHp = 0;
            isBurning = false;
            flammableHp = Flammable.FULL_FLAMMABLE_HP;

            // Remove fire prefab

            parent.OnFire -= Burning;
        }
    }

    public void Burning() {
        if (active) {
            // Increase fire intensity

            Collider[] closeObjects = Physics.OverlapSphere(
                transform.position * Random.Range(0.6f, 1.3f),
                parent.radius * Random.Range(0.2f, 1.0f));

            foreach (Collider obj in closeObjects) {
                // Check if it collides with itself
                if (obj.collider != transform.collider && obj.collider != parent.collider) {
                    Flammable flammable = obj.GetComponent<Flammable>();
                    FireCell cell = obj.GetComponent<FireCell>();

                    if (cell != null) {
                        cell.FireDamage();
                    }
                    else if (flammable != null) {
                        flammable.RespondToFire();
                    }
                }
            }

            //Debug.Log("Burning");

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            //    Flammable flammable = hit.collider.gameObject.GetComponent<Flammable>();
            //    FireCell cell = hit.collider.gameObject.GetComponent<FireCell>();

            //    if (cell != null) {
            //        Debug.Log("Hit cell");
            //        cell.FireDamage();
            //    }
            //    else if (flammable != null) {
            //        Debug.Log("Hit flammable");
            //        flammable.RespondToFire();
            //    }
            //}
        }
    }

    void OnDrawGizmos() {
        if (isBurning) {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, 2f);
        }

        if (drawGizmos) {
            Gizmos.DrawWireCube(transform.position, new Vector3(parent.cellSize, parent.cellSize, parent.cellSize));
        }

        Gizmos.color = Color.white;
    }
}