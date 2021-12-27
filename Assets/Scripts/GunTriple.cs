using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriple : Gun{
    private static readonly int BULLETS_FIRED = 3;
    private static readonly int INIT_STEP = -BULLETS_FIRED / 2; //Left to right

    public float spreadAngle;
    public float bulletOffset;

    public override bool OnFire(Vector2 position) {
        bool shot = false;
        for (int i = INIT_STEP; i < BULLETS_FIRED - 1; i++) {
            Vector2 bulletPosition = position;
            bulletPosition.x += i * bulletOffset;

            Vector2 bulletDirection = Bullet.DEFAULT_DIRECTION;
            bulletDirection = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, i * -spreadAngle)) * bulletDirection;

            Bullet bullet = pool.Spawn(bulletPosition);
            if (bullet) {
                bullet.Fire(bulletDirection, speed, color);
                //Register shot even if only one bullet was fired
                shot |= true;
                continue;
            }
            shot |= false;
        }
        return shot;
    }
}