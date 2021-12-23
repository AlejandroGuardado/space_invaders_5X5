using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionData", menuName = "SpaceInvaders/SessionData")]
public class SessionData : ScriptableObject{
    public LevelData[] levels;
    [Header("Gameplay")]
    public float playerSpawnPosition;
    public float enemySpawnPosition;
    public float gameOverPosition;
    public float bonusPosition;
    public float bonusMultiplier;
    public float sessionOverDelay;
    [Header("Grid")]
    public float gridStartPosition;
    public float gridWidth;
    public float gridLineHeight;
    public float gridUnitWidth;
}