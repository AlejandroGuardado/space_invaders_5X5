using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour{
    public float Current { get; private set; }
    private float shockwaveTime;
    private float shockwaveTimer;
    private bool doShockwave;

    private void Awake() {
        Clear();
    }

    void Update(){
        if (doShockwave) {
            shockwaveTimer += Time.deltaTime;
            Current = 1f - (shockwaveTimer / shockwaveTime);
            if (shockwaveTimer > shockwaveTime) {
                shockwaveTimer = 0f;
                Current = 0f;
                doShockwave = false;
            }
        }
    }

    public void CreateShockwave(float time) {
        if (doShockwave || time <= 0f) return;
        shockwaveTimer = 0f;
        shockwaveTime = time;
        Current = 1f;
        doShockwave = true;
    }

    public void Clear() {
        Current = 0f;
        doShockwave = false;
    }
}