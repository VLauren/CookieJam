using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainChar : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 4;
    [SerializeField] float Gravity = 20;
    [SerializeField] float ModelRotationSpeed = 360;

    Vector3 MoveInput;
    Vector3 ControlMovement;
    float VerticalVelocity;

    Transform Model;

    void Start()
    {
        Model = transform.Find("Model");
    }

    void Update()
    {
        ControlMovement = Vector3.right * MoveInput.x * Time.deltaTime * MovementSpeed;
        GetComponent<CharacterController>().Move(ControlMovement + new Vector3(0, VerticalVelocity, 0));

        if (GetComponent<CharacterController>().isGrounded)
            VerticalVelocity = -1;
        else
            VerticalVelocity -= Time.deltaTime * Gravity;

        if (MoveInput.x == 1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(90, 0, -95), Time.deltaTime * ModelRotationSpeed);
        if (MoveInput.x == -1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(90, 0, 95), Time.deltaTime * ModelRotationSpeed);
    }

    void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = new Vector3(raw.x, 0, 0).normalized;
    }
}
