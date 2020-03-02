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

    [SerializeField]
    AudioClip hitSound;
    AudioSource audioSource;

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
                case "Drone":
                    CollidedWithDrone(other.gameObject.GetComponent<Drone>());
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (gameManager.IsPlaying) {
            switch (other.gameObject.tag) {
                case "Gate End":
                    CollidedWithGateEnd(other.gameObject.GetComponent<Gate>());
                    break;
                case "Gate Rope":
                    CollidedWithGateRope(other.gameObject);
                    break;
                case "MultiplierGravField":
                    other.gameObject.GetComponent<Multiplier>().GravitateTowards(this);
                    break;
                case "MultiplierHitBox":
                    GameObject multiplierObject = other.gameObject.transform.parent.gameObject;multiplierObject.GetComponent<Multiplier>().Collect(this);
                    break;
                default:
                    break;
            }
        }
    }

    void CollidedWithGateRope(GameObject gateRopeObject) {
        Gate gate = gateRopeObject.transform.parent.gameObject.GetComponent<Gate>();
        gate.Explode();
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
