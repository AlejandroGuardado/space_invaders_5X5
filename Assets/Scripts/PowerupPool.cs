using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPool : MonoBehaviour{
    public List<Powerup> rapidPowerups;
    public List<Powerup> triplePowerups;
    public int rapidChance;
    public int tripleChance;

    public Pool<Powerup> rapidPool;
    public Pool<Powerup> triplePool;

    public event Action<PowerupEventArgs> OnPickup;

    private void Awake() {
        rapidPool = new Pool<Powerup>(rapidPowerups);
        triplePool = new Pool<Powerup>(triplePowerups);
        Clear();
    }

    public void Init() {
        RegisterPickups(rapidPool);
        RegisterPickups(triplePool);
    }

    public void Spawn(Vector2 position) {
        if(ShouldSpawnPowerup(rapidChance)) {
            rapidPool.Spawn(position);
        }
        else if(ShouldSpawnPowerup(tripleChance)) {
            triplePool.Spawn(position);
        }
    }

    public void Clear() {
        UnregisterPickups(rapidPool);
        UnregisterPickups(triplePool);
        rapidPool.Clear();
        triplePool.Clear();
    }

    private void RegisterPickups(Pool<Powerup> pool) {
        for (int i = 0; i < pool.entities.Count; i++) {
            Powerup powerup = pool.entities[i];
            powerup.OnPickup += Powerup_OnPickup;
        }
    }

    private void UnregisterPickups(Pool<Powerup> pool) {
        for (int i = 0; i < pool.entities.Count; i++) {
            Powerup powerup = pool.entities[i];
            powerup.OnPickup -= Powerup_OnPickup;
        }
    }

    private void Powerup_OnPickup(PowerupEventArgs args) {
        OnPickup?.Invoke(args);
    }

    private bool ShouldSpawnPowerup(int chance) {
        return UnityEngine.Random.Range(1, chance) == 1;
    }
}