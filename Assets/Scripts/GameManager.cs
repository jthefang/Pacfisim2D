using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* 
    Last script to load: 
        waits on ObjectPooler, DroneManager, GateManager, UIManager, CameraRig
*/

public enum GameState {
    STOPPED, //e.g. for game over
    PAUSED,
    PLAYING,
    STARTING,
}

[Serializable]
public class GameSpeed {
    public int numDronesToSpawn;
    public float droneSpawnFrequencySeconds; 
    public int numGatesToSpawn;
    public float gateSpawnFrequencySeconds;
    public int scoreRangeMin;

    public GameSpeed(int nDronesToSpawn, float droneSpawnFreqSeconds, int nGatesToSpawn, float gateSpawnFreqSeconds) {
        this.numDronesToSpawn = nDronesToSpawn;
        this.droneSpawnFrequencySeconds = droneSpawnFreqSeconds;
        this.numDronesToSpawn = nGatesToSpawn;
        this.gateSpawnFrequencySeconds = gateSpawnFreqSeconds;
    }
}

public class GameManager : MonoBehaviour, ILoadableScript, IDependentScript
{
    #region Singleton
    public static GameManager Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        Instance = this;
    }
    #endregion

    #region GameState
    private GameState _currGameState;
    public GameState CurrGameState
    {
        get {
            return this._currGameState;
        }

        set {  
            GameState prevGameState = this._currGameState;
            this._currGameState = value;   
            if (prevGameState != value)   
                OnGameStateChange?.Invoke(prevGameState, value);
        }
    }
    public bool IsPlaying {
        get {
            return this.CurrGameState == GameState.PLAYING;
        }
    }
    public bool IsGameOver {
        get {
            return this.CurrGameState == GameState.STOPPED;
        }
        set {
            if (value)
                this.CurrGameState = GameState.STOPPED;
        }
    }
    public event Action<GameState, GameState> OnGameStateChange;
    public event Action<GameManager> OnGameStart;
    public event Action<GameManager> OnGameOver;
    public event Action<GameManager> OnGamePause;
    public event Action<GameManager> OnGameResume;
    #endregion
    public float GAME_OVER_DELAY = 1.5f;

    #region GameSpeed
    [SerializeField]
    GameSpeed[] gameSpeedLevels;
    int _currGameSpeedIdx;
    public int CurrGameSpeedIdx {
        get {
            return _currGameSpeedIdx;
        }
        set {
            this._currGameSpeedIdx = value;
            //this should change the game speed
            SetGameSpeedTo(gameSpeedLevels[this._currGameSpeedIdx]);
        }
    }
    #endregion

    #region GameSettings
    GameSettings gameSettings;
    #endregion

    [SerializeField]
    private AudioClip gameStartSound;
    [SerializeField]
    private AudioClip gameOverSound;
    private AudioSource audioSource;

    ScoreManager scoreManager;
    ObjectPooler objectPooler;
    [SerializeField]
    DroneManager droneManager;
    [SerializeField]
    GateManager gateManager;

    public GameObject playerPrefab;
    private Player player;
    public event Action<Player> OnNewPlayer;

    [SerializeField]
    protected GameObject Bounds;
    /**
        The game bounds go from top left: -bounds.x, -bounds.y 
            -> bot right: +bounds.x, +bounds.y
    */
    public Vector3 GameBounds {
        get {
            return Bounds.GetComponent<SpriteRenderer>().bounds.extents;
        }
    }

    #region ILoadableScript
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    bool isInitialized {
        get {
            return this._isInitialized;
        }
        set {
            this._isInitialized = value;
            if (this._isInitialized) {
                OnScriptInitialized?.Invoke(this);
            }
        }   
    }
    public bool IsInitialized () {
        return isInitialized;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        scoreManager = ScoreManager.Instance;
        scoreManager.OnScoreChange += OnScoreChange;

        gameSettings = GameSettings.Instance;

        audioSource = gameObject.GetComponent<AudioSource>();
        
        OnGameStateChange += OnGameStateChangeHandler;
        OnGameStart += OnGameStartHandler;
        OnGameOver += OnGameOverHandler;
        OnGamePause += OnGamePauseHandler;
        OnGameResume += OnGameResumeHandler;

        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (CurrGameState == GameState.PLAYING) {
                CurrGameState = GameState.PAUSED;
            } else if (CurrGameState == GameState.PAUSED) {
                CurrGameState = GameState.PLAYING;
            }
        }
    }

    public void OnAllDependenciesLoaded() {
        //Game doesen't start until this happens
        spawnPlayer();
        CurrGameState = GameState.STARTING;
    }

    void OnGameStateChangeHandler(GameState prevGameState, GameState newGameState) {
        if (newGameState == GameState.STARTING) {
            OnGameStart?.Invoke(this);
        } else if (newGameState == GameState.STOPPED) {
            OnGameOver?.Invoke(this);
        } else if (newGameState == GameState.PAUSED) {
            OnGamePause?.Invoke(this);
        } else if (newGameState == GameState.PLAYING) {
            if (prevGameState == GameState.PAUSED) {
                OnGameResume?.Invoke(this);
            }
        }
    }

    void OnGameStartHandler(GameManager gm) {
        resetPlayer();
        CurrGameSpeedIdx = 0;
        audioSource.PlayOneShot(gameStartSound);
        Invoke("playMainTheme", 0.5f);
        CurrGameState = GameState.PLAYING;
    }

    void OnGameOverHandler(GameManager gm) {
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
    }

    void OnGamePauseHandler(GameManager gm) {
        audioSource.Pause();
    }

    void OnGameResumeHandler(GameManager gm) {
        audioSource.Play();
    }

    void SetGameSpeedTo(GameSpeed gameSpeed)  {
        droneManager.SpawnAmount = gameSpeed.numDronesToSpawn;
        droneManager.SpawnDelay = gameSpeed.droneSpawnFrequencySeconds;
        gateManager.SpawnAmount = gameSpeed.numGatesToSpawn;
        gateManager.SpawnDelay = gameSpeed.gateSpawnFrequencySeconds;
    }

    void OnScoreChange(ScoreManager sm) {
        int nextSpeedIdx = CurrGameSpeedIdx + 1;
        if (nextSpeedIdx < gameSpeedLevels.Length) {
            GameSpeed nextGameSpeed = gameSpeedLevels[nextSpeedIdx];
            if (sm.Score >= nextGameSpeed.scoreRangeMin) { 
                CurrGameSpeedIdx = nextSpeedIdx;
            }
        }
    }

    public void RestartGame() {
        CurrGameState = GameState.STOPPED;
        CurrGameState = GameState.STARTING; //trigger start
    }

    private void playMainTheme() {
        if (IsPlaying)
            audioSource.Play();
    }

    private void spawnPlayer() {
        Vector2 spawnLocation = new Vector2(0, 0);
        player = Instantiate(playerPrefab, spawnLocation, Quaternion.identity).GetComponent<Player>();
        player.gameManager = this;
        OnNewPlayer?.Invoke(player);
    }

    private void resetPlayer() {
        player.transform.position = new Vector2(0, 0);
        player.transform.rotation = Quaternion.identity;
    }

}
