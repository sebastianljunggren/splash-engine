﻿using UnityEngine;
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
                Debug.Log("New cell on fire");
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

        foreach (Collider obj in closeObjects) {
            if (obj.collider != transform.collider) {
                obj.GetComponent<FireCell>().Damage(20);
            }
        }

        //FireEventManager.FireAt(transform.position, radius);
    }

    void OnDrawGizmos() {
        if (isBurning) {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, 1.1f);
        }

        if (hp < 50 && hp != 0) {
            //Gizmos.color = Color.green;
        }

        //Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
        Gizmos.color = Color.white;
    }

    void OnDrawGizmosSelected() {
        if (isBurning) {
            //Burning();
        }
    }
}