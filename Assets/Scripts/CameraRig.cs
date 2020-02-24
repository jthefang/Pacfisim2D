using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraRig : MonoBehaviour, ILoadableScript
{
    public float speed;
    public GameObject target;

    private Transform rigTransform;

    [SerializeField]
    private GameManager gameManager;
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigTransform = this.transform.parent;
        gameManager.OnNewPlayer += OnNewPlayer;

        this._isInitialized = true;
        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (target != null) {
            transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z); // Camera follows the targetwith specified offset position
        }
    }

    void OnNewPlayer(Player player) {
        this.target = player.gameObject;
    }
    
}
