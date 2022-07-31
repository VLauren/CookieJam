using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFXSpawner : MonoBehaviour
{
    void Step()
    {
        CJVisualFX.Effect(1, transform.position + Vector3.down * 0f, transform.rotation);

        // CJGame.AudioSource.SetIntVar("sfx", 0);
        // CJGame.AudioSource.Play("sfx");

        if (Random.value < 0.33f)
            AudioManager.Play("Pasitos1", false);
        else if (Random.value < 0.5f)
            AudioManager.Play("Pasitos2", false);
        else
            AudioManager.Play("Pasitos3", false);
    }
}
