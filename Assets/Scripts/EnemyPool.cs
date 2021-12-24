using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour{
    public List<Enemy> greenEnemies;
    public List<Enemy> blueEnemies;
    public List<Enemy> redEnemies;

    private Pool<Enemy> greenPool;
    private Pool<Enemy> bluePool;
    private Pool<Enemy> redPool;

    private void Awake() {
        greenPool = new Pool<Enemy>(greenEnemies);
        bluePool = new Pool<Enemy>(blueEnemies);
        redPool = new Pool<Enemy>(redEnemies);
    }

    public Enemy Spawn(EnemyType type, Vector2 position) {
        switch (type) {            
            case EnemyType.Green:
                return greenPool.Spawn(position);
            case EnemyType.Blue:
                return bluePool.Spawn(position);
            case EnemyType.Red:
                return redPool.Spawn(position);
            case EnemyType.None:
            default:
                return null;
        }
    }

    public void Clear() {
        greenPool.Clear();
        bluePool.Clear();
        redPool.Clear();
    }
}