using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, ILoadableScript {

    [System.Serializable] // for this to show up in the inspector
    public class Pool { //add pools in the inspector
        public string tag;
        public GameObject prefab;
        public SpriteManager spriteManager;
        public int size; //at this point we'll start reusing objects instead of instantiating new ones

        int _numActive = 0;
        public int NumActive {
            get {
                return this._numActive;
            }
            set {
                this._numActive = value;
            }
        }
    }

    #region Singleton
    public static ObjectPooler Instance;
    //reference this only version as ObjectPooler.Instance.SpawnFromPool("Drone", randPos, Quaternion.Identity);

    private void Awake() {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    Dictionary<string, Pool> tagToPoolObjectDictionary;
    Dictionary<GameObject, string> objectToTagDictionary;
    public Dictionary<string, Queue<GameObject>> tagToPoolQueueDictionary; 
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    public bool IsInitialized () {
        return this._isInitialized;
    }

    void Start() {
        tagToPoolObjectDictionary = new Dictionary<string, Pool>();
        objectToTagDictionary = new Dictionary<GameObject, string>();
        tagToPoolQueueDictionary = new Dictionary<string, Queue<GameObject>>();  

        foreach (Pool pool in pools)  {
            tagToPoolObjectDictionary[pool.tag] = pool;

            // create a Q for each pool
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // populate Q with `size` objects
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); //not yet in the game
                IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
                if (pooledObj != null) {
                    pooledObj.OnObjectInitiate(pool.spriteManager);
                }
                objectPool.Enqueue(obj);
                objectToTagDictionary[obj] = pool.tag;
            }

            tagToPoolQueueDictionary.Add(pool.tag, objectPool);
        }

        this._isInitialized = true;
        OnScriptInitialized?.Invoke(this);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!tagToPoolQueueDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = tagToPoolQueueDictionary[tag].Dequeue();

        if (!objectToSpawn.activeSelf) { //else we're reusing already active object (i.e. reached max)
            tagToPoolObjectDictionary[tag].NumActive += 1;
        }
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null) {
            pooledObj.OnObjectSpawn();
        }

        tagToPoolQueueDictionary[tag].Enqueue(objectToSpawn); //if we reach our max number of objects, reuse this guy
        return objectToSpawn;
    }

    public GameObject GetSpritePrefab(string tag) {
        return tagToPoolObjectDictionary[tag].prefab;
    }

    /**
        Deactivate/kills a sprite spawned from a pool maintained by this object pooler
    */
    public void DeactivateSpriteInPool(GameObject spriteObject) {
        if (!objectToTagDictionary.ContainsKey(spriteObject)) {
            Debug.LogError("Trying to deactivate a sprite game object that does not belong to this object pooler.");
            return;
        }

        spriteObject.SetActive(false);
        string spriteTag = objectToTagDictionary[spriteObject];
        tagToPoolObjectDictionary[spriteTag].NumActive -= 1;
    }

    public bool AllObjectsActiveForPool(string poolTag) {
        Pool p = tagToPoolObjectDictionary[poolTag];
        return p.NumActive >= p.size;
    }

}
