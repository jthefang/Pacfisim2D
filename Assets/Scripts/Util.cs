using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Util {
    public static void RemoveZRotation(Transform transform) {
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z);
        transform.rotation = q;
    }

    public static Vector3 GetSizeOfSprite(GameObject spriteObject) {
        return spriteObject.GetComponent<SpriteRenderer>().bounds.size;
    }
}
