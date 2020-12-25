using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //allows you to load other scenes
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        audioSource.Stop();
        SceneManager.LoadScene(Constants.GAME_SCENE); //a scene we added to the Build Settings
    }
}
