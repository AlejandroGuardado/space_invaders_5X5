using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : GameEntity{
    public Transform player;
    [SerializeField]
    private float showDistance;
    [SerializeField]
    private float shockwaveAddedDistance;
    private static int PositionID;
    private static int ShowDistanceID;
    public Shockwave shockwave;

    private void Awake() {
        Deactivate();
    }

    static Barrier() {
        PositionID = Shader.PropertyToID("_Position");
        ShowDistanceID = Shader.PropertyToID("_ShowDistance");
    }

    void Update(){
        if (Active) {
            sprite.material.SetVector(PositionID, new Vector4(player.position.x, player.position.y, 0, 0));
            sprite.material.SetFloat(ShowDistanceID, showDistance + shockwave.Current * shockwaveAddedDistance);
        }
    }

    public void Shockwave(float time) {
        shockwave.CreateShockwave(time);
    }

    public override void Deactivate() {
        shockwave.Clear();
        base.Deactivate();
    }
}
