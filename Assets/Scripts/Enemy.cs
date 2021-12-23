using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameEntity {
    [Range(1,3)]
    public int health;
   
    public bool dissolve;
    [Range(0, 1)]
    public float dissolveFactor;

    private void Awake() {
        Deactivate();
    }

    private void Update() {
        sprite.material.SetInt("_Dissolve", dissolve ? 1 : 0);
        sprite.material.SetFloat("_DissolveFactor", dissolveFactor);
    }
}