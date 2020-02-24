using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //allows you to load other scenes
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    string gameScene = "MainGame";

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        SceneManager.LoadScene(gameScene); //a scene we added to the Build Settings
    }
}
