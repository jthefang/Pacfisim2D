using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        ToggleGameOverPanel(false);
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.NumResourcesLoaded += 1;
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

}
