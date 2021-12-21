using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour{
    public GameObject canvas;
    public TransitionManager transitionManager;
    public TransitionData transitionData;
    public UnityEngine.Events.UnityEvent OnSplashFinish;

    private void Awake() {
        canvas.SetActive(false);
    }

    public void StartSplashScreen(){
        transitionManager.FadeInImmediatly();
        transitionManager.CutOutImmediatly();
        StartCoroutine(ShowSplashScreen());
    }

    private IEnumerator ShowSplashScreen() {
        canvas.SetActive(true);
        transitionManager.FadeOut(transitionData.splashScreenTransitionTime);
        yield return new WaitForSeconds(transitionData.splashScreenTransitionTime + transitionData.splashScreenHoldTime);
        transitionManager.FadeIn(transitionData.splashScreenTransitionTime);
        yield return new WaitForSeconds(transitionData.splashScreenTransitionTime);
        canvas.SetActive(false);
        if(OnSplashFinish != null) {
            OnSplashFinish.Invoke();
        }
    }
}