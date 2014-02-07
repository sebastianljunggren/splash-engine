using UnityEngine;
using System.Collections;

public class FireCell {
    public Vector3 position;
    public int radius;

    private bool active = true;

    private bool isBurning = false;
    public int hp = 50;

    public FireCell() {

    }

    public bool hitBy(Vector3 position, int radius) {
        return true;
    }

    public void damage(int damage) {
        if (!isBurning) {
            hp -= damage;

            isBurning = hp <= 0;

            if (isBurning) {
                startFire();
            }
        }
    }

    public void startFire() {
        hp = 0;
        isBurning = true;

        FireEventManager.OnFire += burning;
    }

    public void burning() {
        FireEventManager.fireAt(position, radius);
    }
}