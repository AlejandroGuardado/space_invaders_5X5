using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuScreen : GameScreen{
    public GameObject canvas;
    public UnityEngine.UI.Button creditsButton;
    public TransitionManager transitionManager;
    public TransitionData transitionData;

    private void Awake() {
        canvas.SetActive(false);
    }

    public override IEnumerator OnEnter() {
        transitionManager.FadeOutImmediatly();
        transitionManager.CutInImmediatly();
        yield return new WaitForSeconds(transitionData.menuDelayTime);
        transitionManager.CutOut(transitionData.menuTransitionTime);
        yield return new WaitForSeconds(transitionData.menuTransitionTime);
        canvas.SetActive(true);
        creditsButton.interactable = true;
    }

    public override void OnUpdate() {
        return;
    }

    public override IEnumerator OnExit() {
        float wait = transitionData.menuTransitionTime;
        creditsButton.interactable = false;
        transitionManager.FadeIn(wait);
        yield return new WaitForSeconds(wait);
        canvas.SetActive(false);
    }

    public override float GetExitTime() {
        return transitionData.menuTransitionTime;
    }
}