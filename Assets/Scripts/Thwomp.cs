using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    [SerializeField] float FallSpeed;
    [SerializeField] float RiseSpeed;
    [SerializeField] float DownTime;

    Vector3 StartPosition;
    Vector3 TargetPosition;

    void Start()
    {
        StartPosition = transform.position;

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        TargetPosition = hit.point;

        StartCoroutine(ThwompRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(StartPosition, TargetPosition, Color.red);
    }

    IEnumerator ThwompRoutine()
    {
        while (true)
        {
            while (transform.position != TargetPosition)
            {
                Debug.Log("???");
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * FallSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(DownTime);

            while (transform.position != StartPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPosition, Time.deltaTime * RiseSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(1);
        }
    }
}
