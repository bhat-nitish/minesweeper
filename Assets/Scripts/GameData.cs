using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameLevel
{
    Easy = 1,
    Medium = 2,
    Difficult = 3
}

public class GameData
{
    private static GameLevel CurrentLevel { get; set; }

    public static GameLevel GetCurrentGameLevel()
    {
        return CurrentLevel;
    }

    public static void SetGameLevel(GameLevel level)
    {
        CurrentLevel = level;
    }
}