using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour{
    public GameObject canvas;
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Button backButton;
    public TransitionManager transitionManager;
    public TransitionData transitionData;
    public UnityEngine.Events.UnityEvent OnCreditsToMenuTransition;

    public float scrollSpeed;
    public float scrollInitPosition;
    public float scrollEndPosition;
    private float scrollPosition;
    private bool doScroll;

    private void Awake() {
        canvas.SetActive(false);
        doScroll = false;
    }

    public IEnumerator ShowCredits() {
        transitionManager.FadeInImmediatly();
        yield return new WaitForSeconds(transitionData.creditsDelayTime);
        scrollPosition = scrollInitPosition;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        canvas.SetActive(true);
        backButton.gameObject.SetActive(false);
        transitionManager.FadeOut(transitionData.creditsTransitionTime);
        yield return new WaitForSeconds(transitionData.creditsTransitionTime + transitionData.creditsScrollDelayTime);
        doScroll = true;
        backButton.gameObject.SetActive(true);
        backButton.interactable = true;
    }

    public void UpdateCredits() {
        if (!doScroll) return;
        scrollPosition += scrollSpeed * Time.deltaTime;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        if(scrollPosition >= scrollEndPosition) {
            doScroll = false;
            StartCoroutine(ResetScroll());
        }
    }

    public IEnumerator ResetScroll() {
        scrollPosition = scrollInitPosition;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        yield return new WaitForSeconds(transitionData.creditsScrollDelayTime);
        doScroll = true;
    }

    public void OnTransitionToMenu() {
        StopAllCoroutines();
        StartCoroutine(TransitionToMenu());
    }

    private IEnumerator TransitionToMenu() {
        doScroll = false;
        backButton.interactable = false;
        transitionManager.FadeIn(transitionData.creditsTransitionTime);
        yield return new WaitForSeconds(transitionData.creditsTransitionTime);
        canvas.SetActive(false);
        if (OnCreditsToMenuTransition != null) {
            OnCreditsToMenuTransition.Invoke();
        }
    }
}
