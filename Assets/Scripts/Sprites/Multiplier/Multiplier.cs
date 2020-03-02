using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour, IPooledObject
{
    [SerializeField]
    float maxGravitateTime = 0.5f;
    float currGravitateTime = 0.0f;
    bool shouldGravitate;

    [SerializeField]
    float timeTilInactive = 2.0f;

    Transform gravitateTarget; 
    Vector2 originalPosition;

    ScoreManager scoreManager;
    SpriteManager spriteManager;

    AudioSource audioSource;
    [SerializeField]
    AudioClip pickupMultiplierSound;

    // Start is called before the first frame update
    void Start()
    {
        shouldGravitate = false;
        scoreManager = ScoreManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldGravitate) {
            currGravitateTime += Time.deltaTime;
            float t = currGravitateTime / maxGravitateTime;
            if (currGravitateTime > maxGravitateTime) { //just get on top of the player 
                transform.position = gravitateTarget.position;
            } else {
                float newX = Mathf.Lerp(transform.position.x, gravitateTarget.position.x, t);
                float newY = Mathf.Lerp(transform.position.y, gravitateTarget.position.y, t);
                transform.position = new Vector2(newX, newY);
            }
        }
    }

    void FacePlayer() {

    }

    /**
        This multiplier is collected by player
    */
    public void Collect(Player player) {
        audioSource.PlayOneShot(pickupMultiplierSound);
        scoreManager.ScoreMultiplier += 1;
        Die();
    }

    void Die() {
        gameObject.SetActive(false);
        shouldGravitate = false;
    }

    public void GravitateTowards(Player player) {
        gravitateTarget = player.transform;
        originalPosition = transform.position;
        currGravitateTime = 0.0f;
        shouldGravitate = true;
    }

    public void OnObjectInitiate(SpriteManager sm) {
        spriteManager = sm;
        this.transform.SetParent(sm.transform);
    }
    
    public void OnObjectSpawn() {
        Invoke("Die", timeTilInactive);
    }

}
