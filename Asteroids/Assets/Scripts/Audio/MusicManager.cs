using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    #region Serialized Varibales
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioMixer mixer;
    #endregion
    #region Private Varibales
    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource currentSource;
    private Coroutine crossFadeCoroutine;
    private Coroutine fadeOutCoroutine;
    #endregion
    #region Public Varibales
    public static MusicManager Instance;
    #endregion
    #region Public Properties
    public AudioClip[] MusicClips => musicClips;
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

        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();

        sourceA.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        sourceB.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];

        sourceA.loop = true;
        sourceB.loop = true;

        currentSource = sourceA;
    }

    public void PlayRandomMusicWithoutRepetition(int musicRange = -1)
    {
        if (musicClips == null) return;
        if (musicClips.Length < 2) return;
        AudioClip newClip;
        do
        {
            int range = musicRange == -1 ? musicClips.Length : musicRange;
            int randomIndex = Random.Range(0, range);
            newClip = musicClips[randomIndex];
        } while (currentSource.clip == newClip);
        PlayMusic(newClip);
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (currentSource.clip == newClip)
            return;

        AudioSource nextSource = currentSource == sourceA ? sourceB : sourceA;

        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        StopCoroutines();
        crossFadeCoroutine = StartCoroutine(CrossFade(nextSource));
    }

    IEnumerator CrossFade(AudioSource nextSource)
    {
        AudioSource oldSource = currentSource;
        currentSource = nextSource;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;

            nextSource.volume = normalized;
            oldSource.volume = 1f - normalized;

            yield return null;
        }

        oldSource.Stop();
    }

    public void StopMusic(float fadeOutTime = 1f)
    {
        StopCoroutines();
        fadeOutCoroutine = StartCoroutine(FadeOut(currentSource, fadeOutTime));
    }

    IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        source.Stop();
    }

    void StopCoroutines()
    {
        if (crossFadeCoroutine != null)
            StopCoroutine(crossFadeCoroutine);
        if ( fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);
    }
    #endregion
}
