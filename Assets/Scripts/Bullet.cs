using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameEntity{
    public static readonly Vector2 DEFAULT_DIRECTION = Vector2.up;
    public static readonly float DEFAULT_ROTATION;

    public SessionData sessionData;
    public BoxCollider2D box;
    public SpriteRenderer glow;
    public float speed;
    private Vector2 direction;

    static Bullet() {
        DEFAULT_ROTATION = Mathf.Atan2(DEFAULT_DIRECTION.y, DEFAULT_DIRECTION.x);
    }

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
        speed = 0f;
        glow.color = Color.white;
        transform.rotation = Quaternion.identity;
        base.Deactivate();
    }

    public void Fire(Vector2 direction, float speed, Color color) {
        this.direction = direction;

        //Rotation
        float directionRotation = Mathf.Atan2(direction.y, direction.x);
        directionRotation -= DEFAULT_ROTATION;
        directionRotation *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation);

        this.speed = speed;
        glow.color = new Color(color.r, color.g, color.b, glow.color.a);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Deactivate();
    }
}