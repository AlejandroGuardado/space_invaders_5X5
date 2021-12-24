using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNormal : Weapon {
    public override bool OnFire(Vector2 position) {
        Bullet bullet = pool.Spawn(position);
        if (bullet) {
            bullet.Fire(Vector2.up, speed, color);
            return true;
        }
        return false;
    }
}