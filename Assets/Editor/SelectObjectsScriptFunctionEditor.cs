using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectObjectsScriptFunction))]
public class SelectObjectsScriptFunctionEditor: Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        SelectObjectsScriptFunction sosf = (SelectObjectsScriptFunction) target; //points to this SelectObjectsScriptFunction
        
        populateScriptDroplist(sosf);
        populateMethodDroplist(sosf);
    }

    void populateScriptDroplist(SelectObjectsScriptFunction sosf) {
        List<string> attachedScriptNames = new List<string>();
        GUIContent scriptListLabel = new GUIContent("Scripts");

        sosf.attachedScripts = (MonoBehaviour[]) sosf.gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour monoBehaviour in sosf.attachedScripts) {
            attachedScriptNames.Add(monoBehaviour.GetType().Name);
        }

        sosf.scriptIdx = EditorGUILayout.Popup(scriptListLabel, sosf.scriptIdx, attachedScriptNames.ToArray());
    }

    void populateMethodDroplist(SelectObjectsScriptFunction sosf) {
        List<string> methodNames = new List<string>();
        GUIContent methodListLabel = new GUIContent("Methods");
        MonoBehaviour selectedScript = sosf.attachedScripts[sosf.scriptIdx];
        sosf.scriptMethods = (MethodInfo[]) Util.GetScriptMethods(selectedScript);
        foreach (MethodInfo methodInfo in sosf.scriptMethods) {
            methodNames.Add(methodInfo.Name);
        }
        sosf.methodIdx = EditorGUILayout.Popup(methodListLabel, sosf.methodIdx, methodNames.ToArray());
    } 

}