using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfig : ScriptableObject
{
    static GameConfig instance;
    public static GameConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameConfig>("GameConfig");
            }
            return instance;
        }
    }

    public List<string> Levels;

#if UNITY_EDITOR

    [MenuItem("CookieJam/Game config", priority = 23)]
    public static void SelectParameters()
    {
        Selection.activeObject = Resources.Load("GameConfig");
    }

    [MenuItem("Assets/Create/CookieJam/GameConfig")]
    public static void CreateGameConfig()
    {
        GameConfig newValues = ScriptableObject.CreateInstance<GameConfig>();

        // Si no existe la carpeta la creo
        if (!AssetDatabase.IsValidFolder("Assets/Data/Resources"))
            AssetDatabase.CreateFolder("Assets/Data", "Resources");

        AssetDatabase.CreateAsset(newValues, "Assets/Data/Resources/GameConfig.asset");
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newValues;
    }

#endif
}
