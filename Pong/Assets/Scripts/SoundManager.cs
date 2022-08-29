using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource clipButtons;
    [SerializeField] private AudioSource clipCrash;
    [SerializeField] private Slider soundSlider;
    // Start is called before the first frame update
    void Start()
    {
        clipButtons.volume = 0.25f;
        clipCrash.volume = 0.25f;
    }

    public void ChangeSound(float sound)
    {
        clipButtons.volume = sound;
        clipCrash.volume = sound;
    }

    public void PlayButtonSound()
    {
        clipButtons.Play();
    }

    public void OnOffButton()
    {
        if (clipButtons.volume == 0)
        {
            clipButtons.volume = soundSlider.value;
            clipCrash.volume = soundSlider.value;
        }
        else
        {
            clipButtons.volume = 0f;
            clipCrash.volume = 0f;
        }
    }

    public void PlayCrashSound()
    {
        clipCrash.Play();
    }

}
