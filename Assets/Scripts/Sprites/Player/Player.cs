using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    bool canDie = true; //for play testing (god mode) set this to false

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
        HighlightGateEnd(gate);
        gate.DisableExplosion();
        audioSource.PlayOneShot(hitSound);
        Die();
    }

    void CollidedWithDrone(Drone drone) {
        if (drone.HasCooledDown()) {
            HighlightDrone(drone);
            audioSource.PlayOneShot(hitSound);
            Die();
        }
    }

    void HighlightDrone(Drone drone) {
        drone.transform.Find("inner_polygon").GetComponent<SpriteRenderer>().color = Color.white;
    }

    void HighlightGateEnd(Gate gate) {
        GameObject leftGate = gate.LeftGateEnd();
        GameObject rightGate = gate.RightGateEnd();
        float distToLeftGate = Vector2.Distance(leftGate.transform.position, transform.position);
        float distToRightGate = Vector2.Distance(rightGate.transform.position, transform.position);
        if (distToLeftGate < distToRightGate) { //highlight whichever gate is closest to player
            leftGate.GetComponent<SpriteRenderer>().color = Color.white;
        } else {
            rightGate.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void Die() {
        if (canDie)
            gameManager.IsGameOver = true;
    }
}
