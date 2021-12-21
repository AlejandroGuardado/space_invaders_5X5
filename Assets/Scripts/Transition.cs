using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour{
    public Material material;
    public AnimationCurve curve;
    private int fillIndex;
    private float fillRate;
    private float fillCurrentTime;
    private float fillTime;

    private TransitionState state;
    private bool IsTransitioning {
        get { return state != TransitionState.None; }
    }

    void Awake(){
        fillIndex = Shader.PropertyToID("_Fill");
        material.SetFloat(fillIndex, fillRate);
    }

    void Update(){
        if (IsTransitioning) {
            UpdateTransition();
        }
    }

    private void Fade(float time, TransitionState newState) {
        if (IsTransitioning) return;
        fillCurrentTime = 0f;
        fillTime = time;
        fillRate = newState == TransitionState.FadeIn ? 0f : 1f;
        material.SetFloat(fillIndex, fillRate);
        state = newState;
    }

    private void UpdateTransition() {
        fillCurrentTime += Time.deltaTime;
        float fillFactor = Mathf.Clamp01(fillCurrentTime / fillTime);
        fillRate = curve.Evaluate(fillFactor);
        fillRate = state == TransitionState.FadeIn ? fillRate : 1 - fillRate;
        material.SetFloat(fillIndex, fillRate);
        if (fillFactor + float.Epsilon >= 1f) {
            state = TransitionState.None;
        }
    }

    public void FillImmediatly() {
        fillRate = 1f;
        material.SetFloat(fillIndex, fillRate);
    }

    public void ClearImmediatly() {
        fillRate = 0f;
        material.SetFloat(fillIndex, fillRate);
    }

    public void FadeIn(float time) {
        Fade(time, TransitionState.FadeIn);
    }

    public void FadeOut(float time) {
        Fade(time, TransitionState.FadeOut);
    }

    private enum TransitionState {
        None,
        FadeIn,
        FadeOut
    }
}