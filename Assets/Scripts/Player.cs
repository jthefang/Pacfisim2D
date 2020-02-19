using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
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
                    CollidedWithGate(other.gameObject.GetComponent<Gate>());
                    break;
                case "Drone":
                    CollidedWithDrone(other.gameObject.GetComponent<Drone>());
                    break;
                default:
                    Debug.Log("Don't care about this collision");
                    break;
            }
        }
    }

    public void CollidedWithGate(Gate gate) {
        audioSource.PlayOneShot(hitSound);
        gate.Explode();
        Die();
    }

    public void CollidedWithDrone(Drone drone) {
        audioSource.PlayOneShot(hitSound);
        Die();
    }

    public void Die() {
        gameManager.IsGameOver = true;
    }
}
