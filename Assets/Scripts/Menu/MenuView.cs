using System.Collections;
using System.Collections.Generic;
using Base;
using MineSweeper.Game.Minesweeper;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MenuView : MonoBehaviour
{
    public void EasyLevelClicked()
    {
        GameData.SetGameLevel(GameLevel.Easy);
        GameNavigator.NavigateToScene(GameScenes.Game);
    }

    public void MediumLevelClicked()
    {
        GameData.SetGameLevel(GameLevel.Medium);
        GameNavigator.NavigateToScene(GameScenes.Game);
    }

    public void DifficultLevelClicked()
    {
        GameData.SetGameLevel(GameLevel.Difficult);
        GameNavigator.NavigateToScene(GameScenes.Game);
    }
}