using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameEntity {
    public BoxCollider2D box;
    public GameEntityDissolver dissolver;
    [Range(1,10)]
    [SerializeField]
    private int health;
    [Range(1, 10)]
    [SerializeField]
    private int points;
    public bool IsAlive { get { return currentHealth > 0; } }
    public int Points { get { return points; } }

    private int currentHealth;

    public override void Spawn(Vector2 _position) {
        currentHealth = health;
        box.enabled = true;
        base.Spawn(_position);
    }

    public override void Deactivate() {
        currentHealth = 0;
        box.enabled = false;
        base.Deactivate();
        dissolver.Clear();
    }

    private void Hit() {
        //PFX Hit
    }

    public void Kill() {
        //PFX Explosion
        StartCoroutine(OnKill());
    }

    private IEnumerator OnKill() {
        box.enabled = false;
        dissolver.Dissolve();
        yield return new WaitForSeconds(dissolver.dissolveTime);
        Deactivate();
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