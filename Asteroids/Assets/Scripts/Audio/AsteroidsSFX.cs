using UnityEngine;

public class AsteroidsSFX : MonoBehaviour
{
    public AudioClip ExplosionClip;
    public AudioClip ShootClip;
    public AudioClip ClickClip;
    public AudioClip BuyClip;
    public AudioClip ErrorClip;
    public static AsteroidsSFX Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
