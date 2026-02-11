using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    #endregion
    #region Constants
    const string MusicKey = "MusicVolume";
    const string SFXKey = "SFXVolume";
    #endregion
    #region Functions
    void Start()
    {
        float music = PlayerPrefs.GetFloat(MusicKey, 0.3f);
        float sfx = PlayerPrefs.GetFloat(SFXKey, 0.8f);

        if (musicSlider != null)
            musicSlider.value = music;
        if (sfxSlider != null)
            sfxSlider.value = sfx;

        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", LinearToDecibel(value));
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", LinearToDecibel(value));
        PlayerPrefs.SetFloat(SFXKey, value);
    }

    float LinearToDecibel(float value)
    {
        return value > 0.001f ? Mathf.Log10(value) * 20f : -80f;
    }
    #endregion
}
