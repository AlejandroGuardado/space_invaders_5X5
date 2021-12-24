using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatScreen : GameOverScreen {
    protected override void OnEnterGameOver() {
        retryButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }
}