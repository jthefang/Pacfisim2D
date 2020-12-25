using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    #region Singleton
    public static GameSettings Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        if (Instance == null) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        } else if(Instance != this) {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField]
    List<GameDifficulty> gameDifficulties;
    public GameDifficulty CurrGameDifficulty;
    public Dropdown gameDifficultyDropdown;

    [SerializeField]
    List<GameMusicTheme> musicThemes;
    public GameMusicTheme MusicTheme;
    [SerializeField]
    public Dropdown musicThemeDropdown;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + ": " + MusicTheme.themeName);
        if (MusicTheme.themeName == "") {
            SetMainMusicTheme(0);
        }
        if (CurrGameDifficulty.levelName == "") {
            CurrGameDifficulty = gameDifficulties[0];
        }

        GameObject gameDifficultyDropdownGameObject = GameObject.Find(Constants.GAME_DIFFICULTY_DROPDOWN);
        if (gameDifficultyDropdownGameObject != null) {
            gameDifficultyDropdown = gameDifficultyDropdownGameObject.GetComponent<Dropdown>();
            gameDifficultyDropdown.ClearOptions();
            List<string> gameDifficultyOptions = new List<string>();
            foreach (GameDifficulty gd in gameDifficulties)
                gameDifficultyOptions.Add(gd.levelName);
            gameDifficultyDropdown.AddOptions(gameDifficultyOptions);

            gameDifficultyDropdown.value = gameDifficulties.IndexOf(CurrGameDifficulty);
            
            gameDifficultyDropdown.onValueChanged.AddListener(delegate {
                SetGameDifficultyFromDropdownOptions(gameDifficultyDropdown.value);
            });
        }

        GameObject musicThemeDropdownGameObject = GameObject.Find(Constants.MUSIC_DROPDOWN);
        if (musicThemeDropdownGameObject != null) {
            musicThemeDropdown = musicThemeDropdownGameObject.GetComponent<Dropdown>();
            musicThemeDropdown.ClearOptions();
            List<string> musicThemeOptions = new List<string>();
            foreach (GameMusicTheme gmt in musicThemes) 
                musicThemeOptions.Add(gmt.themeName);
            musicThemeDropdown.AddOptions(musicThemeOptions);

            musicThemeDropdown.value = musicThemes.IndexOf(MusicTheme);

            musicThemeDropdown.onValueChanged.AddListener(delegate {
                SetMainMusicTheme(musicThemeDropdown.value);
            });
        }
    }

    public void SetGameDifficultyFromDropdownOptions(int difficultyLevel) {
        CurrGameDifficulty = gameDifficulties[difficultyLevel];
    }

    public void SetMainMusicTheme(int themeIdx) {
        MusicTheme = musicThemes[themeIdx];
    }

}
