using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    #region serialized Variables
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
    [SerializeField] private float originalCameraAmplitude = 0;
    [SerializeField] private float originalCameraFrequency = 0;
    #endregion
    #region Private Variables
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    #endregion
    #region Public Variables
    public static CinemachineCameraShake Instance;
    #endregion
    #region Functions
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            noise.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1f - (shakeTimer / shakeTimerTotal));
            noise.FrequencyGain = Mathf.Lerp(10, 0f, 1f - (shakeTimer / shakeTimerTotal) );

            if (shakeTimer <= 0f)
            {
                noise.AmplitudeGain = originalCameraAmplitude;
                noise.FrequencyGain = originalCameraFrequency;
            }
        }
    }

    public void Shake(float intensity, float duration)
    {
        startingIntensity = intensity;
        shakeTimer = duration;
        shakeTimerTotal = duration;
        noise.AmplitudeGain = intensity;
    }
    #endregion
}
