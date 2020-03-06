﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Drone : MonoBehaviour, IPooledObject
{
    public Transform targetTransform;
    public float speed = 8.0f;
    [SerializeField]
    int numMultipliersOnDeath = 10;
    [SerializeField]
    float multiplierSpawnRadius = 2.0f;

    Collider2D collider;

    SpriteManager spriteManager;
    GameManager gameManager;
    MultiplierManager multiplierManager;
    public event Action<Drone> OnDroneDeath;
    protected ObjectPooler objectPooler;

    Color initialColor;
    SpriteRenderer bodyRenderer;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        multiplierManager = MultiplierManager.Instance;
        gameManager = GameManager.Instance;
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.IsPlaying) {
            LookAtPlayer();
            FollowPlayer();
        }
    }

    public void OnObjectInitiate(SpriteManager sm) {
        spriteManager = sm;
        this.transform.SetParent(sm.transform);

        bodyRenderer = BodyObject().GetComponent<SpriteRenderer>();
        initialColor = bodyRenderer.color;
    }

    public void OnObjectSpawn()  {
        this.targetTransform = spriteManager.player.gameObject.transform;
        bodyRenderer.color = initialColor;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Contains("Explosion")) {
            Die();
        }
    }

    void Die() {
        objectPooler.DeactivateSpriteInPool(this.gameObject);

        /**
            This is to help with performance. There's only so many multipliers we want to be spawning at a single time (especially if a huge horde of drones die at once), else the frame rate drops considerably. We just assume any extra multipliers that need to spawn will be automatically collected by the player.
        */
        if (objectPooler.AllObjectsActiveForPool("Multiplier")) { 
            Multiplier.TriggerCollect(numMultipliersOnDeath);
        } else {
            for (int i = 0; i < numMultipliersOnDeath; i++) {
                multiplierManager.SpawnSpriteAround(this.transform.position, multiplierSpawnRadius);
            }
        }

        OnDroneDeath?.Invoke(this);
    }

    void LookAtPlayer() {
        transform.right = (transform.position - targetTransform.position);
    }

    void FollowPlayer() {
        if (targetTransform != null) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, 				targetTransform.transform.position, Time.deltaTime * speed); //follow the player
        }
    }

    public GameObject BodyObject() {
        return transform.Find("inner_polygon").gameObject;
    }
}
