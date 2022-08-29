using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource clip;
    // Start is called before the first frame update
    void Start()
    {
        clip.volume = 0.25f;
    }

    public void ChangeVolume(float volume)
    {
        clip.volume = volume;
    }

    public void OnOffMusic()
    {
        if (clip.isPlaying)
        {
            clip.Pause();
        }
        else
        {
            clip.UnPause();
        }
    }
}
