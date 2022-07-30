using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFXSpawner : MonoBehaviour
{
    void Step()
    {
        CJVisualFX.Effect(1, transform.position + Vector3.down * 0f, transform.rotation);

        CJGame.AudioSource.SetIntVar("sfx", 0);
        CJGame.AudioSource.Play("sfx");
    }
}
