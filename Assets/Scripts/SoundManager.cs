using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour, ILoadableScript
{
    #region Singleton
    public static SoundManager Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

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

    AudioSource audioSource;

    GameManager gameManager;
    GameMusicTheme musicTheme;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnGamePause += OnGamePause;
        gameManager.OnGameResume += OnGameResume;

        audioSource = GetComponent<AudioSource>();
        musicTheme = GameSettings.Instance.MusicTheme;
        audioSource.clip = musicTheme.mainTheme;
        
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region GameStateEventHandlers
    void OnGameStart(GameManager gm) {
        PlayGameStart();
        Invoke("PlayMainTheme", 0.5f);
    }

    void OnGameOver(GameManager gm) {
        StopMusicAndPlayGameOver();
    }

    void OnGamePause(GameManager gm) {
        PauseMusic();
    }

    void OnGameResume(GameManager gm) {
        ResumeMusic();
    }
    #endregion

    public void PlayGameStart() {
        if (musicTheme.startSound != null)
            audioSource.PlayOneShot(musicTheme.startSound);
    }

    public void PlayMainTheme() {
        if (gameManager.IsPlaying)
            audioSource.Play();
    }

    public void PauseMusic() {
        audioSource.Pause();
    }

    public void ResumeMusic() {
        audioSource.Play();
    }

    public void StopMusicAndPlayGameOver() {
        audioSource.Stop();
        audioSource.PlayOneShot(musicTheme.loseSound);
    }

}
