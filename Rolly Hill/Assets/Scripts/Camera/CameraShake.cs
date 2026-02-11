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
    private Vector3 flyOriginalPosition;
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

        if (GameManager.Instance.IsInSky)
            flyOriginalPosition = _camera.transform.localPosition;

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * strength;

            if (GameManager.Instance.IsInSky)
                _camera.transform.localPosition = flyOriginalPosition + randomOffset;
            else
                _camera.transform.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (GameManager.Instance.IsInSky)
            _camera.transform.localPosition = flyOriginalPosition;
        else
            _camera.transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }

    public void UpdateFlyOriginalPosition()
    {
        flyOriginalPosition = _camera.transform.localPosition;
    }
    #endregion
}
