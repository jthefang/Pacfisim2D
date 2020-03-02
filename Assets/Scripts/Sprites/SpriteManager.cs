using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ILoadableScript
{
    protected Vector2 spawnLocPadding; //make sure sprite doesn't spawn at edge of bounds
    protected float minX, maxX, minY, maxY;
    
    protected GameManager gameManager;
    public Player player;
    protected ObjectPooler objectPooler;
    protected List<GameObject> sprites;

    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPooler.Instance;

        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnNewPlayer += OnNewPlayer;

        InitSpriteManager();
        this._isInitialized = true;
    }

    public virtual void InitSpriteManager()
    {
        sprites = new List<GameObject>();
        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public virtual string GetSpriteName() {
        return "";
    }

    /**
        Get padding around spawn variables s.t. all of a sprite spawns inbounds
    */
    public virtual Vector2 GetSpawnLocPadding() {
        GameObject spriteObject = objectPooler.GetSpritePrefab(GetSpriteName());
        Vector3 spriteSize = Util.GetSizeOfSprite(spriteObject); 
        return new Vector2(spriteSize.x / 2, spriteSize.y / 2);
    }

    /**
        Init spawn variables such that sprites never spawn out of bounds
    */
    public virtual void InitSpawnLocations() {
        spawnLocPadding = GetSpawnLocPadding();
        minX = -gameManager.bounds.x + spawnLocPadding.x;
        maxX = gameManager.bounds.x - spawnLocPadding.x;
        minY = -gameManager.bounds.y + spawnLocPadding.y;
        maxY = gameManager.bounds.y - spawnLocPadding.y;
    }

    void OnNewPlayer(Player player) {
        this.player = player;
    }

    public virtual void OnGameStart(GameManager gm) {
        InitSpawnLocations();
    }

    public virtual void OnGameOver(GameManager gm) {
        Invoke("DestroyAll", gameManager.GAME_OVER_DELAY);
    }


    public virtual void SpawnSprites() {
        //pass
    }

    public void DestroyAll() {
        foreach (GameObject sprite in sprites) {
            sprite.SetActive(false);
        }
        sprites = new List<GameObject>();
    }

}
