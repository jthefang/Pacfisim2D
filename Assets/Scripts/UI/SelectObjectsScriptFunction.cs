/* Script to select an object's function */
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SelectObjectsScriptFunction : MonoBehaviour
{
    public GameObject gameObject;

    [HideInInspector]
    public int scriptIdx = 0; // which script attached to the gameObject to select
    [HideInInspector]
    public MonoBehaviour[] attachedScripts;

    [HideInInspector]
    public int methodIdx = 0; // which method in the script to select
    [HideInInspector]
    public MethodInfo[] scriptMethods;
    // public List<MethodInfo> scriptMethods;

    public MonoBehaviour SelectedScript() {
        return attachedScripts[scriptIdx];
    }
    public MethodInfo SelectedMethod() {
        return scriptMethods[methodIdx];
    }
}
