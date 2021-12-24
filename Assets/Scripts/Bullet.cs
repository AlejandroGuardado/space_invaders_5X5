using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameEntity{
    public SessionData sessionData;
    public BoxCollider2D box;
    public SpriteRenderer glow;
    public float speed;
    private Vector2 direction;

    void Update() {
        if (Active) {
            transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
            if(transform.position.y > sessionData.bulletKillzonePosition) {
                Deactivate();
            }
        }    
    }

    public override void Activate() {
        box.enabled = true;
        glow.enabled = true;
        base.Activate();
    }

    public override void Deactivate() {
        box.enabled = false;
        glow.enabled = false;
        base.Deactivate();
    }

    public void Fire(Vector2 direction, float speed, Color color) {
        this.direction = direction;
        this.speed = speed;
        glow.color = new Color(color.r, color.g, color.b, glow.color.a);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Deactivate();
    }
}