using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : PeriodicSpawningSpriteManager
{
    bool firstSpawn;

    public override void OnGameStart(GameManager gm) {
        firstSpawn = true;
        base.OnGameStart(gm);
    }

    public override string GetSpriteName() {
        return "Gate";
    }

    public override Vector2 GetSpawnLocPadding() {
        GameObject gateObject = objectPooler.GetSpritePrefab(GetSpriteName());
        Vector3 ropeSize = Util.GetSizeOfSprite(gateObject.transform.Find("Rope").gameObject); 
        Vector3 gateEndSize = Util.GetSizeOfSprite(gateObject.transform.Find("Right Gate End").gameObject); 

        //same in both directions since gate can rotate and x is the long axis
        return new Vector2(ropeSize.x + 2*gateEndSize.x, ropeSize.x + 2*gateEndSize.x);
    }

    public override void SpawnSprites() {
        if (firstSpawn) {
            SpawnGateNearOrigin();
            firstSpawn = false;
        } else {
            for (int i = 0; i < SpawnAmount; i++) {
                SpawnGate();
            }
        }
    }

    void SpawnGateNearOrigin() {
        SpawnGateAt(new Vector2(2.5f, 1.5f));
    }

    public void SpawnGate() {
        if (ShouldSpawn) {
            Vector2 randPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            while (Vector2.Distance(randPos, player.transform.position) < 5 * playerSize) { 
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
