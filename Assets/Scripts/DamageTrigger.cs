using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] bool ReturnToLastSafePos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainChar>() != null)
        {
            other.GetComponent<MainChar>().ApplyDamage(ReturnToLastSafePos);
        }
    }
}
