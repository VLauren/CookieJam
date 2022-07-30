using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJVisualFX : MonoBehaviour
{
    public static CJVisualFX Instance { get; private set; }

    public List<GameObject> EffectsPrefabs;

    void Awake()
    {
        Instance = this;
    }

    public static GameObject Effect(int _index, Vector3 _position, Quaternion _rotation = new Quaternion())
    {
        if (Instance.EffectsPrefabs.Count > _index)
        {
            return Instantiate(Instance.EffectsPrefabs[_index], _position, _rotation);
        }
        else
        {
            Debug.Log("No existe el efecto " + _index);
            return null;
        }
    }
}
