using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity{
    [Range(0,1)]
    public float dissolve;

    private void Awake() {
        Deactivate();
    }

    private void Update() {
        sprite.material.SetFloat("_DissolveFactor", dissolve);
    }
}
