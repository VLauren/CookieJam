using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainChar>() != null)
        {
            CJGame.NextLevel();
        }
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