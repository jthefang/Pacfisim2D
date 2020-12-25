/* Defines what goes into a music theme for the game */
using System;
using UnityEngine;

[Serializable]
public class GameMusicTheme {
    public string themeName;
    public AudioClip startSound;
    public AudioClip mainTheme;
    public AudioClip loseSound;
}