using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum GameDifficultyLevel {
    NORMAL,
    BEGINNER
}

[Serializable]
public class GameDifficulty {
    public float relativeGateEndSize;
}

[Serializable]
public class GameMusicTheme {
    public AudioClip startSound;
    public AudioClip mainTheme;
    public AudioClip loseSound;
}

public class GameSettings : MonoBehaviour
{
    #region Singleton
    public static GameSettings Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        } else if(Instance != this) {
            Destroy (gameObject);
        }
    }
    #endregion

    [SerializeField]
    List<GameDifficulty> gameDifficulties;
    GameDifficultyLevel currGameDifficultyLevel;
    GameDifficulty _currGameDifficulty;
    public GameDifficulty CurrGameDifficulty {
        get {
            return _currGameDifficulty;
        }
        set {
            _currGameDifficulty = value;
        }
    }

    GameMusicTheme _musicTheme;
    public GameMusicTheme MusicTheme {
        get {
            return _musicTheme;
        }
    }
    [SerializeField]
    List<GameMusicTheme> musicThemes;

    // Start is called before the first frame update
    void Start()
    {
        CurrGameDifficulty = gameDifficulties[(int) GameDifficultyLevel.NORMAL];
        SetMainMusicTheme(0);
    }

    // Update is called once per frame
    void Update()
    {   

    }

    public void SetGameDifficultyFromDropdownOptions(int difficultyLevel) {
        CurrGameDifficulty = gameDifficulties[difficultyLevel];
        currGameDifficultyLevel = (GameDifficultyLevel) difficultyLevel;
    }

    public void SetMainMusicTheme(int themeIdx) {
        _musicTheme = musicThemes[themeIdx];
    }

}
