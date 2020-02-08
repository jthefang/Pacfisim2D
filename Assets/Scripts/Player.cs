using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<Player> onPlayerDeath;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (gameManager.GetGameState() == GameManager.GameState.PLAYING) {
            if (other.gameObject.tag.Contains("Gate End")) {
                CollidedWithGate(other.gameObject.GetComponent<Gate>());
                return;
            }     
            Drone drone = other.gameObject.GetComponent<Drone>();
            if (drone) {
                CollidedWithDrone(drone);
            } 
        }
    }

    public void CollidedWithGate(Gate gate) {
        if (onPlayerDeath != null) {
            gate.Explode();
            onPlayerDeath(this);
        }
    }

    public void CollidedWithDrone(Drone drone) {
        if (onPlayerDeath != null) {
            onPlayerDeath(this);
        }
    }
}
