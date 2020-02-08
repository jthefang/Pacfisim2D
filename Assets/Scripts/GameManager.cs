using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        STOPPED, //e.g. for game over
        PAUSED,
        PLAYING,
    }

    public CameraRig cameraRig;
    public GameObject playerPrefab;
    public DroneManager droneManager;
    public GateManager gateManager;
    public UIManager uiManager;
    public AudioClip gameStartSound;
    public AudioClip gameOverSound;

    private GameState currGameState;
    private AudioSource audioSource;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameState GetGameState() {
        return currGameState;
    }

    void onPlayerDeath(Player player) {
        GameOver();
	}

    public void GameOver() {
        SetGameStateTo(GameState.STOPPED);
        uiManager.ToggleGameOverPanel(true);
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);

        Destroy(player.gameObject);
        droneManager.SetShouldSpawnDrones(false);
        droneManager.DestroyAllDrones();
        gateManager.SetShouldSpawnGates(false);
        gateManager.DestroyAllGates();
    }

    public void RestartGame() {
        uiManager.ToggleGameOverPanel(false);
        StartGame();
    }

    public void StartGame() {
        spawnPlayer();
        droneManager.InitDroneManager();
        droneManager.player = player;
        droneManager.SpawnDrones();
        gateManager.Reset();

        SetGameStateTo(GameState.PLAYING);
        audioSource.PlayOneShot(gameStartSound);
        Invoke("playMainTheme", 0.5f);
    }
    
    public void SetGameStateTo(GameState gs) {
        currGameState = gs;
    }

    private void playMainTheme() {
        audioSource.Play();
    }

    private void spawnPlayer() {
        Vector2 spawnLocation = new Vector2(0, 0);
        player = Instantiate(playerPrefab, spawnLocation, Quaternion.identity).GetComponent<Player>();
        player.gameManager = this;
		player.onPlayerDeath += onPlayerDeath; //subscribes for the onPlayerDeath event
        cameraRig.target = player.gameObject;
    }

}
