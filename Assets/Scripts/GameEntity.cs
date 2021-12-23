using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour{
    public SpriteRenderer sprite;
    public bool Active { get; private set; }    

    public virtual void Activate() {
        sprite.enabled = true;
    }

    public virtual void Deactivate() {
        sprite.enabled = false;
    }

    public virtual void Spawn(Vector2 position) {
        transform.position = position;
        Activate();
    }
}