using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour{
    public GameObject canvas;
    public Transition FadeTransition;
    public Transition CutTransition;

    private static TransitionManager _manager;
    public static TransitionManager Instance {
        get { return _manager; }
        private set { 
            if(_manager == null) {
                _manager = value;
            } 
        }
    }

    private void Awake() {
        Instance = this;
        canvas.SetActive(true);
    }

    public void FadeInImmediatly() {
        FadeTransition.FillImmediatly();
    }

    public void FadeOutImmediatly() {
        FadeTransition.ClearImmediatly();
    }

    public void CutInImmediatly() {
        CutTransition.FillImmediatly();
    }

    public void CutOutImmediatly() {
        CutTransition.ClearImmediatly();
    }

    public void FadeIn(float time) {
        FadeTransition.FadeIn(time);
    }

    public void FadeOut(float time) {
        FadeTransition.FadeOut(time);
    }

    public void CutIn(float time) {
        CutTransition.FadeIn(time);
    }

    public void CutOut(float time) {
        CutTransition.FadeOut(time);
    }
}
