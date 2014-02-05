using UnityEngine;
using System.Collections;

public class FireCell {
    public Vector3 position;
    public int radius;

    private bool active;

    private bool isBurning = false;
    public int hp = 50;

    public FireCell(bool active = true) {
        this.active = active;

        FireSpread.TakeDamage += damaged;
    }

    public void damaged(Vector3 position, int radius, int damage) {
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

        FireSpread.OnFire += burning;
        FireSpread.TakeDamage -= damaged;
    }

    public void burning() {
        FireSpread.fireAt(position, radius);
    }
}