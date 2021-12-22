using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : GameScreen{
    public GameObject canvas;
    public TransitionManager transitionManager;
    public TransitionData transitionData;
    public UnityEngine.Events.UnityEvent OnSplashFinish;

    private void Awake() {
        canvas.SetActive(false);
    }

    public override IEnumerator OnEnter() {
        transitionManager.FadeInImmediatly();
        transitionManager.CutOutImmediatly();
        canvas.SetActive(true);
        transitionManager.FadeOut(transitionData.splashScreenTransitionTime);
        yield return new WaitForSeconds(transitionData.splashScreenTransitionTime + transitionData.splashScreenHoldTime);
        if (OnSplashFinish != null) {
            OnSplashFinish.Invoke();
        }
    }

    public override void OnUpdate() {
        return;
    }

    public override IEnumerator OnExit() {
        float wait = transitionData.splashScreenTransitionTime;
        transitionManager.FadeIn(wait);
        yield return new WaitForSeconds(wait);
        canvas.SetActive(false);
    }

    public override float GetExitTime() {
        return transitionData.splashScreenTransitionTime;
    }
}