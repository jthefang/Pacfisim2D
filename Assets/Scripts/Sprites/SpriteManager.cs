using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ILoadableScript, IDependentScript
{
    protected Vector2 spawnLocPadding; //make sure sprite doesn't spawn at edge of bounds
    protected float minX, maxX, minY, maxY;
    protected float playerSize;
    
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
        
    }

    public void OnAllDependenciesLoaded() {
        InitSpriteManager();
        this._isInitialized = true;
    }

    public virtual void InitSpriteManager()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnNewPlayer += OnNewPlayer;

        objectPooler = ObjectPooler.Instance;
        sprites = new List<GameObject>();

        InitSpawnLocations();
        
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
        minX = -gameManager.GameBounds.x + spawnLocPadding.x;
        maxX = gameManager.GameBounds.x - spawnLocPadding.x;
        minY = -gameManager.GameBounds.y + spawnLocPadding.y;
        maxY = gameManager.GameBounds.y - spawnLocPadding.y;
    }

    void OnNewPlayer(Player player) {
        this.player = player;
        Vector2 playerSizeVector = Util.GetSizeOfSprite(player.gameObject);
        playerSize = Mathf.Max(playerSizeVector.x, playerSizeVector.y);
    }

    public virtual void OnGameStart(GameManager gm) {
        //pass
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
