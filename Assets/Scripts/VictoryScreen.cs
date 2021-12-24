using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryScreen : GameOverScreen {
    protected override void OnEnterGameOver() {
        retryButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(session.HasNextLevel);
    }
}