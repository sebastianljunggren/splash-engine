﻿using UnityEngine;
using System.Collections;
using System.IO;

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

    public bool IsBurning {
        get { return isBurning; }
    }

    void Start() {
        transform.localScale = new Vector3(parent.cellSize, parent.cellSize, parent.cellSize);
    }

    void Update() {

    }

    public void Instantiate(Flammable parent) {
        this.parent = parent;

        // Follow the parent
        transform.parent = parent.transform;

        // Set the hp to default values
        flammableHp = parent.fullFlammableHp;
        fireHp = parent.fullFireHp;
    }

    public void FireDamage() {
        if (active && !isBurning) {
            flammableHp -= damage;

            isBurning = flammableHp <= 0;

            if (isBurning) {
                StartFire();
            }
        }
    }

    public void WaterDamage() {
        if (active && isBurning) {
            fireHp -= damage;

            if (fireHp <= 0) {
                ExtinguishFire();
            }
        }
    }

    public void StartFire() {
        if (active) {
            // Assure it is burning
            flammableHp = 0;
            isBurning = true;

            if (!drawGizmos) {
                // Spawn fire at the current position
                // * Random.Range(0.95f, 1.05f)
                fire = (FireBehaviour)Instantiate(firePrefab, transform.position, Quaternion.identity);
                fire.transform.parent = transform;
            }

            // Add event method
            parent.OnFire += Burning;

            // Set new layer
            this.gameObject.layer = 0;
        }
    }

    public void ExtinguishFire() {
        if (active) {
            // Assure it's extinguished
            fireHp = 0;
            isBurning = false;
            flammableHp = parent.fullFlammableHp;

            // Deactivate cell so it cannot ignite again
            active = false;

            fire.active = false;

            // Remove event method
            parent.OnFire -= Burning;
        }
    }

    public void Burning() {
        if (active) {
            // Get all surrounding objects
            Collider[] closeObjects = Physics.OverlapSphere(
                transform.position * Random.Range(0.6f, 1.3f),
                parent.radius * Random.Range(0.2f, 1.0f));

            foreach (Collider obj in closeObjects) {
                // Check if it collides with itself
                if (obj.collider != transform.collider && obj.collider != parent.collider) {
                    Flammable flammable = obj.GetComponent<Flammable>();
                    FireCell cell = obj.GetComponent<FireCell>();

                    // Is it a FireCell?
                    if (cell != null) {
                        cell.FireDamage();
                    }
                    // Is it a Flammable?
                    else if (flammable != null) {
                        flammable.RespondToFire();
                    }
                }
            }
        }
    }

    public void Water() {
        if (active) {
            WaterDamage();

            // Get all surrounding objects
            Collider[] closeObjects = Physics.OverlapSphere(transform.position, parent.radius);

            foreach (Collider obj in closeObjects) {
                // Check if it collides with itself
                if (obj.collider != transform.collider && obj.collider != parent.collider) {
                    FireCell cell = obj.GetComponent<FireCell>();

                    if (cell != null) {
                        cell.WaterDamage();
                    }
                }
            }
        }
    }

    void OnDrawGizmos() {
        if (isBurning) {
            Gizmos.color = Color.red;
        }

        if (drawGizmos) {
            Gizmos.DrawWireCube(transform.position, new Vector3(parent.cellSize, parent.cellSize, parent.cellSize));
            Gizmos.color = Color.white;
        }
    }
}