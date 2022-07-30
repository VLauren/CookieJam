using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] GameObject Platform;
    [SerializeField] GameObject SurfaceArea;
    [SerializeField] GameObject Character;

    [Header("Points")]
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;
    [SerializeField] float MoveSpeed = 3.0f;
    [SerializeField] float EndWait = 1.0f;
    Transform TargetPoint;
    bool CharacterIsOnTop = false;

    private void Awake()
    {
        Character = FindObjectOfType<MainChar>().gameObject;
    }

    void Start()
    {
        TargetPoint = EndPoint;
        Platform.transform.position = StartPoint.position;
        SetSurfaceArea();

        StartCoroutine(PlatformRoutine());
    }

    void Update()
    {
        Debug.DrawLine(StartPoint.position, EndPoint.position, Color.blue);
    }

    IEnumerator PlatformRoutine()
    {
        while (true)
        {
            while (Platform.transform.position != TargetPoint.position)
            {
                Vector3 NextPosition = Vector3.MoveTowards(Platform.transform.position, TargetPoint.position, Time.deltaTime * MoveSpeed);
                Vector3 Displacement = NextPosition - Platform.transform.position;
                if (Character != null && CharacterIsOnTop)
                {
                    Character.GetComponent<CharacterController>().Move(Displacement);
                }
                SurfaceArea.transform.position += Displacement;
                Platform.transform.position += Displacement;
                yield return null;
            }
            SwitchTarget();
            yield return new WaitForSeconds(EndWait);
        }
    }

    private void SwitchTarget()
    {
        if (TargetPoint == EndPoint)
            TargetPoint = StartPoint;
        else
            TargetPoint = EndPoint;
    }

    private void SetSurfaceArea()
    {
        Vector3 SurfaceAreaOffset = Vector3.up * 0.25f;
        SurfaceArea.transform.position = Platform.transform.position + SurfaceAreaOffset;
        SurfaceArea.transform.rotation = Platform.transform.rotation;
        SurfaceArea.transform.localScale = Platform.transform.localScale;
        SurfaceArea.GetComponent<BoxCollider>().center = Platform.GetComponent<BoxCollider>().center;
        SurfaceArea.GetComponent<BoxCollider>().size = Platform.GetComponent<BoxCollider>().size;
    }

    public void SetCharacterIsOnTop(bool isOnTop)
    {
        CharacterIsOnTop = isOnTop;
    }
}
