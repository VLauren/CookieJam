using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    [SerializeField] int healthAmount = 1;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<MainChar>()) {
            other.GetComponent<MainChar>().RestoreHealth(healthAmount);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
