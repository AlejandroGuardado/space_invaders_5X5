using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SessionScreen : GameScreen{
    public GameObject scoreCanvas;
    public GameObject powerupCanvas;
    public GameObject touchCanvas;
    public Text scoreText;
    public SessionData sessionData;
    public TransitionData transitionData;

    public UnityEvent OnVictory;
    public UnityEvent OnDefeat;

    private int levelIndex;
    private int score;

    private void Awake() {
        levelIndex = 0;
        HideUI();
    }

    public override IEnumerator OnEnter() {
        TransitionManager.Instance.FadeOutImmediatly();
        TransitionManager.Instance.CutInImmediatly();
        yield return new WaitForSeconds(transitionData.sessionDelayTime);
        TransitionManager.Instance.CutOut(transitionData.sessionTransitionTime);

        //Init Session
        InitScore();

        yield return new WaitForSeconds(transitionData.sessionTransitionTime);
    }

    public override void OnUpdate() {
        return;
    }

    public override IEnumerator OnExit() {
        yield return new WaitForEndOfFrame();
    }

    public override float GetExitTime() {
        return 0f;
    }

    private void HideUI() {
        scoreCanvas.SetActive(false);
        powerupCanvas.SetActive(false);
        touchCanvas.SetActive(false);
    }

    private void InitScore() {
        scoreCanvas.SetActive(true);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = string.Format("{0:00000000}", score);
    }
}