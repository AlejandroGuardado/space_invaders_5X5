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
    private LevelData levelData;
    private int score;
    private List<LevelSpot> grid;

    private void Awake() {
        levelIndex = 0;
        HideUI();
    }

    public override IEnumerator OnEnter() {
        TransitionManager.Instance.FadeOutImmediatly();
        TransitionManager.Instance.CutInImmediatly();
        yield return new WaitForSeconds(transitionData.sessionDelayTime);
        TransitionManager.Instance.CutOut(transitionData.sessionTransitionTime);
        InitSession();
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

    public void SetLevelIndex(int levelIndex) {
        //Level Index from button events converted to zero-based
        this.levelIndex = levelIndex - 1;
    }

    private void InitSession() {
        InitScore();
        LoadLevel();
    }

    private void LoadLevel() {
        if (levelIndex < 0 || levelIndex > sessionData.levels?.Length - 1) {
            //Converted back to one-based
            Debug.LogError($"Level {++levelIndex} was not found");
            return;
        }
        levelData = sessionData.levels[levelIndex];

        //How many spots?
        int spots = 0;
        for (int i = 0; i < levelData.lines.Length; i++) {
            spots += levelData.lines[i].NumberSpots;
        }
        int repeat = levelData.repeat > 1 ? levelData.repeat : 1;
        spots *= repeat;

        //Generate grid
        grid = new List<LevelSpot>(spots);
        List<int> spotEnemyAssigment = new List<int>(spots);
        for (int i = 0; i < spots; i++) {
            grid.Add(new LevelSpot());
            spotEnemyAssigment.Add(i);
        }

        //Assign enemies randomly, remaining spots are set to Green
        AssignRandomEnemyToSpots(spotEnemyAssigment, EnemyType.Red, levelData.redEnemiesNeeded);
        AssignRandomEnemyToSpots(spotEnemyAssigment, EnemyType.Blue, levelData.blueEnemiesNeeded);        
        for (int i = 0; i < spotEnemyAssigment.Count; i++) {
            grid[spotEnemyAssigment[i]].EnemyType = EnemyType.Green;
        }

        //Starting position for spots
        //Per line, each "xposition" corresponds to a spot. 
        //O(n3) may be improved by caching positions per line
        float lineHeight = sessionData.gridStartPosition;
        int currentSpotIndex = 0;
        for (int rep = 0; rep < repeat; rep++) {
            for (int lineIndex = 0; lineIndex < levelData.lines.Length; lineIndex++) {
                LevelLine line = levelData.lines[lineIndex];
                for (int positionIndex = 0; positionIndex < line.NumberSpots; positionIndex++) {
                    grid[currentSpotIndex].position = new Vector2(line.xPositions[positionIndex], lineHeight);
                    currentSpotIndex++;
                }
                lineHeight += sessionData.gridLineHeight;
            }
        }
    }

    private void AssignRandomEnemyToSpots(List<int> spotIndexes, EnemyType type, int needed) {
        for (int i = 0; i < needed; i++) {
            if (spotIndexes.Count == 0) break;
            int randomIndex = Random.Range(0, spotIndexes.Count);
            grid[spotIndexes[randomIndex]].EnemyType = type;
            spotIndexes.RemoveAt(randomIndex);
        }
    }

    private void HideUI() {
        scoreCanvas.SetActive(false);
        powerupCanvas.SetActive(false);
        touchCanvas.SetActive(false);
    }

    private void InitScore() {
        score = 0;
        scoreCanvas.SetActive(true);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = string.Format("{0:00000000}", score);
    }
}