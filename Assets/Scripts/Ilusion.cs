using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ilusion : MonoBehaviour
{
    GameObject IlusionObject;

    void Start()
    {
        IlusionObject = gameObject.transform.GetChild(0).gameObject;
    }

    public void ShowIlusion()
    {
        GetComponent<Renderer>().enabled = false;
        IlusionObject.SetActive(true);
    }

    public void ShowReal()
    {
        GetComponent<Renderer>().enabled = true;
        IlusionObject.SetActive(false);
    }
}
