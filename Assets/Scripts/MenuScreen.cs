using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuScreen : GameScreen{
    public GameObject canvas;
    public UnityEngine.UI.Button creditsButton;
    public TransitionManager transitionManager;
    public TransitionData transitionData;

    public UnityEvent OnMenuToCreditsTransition;
    public UnityEvent OnMenuToSessionTransition;


    private void Awake() {
        canvas.SetActive(false);
    }

    /*public void OnCreditsSelect() {
        StartCoroutine(TransitionToCredits());
    }*/

    /*public void OnLevelSelect(int levelIndex) {
        Debug.Log(levelIndex);
    }*/

    /*public IEnumerator TransitionToCredits() {
        
        //if (OnMenuToCreditsTransition != null) {
        //    OnMenuToCreditsTransition.Invoke();
        //}
    }*/

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