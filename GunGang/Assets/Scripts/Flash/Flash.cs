using System;
using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private ForwardMovement[] flashMovements;
    [SerializeField] private GameObject flashObject;
    [SerializeField] private float minFlashSpeed = 0.5f;
    [SerializeField] private float maxFlashSpeed = 1.5f;
    [SerializeField] private float flashDuration = 0.05f;
    [SerializeField] private float sphereRange = 2f;
    #endregion
    #region Private Variables
    private Coroutine flashCoroutine;
    #endregion
    #region Events
    public Action OnFlashEnd;
    #endregion
    #region Public Properties
    public GameObject FlashObject { get => flashObject; set => flashObject = value; }
    #endregion
    #region Functions
    public void SetFlashValues(Vector3 flashValues)
    {
        minFlashSpeed = flashValues.x;
        maxFlashSpeed = flashValues.y;
        flashDuration = flashValues.z;
    }

    public void StartFlash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        InitializeFlashMovements();
        flashObject.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        flashObject.SetActive(false);
        OnFlashEnd?.Invoke();
    }

    public void InitializeFlashMovements()
    {
        int total = flashMovements.Length;
        for (int i = 0; i < total; i++)
        {
            flashMovements[i].transform.localPosition = Vector3.zero;
            flashMovements[i].Init(UnityEngine.Random.insideUnitSphere * sphereRange);
        }
    }

    public void ResetFlash()
    {
        flashObject.SetActive(false);
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }
    #endregion
}
