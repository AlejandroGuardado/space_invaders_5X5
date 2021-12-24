using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour{
    public WeaponNormal normal;
    public PlayerWeapon currentWeapon;

    //Returns cooldown
    public bool Fire(Vector2 position, out float cooldown) {
        bool fired = false;
        switch (currentWeapon) {
            case PlayerWeapon.Normal:
                cooldown = normal.Cooldown;
                return normal.Fire(position);
            case PlayerWeapon.Triple:
                //break;
            case PlayerWeapon.Quick:
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

public enum PlayerWeapon {
    Normal,
    Triple,
    Quick
}