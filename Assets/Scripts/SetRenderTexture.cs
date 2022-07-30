using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRenderTexture : MonoBehaviour
{
    // public Material MaterialToSetup;
    public RenderTexture RenderTexture;

    void Start()
    {
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().material.SetTexture("_MainTex", RenderTexture);
        }
        if (GetComponent<RawImage>() != null)
        {
            GetComponent<RawImage>().material.SetTexture("_MainTex", RenderTexture);
        }
    }
}
