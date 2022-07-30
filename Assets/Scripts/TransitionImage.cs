using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TransitionImage : MonoBehaviour
{
    public static TransitionImage Instance { get; private set; }

    [SerializeField] float MinSaturation;
    [SerializeField] float RealityTransitionSpeed;
    [SerializeField] float IllusionTransitionSpeed;

    Material Mat;
    ColorAdjustments ColorAdjust;

    float CurrentTransitionProgress;

    private void Awake()
    {
        Instance = this;

        FindObjectOfType<Volume>().profile.TryGet(out ColorAdjust);
    }

    void Start()
    {
        Mat = GetComponent<Image>().material;
    }

    

    private void Update()
    {
        if (CJGame.Reality)
            CurrentTransitionProgress = Mathf.MoveTowards(CurrentTransitionProgress, 0.6f, Time.deltaTime * RealityTransitionSpeed);
        else
            CurrentTransitionProgress = Mathf.MoveTowards(CurrentTransitionProgress, 0f, Time.deltaTime * IllusionTransitionSpeed);

        SetProgress(CurrentTransitionProgress);
    }

    public static void SetProgress(float _progress)
    {
        // Instance.ColorAdjust.saturation.value = Mathf.Lerp(0, Instance.MinSaturation, _progress);

        float matProgress = Mathf.Clamp(_progress * 2 - 0.6f, 0, 0.6f);
        Instance.Mat.SetFloat("_Progress", matProgress);
    }
}
