using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : SpriteManager
{
    public override void OnGameStart(GameManager gm) {
        ResetSpawnTimer();
        SpawnGateNearOrigin();
    }

    public override void SpawnSprites() {
        SpawnGate();
    }

    void SpawnGateNearOrigin() {
        SpawnGateAt(new Vector2(1.5f, 1.5f));
    }

    public void SpawnGate() {
        if (ShouldSpawn) {
            float padding = 1.5f; //make sure gate doesn't spawn at edge of bounds
            Vector2 randPos = new Vector2(Random.Range(-bounds.x + padding, bounds.x - padding), Random.Range(-bounds.y + padding, bounds.y - padding));
            SpawnGateAt(randPos);
        }
    }

    private void SpawnGateAt(Vector2 spawnPos) {
        Gate gate = Instantiate(spritePrefab, spawnPos, Quaternion.identity).GetComponent<Gate>();
        gate.transform.SetParent(this.transform);
        sprites.Add(gate.gameObject);
    }

}
