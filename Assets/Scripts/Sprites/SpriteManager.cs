using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ILoadableScript
{
    protected GameManager gameManager;
    protected Vector3 bounds;
    protected List<GameObject> sprites;

    public Player player;

    protected ObjectPooler objectPooler;

    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.Instance;
        bounds = gameManager.bounds;
        InitSpriteManager();
        this._isInitialized = true;
    }

    public virtual void InitSpriteManager()
    {
        objectPooler = ObjectPooler.Instance;
        sprites = new List<GameObject>();

        gameManager.OnGameStart += OnGameStart;
        gameManager.OnGameOver += OnGameOver;
        gameManager.OnNewPlayer += OnNewPlayer;

        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public virtual void OnGameStart(GameManager gm) {
        
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

    public void DestroyAll() {
        foreach (GameObject sprite in sprites) {
            sprite.SetActive(false);
        }
        sprites = new List<GameObject>();
    }

}
