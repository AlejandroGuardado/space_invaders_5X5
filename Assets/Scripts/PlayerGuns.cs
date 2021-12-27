using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuns : MonoBehaviour{
    public GunNormal normal;
    public GunNormal rapid;
    public GunTriple triple;
    [SerializeField]
    private PlayerGun currentGun;

    /// <summary>
    /// Fire currently selected gun. Returns if it was able to fire.
    /// </summary>
    /// <param name="position">Muzzle position</param>
    /// <param name="cooldown">How long it takes to fire again</param>
    /// <returns></returns>
    public bool Fire(Vector2 position, out float cooldown) {
        bool fired = false;
        switch (currentGun) {
            case PlayerGun.Normal:
                cooldown = normal.Cooldown;
                return normal.Fire(position);
            case PlayerGun.Rapid:
                cooldown = rapid.Cooldown;
                return rapid.Fire(position);
            case PlayerGun.Triple:
                cooldown = triple.Cooldown;
                return triple.Fire(position);
            default:
                cooldown = 0f;
                break;
        }
        return fired;
    }

    public void SetGun(PlayerGun gun) {
        currentGun = gun;
    }

    public void Clear() {
        normal.Clear();
        rapid.Clear();
        triple.Clear();
    }
}

public enum PlayerGun {
    Normal,
    Rapid,
    Triple
}