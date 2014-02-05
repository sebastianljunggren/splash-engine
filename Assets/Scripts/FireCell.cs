using UnityEngine;
using System.Collections;

public class FireCell {
    private Vector3 position;
    private int radius;
    private bool active;

    public int hp = 50;  

    public FireCell(bool active = true) {
        this.active = active;
    }

    public bool isBurning() {
        return hp <= 0;
    }

    public void damage(int damage) {
        if (hp > 0) {
            hp -= damage;
        }
    }
}