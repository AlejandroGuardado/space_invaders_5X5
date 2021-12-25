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
    public GameObject startGameCanvas;
    public Barrier[] barriers;
    public SwapButton[] swapButtons;
    public FillImage fireButton;
    public Text scoreText;
    public Text bonusText;
    public SessionData sessionData;
    public TransitionData transitionData;

    public UnityEvent OnVictory;
    public UnityEvent OnDefeat;

    private int levelIndex;
    private LevelData levelData;
    public int Score {  get; private set; }
    public int LevelIndex { get { return levelIndex; } }
    public int LevelIndex_OneBased { get { return levelIndex + 1; } }
    public bool HasNextLevel { get { return LevelIndex_OneBased < sessionData.levels.Length; } }
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
        scoreText.text = $"BONUSX{sessionData.bonusMultiplier}";
        ClearUI();
    }

    public override IEnumerator OnEnter() {
        state = SessionState.Start;
        TransitionManager.Instance.FadeOutImmediatly();
        TransitionManager.Instance.CutInImmediatly();
        InitSession();
        yield return new WaitForSeconds(transitionData.sessionDelayTime);
        TransitionManager.Instance.CutOut(transitionData.sessionTransitionTime);
        yield return new WaitForSeconds(transitionData.sessionTransitionTime);
        StartCoroutine(ShowStartGame());
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
        for (int i = 0; i < swapButtons.Length; i++) {
            swapButtons[i].OnRelease();
        }
    }

    public void SetLevelIndex(int levelIndex) {
        //Level Index from button events converted to zero-based
        this.levelIndex = levelIndex - 1;
    }

    private void InitSession() {
        ClearUI();
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
        #if UNITY_ANDROID
        touchCanvas.SetActive(true);
        #endif
    }

    private void StopGameplay() {
        player.OnWeaponFired.RemoveListener(OnWeaponFired);
        player.weapons.Clear();
        player.RemoveControl();
        #if UNITY_ANDROID
        touchCanvas.SetActive(false);
        #endif
    }

    private void GameOver() {
        state = SessionState.Over;
        StopGameplay();
    }

    private IEnumerator Victory() {
        GameOver();
        yield return new WaitForSeconds(sessionData.sessionVictoryDelay);
        if(OnVictory != null) {
            OnVictory.Invoke();
        }
        ClearUI();
    }

    private IEnumerator Defeat() {
        GameOver();
        player.Kill();
        yield return new WaitForSeconds(sessionData.sessionDefeatDelay);
        if (OnDefeat != null) {
            OnDefeat.Invoke();
        }
        ClearUI();
    }

    private void LoadLevel() {
        if (levelIndex < 0 || levelIndex > sessionData.levels?.Length - 1) {
            Debug.LogError($"Level {LevelIndex_OneBased} was not found");
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
        if (bonus) {
            StopCoroutine(ShowBonus());
            StartCoroutine(ShowBonus());
        }
        Score += enemy.Points * (bonus ? sessionData.bonusMultiplier : 1);
        UpdateScoreText();
        enemy.Kill();
    }

    private void OnWeaponFired(float cooldown) {
        //Show Reload Label
        StopCoroutine(HideReloadLabel(cooldown));
        reloadCanvas.SetActive(true);
        StartCoroutine(HideReloadLabel(cooldown));

        
        fireButton.Fill(cooldown);
        for (int i = 0; i < barriers.Length; i++) {
            barriers[i].Shockwave(sessionData.playerFireShockwaveTime);
        }

        IEnumerator HideReloadLabel(float cooldown) {
            yield return new WaitForSeconds(cooldown);
            reloadCanvas.SetActive(false);
        }
    }

    private void ClearUI() {
        scoreCanvas.SetActive(false);
        powerupCanvas.SetActive(false);
        touchCanvas.SetActive(false);
        reloadCanvas.SetActive(false);
        startGameCanvas.SetActive(false);
        for (int i = 0; i < swapButtons.Length; i++) {
            swapButtons[i].OnRelease();
        }
        fireButton.Clear();
    }

    private void InitScore() {
        Score = 0;
        scoreCanvas.SetActive(true);
        UpdateScoreText();
        bonusText.enabled = false;
    }

    private void UpdateScoreText() {
        UpdateScoreText(scoreText, Score);
    }

    private IEnumerator ShowStartGame() {
        yield return new WaitForSeconds(sessionData.sessionStartGameImageDelay);
        startGameCanvas.SetActive(true);
        yield return new WaitForSeconds(sessionData.bonusTextShowTime);
        startGameCanvas.SetActive(false);
    }

    private IEnumerator ShowBonus() {
        //Hide bonus text in case it was shown already
        bonusText.enabled = false;
        yield return new WaitForSeconds(0.1f);
        bonusText.enabled = true;
        yield return new WaitForSeconds(sessionData.bonusTextShowTime);
        bonusText.enabled = false;
    }

    public static void UpdateScoreText(Text scoreText, float score) {
        scoreText.text = string.Format("{0:00000000}", score);
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