using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
    public static UIVignette Instance;
    [SerializeField] private Image vignetteImage;

    private Coroutine colorCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowVignetteWithAnimation(float duration)
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }
        colorCoroutine = StartCoroutine(VignetteAnimation(duration * 0.5f, duration * 0.5f));
    }

    IEnumerator VignetteAnimation(float timeToAppear, float timeToDissappear)
    {
        vignetteImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color originalColor = vignetteImage.color;
        while (elapsedTime < timeToAppear)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / timeToAppear);
            vignetteImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        vignetteImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        //yield return new WaitForSeconds(timeToDissappear);

        elapsedTime = 0f;
        while (elapsedTime < timeToDissappear)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / timeToDissappear);
            vignetteImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        vignetteImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        vignetteImage.gameObject.SetActive(false);
    }
}
