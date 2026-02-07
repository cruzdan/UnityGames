using UnityEngine;

public class WebAudioUnlock : MonoBehaviour
{
    bool unlocked = false;

    void Update()
    {
        if (unlocked) return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.volume = 0f;
            audio.Play();
            unlocked = true;
        }
    }
}
