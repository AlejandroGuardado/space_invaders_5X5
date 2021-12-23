using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameFSM {
    public SplashScreen splashScreen;
    public MenuScreen menuScreen;
    public CreditsScreen creditsScreen;
    public SessionScreen sessionScreen;
    public Skip skip;    

    void Start(){
        #if UNITY_EDITOR || DEBUG

        switch (skip) {
            case Skip.ToMenu:
                Init(menuScreen);
                break;
            case Skip.ToCredits:
                Init(creditsScreen);
                break;
            case Skip.ToSession:
                Init(sessionScreen);
                break;
            case Skip.None:
            default:
                Init(splashScreen);
                break;
        }

        #else

        Init(splashScreen);

        #endif
    }

    void Update(){
        UpdateScreen();
    }

    public void OnSplashFinish() {
        ChangeScreen(menuScreen);
    }

    public void OnMenuToCreditsTransition() {
        ChangeScreen(creditsScreen);
    }

    public void OnCreditsToMenuTransition() {
        ChangeScreen(menuScreen);
    }

    public void OnLevelSelect(int levelIndex) {
        ChangeScreen(sessionScreen);
    }

    public enum Skip {
        None,
        ToMenu,
        ToCredits,
        ToSession
    }
}