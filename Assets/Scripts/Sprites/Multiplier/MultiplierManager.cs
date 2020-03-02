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

    public override void OnGameStart(GameManager gm) {
        GameObject multiplierObject = objectPooler.GetSpritePrefab("Multiplier");
        
        Vector3 multiplierSize = Util.GetSizeOfSprite(multiplierObject.transform.Find("outer_shell").gameObject); 
        //make sure multiplier doesn't spawn near edge
        xPadding = multiplierSize.x / 2;
        yPadding = multiplierSize.y / 2;
    }

    public GameObject SpawnSprite(Vector2 pos, Quaternion rotation) {
        GameObject multiplierObject = objectPooler.SpawnFromPool("Multiplier", pos, rotation);
        Util.RemoveZRotation(multiplierObject.transform);

        sprites.Add(multiplierObject);
        return multiplierObject;
    }

    public GameObject SpawnSpriteAround(Vector2 pos, float spawnRadius) {
        float minX = Mathf.Max(-gameManager.bounds.x + xPadding, pos.x - spawnRadius);
        float maxX = Mathf.Min(pos.x + spawnRadius, gameManager.bounds.x - xPadding);
        float minY = Mathf.Max(-gameManager.bounds.y + yPadding, pos.y - spawnRadius);
        float maxY = Mathf.Min(pos.y + spawnRadius, gameManager.bounds.y - yPadding);
        
        Vector2 randPos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        return SpawnSprite(randPos, UnityEngine.Random.rotation);
    }
}
