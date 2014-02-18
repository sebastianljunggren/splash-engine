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

    public int flammableHp;
    public int fireHp;

    void Start() {
        transform.localScale = new Vector3(parent.cellSize, parent.cellSize, parent.cellSize);
    }

    void Update() {

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

            if (fireHp <= 0) {
                ExtinguishFire();
            }
        }
    }

    public void instantiate(Flammable parent) {
        this.parent = parent;

        flammableHp = this.parent.FULL_FLAMMABLE_HP;
        fireHp = this.parent.FULL_FIRE_HP;
    }

    public void StartFire() {
        if (active) {
            flammableHp = 0;
            isBurning = true;

            if (!drawGizmos) {
                // Spawn fire at the current position
                fire = (FireBehaviour)Instantiate(firePrefab, transform.position, Quaternion.identity);
            }

            parent.OnFire += Burning;
        }
    }

    public void ExtinguishFire() {
        if (active) {
            fireHp = 0;
            isBurning = false;
            flammableHp = parent.FULL_FLAMMABLE_HP;

            // Decrease fire intensity

            parent.OnFire -= Burning;
        }
    }

    public void Burning() {
        if (active) {
            //fire.IncreaseIntensity();

            Collider[] closeObjects = Physics.OverlapSphere(
                transform.position * Random.Range(0.6f, 1.3f),
                parent.radius * Random.Range(0.2f, 1.3f));

            foreach (Collider obj in closeObjects) {
                // Check if it collides with itself
                if (obj.collider != transform.collider) {
                    Flammable flammable = obj.GetComponent<Flammable>();
                    FireCell cell = obj.GetComponent<FireCell>();

                    if (cell != null) {
                        cell.FireDamage();
                    }
                    else if (flammable != null) {
                        flammable.RespondToFire(transform.position, parent.radius, 20);
                    }
                }
            }
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