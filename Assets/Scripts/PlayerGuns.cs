using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuns : MonoBehaviour{
    public GunNormal normal;
    public PlayerGun currentWeapon;

    //Returns cooldown
    public bool Fire(Vector2 position, out float cooldown) {
        bool fired = false;
        switch (currentWeapon) {
            case PlayerGun.Normal:
                cooldown = normal.Cooldown;
                return normal.Fire(position);
            case PlayerGun.Triple:
                //break;
            case PlayerGun.Quick:
                //break;
            default:
                cooldown = 0f;
                break;
        }
        return fired;
    }

    public void Clear() {
        normal.Clear();
    }
}

public enum PlayerGun {
    Normal,
    Triple,
    Quick
}