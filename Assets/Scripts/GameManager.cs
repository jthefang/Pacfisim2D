﻿using System.Collections;
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

public class GameManager : MonoBehaviour, ILoadableScript, IDependentScript
{
    private GameState _currGameState;
    public GameState CurrGameState
    {
        get {
            return this._currGameState;
        }

        set {  
            GameState prevGameState = this._currGameState;
            this._currGameState = value;     
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

    public GameObject playerPrefab;
    [SerializeField]
    private AudioClip gameStartSound;
    [SerializeField]
    private AudioClip gameOverSound;

    private AudioSource audioSource;
    private Player player;
    public event Action<Player> OnNewPlayer;

    public event Action<ILoadableScript> OnScriptInitialized;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        OnGameStateChange += OnGameStateChangeHandler;
        OnGameStart += OnGameStartHandler;
        OnGameOver += OnGameOverHandler;

        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void OnAllDependenciesLoaded() {
        //Game doesen't start until this happens
        spawnPlayer();
        CurrGameState = GameState.STARTING;
    }

    void OnGameStateChangeHandler(GameState prevGameState, GameState newGameState) {
        if (prevGameState != newGameState) {
            if (newGameState == GameState.STARTING) {
                OnGameStart?.Invoke(this);
            } else if (newGameState == GameState.STOPPED) {
                OnGameOver?.Invoke(this);
            }
        } 
    }

    void OnGameStartHandler(GameManager gm) {
        resetPlayer();
        audioSource.PlayOneShot(gameStartSound);
        Invoke("playMainTheme", 0.5f);
        CurrGameState = GameState.PLAYING;
    }

    void OnGameOverHandler(GameManager gm) {
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
    }

    public void RestartGame() {
        CurrGameState = GameState.STOPPED;
        CurrGameState = GameState.STARTING; //trigger start
    }

    private void playMainTheme() {
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
