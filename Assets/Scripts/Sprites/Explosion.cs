using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionSound;
    public float explosionDuration = 0.1f;
    public float blastRadius;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = blastRadius;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(explosionSound);

        Invoke("SelfDestruct", explosionDuration);
    }

    void SelfDestruct() {
        //disable explosion (i.e. drones dying on impact)
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject, explosionSound.length); //wait for audio to finish
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
