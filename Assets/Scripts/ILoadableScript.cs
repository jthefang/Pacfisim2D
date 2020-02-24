using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface ILoadableScript {
    // should be triggered after the script is loaded
    event Action<ILoadableScript> OnScriptInitialized; 
    bool IsInitialized(); //dependent scripts will only wait on LoadableScripts where this is false (haven't been initialized yet), see ScriptDependencyManager.GenerateDependencyDict

    /** 
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    void Start()
    {
        ...
        _isInitialized = true;
        OnScriptInitialized?.Invoke(this);
    }
    */

    /**
        In corresponding IDependentScript
        public class PlayerController2D : MonoBehaviour, IDependentScript {
            bool propulsionLoaded;

            void Start() {
                ...
                List<ILoadableScript> dependencies = new List<ILoadableScript>();
                dependencies.Add(propulsion);
                ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
                propulsionLoaded = propulsion.IsInitialized();;
            }

            void FixedUpdate() {
                if (!propulsionLoaded) {
                    return;
                }
                ...
            }

            public void OnAllDependenciesLoaded() {
                propulsionLoaded = true;
            }
    */
}
