using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected GameManager _gameManager;
    public GameManager gameManager {
        get {
            return this._gameManager;
        }
        set {
            this._gameManager = value;
        }
    }
    public AudioClip hitSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (gameManager.IsPlaying) {
            switch (other.gameObject.tag) {
                case "Gate End":
                    CollidedWithGateEnd(other.gameObject.GetComponent<Gate>());
                    break;
                case "Drone":
                    CollidedWithDrone(other.gameObject.GetComponent<Drone>());
                    break;
                default:
                    break;
            }
        }
    }

    void CollidedWithGateEnd(Gate gate) {
        audioSource.PlayOneShot(hitSound);
        Die();
    }

    void CollidedWithDrone(Drone drone) {
        audioSource.PlayOneShot(hitSound);
        Die();
    }

    public void Die() {
        gameManager.IsGameOver = true;
    }
}
