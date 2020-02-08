using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleGameOverPanel(bool shouldShow) {
        gameOverPanel.active = shouldShow;
    }

}
