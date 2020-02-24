using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Propulsion : MonoBehaviour, ILoadableScript
{
    ParticleSystem particleSystem;
    AudioSource audioSource;
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        this._isInitialized = true;
        OnScriptInitialized?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play() {
        if (!audioSource.isPlaying)
            audioSource.Play();
        if (!particleSystem.isPlaying)
            particleSystem.Play();
    }

    public void Stop() {
        audioSource.Stop();
        particleSystem.Stop();
    }
}
