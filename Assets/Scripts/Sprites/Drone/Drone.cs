using System.Collections;
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

    SpriteManager spriteManager;
    GameManager gameManager;
    MultiplierManager multiplierManager;
    public event Action<Drone> OnDroneDeath;
    protected ObjectPooler objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        multiplierManager = MultiplierManager.Instance;
        gameManager = GameManager.Instance;
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
    }

    public void OnObjectSpawn()  {
        this.targetTransform = spriteManager.player.gameObject.transform;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Contains("Explosion")) {
            Die();
        }
    }

    void Die() {
        gameObject.SetActive(false);

        for (int i = 0; i < numMultipliersOnDeath; i++) {
            GameObject multiplierObject = multiplierManager.SpawnSpriteAround(this.transform.position, multiplierSpawnRadius);
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
}
