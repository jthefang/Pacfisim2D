using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteManager {
    bool ShouldSpawn { get; set; }
    void SpawnSprites();
    void DestroyAll();
    void OnGameStart(GameManager gm);
    void OnGameOver(GameManager gm);
}