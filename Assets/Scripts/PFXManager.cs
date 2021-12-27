using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFXManager : MonoBehaviour{
    public PFXPool hitPool;
    public PFXPool explosionPool;
    public PFXPool powerupPickupPool;

    private static PFXManager _manager;
    public static PFXManager Instance {
        get { return _manager; }
        private set {
            if (_manager == null) {
                _manager = value;
            }
        }
    }

    private void Awake() {
        Instance = this;
    }

    public void EmitHit(Vector2 position) {
        hitPool.Spawn(position);
    }

    public void EmitExplosion(Vector2 position) {
        explosionPool.Spawn(position);
    }

    public void EmitPowerupPickup(Vector2 position) {
        powerupPickupPool.Spawn(position);
    }
}
