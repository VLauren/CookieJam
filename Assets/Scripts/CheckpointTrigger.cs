using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    GameObject Glow;

    private void Start()
    {
        Glow = transform.Find("Glow").gameObject;
        Glow.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainChar>() != null)
        {
            if (Glow.activeSelf == false)
            {
                other.GetComponent<MainChar>().SetNewCheckpoint(transform);

                // Apago todos
                foreach (var cp in FindObjectsOfType<CheckpointTrigger>())
                    cp.ShowGlow(false);

                // Brillo yo
                ShowGlow(true);
            }
        }
    }

    public void ShowGlow(bool _show)
    {
        Glow.SetActive(_show);
    }
}
