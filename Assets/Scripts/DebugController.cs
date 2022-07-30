using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    void Update()
    {
        // ========================================
        // TECLAS DE DEBUG

        Keyboard keyboard = Keyboard.current;
        if (keyboard.zKey.wasPressedThisFrame)
        {
            gameObject.GetComponent<MainChar>().ApplyDamage();
        }
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowReal();
        }
        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowIlusion();
        }

        if (keyboard.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(0);

        if (keyboard.tKey.wasPressedThisFrame)
        {
            gameObject.GetComponent<MainChar>().MoveToLastSafePosition();
        }
        if (keyboard.yKey.wasPressedThisFrame)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(0, 0, 0);
            GetComponent<CharacterController>().enabled = true;
        }
    }
}
