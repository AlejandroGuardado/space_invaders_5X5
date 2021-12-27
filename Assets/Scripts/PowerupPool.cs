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

    private List<PowerupType> spawned;

    private void Awake() {
        rapidPool = new Pool<Powerup>(rapidPowerups);
        triplePool = new Pool<Powerup>(triplePowerups);
        spawned = new List<PowerupType>(2);
        Clear();
    }

    public void Init() {
        RegisterPickups(rapidPool);
        RegisterPickups(triplePool);
    }

    public void Spawn(Vector2 position) {
        spawned.Clear();
        if (ShouldSpawnPowerup(rapidChance)) {
            spawned.Add(PowerupType.Rapid);
        }
        if(ShouldSpawnPowerup(tripleChance)) {
            spawned.Add(PowerupType.Triple);
        }

        if(spawned.Count == 1) {
            Spawn(spawned[0], position);
        }
        //If more than one type of powerup spawned, select one randomly
        else if (spawned.Count > 1) {
            Spawn(spawned[UnityEngine.Random.Range(0, spawned.Count)], position);
        }
    }

    private void Spawn(PowerupType type, Vector2 position) {
        switch (type) {
            case PowerupType.Rapid:
                rapidPool.Spawn(position);
                break;
            case PowerupType.Triple:
                triplePool.Spawn(position);
                break;
            default:
                break;
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
        return UnityEngine.Random.Range(1, chance + 1) == 1;
    }

    private enum PowerupType {
        Rapid,
        Triple
    }
}