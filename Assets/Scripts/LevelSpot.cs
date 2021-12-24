using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpot{
    public EnemyType EnemyType { get; set; }
    public LevelSpotStatus Status { get; set; }
    public Enemy enemy { get; set; }
    public Vector2 position;

    public LevelSpot() {
        EnemyType = EnemyType.None;
        Status = LevelSpotStatus.Standby;
        enemy = null;
    }
}

/// <summary>
/// Standby = Enemy has not spawned yet
/// Active = Spot needs updating
/// Off = Enemy was killed; no longer needs updating
/// </summary>
public enum LevelSpotStatus {
    Standby,
    Active,
    Off
}