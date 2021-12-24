using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFSM : MonoBehaviour {
    private GameScreen currentScreen;

    public void Init(GameScreen screen) {
        currentScreen = screen;
        if(currentScreen != null) {
            StartCoroutine(currentScreen.OnEnter());
        }
    }

    public void UpdateScreen() {
        if (currentScreen != null) {
            currentScreen.OnUpdate();
        }
    }

    public void LateUpdateScreen() {
        if (currentScreen != null) {
            currentScreen.OnLateUpdate();
        }
    }

    public void ChangeScreen(GameScreen screen) {
        StartCoroutine(Change(screen));
    }

    private IEnumerator Change(GameScreen screen) {
        float wait = 0f;
        if (currentScreen != null) {
            StartCoroutine(currentScreen.OnExit());
            wait = currentScreen.GetExitTime();
        }
        if (wait > 0f) yield return new WaitForSeconds(wait);
        Init(screen);
    }
}