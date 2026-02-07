using UnityEngine;
using System.Collections;

public class ScoreBounceTMP : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private float bounceScale = 1.3f;
    [SerializeField] private float bounceDuration = 0.12f;
    [SerializeField] private Transform scoreTransform;
    #endregion
    #region Private Variables
    private Vector3 originalScale;
    private Coroutine bounceRoutine;
    #endregion
    #region Functions
    void Awake()
    {
        if (scoreTransform == null)
            scoreTransform = transform;
        originalScale = scoreTransform.localScale;
    }

    public void Bounce()
    {
        if (bounceRoutine != null)
            StopCoroutine(bounceRoutine);

        bounceRoutine = StartCoroutine(BounceRoutine());
    }

    IEnumerator BounceRoutine()
    {
        float half = bounceDuration * 0.5f;

        scoreTransform.localScale = originalScale * bounceScale;
        yield return new WaitForSeconds(half);

        scoreTransform.localScale = originalScale;
        bounceRoutine = null;
    }
    #endregion
}
