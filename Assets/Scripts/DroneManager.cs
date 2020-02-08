using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public GameObject Bounds;
    public GameObject dronePrefab;
    public int spawnAmt = 15;
    public float spawnDelay = 3.0f;
    public Player player;
    public AudioClip spawnSound;
    public bool shouldSpawnDrones = true;
    
    private enum Corner {
        TOPLEFT = 0,
        TOPRIGHT = 1,
        BOTLEFT = 2,
        BOTRIGHT = 3
    };
    private float[,] spawnLocations;
    private List<GameObject> drones;
    private float timeTilSpawn;
    private bool isInitialized = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeTilSpawn -= Time.deltaTime;
        if (shouldSpawnDrones && timeTilSpawn < 0) { //time to spawnDrones
            SpawnDrones();
            ResetSpawnTimer();
        }
    }

    public void InitDroneManager() {
        if (!isInitialized) {
            audioSource = gameObject.GetComponent<AudioSource>();
            initSpawnLocations();
            timeTilSpawn = spawnDelay;
            drones = new List<GameObject>();
            isInitialized = true;
        } else {
            Reset();
        }
    }

    /**
        Can spawn in (top/bot, left/right), determined by the bounds
            Each spawn corner is 1/4 of the width and height of the bounds
    */
    void initSpawnLocations() {
        Vector3 bounds = Bounds.GetComponent<SpriteRenderer>().bounds.extents;
        spawnLocations = new float[4,4] {
            {-bounds.x, -bounds.x / 2, bounds.y / 2, bounds.y}, 
            {bounds.x / 2, bounds.x, bounds.y / 2, bounds.y}, 
            {-bounds.x, -bounds.x / 2, -bounds.y, -bounds.y / 2}, 
            {bounds.x / 2, bounds.x, -bounds.y, -bounds.y / 2}
        };
    }

    public void SpawnDrones() {
        int spawnIdx = Random.Range(0, 4);
        //Make sure this spawn location is not where the player currently is (within a margin)
        Vector3 playerPos = player.transform.position;
        float margin = 2;
        bool withinX = (playerPos.x >= spawnLocations[spawnIdx, 0] - margin) && (playerPos.x <= spawnLocations[spawnIdx, 1] + margin);
        bool withinY = (playerPos.y >= spawnLocations[spawnIdx, 2] - margin) && (playerPos.y <= spawnLocations[spawnIdx, 3] + margin);
        if (withinX && withinY) {
            spawnIdx = (spawnIdx + 1) % 4;
        }
        
        audioSource.PlayOneShot(spawnSound);
        float padding = 0.5f; //make sure drone doesn't spawn at edge
        for (int i = 0; i < spawnAmt; i++) {
            Vector2 spawnLoc = new Vector2(Random.Range(spawnLocations[spawnIdx, 0] + padding, spawnLocations[spawnIdx, 1] - padding), Random.Range(spawnLocations[spawnIdx, 2] + padding, spawnLocations[spawnIdx, 3] - padding));
            
            Drone drone = Instantiate(dronePrefab, spawnLoc, Quaternion.identity).GetComponent<Drone>();
            drone.targetTransform = player.gameObject.transform;
            drone.transform.SetParent(this.transform);
            drones.Add(drone.gameObject);
        }
    }

    public void SetShouldSpawnDrones(bool shouldSpawn) {
        shouldSpawnDrones = shouldSpawn;
    }

    public void ResetSpawnTimer() {
        timeTilSpawn = spawnDelay;
    }

    public void DestroyAllDrones() {
        foreach (GameObject drone in drones) {
            Destroy(drone);
        }
    }

    public void Reset() {
        ResetSpawnTimer();
        SetShouldSpawnDrones(true);
    }

}
