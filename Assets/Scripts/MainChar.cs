using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 4;
    [SerializeField] float Gravity = 20;
    [SerializeField] float ModelRotationSpeed = 360;
    [SerializeField] float JumpStrength = 1;

    int CurrentHP = 3;

    bool JumpPressed;

    Vector3 MoveInput;
    bool DownPressed;
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
        {
            VerticalVelocity = -0.1f;
            if (JumpPressed)
            {
                // Animator.SetTrigger("Jump");
                // Animator.SetBool("Grounded", false);
                VerticalVelocity = JumpStrength;
                JumpPressed = false;
            }
            else
            {
                // Animator.SetBool("Grounded", true);
            }
        }
        else
            VerticalVelocity -= Time.deltaTime * Gravity;

        if (MoveInput.x == 1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(90, 0, -95), Time.deltaTime * ModelRotationSpeed);
        if (MoveInput.x == -1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(90, 0, 95), Time.deltaTime * ModelRotationSpeed);

        if(MoveInput.x == 0 && DownPressed)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowReal();
        }
        if(MoveInput.x != 0)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowIlusion();
        }

        // ========================================
        // TECLAS DE DEBUG

        Keyboard keyboard = Keyboard.current;
        if (keyboard.zKey.wasPressedThisFrame)
            ApplyDamage();
        if(keyboard.digit1Key.wasPressedThisFrame)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowReal();
        }
        if(keyboard.digit2Key.wasPressedThisFrame)
        {
            foreach (var cosa in FindObjectsOfType<Ilusion>())
                cosa.ShowIlusion();
        }

        if (keyboard.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(0);
    }

    void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = new Vector3(raw.x, 0, 0).normalized;

        DownPressed = raw.y < 0;
    }

    void OnJump(InputValue _value)
    {
        JumpPressed = true;
    }

    Coroutine BlinkRoutine;

    bool Invulnerable;

    void ApplyDamage()
    {
        if (Invulnerable)
            return;

        CurrentHP--;
        if (BlinkRoutine != null)
            StopCoroutine(BlinkRoutine);
        BlinkRoutine = StartCoroutine(DamageBlink());

        // HACK TODO volver al ultimo checkpoint o algo
        if (CurrentHP <= 0)
            SceneManager.LoadScene(0);
    }

    IEnumerator DamageBlink()
    {
        Invulnerable = true;
        for (int i = 0; i < 10; i++)
        {
            Model.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.05f);

            Model.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.05f);
        }
        Invulnerable = false;
    }
}
