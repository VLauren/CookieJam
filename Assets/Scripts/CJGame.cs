using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CJGame : MonoBehaviour
{
    public static CJGame Instance { get; private set; }

    static int CurrentLevelIndex = 0;

    public static void LoadFirstLevel()
    {
        CurrentLevelIndex = 0;
        SceneManager.LoadScene(GameConfig.Instance.Levels[0]);
    }

    public static void NextLevel()
    {
        CurrentLevelIndex++;
        if (CurrentLevelIndex < GameConfig.Instance.Levels.Count)
        {
            SceneManager.LoadScene(GameConfig.Instance.Levels[CurrentLevelIndex]);
        }
        else
        {
            Debug.Log("TODOS LOS NIVELES COMPLETOS");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
