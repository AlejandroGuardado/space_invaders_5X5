using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Victory and Defeat screens use the same canvas
/// <br>They differ on which buttons they enable</br>
/// </summary>
public abstract class GameOverScreen : GameScreen{
    public SessionScreen session;
    public TransitionData transitionData;
    public GameObject backCanvas;
    public GameObject canvas;
    public Text titleText;
    public Text scoreText;
    public Button retryButton;
    public Button nextButton;

    public abstract string Title { get; }

    private void Awake() {
        backCanvas.SetActive(false);
        canvas.SetActive(false);
    }

    public override float GetExitTime() {
        return transitionData.gameoverTransitionTime;
    }

    public override IEnumerator OnEnter() {
        SetScore();
        OnEnterGameOver();
        backCanvas.SetActive(true);
        canvas.SetActive(true);
        titleText.text = Title;
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator OnExit() {
        TransitionManager.Instance.CutIn(GetExitTime());
        yield return new WaitForSeconds(GetExitTime());
        backCanvas.SetActive(false);
        canvas.SetActive(false);
        session.Clear();
        yield return new WaitForEndOfFrame();
    }

    public override void OnUpdate() {
        return;
    }

    private void SetScore() {
        SessionScreen.UpdateScoreText(scoreText, session.Score);
    }

    protected abstract void OnEnterGameOver();
}