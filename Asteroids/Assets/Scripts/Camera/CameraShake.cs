using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    #region Public Varibles
    public static CameraShake Instance;
    #endregion
    #region Serialized Variables
    [SerializeField] private Camera _camera;
    [Header("Shake Settings")]
    [SerializeField] private float defaultDuration = 0.15f;
    [SerializeField] private float defaultStrength = 0.3f;
    #endregion
    #region Private Variables
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;
    #endregion
    #region Functions
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        originalPosition = _camera.transform.localPosition;
    }

    public void Shake(float duration = -1f, float strength = -1f)
    {
        if (duration <= 0) duration = defaultDuration;
        if (strength <= 0) strength = defaultStrength;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * strength;
            _camera.transform.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        _camera.transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
    #endregion
}
