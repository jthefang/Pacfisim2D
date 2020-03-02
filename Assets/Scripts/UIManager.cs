using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ILoadableScript
{
    [SerializeField]
    private GameManager gameManager;
    public GameObject gameOverPanel;
    [SerializeField]
    Text scoreIntText;
    [SerializeField]
    Text scoreMultiplierIntText;

    ScoreManager scoreManager;

    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleGameOverPanel(false);
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        scoreManager = ScoreManager.Instance;
        scoreManager.OnScoreChange += OnScoreChange;
        scoreManager.OnScoreMultiplierChange += OnScoreMultiplierChange;

        _isInitialized = true;
        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        
    }

    void OnGameStart(GameManager gm) {
        ToggleGameOverPanel(false);
    }

    void OnGameOver(GameManager gm) {
        Invoke("ShowGameOverPanel", gameManager.GAME_OVER_DELAY);
    }

    void OnScoreChange(ScoreManager scoreManager) {
        scoreIntText.text = scoreManager.Score.ToString();
    }

    void OnScoreMultiplierChange(ScoreManager scoreManager) {
        scoreMultiplierIntText.text = "x " + scoreManager.ScoreMultiplier.ToString();
    }

    void ShowGameOverPanel() {
        ToggleGameOverPanel(true);
    }

    public void ToggleGameOverPanel(bool shouldShow) {
        gameOverPanel.SetActive(shouldShow);
    }

}
