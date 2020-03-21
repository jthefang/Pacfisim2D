using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    A sprite manager that periodically spawns sprites (e.g. drones or gates)
*/
public class PeriodicSpawningSpriteManager : SpriteManager
{
    public int SpawnAmount = 15;
    public float SpawnDelay = 3.0f;
    protected float timeTilSpawn;
    AudioSource audioSource;
    [SerializeField]
    AudioClip spawnSound;

    [SerializeField]
    bool _shouldSpawn = true;
    public bool ShouldSpawn {
        get {
            return this._shouldSpawn;
        }
        set {
            this._shouldSpawn = value;
        }
    }

    protected ScoreManager scoreManager;

    public override void InitSpriteManager() {
        audioSource = gameObject.GetComponent<AudioSource>();
        scoreManager = ScoreManager.Instance;

        base.InitSpriteManager();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsInitialized() && gameManager.IsPlaying) {
            timeTilSpawn -= Time.deltaTime;
            if (timeTilSpawn < 0) {
                PeriodicSpawn();
            }
        }
    }

    public override void OnGameStart(GameManager gm) {
        base.OnGameStart(gm);
        PeriodicSpawn();
    }

    public override void OnGameOver(GameManager gm) {
        Invoke("DestroyAll", gameManager.GAME_OVER_DELAY);
    }

    void OnNewPlayer(Player player) {
        this.player = player;
    }

    public void PlaySpawnSound() {
        audioSource.PlayOneShot(spawnSound);
    }

    public void ResetSpawnTimer() {
        timeTilSpawn = SpawnDelay;
    }

    protected void PeriodicSpawn() {
        ResetSpawnTimer();
        SpawnSprites();
    }

}
