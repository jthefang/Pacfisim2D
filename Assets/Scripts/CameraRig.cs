using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float speed;
    public GameObject target;

    private Transform rigTransform;

    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rigTransform = this.transform.parent;
        gameManager.OnNewPlayer += OnNewPlayer;
        gameManager.NumResourcesLoaded += 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (target != null) {
            transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z); // Camera follows the targetwith specified offset position
        }
    }

    void OnNewPlayer(Player player) {
        this.target = player.gameObject;
    }
}
