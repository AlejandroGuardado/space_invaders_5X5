using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour{
    public SpriteRenderer sprite;
    public bool Active { get; private set; }    

    public virtual void Activate() {
        gameObject.SetActive(true);
        sprite.enabled = true;
        Active = true;
    }

    public virtual void Deactivate() {
        gameObject.SetActive(false);
        sprite.enabled = false;
        Active = false;
    }

    public virtual void Spawn(Vector2 position) {
        transform.position = position;
        Activate();
    }
}