using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public SplashScreenManager splashScreenManager;
    public MenuManager menuManager;
    public CreditsManager creditsManager;
    private GameState state;

    void Start(){
        state = GameState.Splash;
        splashScreenManager.StartSplashScreen();
    }

    void Update(){
        switch (state) {
            case GameState.Menu:
                menuManager.UpdateMenu();
                break;
            case GameState.Credits:
                creditsManager.UpdateCredits();
                break;
            default:
                break;
        }
    }

    public void OnSplashFinish() {
        ShowMenu();
    }

    public void OnMenuToCreditsTransition() {
        state = GameState.Credits;
        StartCoroutine(creditsManager.ShowCredits());
    }

    public void OnCreditsToMenuTransition() {
        ShowMenu();
    }

    private void ShowMenu() {
        state = GameState.Menu;
        StartCoroutine(menuManager.ShowMenu());
    }

    private enum GameState {
        Splash,
        Menu,
        Credits,
        Session,
        Victory,
        GameOver
    }
}