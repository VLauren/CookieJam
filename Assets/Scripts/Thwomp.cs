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
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * FallSpeed);
                yield return null;
            }

            CJGame.AudioSource.SetIntVar("sfx", 5);
            CJGame.AudioSource.Play("sfx");
            CJVisualFX.Effect(2, transform.position, transform.rotation);

            float xDist = Mathf.Abs(MainChar.Instance.transform.position.x - transform.position.x);
            float yDist = Mathf.Abs(MainChar.Instance.transform.position.y - transform.position.y);

            if (xDist < 8 && yDist < 10)
            {
                MainChar.Instance.CamShake(0.2f, 2f);
                AudioManager.Play("thwomp", false, 1);
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
