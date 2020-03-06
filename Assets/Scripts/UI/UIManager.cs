using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ILoadableScript, IDependentScript
{
    [SerializeField]
    private GameManager gameManager;
    public GameObject gameOverPanel;
    [SerializeField]
    Text scoreIntText;
    [SerializeField]
    Text scoreMultiplierIntText;
    [SerializeField]
    ActivatablePanel pauseMenuPanel;

    ScoreManager scoreManager;

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
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnGamePause += OnGamePause;
        gameManager.OnGameResume += OnGameResume;
        scoreManager = ScoreManager.Instance;
        scoreManager.OnScoreChange += OnScoreChange;
        scoreManager.OnScoreMultiplierChange += OnScoreMultiplierChange;
    }

    public void OnAllDependenciesLoaded() {
        ToggleGameOverPanel(false);
        pauseMenuPanel.ToggleActive(false);
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {   

    }

    void OnGamePause(GameManager gm) {
        pauseMenuPanel.ToggleActive(true);
    }

    void OnGameResume(GameManager gm) {
        pauseMenuPanel.ToggleActive(false);
    }

    void OnGameStart(GameManager gm) {
        ToggleGameOverPanel(false);
    }

    void OnGameOver(GameManager gm) {
        Invoke("ShowGameOverPanel", gameManager.GAME_OVER_DELAY);
    }

    void OnScoreChange(ScoreManager scoreManager) {
        scoreIntText.text = scoreManager.Score.ToString("n0");
    }

    void OnScoreMultiplierChange(ScoreManager scoreManager) {
        scoreMultiplierIntText.text = "x " + scoreManager.ScoreMultiplier.ToString("n0");
    }

    void ShowGameOverPanel() {
        ToggleGameOverPanel(true);
    }

    public void ToggleGameOverPanel(bool shouldShow) {
        gameOverPanel.SetActive(shouldShow);
    }

}
