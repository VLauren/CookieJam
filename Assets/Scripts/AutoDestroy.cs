using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float TimeToDestroy = 1.0f;
    float StartTime;

    private void Start()
    {
        StartTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > StartTime + TimeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
