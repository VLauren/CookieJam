using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MainChar>() != null)
        {
            gameObject.GetComponentInParent<MovingPlatform>().SetCharacterIsOnTop(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MainChar>() != null)
        {
            gameObject.GetComponentInParent<MovingPlatform>().SetCharacterIsOnTop(false);
        }
    }
}
