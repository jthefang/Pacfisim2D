/* Defines Game Difficulty levels */
using System;

[Serializable]
public class GameDifficulty {
    /* Defines what game difficulty specifies */
    public string levelName; //e.g. NORMAL or BEGINNER
    public float relativeGateEndSize;
}