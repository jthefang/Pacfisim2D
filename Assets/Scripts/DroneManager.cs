using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : SpriteManager
{   
    private enum Corner {
        TOPLEFT = 0,
        TOPRIGHT = 1,
        BOTLEFT = 2,
        BOTRIGHT = 3
    };
    private float[,] spawnLocations;
    private Player player;

    // Start is called before the first frame update
    public override void InitSpriteManager()
    {
        initSpawnLocations();
        gameManager.OnNewPlayer += OnNewPlayer;
        base.InitSpriteManager();
    }

    void OnNewPlayer(Player player) {
        this.player = player;
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

    public override void SpawnSprites() {
        if (ShouldSpawn) {
            int spawnIdx = Random.Range(0, 4);
            //Make sure this spawn location is not where the player currently is (within a margin)
            Vector3 playerPos = player.transform.position;
            float margin = 2;
            bool withinX = (playerPos.x >= spawnLocations[spawnIdx, 0] - margin) && (playerPos.x <= spawnLocations[spawnIdx, 1] + margin);
            bool withinY = (playerPos.y >= spawnLocations[spawnIdx, 2] - margin) && (playerPos.y <= spawnLocations[spawnIdx, 3] + margin);
            if (withinX && withinY) {
                spawnIdx = (spawnIdx + 1) % 4;
            }
            
            PlaySpawnSound();
            float padding = 0.5f; //make sure drone doesn't spawn at edge
            for (int i = 0; i < spawnAmt; i++) {
                Vector2 spawnLoc = new Vector2(Random.Range(spawnLocations[spawnIdx, 0] + padding, spawnLocations[spawnIdx, 1] - padding), Random.Range(spawnLocations[spawnIdx, 2] + padding, spawnLocations[spawnIdx, 3] - padding));
                
                Drone drone = Instantiate(spritePrefab, spawnLoc, Quaternion.identity).GetComponent<Drone>();
                drone.targetTransform = player.gameObject.transform;
                drone.transform.SetParent(this.transform);
                sprites.Add(drone.gameObject);
            }
        }
    }

}