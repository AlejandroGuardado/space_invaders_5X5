using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameEntity {
    public GameEntityDissolver dissolver;
    [Range(1,10)]
    [SerializeField]
    private int health;
    [Range(1, 10)]
    [SerializeField]
    private int points;
    public bool IsAlive { get { return currentHealth > 0; } }
    
    private int currentHealth;

    private void Awake() {
        Deactivate();
    }

    public override void Spawn(Vector2 _position) {
        currentHealth = health;
        base.Spawn(_position);
    }

    public override void Deactivate() {
        currentHealth = 0;
        base.Deactivate();
    }

    private void Hit() {
        //PFX Hit
    }

    private void Kill() {
        //PFX Explosion
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (IsAlive) {
            currentHealth--;
            if (!IsAlive) {
                Kill();
            }
            else {
                Hit();
            }
        }
    }
}

public enum EnemyType {
    None,
    Green,
    Blue,
    Red
}