using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase para fundidos a negro.
/// </summary>
public class FadeUI : MonoBehaviour
{
    static FadeUI Instance;
    static bool ActiveFade;

    Image FadeImage;

    void Awake()
    {
        Instance = this;
        FadeImage = GetComponent<Image>();
    }

    /// <summary>
    /// Fundido de entrada desde negro.
    /// </summary>
    /// <param name="_time"></param>
    /// <param name="_delay"></param>
    public static void FadeIn(float _time = 1, float _delay = 0)
    {
        Instance.StartCoroutine(Instance.FadeRoutine(1, 0, _time, _delay));
    }

    /// <summary>
    /// Fundido de salida a negro.
    /// </summary>
    /// <param name="_time"></param>
    public static void FadeOut(float _time = 1)
    {
        Instance.StartCoroutine(Instance.FadeRoutine(0, 1, _time, 0));
    }

    IEnumerator FadeRoutine(float _startAlpha, float _endAlpha, float _time, float _delay)
    {
        ActiveFade = true;

        float currentAlpha = _startAlpha;
        float count = 0;

        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, _startAlpha);

        yield return new WaitForSeconds(_delay);

        while (count < _time)
        {
            currentAlpha = _startAlpha + (_endAlpha - _startAlpha) * (count / _time);
            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, currentAlpha);
            count += Time.deltaTime;
            yield return null;
        }

        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, _endAlpha);

        ActiveFade = false;
    }
}
