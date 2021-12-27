using System;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : GameEntity{
    public BoxCollider2D box;
    public PlayerGun gun;
    public string name;
    public float duration;
    public float speed;
    public Color color;
    public SessionData sessionData;
    private float powerupKillzone;

    public event Action<PowerupEventArgs> OnPickup;

    void Awake() {
        powerupKillzone = sessionData.powerupKillzone;
    }

    public override void Activate() {
        box.enabled = true;
        base.Activate();
    }

    public override void Deactivate() {
        box.enabled = false;
        base.Deactivate();
    }

    void Update() {
        if (Active) {
            transform.position = (Vector2)transform.position - new Vector2(0f, speed * Time.deltaTime);
            if(transform.position.y <= powerupKillzone) {
                Deactivate();
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        OnPickup?.Invoke(new PowerupEventArgs() {
            gun = gun,
            duration = duration,
            color = color,
            position = transform.position
        });
        Deactivate();
    }
}

public class PowerupEventArgs : EventArgs {
    public PlayerGun gun;
    public float duration;
    public Vector2 position;
    public Color color;
    public string name;
}