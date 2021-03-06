using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunNormal : Gun {
    public override bool OnFire(Vector2 position) {
        Bullet bullet = pool.Spawn(position);
        if (bullet) {
            bullet.Fire(Bullet.DEFAULT_DIRECTION, speed, color);
            return true;
        }
        return false;
    }
}