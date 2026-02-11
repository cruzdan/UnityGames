using UnityEngine;
using System.Collections;

public class ScaleBounceElastic : MonoBehaviour
{
    public float startScale = 1.3f;
    public float duration = 0.2f;

    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        transform.localScale = originalScale * startScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(
                originalScale * startScale,
                originalScale,
                t
            );
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
