using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public static class Util {
    public static void RemoveZRotation(Transform transform) {
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z);
        transform.rotation = q;
    }

    public static Vector3 GetSizeOfSprite(GameObject spriteObject) {
        return spriteObject.GetComponent<SpriteRenderer>().bounds.size;
    }

    public static MethodInfo[] GetScriptMethods(MonoBehaviour monoBehaviour) {
        // BindingFlags is located in System.Reflection - modify these to your liking to get the methods you're interested in
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly; //only public methods I declared
        
        List<MethodInfo> methods = new List<MethodInfo>();
        methods.AddRange(monoBehaviour.GetType().GetMethods(flags));  

        return methods.ToArray();
    }
}
