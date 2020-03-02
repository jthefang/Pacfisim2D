using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatablePanel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ToggleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleActive(bool isActive) {
        gameObject.SetActive(isActive);
    }

    public void Open() {
        ToggleActive(true);
    }

    public void Close() {
        ToggleActive(false);
    }

}
