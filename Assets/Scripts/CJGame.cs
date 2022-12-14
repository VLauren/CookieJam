using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KrillAudio.Krilloud;


public static class CJGame
{
    // public static CJGame Instance; { get; private set; }

    static int CurrentLevelIndex = 0;

    public static bool Reality = false;

    public static KLAudioSource AudioSource;

    public static void LoadFirstLevel()
    {
        CurrentLevelIndex = 0;
        SceneManager.LoadScene(GameConfig.Instance.Levels[0]);

        AudioManager.Play("galleta_music_test2", true);
        AudioManager.Play("galleta_music_test2_capa_extra", true, 0.001f);
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

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
