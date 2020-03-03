using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : PeriodicSpawningSpriteManager
{  
    private enum Corner {
        TOPLEFT = 0,
        TOPRIGHT = 1,
        BOTLEFT = 2,
        BOTRIGHT = 3
    };
    private float[,] spawnLocations;

    /**
        Can spawn in (top/bot, left/right), determined by the bounds
            Each spawn corner is 1/4 of the width and height of the bounds
    */
    public override void InitSpawnLocations() {
        base.InitSpawnLocations();
        spawnLocations = new float[4,4] {
            {minX, -gameManager.GameBounds.x / 2, gameManager.GameBounds.y / 2, maxY}, //top left sixteenth quadrant
            {gameManager.GameBounds.x / 2, maxX, gameManager.GameBounds.y / 2, maxY}, //top right
            {minX, -gameManager.GameBounds.x / 2, minY, -gameManager.GameBounds.y / 2}, //bot left
            {gameManager.GameBounds.x / 2, maxX, minY, -gameManager.GameBounds.y / 2} //bot right
        };
    }

    public override string GetSpriteName() {
        return "Drone";
    }

    public override void SpawnSprites() {
        if (ShouldSpawn) {
            int spawnIdx = GetSpawnCornerIdx();
            
            PlaySpawnSound();
            float padding = 0.5f; //make sure drone doesn't spawn at edge
            for (int i = 0; i < SpawnAmount; i++) {
                Vector2 spawnLoc = new Vector2(Random.Range(spawnLocations[spawnIdx, 0] + padding, spawnLocations[spawnIdx, 1] - padding), Random.Range(spawnLocations[spawnIdx, 2] + padding, spawnLocations[spawnIdx, 3] - padding));
                
                GameObject droneObj = objectPooler.SpawnFromPool("Drone", spawnLoc, Quaternion.identity);
                droneObj.GetComponent<Drone>().OnDroneDeath += OnDroneDeath;
                sprites.Add(droneObj);
            }
        }
    }

    int GetSpawnCornerIdx() {
        int spawnIdx = Random.Range(0, 4);

        //Make sure this spawn location is not where the player currently is (within a margin)
        Vector3 playerPos = player.transform.position;
        float margin = 5*playerSize;
        bool withinX = (playerPos.x >= spawnLocations[spawnIdx, 0] - margin) && (playerPos.x <= spawnLocations[spawnIdx, 1] + margin);
        bool withinY = (playerPos.y >= spawnLocations[spawnIdx, 2] - margin) && (playerPos.y <= spawnLocations[spawnIdx, 3] + margin);

        //Debug.Log("(minX, minY) -> (maxX, maxY) : " + spawnLocations[spawnIdx, 0] + ", " + spawnLocations[spawnIdx, 2] + " -> " + + spawnLocations[spawnIdx, 1] + ", " + spawnLocations[spawnIdx, 3]);
        if (withinX && withinY) { //move to next spawn location if so...
            spawnIdx = (spawnIdx + 1) % 4;
        }
        return spawnIdx;
    }

    void OnDroneDeath(Drone drone) {
        scoreManager.UpdateScoreWithEvent("droneDeath");
    }

    public override void OnGameOver(GameManager gm) {
        foreach (GameObject drone in sprites) {
            drone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        base.OnGameOver(gm);
    }

}