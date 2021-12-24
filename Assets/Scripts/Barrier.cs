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

    private float currentShockwave;
    private float shockwaveTime;
    private float shockwaveTimer;
    private bool doShockwave;

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
            sprite.material.SetFloat(ShowDistanceID, showDistance + currentShockwave);
            UpdateShockwave();
        }
    }

    public void Shockwave(float time) {
        if (doShockwave || time <= 0f) return;
        shockwaveTimer = 0f;
        shockwaveTime = time;
        currentShockwave = shockwaveAddedDistance;
        doShockwave = true;
    }

    private void UpdateShockwave() {
        if (doShockwave) {
            shockwaveTimer += Time.deltaTime;
            currentShockwave = shockwaveAddedDistance * (1 - (shockwaveTimer / shockwaveTime));
            if (shockwaveTimer > shockwaveTime) {
                shockwaveTimer = 0f;
                currentShockwave = 0f;
                doShockwave = false;
            }
        }
    }

    public override void Deactivate() {
        currentShockwave = 0f;
        doShockwave = false;
        base.Deactivate();
    }
}
