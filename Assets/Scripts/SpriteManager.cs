using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ISpriteManager
{
    [SerializeField]
    protected GameManager gameManager;

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

    // Start is called before the first frame update
    void Start() {
        InitSpriteManager();
    }

    public virtual void InitSpriteManager()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        bounds = Bounds.GetComponent<SpriteRenderer>().bounds.extents;

        sprites = new List<GameObject>();

        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.NumResourcesLoaded += 1;
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

    public void OnGameOver(GameManager gm) {
        DestroyAll();
    }

    public virtual void SpawnSprites() {
        //pass
    }

    public void PlaySpawnSound() {
        audioSource.PlayOneShot(spawnSound);
    }

    public void DestroyAll() {
        foreach (GameObject sprite in sprites) {
            Destroy(sprite);
        }
        sprites = new List<GameObject>();
    }

    public void ResetSpawnTimer() {
        timeTilSpawn = spawnDelay;
    }

}
