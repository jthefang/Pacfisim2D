using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Vector2 rotateSpeedRange = new Vector2(30.0f, 50.0f);
    public Vector2 driftForceRange = new Vector2(5.0f, 10.0f);
    public Vector3 driftDirection;
    public float rotateSpeed;
    public float initialDriftForce;
    public GameObject explosionPrefab;
    public float blastRadius;

    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        InitGate();
    }

    public void InitGate() {
        float sign = Random.Range(-0.5f, 0.5f);
        rotateSpeed = Mathf.Sign(sign) * Random.Range(rotateSpeedRange.x, rotateSpeedRange.y);
        initialDriftForce = Random.Range(driftForceRange.x, driftForceRange.y);
        driftDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(driftDirection * initialDriftForce);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Spin();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Contains("Player")) {
            Explode();
        } 
    }

    void Spin() {
        transform.RotateAround(transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
    }   

    public void Explode() {
        explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector2(blastRadius, blastRadius);
        Destroy(this.gameObject);
    }
}
