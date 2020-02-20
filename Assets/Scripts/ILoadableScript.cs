using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface ILoadableScript {
    // should be triggered after the script is loaded
    event Action<ILoadableScript> OnScriptInitialized; 
}
