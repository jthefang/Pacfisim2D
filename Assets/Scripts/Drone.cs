﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Transform targetTransform;
    public float speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAtPlayer();
        FollowPlayer();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Contains("Explosion")) {
            Die();
        }
    }

    void Die() {
        Destroy(this.gameObject);
    }

    void LookAtPlayer() {
        transform.right = (transform.position - targetTransform.position);
    }

    void FollowPlayer() {
        if (targetTransform != null) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, 				targetTransform.transform.position, Time.deltaTime * speed); //follow the player
        }
    }
}
