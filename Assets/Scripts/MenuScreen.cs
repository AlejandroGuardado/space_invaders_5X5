using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuScreen : GameScreen{
    public GameObject canvas;
    public UnityEngine.UI.Button creditsButton;
    public TransitionData transitionData;

    private void Awake() {
        canvas.SetActive(false);
    }

    public override IEnumerator OnEnter() {
        TransitionManager.Instance.FadeOutImmediatly();
        TransitionManager.Instance.CutInImmediatly();
        yield return new WaitForSeconds(transitionData.menuDelayTime);
        TransitionManager.Instance.CutOut(transitionData.menuTransitionTime);
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
        TransitionManager.Instance.FadeIn(wait);
        yield return new WaitForSeconds(wait);
        canvas.SetActive(false);
    }

    public override float GetExitTime() {
        return transitionData.menuTransitionTime;
    }
}