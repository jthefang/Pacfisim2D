using UnityEngine;

public interface IPooledObject {
    void OnObjectInitiate(SpriteManager sm);
    void OnObjectSpawn();
}
