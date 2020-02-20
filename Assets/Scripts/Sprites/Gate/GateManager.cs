using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : SpriteManager
{
    float padding = 2.0f; //make sure gate doesn't spawn at edge of bounds
    float minX, maxX, minY, maxY;

    public override void InitSpawnLocations() {
        minX = -bounds.x + padding;
        maxX = bounds.x - padding;
        minY = -bounds.y + padding;
        maxY = bounds.y - padding;
    }

    public override void OnGameStart(GameManager gm) {
        ResetSpawnTimer();
        SpawnGateNearOrigin();
    }

    public override void SpawnSprites() {
        SpawnGate();
    }

    void SpawnGateNearOrigin() {
        SpawnGateAt(new Vector2(2.5f, 1.5f));
    }

    public void SpawnGate() {
        if (ShouldSpawn) {
            Vector2 randPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            while (Vector2.Distance(randPos, player.transform.position) < 1.5f) { 
                //make sure gate doesn't spawn close to player
                randPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            }
            SpawnGateAt(randPos);
        }
    }

    private void SpawnGateAt(Vector2 spawnPos) {
        GameObject gateObj = objectPooler.SpawnFromPool("Gate", spawnPos, Quaternion.identity);
        //Gate gate = gateObj.GetComponent<Gate>();
        sprites.Add(gateObj);
    }

}
