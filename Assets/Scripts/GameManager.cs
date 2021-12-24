using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameFSM {
    public SplashScreen splashScreen;
    public MenuScreen menuScreen;
    public CreditsScreen creditsScreen;
    public SessionScreen sessionScreen;
    public VictoryScreen victoryScreen;
    public DefeatScreen defeatScreen;
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

    void LateUpdate() {
        LateUpdateScreen();
    }

    public void ToCreditsTransition() {
        ChangeScreen(creditsScreen);
    }

    public void ToMenuTransition() {
        ChangeScreen(menuScreen);
    }

    public void ToVictoryTransition() {
        ChangeScreen(victoryScreen);
    }

    public void ToDefeatTransition() {
        ChangeScreen(defeatScreen);
    }

    public void OnLevelSelect(int levelIndex) {
        sessionScreen.SetLevelIndex(levelIndex);
        ChangeScreen(sessionScreen);
    }

    public void OnNextLevel() {
        sessionScreen.SetLevelIndex(sessionScreen.LevelIndex_OneBased + 1);
        ChangeScreen(sessionScreen);
    }

    public void OnRetryLevel() {
        sessionScreen.SetLevelIndex(sessionScreen.LevelIndex_OneBased);
        ChangeScreen(sessionScreen);
    }

    public enum Skip {
        None,
        ToMenu,
        ToCredits,
        ToSession
    }
}