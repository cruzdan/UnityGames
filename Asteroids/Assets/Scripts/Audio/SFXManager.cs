using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private AudioMixer mixer;
    #endregion
    #region Public Variables
    public static SFXManager Instance;
    #endregion
    #region Private Variables
    private AudioSource audioSource;
    #endregion
    #region Functions
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitchRandom = 0.05f)
    {
        if (clip == null) return;

        audioSource.pitch = 1f + Random.Range(-pitchRandom, pitchRandom);
        audioSource.PlayOneShot(clip, volume);
    }
    #endregion
}
