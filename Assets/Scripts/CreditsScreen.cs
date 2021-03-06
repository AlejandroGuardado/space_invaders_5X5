using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : GameScreen{
    public GameObject canvas;
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Button backButton;
    public TransitionData transitionData;

    public float scrollSpeed;
    public float scrollInitPosition;
    public float scrollEndPosition;
    private float scrollPosition;
    private bool doScroll;

    private void Awake() {
        canvas.SetActive(false);
        doScroll = false;
    }

    public override IEnumerator OnEnter() {
        TransitionManager.Instance.FadeInImmediatly();
        TransitionManager.Instance.CutOutImmediatly();
        yield return new WaitForSeconds(transitionData.creditsDelayTime);
        scrollPosition = scrollInitPosition;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        canvas.SetActive(true);
        backButton.gameObject.SetActive(false);
        TransitionManager.Instance.FadeOut(transitionData.creditsTransitionTime);
        yield return new WaitForSeconds(transitionData.creditsTransitionTime + transitionData.creditsScrollDelayTime);
        doScroll = true;
        backButton.gameObject.SetActive(true);
        backButton.interactable = true;
    }

    public override void OnUpdate() {
        if (!doScroll) return;
        scrollPosition += scrollSpeed * Time.deltaTime;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        if (scrollPosition >= scrollEndPosition) {
            doScroll = false;
            StartCoroutine(ResetScroll());
        }
    }

    public override IEnumerator OnExit() {
        StopAllCoroutines();
        float wait = transitionData.creditsTransitionTime;
        doScroll = false;
        backButton.interactable = false;
        TransitionManager.Instance.FadeIn(wait);
        yield return new WaitForSeconds(wait);
        canvas.SetActive(false);
    }

    public override float GetExitTime() {
        return transitionData.creditsTransitionTime;
    }

    private IEnumerator ResetScroll() {
        scrollPosition = scrollInitPosition;
        text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, scrollPosition, 0);
        yield return new WaitForSeconds(transitionData.creditsScrollDelayTime);
        doScroll = true;
    }
}