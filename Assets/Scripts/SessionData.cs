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
    public int bonusMultiplier;
    public float bonusTextShowTime;
    public float sessionStartDelay;
    public float sessionVictoryDelay;
    public float sessionDefeatDelay;
    public float bulletKillzonePosition;
    public PlayerGun defaultGun;
    public float playerFireShockwaveTime;
    [Header("Grid")]
    public float gridStartPosition;
    public float gridLineHeight;
    public float gridWidth;
}