using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainChar>() != null)
        {
            StartCoroutine(LevelEndRoutine());
        }
    }

    IEnumerator LevelEndRoutine()
    {
        MainChar.Instance.VictoryAnimation();

        // CJGame.AudioSource.SetIntVar("musica", 2);
        // CJGame.AudioSource.Play("musica");
        AudioManager.Play("music_ganar", false, 0.75f);

        yield return new WaitForSeconds(5);

        FadeUI.FadeOut(1);

        yield return new WaitForSeconds(1);

        CJGame.NextLevel();
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(0.5f, 0.5f, 1, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.color = new Color(0, 0, 1, 0.7f);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
