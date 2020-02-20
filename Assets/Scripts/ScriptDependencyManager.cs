using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    Ensures we load all script dependencies in the right order

    DependentScripts = List of scripts that depend on other scripts to load first, before they can load
        must be of type IDependentScript

    ScriptDependencies = List of lists of scripts that the dependent scripts wait on to load
        must be of type ILoadableScript

    Each IDependentScript in DependentScripts should have a corresponding list of ILoadableScript's in ScriptDepndencies 
*/
public class ScriptDependencyManager : MonoBehaviour
{
    [SerializeField]
    List<MonoBehaviour> dependentScripts;
    [SerializeField]
    ScriptDependencies scriptDependencies;
    //maps dependent script --> scripts it's waiting on to load
    Dictionary<IDependentScript, List<ILoadableScript>> dependencyDictionary;
    // maps scripts --> scripts waiting on it to load
    Dictionary<ILoadableScript, List<IDependentScript>> reverseDependencyDict;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDependencyDict();
        GenerateReverseDict();
        SubscribeToAllDependencies();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void GenerateDependencyDict() {
        if (dependentScripts.Count != scriptDependencies.list.Count) {
            Debug.LogError("There should be a list of script dependencies for every dependent script in Dependent Scripts. Found " + dependentScripts.Count + " dependent scripts but only " + scriptDependencies.list.Count + " lists of script dependencies.");
            return;
        }

        dependencyDictionary = new Dictionary<IDependentScript, List<ILoadableScript>>();
        for (int i = 0; i < dependentScripts.Count; i++) {
            MonoBehaviour script = dependentScripts[i];
            if (script is IDependentScript) {
                List<ILoadableScript> dependencies = new List<ILoadableScript>();
                foreach (MonoBehaviour dependency in scriptDependencies.list[i].list) {
                    if (dependency is ILoadableScript) {
                        dependencies.Add((ILoadableScript) dependency);
                    } else {
                        Debug.LogError(dependency.GetType().Name + " is not an ILoadableScript.");
                    }
                }
                dependencyDictionary[(IDependentScript) script] = dependencies;
            } else {
                Debug.LogError(script.GetType().Name + " is not an IDependentScript.");
            }   
        }
    }

    void GenerateReverseDict() {
        reverseDependencyDict = new Dictionary<ILoadableScript, List<IDependentScript>>();
        foreach (var item in dependencyDictionary) {
            IDependentScript dep = item.Key;
            foreach (ILoadableScript script in item.Value) {
                if (!reverseDependencyDict.ContainsKey(script)) {
                    reverseDependencyDict[script] = new List<IDependentScript>();
                } 
                reverseDependencyDict[script].Add(dep);
            }
        }
    }

    void SubscribeToAllDependencies() {
        foreach (ILoadableScript script in reverseDependencyDict.Keys) {
            script.OnScriptInitialized += OnDependencyIntialized;
        }
    }

    void OnDependencyIntialized(ILoadableScript script) {
        //Debug.Log(script.GetType().Name + " loaded!");
        foreach (IDependentScript dep in reverseDependencyDict[script]) {
            dependencyDictionary[dep].Remove(script);
            //Debug.Log(dep.GetType().Name + " has " + dependencyDictionary[dep].Count + " dependencies left.");
            if (dependencyDictionary[dep].Count <= 0) {
                dep.OnAllDependenciesLoaded();
            }
        }
    }
}

[Serializable]
public class Dependencies
{
    public List<MonoBehaviour> list;
}
 
[Serializable]
public class ScriptDependencies
{
    public List<Dependencies> list;
}
 
