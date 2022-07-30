using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] bool ReturnToLastSafePos;
    [SerializeField] bool IgnoreInvul;
    [SerializeField] bool PlaySpikeSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainChar>() != null)
        {
            other.GetComponent<MainChar>().ApplyDamage(ReturnToLastSafePos, IgnoreInvul, PlaySpikeSound);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
