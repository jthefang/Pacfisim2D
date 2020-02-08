using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float speed;
    public GameObject target;

    private Transform rigTransform;

    // Start is called before the first frame update
    void Start()
    {
        rigTransform = this.transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (target == null) {
            return;
        }
        transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z); // Camera follows the targetwith specified offset position
    }
}
