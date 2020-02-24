using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ILoadableScript
{
    [SerializeField]
    public GameManager gameManager;

    public GameObject spritePrefab;
    protected List<GameObject> sprites;

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

    [SerializeField]
    protected GameObject Bounds;
    protected Vector3 bounds;

    public Player player;

    protected ObjectPooler objectPooler;

    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start() {
        InitSpriteManager();
        this._isInitialized = true;
    }

    public virtual void InitSpriteManager()
    {
        objectPooler = ObjectPooler.Instance;
        audioSource = gameObject.GetComponent<AudioSource>();
        bounds = Bounds.GetComponent<SpriteRenderer>().bounds.extents;

        sprites = new List<GameObject>();
        InitSpawnLocations();

        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnNewPlayer += OnNewPlayer;

        OnScriptInitialized?.Invoke(this);
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
                SpawnSprites();
                ResetSpawnTimer();
            }
        }
    }

    public virtual void OnGameStart(GameManager gm) {
        ResetSpawnTimer();
        SpawnSprites();
    }

    public virtual void OnGameOver(GameManager gm) {
        Invoke("DestroyAll", gameManager.GAME_OVER_DELAY);
    }

    void OnNewPlayer(Player player) {
        this.player = player;
    }

    public virtual void SpawnSprites() {
        //pass
    }

    public void PlaySpawnSound() {
        audioSource.PlayOneShot(spawnSound);
    }

    public void DestroyAll() {
        foreach (GameObject sprite in sprites) {
            sprite.SetActive(false);
        }
        sprites = new List<GameObject>();
    }

    public void ResetSpawnTimer() {
        timeTilSpawn = spawnDelay;
    }

}
