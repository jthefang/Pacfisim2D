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
    }

    #region Singleton
    public static ObjectPooler Instance;
    //reference this only version as ObjectPooler.Instance.SpawnFromPool("Drone", randPos, Quaternion.Identity);

    private void Awake() {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary; 
    public event Action<ILoadableScript> OnScriptInitialized;

    void Start() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();  

        foreach (Pool pool in pools)  {
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
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        OnScriptInitialized?.Invoke(this);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null) {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn); //if we reach our max number of objects, reuse this guy

        return objectToSpawn;
    }

    public void OnBeforeSerialize()
    {

    }
    
    public void OnAfterDeserialize()
    {

    }

}
