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

    [Space]
    [SerializeField] SkinnedMeshRenderer BodyA;
    [SerializeField] SkinnedMeshRenderer BodyB;
    [SerializeField] SkinnedMeshRenderer BodyC;
    [SerializeField] MeshRenderer FaceA;
    [SerializeField] MeshRenderer FaceB;
    [SerializeField] MeshRenderer FaceC;
    int MaxHP = 3;
    int CurrentHP = 3;

    bool JumpPressed;

    Vector3 MoveInput;
    bool DownPressed;
    Vector3 ControlMovement;
    float VerticalVelocity;

    Transform Model;
    Renderer CurrentModelRenderer;

    Vector3 LastSafePosition;

    Animator Animator;

    void Start()
    {
        Model = transform.Find("Model");
        Animator = Model.GetComponent<Animator>();
        UpdateDamageRenderers();
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

    public void RestoreHealth(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);
        Debug.Log("Mi vida ahora es: " + CurrentHP);
    }

    public void ApplyDamage(bool _returnToLastSafePos = false, bool _ignoreInvul = false)
    {
        if (Invulnerable && !_ignoreInvul)
            return;

        CurrentHP--;
        Debug.Log("Mi vida ahora es: " + CurrentHP);
        if (BlinkRoutine != null)
            StopCoroutine(BlinkRoutine);
        BlinkRoutine = StartCoroutine(DamageBlink());

        if (_returnToLastSafePos)
            MoveToLastSafePosition();

        // TODO quitar trozo de galleta

        UpdateDamageRenderers();

        if (CurrentHP <= 0)
            Death();
    }

    public void RestoreHP()
    {
        CurrentHP = 3;
        // TODO volver a poner modelo completo
    }

    public void Death()
    {
        // HACK TODO volver al ultimo checkpoint o algo
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
        for (int i = 0; i < 7; i++)
        {
            CurrentModelRenderer.enabled = true;

            yield return new WaitForSeconds(0.06f);

            CurrentModelRenderer.enabled = false;

            yield return new WaitForSeconds(0.06f);
        }
        CurrentModelRenderer.enabled = true;
        Invulnerable = false;
    }

    void UpdateDamageRenderers()
    {
        BodyA.enabled = false;
        BodyB.enabled = false;
        BodyC.enabled = false;
        FaceA.enabled = false;
        FaceB.enabled = false;
        FaceC.enabled = false;

        if(CurrentHP == 3)
        {
            BodyA.enabled = true;
            FaceA.enabled = true;
            CurrentModelRenderer = BodyA;
        }
        else if(CurrentHP == 2)
        {
            BodyB.enabled = true;
            FaceB.enabled = true;
            CurrentModelRenderer = BodyB;
        }
        else
        {
            BodyC.enabled = true;
            FaceC.enabled = true;
            CurrentModelRenderer = BodyC;
        }
    }
}
