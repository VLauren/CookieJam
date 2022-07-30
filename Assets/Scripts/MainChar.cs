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
    Renderer ModelRenderer;

    Vector3 LastSafePosition;

    Animator Animator;

    void Start()
    {
        Model = transform.Find("Model");
        ModelRenderer = Model.Find("Moja").GetComponent<Renderer>();
        Animator = Model.GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento
        ControlMovement = Vector3.right * MoveInput.x * Time.deltaTime * MovementSpeed;
        GetComponent<CharacterController>().Move(ControlMovement + new Vector3(0, VerticalVelocity, 0));
        Animator.SetFloat("Movement", Mathf.Abs(MoveInput.x));

        // Salto y gravedad
        if (GetComponent<CharacterController>().isGrounded)
        {
            VerticalVelocity = -0.01f;
            if (JumpPressed)
            {
                Animator.SetTrigger("Jump");
                Animator.SetBool("Grounded", false);
                VerticalVelocity = JumpStrength;
                JumpPressed = false;
            }
            else
            {
                Animator.SetBool("Grounded", true);
            }

            if (!Invulnerable)
                LastSafePosition = transform.position;
        }
        else
            VerticalVelocity -= Time.deltaTime * Gravity;

        if (MoveInput.x == 1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, 95, 0), Time.deltaTime * ModelRotationSpeed);
        if (MoveInput.x == -1)
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, -95, 0), Time.deltaTime * ModelRotationSpeed);

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

        if (keyboard.tKey.wasPressedThisFrame)
        {
            MoveToLastSafePosition();
        }
        if (keyboard.yKey.wasPressedThisFrame)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(0, 0, 0);
            GetComponent<CharacterController>().enabled = true;
        }
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

    public void ApplyDamage(bool _returnToLastSafePos = false, bool _ignoreInvul = false)
    {
        if (Invulnerable && !_ignoreInvul)
            return;

        CurrentHP--;
        if (BlinkRoutine != null)
            StopCoroutine(BlinkRoutine);
        BlinkRoutine = StartCoroutine(DamageBlink());

        if (_returnToLastSafePos)
            MoveToLastSafePosition();

        // HACK TODO volver al ultimo checkpoint o algo
        if (CurrentHP <= 0)
            SceneManager.LoadScene(0);
    }

    public void MoveToLastSafePosition()
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = LastSafePosition;
        GetComponent<CharacterController>().enabled = true;
    }

    IEnumerator DamageBlink()
    {
        Invulnerable = true;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("blink");
            ModelRenderer.enabled = false;

            yield return new WaitForSeconds(0.05f);

            ModelRenderer.enabled = true;

            yield return new WaitForSeconds(0.05f);
        }
        Invulnerable = false;
    }
}
