using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActivatablePanel : MonoBehaviour, ILoadableScript
{
    #region ILoadableScript
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    bool isInitialized {
        get {
            return this._isInitialized;
        }
        set {
            this._isInitialized = value;
            if (this._isInitialized) {
                OnScriptInitialized?.Invoke(this);
            }
        }   
    }
    public bool IsInitialized () {
        return isInitialized;
    }
    #endregion

    bool isActive {
        get {
            return gameObject.activeSelf;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isInitialized = true;
        ToggleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipActive() {
        ToggleActive(!isActive);
    }

    public void ToggleActive(bool isActive) {
        gameObject.SetActive(isActive);
    }

    public void Open() {
        ToggleActive(true);
    }

    public void Close() {
        ToggleActive(false);
    }

}
