using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierManager : SpriteManager
{
    #region Singleton
    public static MultiplierManager Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        Instance = this;
    }
    #endregion

    float xPadding, yPadding;

    public override string GetSpriteName() {
        return "Multiplier";
    }

    public override Vector2 GetSpawnLocPadding() {
        GameObject multiplierObject = objectPooler.GetSpritePrefab(GetSpriteName());
        
        Vector3 multiplierSize = Util.GetSizeOfSprite(multiplierObject.transform.Find("outer_shell").gameObject); 
        return new Vector2(multiplierSize.x / 2, multiplierSize.y / 2);
    }

    public GameObject SpawnSprite(Vector2 pos, Quaternion rotation) {
        GameObject multiplierObject = objectPooler.SpawnFromPool("Multiplier", pos, rotation);
        Util.RemoveZRotation(multiplierObject.transform);

        sprites.Add(multiplierObject);
        return multiplierObject;
    }

    public GameObject SpawnSpriteAround(Vector2 pos, float spawnRadius) {
        float newMinX = Mathf.Max(minX, pos.x - spawnRadius);
        float newMaxX = Mathf.Min(pos.x + spawnRadius, maxX);
        float newMinY = Mathf.Max(minY, pos.y - spawnRadius);
        float newMaxY = Mathf.Min(pos.y + spawnRadius, maxY);
        
        Vector2 randPos = new Vector2(UnityEngine.Random.Range(newMinX, newMaxX), UnityEngine.Random.Range(newMinY, newMaxY));
        return SpawnSprite(randPos, UnityEngine.Random.rotation);
    }
}
