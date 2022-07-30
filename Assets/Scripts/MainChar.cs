using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using KrillAudio.Krilloud;
using Cinemachine;

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    [Header("Movement")]
    [SerializeField] float MovementSpeed = 4;
    [SerializeField] float Gravity = 20;
    [SerializeField] float ModelRotationSpeed = 360;
    [SerializeField] float JumpStrength = 1;

    [Space]
    [Header("Body Models")]
    [SerializeField] SkinnedMeshRenderer BodyA;
    [SerializeField] SkinnedMeshRenderer BodyB;
    [SerializeField] SkinnedMeshRenderer BodyC;
    [SerializeField] MeshRenderer FaceA;
    [SerializeField] MeshRenderer FaceB;
    [SerializeField] MeshRenderer FaceC;
    [SerializeField] MeshRenderer FaceD;


    [Header("Health")]
    [SerializeField] int MaxHP = 3;
    [SerializeField] int CurrentHP = 3;

    [Header("Checkpoints")]
    [SerializeField] Transform CurrentCheckpoint;

    bool JumpPressed;

    Vector3 MoveInput;
    bool DownPressed;
    Vector3 ControlMovement;
    float VerticalVelocity;

    Transform Model;
    Renderer CurrentModelRenderer;

    [SerializeField] Vector3 LastSafePosition;

    Animator Animator;

    float IdleTime;
    bool GoingRight = true;

    bool CantControl;

    private void Awake()
    {
        Instance = this;
        CJGame.AudioSource = GetComponent<KLAudioSource>();
    }

    void Start()
    {
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        if (spawnPoint != null)
        {
            SetNewCheckpoint(spawnPoint.transform);
            MoveToLastCheckpoint();
        }
        
        Model = transform.Find("Model");
        Animator = Model.GetComponent<Animator>();
        UpdateDamageRenderers();

        CJGame.AudioSource.SetIntVar("musica", 0);
        CJGame.AudioSource.Play("musica");

        FadeUI.FadeIn(1, 0);
    }

    void Update()
    {
        // Movement();

        if (MoveInput.x == 1)
        {
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, 95, 0), Time.deltaTime * ModelRotationSpeed);
            GoingRight = true;
            IdleTime = 0;
        }
        else if (MoveInput.x == -1)
        {
            Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, -95, 0), Time.deltaTime * ModelRotationSpeed);
            GoingRight = false;
            IdleTime = 0;
        }
        else
        {
            if (GoingRight)
                Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, 150, 0), Time.deltaTime * ModelRotationSpeed);
            else
                Model.rotation = Quaternion.RotateTowards(Model.rotation, Quaternion.Euler(0, -150, 0), Time.deltaTime * ModelRotationSpeed);

            IdleTime += Time.deltaTime;
        }

        if (!DownPressed && IdleTime > 1)
            DownPressed = true;

        if (!CJGame.Reality && MoveInput.x == 0 && DownPressed && !CantControl)
        {
            // foreach (var cosa in FindObjectsOfType<Ilusion>())
            // cosa.ShowReal();
            CJGame.Reality = true;
        }
        if (MoveInput.x != 0)
        {
            // foreach (var cosa in FindObjectsOfType<Ilusion>())
            // cosa.ShowIlusion();
            CJGame.Reality = false;
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position += Random.onUnitSphere * 1000;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Movimiento
        // ControlMovement = Vector3.right * MoveInput.x * Time.deltaTime * MovementSpeed;
        ControlMovement = Vector3.right * MoveInput.x * Time.fixedDeltaTime * MovementSpeed;

        if (CantControl)
        {
            ControlMovement = Vector3.zero;
            MoveInput = Vector3.zero;
        }

        Animator.SetFloat("Movement", Mathf.Abs(MoveInput.x));

        // Salto y gravedad
        if (GetComponent<CharacterController>().isGrounded)
        {
            VerticalVelocity = -0.01f;
            if (JumpPressed && !CantControl)
            {
                Animator.SetTrigger("Jump");
                Animator.SetBool("Grounded", false);
                VerticalVelocity = JumpStrength;
                JumpPressed = false;

                CJGame.AudioSource.SetIntVar("sfx", 1);
                CJGame.AudioSource.Play("sfx");

                CJVisualFX.Effect(0, transform.position + Vector3.down * 0.5f, transform.rotation);

                IdleTime = 0;
            }
            else
            {
                Animator.SetBool("Grounded", true);
            }

            if (!Invulnerable)
                LastSafePosition = transform.position;
        }
        else
            // VerticalVelocity -= Time.deltaTime * Gravity;
            VerticalVelocity -= Time.fixedDeltaTime * Gravity;

        GetComponent<CharacterController>().Move(ControlMovement + new Vector3(0, VerticalVelocity, 0));
    }

    void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = new Vector3(raw.x, 0, 0).normalized;

        DownPressed = raw.y < 0;
    }

    void OnJump(InputValue _value)
    {
        if (GetComponent<CharacterController>().isGrounded)
            JumpPressed = true;
    }

    Coroutine BlinkRoutine;

    bool Invulnerable;

    public void RestoreHealth(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);

        UpdateDamageRenderers();
    }

    public void ApplyDamage(bool _returnToLastSafePos = false, bool _ignoreInvul = false, bool _spikeSound = false)
    {
        if (Invulnerable && !_ignoreInvul)
            return;

        CurrentHP = Mathf.Clamp(CurrentHP - 1, 0, MaxHP);

        MainChar.Instance.CamShake(0.1f, 2.5f);

        CJGame.AudioSource.SetIntVar("sfx", 3);
        CJGame.AudioSource.Play("sfx");

        // asumo que si ignore invul, es leche
        CJGame.AudioSource.SetIntVar("sfx", 4);
        CJGame.AudioSource.Play("sfx");

        // sonido de pinchos
        CJGame.AudioSource.SetIntVar("sfx", 6);
        CJGame.AudioSource.Play("sfx");

        if (BlinkRoutine != null && !CantControl)
            StopCoroutine(BlinkRoutine);
        BlinkRoutine = StartCoroutine(DamageBlink());

        if (_returnToLastSafePos)
        {
            MoveToLastSafePosition();
        }

        // Quitar trozo de galleta
        UpdateDamageRenderers();

        if (CurrentHP <= 0)
            Death();
    }

    public void Death()
    {
        if (!CantControl)
            StartCoroutine(DeathRoutine());
    }

    public IEnumerator DeathRoutine()
    {
        CantControl = true;
        Animator.SetTrigger("Death");

        Invulnerable = true;

        yield return new WaitForSeconds(5);

        FadeUI.FadeOut(1);
        
        yield return new WaitForSeconds(1);

        RestoreHealth(MaxHP);
        if (!MoveToLastCheckpoint())
        {
            MoveToLastSafePosition();
        }

        Invulnerable = false;
        Animator.SetTrigger("Reset");
        CantControl = false;

        FadeUI.FadeIn(1);
    }

    public void SetNewCheckpoint(Transform newCheckpoint)
    {
        if (CurrentCheckpoint == newCheckpoint)
            return;
        CurrentCheckpoint = newCheckpoint;
        Debug.Log("Set new checkpoint");
    }

    public bool MoveToLastCheckpoint()
    {
        if (CurrentCheckpoint == null)
        {
            Debug.Log("No checkpoint");
            return false;
        }
        GetComponent<CharacterController>().enabled = false;
        transform.position = CurrentCheckpoint.position;
        GetComponent<CharacterController>().enabled = true;
        Debug.Log("Checkpoint loaded");

        if (BlinkRoutine != null)
            StopCoroutine(BlinkRoutine);

        return true;
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

        Debug.Log("Update " + CurrentHP);
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

    public void VictoryAnimation()
    {
        CantControl = true;

        FaceA.enabled = false;
        FaceB.enabled = false;
        FaceC.enabled = false;
        FaceD.enabled = true;

        Animator.SetTrigger("Victory");
    }

    Coroutine CSRoutine;

    public void CamShake(float _time, float _intensity)
    {
        if (CSRoutine != null)
            StopCoroutine(CSRoutine);
        StartCoroutine(CamShakeRoutine(_time, _intensity));
    }

    IEnumerator CamShakeRoutine(float _time, float _intensity)
    {
        var vcn = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        vcn.m_AmplitudeGain = _intensity;

        yield return new WaitForSeconds(_time);

        vcn.m_AmplitudeGain = 0;
    }
}
