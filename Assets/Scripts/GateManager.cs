using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public GameObject Bounds;
    public GameObject gatePrefab;
    public int spawnAmt = 15;
    public float spawnDelay = 3.0f;
    public AudioClip spawnSound;
    public bool shouldSpawnGates = true;

    private float timeTilSpawn;
    private Vector3 bounds;
    private List<GameObject> gates;

    // Start is called before the first frame update
    void Start()
    {
        bounds = Bounds.GetComponent<SpriteRenderer>().bounds.extents;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeTilSpawn -= Time.deltaTime;
        if (shouldSpawnGates && timeTilSpawn < 0) {
            SpawnGate();
            ResetSpawnTimer();
        }
    }

    public void SpawnGateNearOrigin() {
        SpawnGateAt(new Vector2(1.5f, 1.5f));
    }

    public void SpawnGate() {
        float padding = 1.5f; //make sure gate doesn't spawn at edge of bounds
        Vector2 randPos = new Vector2(Random.Range(-bounds.x + padding, bounds.x - padding), Random.Range(-bounds.y + padding, bounds.y - padding));
        SpawnGateAt(randPos);
    }

    private void SpawnGateAt(Vector2 spawnPos) {
        Gate gate = Instantiate(gatePrefab, spawnPos, Quaternion.identity).GetComponent<Gate>();
        gate.transform.SetParent(this.transform);
        gates.Add(gate.gameObject);
    }

    public void SetShouldSpawnGates(bool shouldSpawn) {
        shouldSpawnGates = shouldSpawn;
    }

    public void DestroyAllGates() {
        foreach (GameObject gate in gates) {
            Destroy(gate);
        }
    }

    public void ResetSpawnTimer() {
        timeTilSpawn = spawnDelay;
    }

    public void Reset() {
        if (gates != null) {
            DestroyAllGates();
        }
        gates = new List<GameObject>();
        ResetSpawnTimer();
        SetShouldSpawnGates(true);
        SpawnGateNearOrigin();
    }

}
