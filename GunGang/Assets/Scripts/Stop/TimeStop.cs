using System.Collections;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    #region Serialized Varibales
    [SerializeField] private float stopDuration = 0.07f;
    #endregion
    #region Private Varibales
    private Coroutine stopCoroutine;
    #endregion
    #region Public Varibales
    public static TimeStop Instance;
    #endregion
    #region Functions
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StopTime()
    {
        if (stopCoroutine != null)
            StopCoroutine(stopCoroutine);
        stopCoroutine = StartCoroutine(StopTimeRoutine());
    }

    IEnumerator StopTimeRoutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(stopDuration);
        Time.timeScale = 1f;
    }
    #endregion
}
