using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    Ensures we load all script dependencies in the right order

    DependentScripts = List of lists of scripts that depend on other scripts to load first, before they can load
        each script must be of type `IDependentScript`

    ScriptDependencies = List of lists of scripts that the dependent scripts wait on to load
        each script must be of type `ILoadableScript`

    Each list D of IDependentScript's in DependentScripts should have a corresponding list L of ILoadableScript's in ScriptDependencies 
        foreach dependentScript in D:
            dependentScript is dependent on all ILoadableScript's in L
*/
public class ScriptDependencyManager : MonoBehaviour
{
    #region Singleton
    public static ScriptDependencyManager Instance;
    //reference this only version as ScriptDependencyManager.Instance

    private void Awake() {
        Instance = this;
    }
    #endregion

    [SerializeField]
    List<DependentScripts> dependentScripts;
    [SerializeField]
    ScriptDependencies scriptDependencies;
    [SerializeField]
    bool verbose;
    //maps dependent script --> scripts it's still waiting on to load
    Dictionary<IDependentScript, List<ILoadableScript>> dependencyDictionary;
    // maps scripts --> scripts waiting on it to load
    Dictionary<ILoadableScript, List<IDependentScript>> reverseDependencyDict;

    // Start is called before the first frame update
    void Start()
    {
        dependencyDictionary = new Dictionary<IDependentScript, List<ILoadableScript>>();
        reverseDependencyDict = new Dictionary<ILoadableScript, List<IDependentScript>>();

        GenerateDependencyDicts();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void GenerateDependencyDicts() {
        if (dependentScripts.Count != scriptDependencies.list.Count) {
            Debug.LogError("There should be a list of script dependencies for every dependent script in Dependent Scripts. Found " + dependentScripts.Count + " dependent scripts but only " + scriptDependencies.list.Count + " lists of script dependencies.");
            return;
        }

        for (int i = 0; i < dependentScripts.Count; i++) {
            List<MonoBehaviour> D = dependentScripts[i].list;
            for (int j = 0; j < D.Count; j++) {
                if (!(D[j] is IDependentScript)) {
                    Debug.LogError(D[j].GetType().Name + " is not an IDependentScript.");
                    continue;
                }
                IDependentScript currScript = (IDependentScript) D[j];
                
                List<ILoadableScript> currScriptDependencies = new List<ILoadableScript>();
                foreach (MonoBehaviour dependency in scriptDependencies.list[i].list) {
                    if (!(dependency is ILoadableScript)) {
                        Debug.LogError(dependency.GetType().Name + " is not an ILoadableScript.");
                        continue;
                    }
                    currScriptDependencies.Add((ILoadableScript) dependency);
                }

                UpdateDependencyDicts(currScript, currScriptDependencies);
            }
        }
    }

    public void UpdateDependencyDicts(IDependentScript newDependentScript, List<ILoadableScript> newDependencies) {
        if (dependencyDictionary.ContainsKey(newDependentScript)) {
            Debug.Log("Already stated dependencies for " + newDependentScript.GetType().Name);
            return; 
        }

        List<ILoadableScript> waitableDependencies = new List<ILoadableScript>();
        foreach (ILoadableScript currDependency in newDependencies) {
            //only wait on dependencies that haven't initialized yet 
            if (!currDependency.IsInitialized()) { 
                waitableDependencies.Add(currDependency);

                //Also update reverseDependencyDict
                if (reverseDependencyDict.ContainsKey(currDependency)) {
                    reverseDependencyDict[currDependency].Add(newDependentScript);
                } else { 
                    //first time that the dependency is added to reverseDict
                    currDependency.OnScriptInitialized += OnDependencyIntialized; //subscribe to initialization event of this dependency

                    List<IDependentScript> newDependentScripts = new List<IDependentScript>();
                    newDependentScripts.Add(newDependentScript);
                    reverseDependencyDict[currDependency] = newDependentScripts; 
                }

                if (verbose) {
                    Debug.Log(newDependentScript.GetType().Name + " has dependency " + currDependency.GetType().Name);
                }
            } else if (verbose) {
                Debug.Log(currDependency.GetType().Name + " is already loaded for dependent " + newDependentScript.GetType().Name);
            }
        }

        if (waitableDependencies.Count <= 0) {
            newDependentScript.OnAllDependenciesLoaded();
        } else {
            dependencyDictionary[newDependentScript] = waitableDependencies;
        }
    }

    void OnDependencyIntialized(ILoadableScript script) {
        if (verbose)
            Debug.Log(script.GetType().Name + " loaded!");
        foreach (IDependentScript dep in reverseDependencyDict[script]) {
            dependencyDictionary[dep].Remove(script);
            if (verbose)
                Debug.Log("\t" + dep.GetType().Name + " has " + dependencyDictionary[dep].Count + " dependencies left.");
                
            if (dependencyDictionary[dep].Count <= 0) {
                dep.OnAllDependenciesLoaded();
            }
        }
    }
}

[Serializable]
public class DependentScripts {
    public List<MonoBehaviour> list;
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
 
