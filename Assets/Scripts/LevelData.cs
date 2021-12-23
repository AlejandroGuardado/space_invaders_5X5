using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SpaceInvaders/LevelData")]
public class LevelData : ScriptableObject {
    [Header("Layout")]
    public LevelLine[] lines;
    public int repeat;    
    [Header("Enemies")]
    public float speed;
    public int redEnemiesNeeded;
    public int blueEnemiesNeeded;
}