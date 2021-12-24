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
    public int Score {  get; private set; }
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
        yield return new WaitForEndOfFrame();
    }

    public override float GetExitTime() {
        return 0f;
    }

    public void Clear() {
        grid.Clear();
        player.Deactivate();
        for (int i = 0; i < barriers.Length; i++) {
            barriers[i].Deactivate();
        }
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
        player.OnWeaponFired.AddListener(OnWeaponFired);
    }

    private void StopGameplay() {
        player.OnWeaponFired.RemoveListener(OnWeaponFired);
        player.weapons.Clear();
    }

    private void GameOver() {
        state = SessionState.Over;
        StopGameplay();
    }

    private IEnumerator Victory() {
        GameOver();
        player.RemoveControl();
        yield return new WaitForSeconds(sessionData.sessionVictoryDelay);
        if(OnVictory != null) {
            OnVictory.Invoke();
        }
        Debug.Log("Victory");
    }

    private IEnumerator Defeat() {
        GameOver();
        player.Kill();
        yield return new WaitForSeconds(sessionData.sessionDefeatDelay);
        if (OnDefeat != null) {
            OnDefeat.Invoke();
        }
        Debug.Log("Defeat");
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

        //First round of spawns
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
            StartCoroutine(Defeat());
        }
    }

    private void CheckKilledEnemies() {
        bool allKilled = true;
        for (int i = 0; i < grid.Count; i++) {
            LevelSpot spot = grid[i];
            if (spot.Status == LevelSpotStatus.Active && spot.enemy && !spot.enemy.IsAlive) {
                spot.Status = LevelSpotStatus.Off;
                OnEnemyKilled(spot.enemy);
            }
            allKilled &= spot.Status == LevelSpotStatus.Off;
        }
        if (allKilled) {
            StartCoroutine(Victory());
        }
    }

    private void OnEnemyKilled(Enemy enemy) {
        bool bonus = enemy.transform.position.y > sessionData.bonusPosition;
        Score += enemy.Points * (bonus ? sessionData.bonusMultiplier : 1);
        UpdateScoreText();
        enemy.Kill();
    }

    private void OnWeaponFired(float cooldown) {
        //Show Reload Label
        StopCoroutine(HideReloadLabel(cooldown));
        reloadCanvas.SetActive(true);
        StartCoroutine(HideReloadLabel(cooldown));

        IEnumerator HideReloadLabel(float cooldown) {
            yield return new WaitForSeconds(cooldown);
            reloadCanvas.SetActive(false);
        }
    }

    private void HideUI() {
        scoreCanvas.SetActive(false);
        powerupCanvas.SetActive(false);
        touchCanvas.SetActive(false);
        reloadCanvas.SetActive(false);
    }

    private void InitScore() {
        Score = 0;
        scoreCanvas.SetActive(true);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = string.Format("{0:00000000}", Score);
    }

    private enum SessionState {
        Start,
        Active,
        Over
    }

    private enum GameOverState {
        Victory,
        Defeat
    }
}