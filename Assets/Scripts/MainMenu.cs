using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb.anyKey.wasPressedThisFrame)
            CJGame.LoadFirstLevel();
    }
}
