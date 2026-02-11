using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnDeadPhaseStarts;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SFXManager.Instance.Play(AudioConstants.Instance.BallHittedAudio);
            OnDeadPhaseStarts.TriggerEvent();
            CameraShake.Instance.Shake(0.2f, 0.3f);
        }
    }
}
