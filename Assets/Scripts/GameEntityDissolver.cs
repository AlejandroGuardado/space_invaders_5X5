using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntityDissolver : MonoBehaviour{
    public GameEntity entity;
    public float dissolveTime;
    private bool doDissolve;
    private float dissolveFactor;    
    private float currentDissolveTime;

    private static int DissolveID;
    private static int DissolveFactorID;

    static GameEntityDissolver() {
        DissolveID = Shader.PropertyToID("_Dissolve");
        DissolveFactorID = Shader.PropertyToID("_DissolveFactor");
    }

    private void Update() {
        if (doDissolve) {
            currentDissolveTime += Time.deltaTime;
            dissolveFactor = Mathf.Clamp01(currentDissolveTime / dissolveTime);
            entity.sprite.material.SetFloat(DissolveFactorID, dissolveFactor);
            if (currentDissolveTime > dissolveTime) {
                Clear();
            }
        }
    }

    public void Dissolve() {
        if (dissolveTime <= 0f) return;
        entity.sprite.material.SetInt(DissolveID, 1);
        dissolveFactor = 0f;
        currentDissolveTime = 0f;
        doDissolve = true;
    }

    public void Clear() {
        doDissolve = false;
        entity.sprite.material.SetInt(DissolveID, 0);
        dissolveFactor = 0f;
        currentDissolveTime = 0f;
        dissolveTime = 0f;
    }
}