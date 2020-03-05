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

    // How the multiplier pellet drifts after spawning from the ashes of a drone
    Vector3 driftDirection; //should be random
    [SerializeField]
    float driftSpeed = 0.1f; //should be really small

    ScoreManager scoreManager;
    ObjectPooler objectPooler;
    SpriteManager spriteManager;
    GameManager gameManager;

    /*AudioSource audioSource;
    [SerializeField]
    AudioClip pickupMultiplierSound;*/

    // Start is called before the first frame update
    void Start()
    {
        shouldGravitate = false;
        scoreManager = ScoreManager.Instance;
        objectPooler = ObjectPooler.Instance;
        gameManager = GameManager.Instance;
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.IsPlaying) {
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
            } else {
                Drift();
            }
        }
    }

    void FacePlayer() {

    }

    public static void TriggerCollect(int numTimes) {
        ScoreManager.Instance.ScoreMultiplier += 1 * numTimes;
    }

    /**
        This multiplier is collected by player
    */
    public void Collect(Player player) {
        //audioSource.PlayOneShot(pickupMultiplierSound);
        TriggerCollect(1);
        Die();
    }

    void Die() {
        objectPooler.DeactivateSpriteInPool(this.gameObject);
        shouldGravitate = false;
    }

    /**
        This multiplier's "grav field" (Box Collider) is run into by player
    */
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

    void Drift() {
        transform.position += driftSpeed * driftDirection * Time.deltaTime;
    }

    public void Bounce() {
        driftDirection *= -1;
    }
    
    public void OnObjectSpawn() {
        driftDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        driftDirection.Normalize();
        Invoke("Die", timeTilInactive);
    }

}
