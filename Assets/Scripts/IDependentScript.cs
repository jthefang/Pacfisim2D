using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    A script that depends on other scripts to load first
*/
public interface IDependentScript {
    void OnAllDependenciesLoaded();
}
