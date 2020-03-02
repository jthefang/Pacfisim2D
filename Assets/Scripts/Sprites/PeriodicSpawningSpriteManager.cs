using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    A sprite manager that periodically spawns sprites (e.g. drones or gates)
*/
public class PeriodicSpawningSpriteManager : SpriteManager
{
    [SerializeField]
    protected int spawnAmt = 15;
    [SerializeField]
    protected float spawnDelay = 3.0f;
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

    public override void InitSpriteManager() {
        audioSource = gameObject.GetComponent<AudioSource>();
        InitSpawnLocations();

        base.InitSpriteManager();
    }

    public virtual void InitSpawnLocations() {
        //pass
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.IsPlaying) {
            timeTilSpawn -= Time.deltaTime;
            if (timeTilSpawn < 0) {
                PeriodicSpawn();
            }
        }
    }

    public override void OnGameStart(GameManager gm) {
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
        timeTilSpawn = spawnDelay;
    }

    protected void PeriodicSpawn() {
        ResetSpawnTimer();
        SpawnSprites();
    }

}
