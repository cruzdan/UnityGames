using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class FloatingTextTMP : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private float floatSpeed = 40f;
    [SerializeField] private float lifeTime = 0.8f;
    [SerializeField] private float fadeTime = 0.3f;
    #endregion
    #region Private Variables
    private TextMeshProUGUI text;
    private RectTransform rect;
    private Color originalColor;
    private Coroutine floatingCoroutine;
    #endregion
    #region Actions
    public Action OnEnd;
    #endregion
    #region Functions
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
        originalColor = text.color;
    }

    public void StartFloatingText()
    {
        if (floatingCoroutine != null)
            StopCoroutine(floatingCoroutine);
        floatingCoroutine = StartCoroutine(FloatRoutine());
    }

    IEnumerator FloatRoutine()
    {
        float elapsed = 0f;

        while (elapsed < lifeTime)
        {
            rect.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

            if (elapsed > lifeTime - fadeTime)
            {
                float t = (elapsed - (lifeTime - fadeTime)) / fadeTime;
                text.color = new Color(
                    originalColor.r,
                    originalColor.g,
                    originalColor.b,
                    Mathf.Lerp(1f, 0f, t)
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        OnEnd?.Invoke();
    }
    #endregion
}
