using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class TestAudio : MonoBehaviour
{
    KLAudioSource source;
    void Start()
    {
        print("start");
        GetComponent<KLAudioSource>().SetIntVar("musica", 0);
        GetComponent<KLAudioSource>().SetIntVar("realidad", 1);
        GetComponent<KLAudioSource>().Play("musica");

        // GetComponent<KLAudioSource>().SetIntVar("sfx", 1);
        // GetComponent<KLAudioSource>().Play("sfx");

        // GetComponent<KLAudioSource>().SetIntVar("lobo", 2);
        // GetComponent<KLAudioSource>().Play("lobo");

    }
}
