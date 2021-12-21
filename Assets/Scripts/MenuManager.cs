using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour{
    public GameObject canvas;
    public UnityEngine.UI.Button creditsButton;
    public TransitionManager transitionManager;
    public TransitionData transitionData;
    public UnityEngine.Events.UnityEvent OnMenuToCreditsTransition;

    private void Awake() {
        canvas.SetActive(false);
    }

    public IEnumerator ShowMenu() {
        transitionManager.FadeOutImmediatly();
        transitionManager.CutInImmediatly();
        yield return new WaitForSeconds(transitionData.menuDelayTime);
        transitionManager.CutOut(transitionData.menuTransitionTime);
        yield return new WaitForSeconds(transitionData.menuTransitionTime);
        canvas.SetActive(true);
        creditsButton.interactable = true;
    }

    public void UpdateMenu() {

    }

    public void OnTransitionToCredits() {
        StartCoroutine(TransitionToCredits());
    }

    public IEnumerator TransitionToCredits() {
        creditsButton.interactable = false;
        transitionManager.FadeIn(transitionData.menuTransitionTime);
        yield return new WaitForSeconds(transitionData.menuTransitionTime);
        canvas.SetActive(false);
        if (OnMenuToCreditsTransition != null) {
            OnMenuToCreditsTransition.Invoke();
        }
    }
}