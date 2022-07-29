using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainChar : MonoBehaviour
{
    Vector3 MoveInput;
    Vector3 ControlMovement;
    float VerticalVelocity;

    void Start()
    {
        
    }

    void Update()
    {
        ControlMovement = Vector3.right * MoveInput.x * Time.deltaTime * 3;
        GetComponent<CharacterController>().Move(ControlMovement + new Vector3(0, VerticalVelocity, 0));
    }

    void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = new Vector3(raw.x, 0, 0).normalized;
    }
}
