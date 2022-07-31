using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float FXVolumeMultiplier = 1;

    static AudioSource source;
    static Dictionary<string, AudioSource> loopingSounds;

    static AudioManager Instance;

    // static bool PlayHidraulic = false;
    // static bool PlaySoil = false;

    // public float MusicVolumeMultiplier;

    // float CurrentHidraulicVolume = 0;
    // float HidraulicFadeTime = 0.1f;

    // float CurrentSoilVolume = 0;
    // float SoilFadeTime = 0.1f;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            source = GetComponent<AudioSource>();
            loopingSounds = new Dictionary<string, AudioSource>();
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static void Play(string _name, bool _loop, float _volume = 0.5f)
    {
        AudioClip clip = Resources.Load<AudioClip>(_name);

        if (!_loop)
            source.PlayOneShot(clip, _volume);
        else
        {
            if(!loopingSounds.ContainsKey(_name))
            {
                AudioSource newSource = source.gameObject.AddComponent<AudioSource>();
                loopingSounds.Add(_name, newSource);

                newSource.clip = clip;
                newSource.spatialBlend = 0;
                // newSource.volume = _volume * GameValues.Instance.BaseAudioFXVolume;
                newSource.volume = _volume;
                newSource.loop = true;

                newSource.Play();
            }
            else
            {
                // loopingSounds[_name].volume = _volume * GameValues.Instance.BaseAudioFXVolume;
                loopingSounds[_name].volume = _volume;
            }
        }
    }

    public static void Stop(string _name)
    {
        if(loopingSounds.ContainsKey(_name))
        {
            loopingSounds[_name].Stop();
            Destroy(loopingSounds[_name]);
            loopingSounds.Remove(_name);
        }
    }

    /*
    public static void HidraulicSound()
    {
        PlayHidraulic = true;
    }

    public static void SoilSound()
    {
        PlaySoil = true;
    }

    private void LateUpdate()
    {
        if(PlayHidraulic)
        {
            if (CurrentHidraulicVolume < 1)
                CurrentHidraulicVolume += Time.deltaTime / HidraulicFadeTime;
            if (CurrentHidraulicVolume > 1)
                CurrentHidraulicVolume = 1;

            float volume = Mathf.Lerp(0, 0.2f, CurrentHidraulicVolume) * GameValues.Instance.BaseAudioFXVolume;
            Play("hidraulicosLoop", true, volume);
        }
        else
        {
            Stop("hidraulicosLoop");
            CurrentHidraulicVolume = 0;
        }

        PlayHidraulic = false;

        if(PlaySoil)
        {
            if (CurrentSoilVolume < 1)
                CurrentSoilVolume += Time.deltaTime / SoilFadeTime;
            if (CurrentSoilVolume > 1)
                CurrentSoilVolume = 1;

            float volume = Mathf.Lerp(0, 0.2f, CurrentSoilVolume) * GameValues.Instance.BaseAudioFXVolume;
            Play("sanddrop", true, volume);
        }
        else
        {
            Stop("sanddrop");
            CurrentSoilVolume = 0;
        }

        PlaySoil = false;

        // OnVolumeChanged();
    }
    */

    // static List<AudioListener> AudioListeners;

    // public static void AddAudioListener(AudioListener _listener)
    // {
        // if (AudioListeners == null)
            // AudioListeners = new List<AudioListener>();
        // AudioListeners.Add(_listener);
    // }

    // public void OnVolumeChanged()
    // {
        // AudioListener.volume = FXVolumeMultiplier;
    // }
}
