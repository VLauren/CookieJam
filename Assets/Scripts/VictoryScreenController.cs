using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VictoryScreenController : MonoBehaviour
{
    void OnAnyKey(InputValue value)
    {
        Debug.Log("Loading first level");
        CJGame.LoadMainMenu();
    }
}
