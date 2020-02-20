using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UIManager : MonoBehaviour, ILoadableScript
{
    [SerializeField]
    private GameManager gameManager;
    public GameObject gameOverPanel;
    public event Action<ILoadableScript> OnScriptInitialized;

    // Start is called before the first frame update
    void Start()
    {
        ToggleGameOverPanel(false);
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;

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
        ToggleGameOverPanel(true);
    }

    public void ToggleGameOverPanel(bool shouldShow) {
        gameOverPanel.SetActive(shouldShow);
    }

    public void OnBeforeSerialize()
    {

    }
    
    public void OnAfterDeserialize()
    {

    }

}
