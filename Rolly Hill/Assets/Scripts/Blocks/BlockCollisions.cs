using UnityEngine;

public class BlockCollisions : MonoBehaviour
{
    [SerializeField] private GameEvent OnBlockTouched;
    [SerializeField] private BoxCollider _boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ParentToTransform(other.transform);
            DisableCollider();
            SFXManager.Instance.Play(AudioConstants.Instance.BlockGettedAudio);
            CameraShake.Instance.Shake(0.05f, 0.05f);
            OnBlockTouched.TriggerEvent();
        }
    }
    void ParentToTransform(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
    }
    void DisableCollider()
    {
        _boxCollider.enabled = false;
    }
}
