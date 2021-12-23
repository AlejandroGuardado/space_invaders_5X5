using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : GameEntity{
    public Transform player;
    private static int PositionID;

    private void Awake() {
        Deactivate();
    }

    static Barrier() {
        PositionID = Shader.PropertyToID("_Position");
    }

    void Update(){
        if (Active) {
            sprite.material.SetVector(PositionID, new Vector4(player.position.x, player.position.y, 0, 0));
        }   
    }
}
