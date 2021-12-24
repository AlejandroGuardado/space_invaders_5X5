using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SessionScreen : GameScreen{
    public Player player;
    public EnemyPool enemyPool;
    public GameObject scoreCanvas;
    public GameObject powerupCanvas;
    public GameObject touchCanvas;
    public GameObject reloadCanvas;
    public Barrier[] barriers;
    public Text scoreText;
    public SessionData sessionData;
    public TransitionData transitionData;

    public UnityEvent OnVictory;
    public UnityEvent OnDefeat;

    private int levelIndex;
    private LevelData levelData;
    private int score;
    private List<LevelSpot> grid;
    private SessionState state;

    public bool IsSessionActive {
        get {
            return state == SessionState.Active;
        }
    }

    private void Awake() {
        levelIndex = 0;
        state = SessionState.Start;
        player.Deactivate();
        HideUI();
    }

    public override IEnumerator OnEnter() {
        state = SessionState.Start;
        TransitionManager.Instance.FadeOutImmediatly();
        TransitionManager.Instance.CutInImmediatly();
        InitSession();
        yield return new WaitForSeconds(transitionData.sessionDelayTime);
        TransitionManager.Instance.CutOut(transitionData.sessionTransitionTime);
        yield return new WaitForSeconds(transitionData.sessionTransitionTime);
        yield return new WaitForSeconds(sessionData.sessionStartDelay);
        StartGameplay();
    }

    public override void OnUpdate() {
        if (IsSessionActive) {
            SpawnSpots();
            CheckKilledEnemies();
        }
    }

    public override void OnLateUpdate() {
        if (IsSessionActive) {
            MoveSpots();
        }
    }

    public override IEnumerator OnExit() {
        state = SessionState.Over;
        grid.Clear();
        player.Deactivate();
        for (int i = 0; i < barriers.Length; i++) {
            barriers[i].Deactivate();
        }
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
        enemyPool.Clear();
        LoadLevel();
        player.Spawn(new Vector2(0, sessionData.playerSpawnPosition));
        for (int i = 0; i < barriers.Length; i++) {
            barriers[i].Activate();
        }
    }

    private void StartGameplay() {
        state = SessionState.Active;
        player.GainControl();
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

        SpawnSpots();
    }

    private void AssignRandomEnemyToSpots(List<int> spotIndexes, EnemyType type, int needed) {
        for (int i = 0; i < needed; i++) {
            if (spotIndexes.Count == 0) break;
            int randomIndex = Random.Range(0, spotIndexes.Count);
            grid[spotIndexes[randomIndex]].EnemyType = type;
            spotIndexes.RemoveAt(randomIndex);
        }
    }

    private void SpawnSpots() {
        for (int i = 0; i < grid.Count; i++) {
            LevelSpot spot = grid[i];
            bool standby = spot.Status == LevelSpotStatus.Standby;
            bool canEnemySpawn = spot.enemy == null;
            bool passSpawnPosition = spot.position.y <= sessionData.enemySpawnPosition;
            if (standby && canEnemySpawn && passSpawnPosition) {
                spot.enemy = enemyPool.Spawn(spot.EnemyType, spot.position);
                if(spot.enemy != null) {
                    spot.Status = LevelSpotStatus.Active;
                }
            }
        }
    }

    private void MoveSpots() {
        bool crossedGameOverLine = false;
        float yDelta = levelData.speed * Time.deltaTime;
        for (int i = 0; i < grid.Count; i++) {
            LevelSpot spot = grid[i];
            if (spot.Status == LevelSpotStatus.Off) continue;

            //Go down
            spot.position.y -= yDelta;
            //If any enemy crossed line, it's game over
            crossedGameOverLine |= spot.position.y <= sessionData.gameOverPosition;

            if (spot.Status == LevelSpotStatus.Active) {
                spot.enemy.transform.position = spot.position;
            }
        }
        if (crossedGameOverLine) {
            Debug.Log("Game Over");
            state = SessionState.Over;
        }
    }

    private void CheckKilledEnemies() {
        for (int i = 0; i < grid.Count; i++) {
            LevelSpot spot = grid[i];
            if (spot.Status == LevelSpotStatus.Active && spot.enemy && !spot.enemy.IsAlive) {
                spot.Status = LevelSpotStatus.Off;
                //Enemy OnKill - dissolve and deactivate coroutine
                OnEnemyKilled(spot.enemy.Points, spot.position.y);
            }
        }
    }

    private void OnEnemyKilled(int points, float yPosition) {
        bool bonus = yPosition > sessionData.bonusPosition;
        score += points * (bonus ? sessionData.bonusMultiplier : 1);
    }

    private void HideUI() {
        scoreCanvas.SetActive(false);
        powerupCanvas.SetActive(false);
        touchCanvas.SetActive(false);
        reloadCanvas.SetActive(false);
    }

    private void InitScore() {
        score = 0;
        scoreCanvas.SetActive(true);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = string.Format("{0:00000000}", score);
    }

    private enum SessionState {
        Start,
        Active,
        Over
    }
}