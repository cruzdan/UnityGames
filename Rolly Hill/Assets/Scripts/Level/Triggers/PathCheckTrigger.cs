using UnityEngine;

public class PathCheckTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnBallBigger;
    [SerializeField] private int _minimumBlocksToMakeBigger;
    private BlockCounter _ballBlockCounter;
    private Vector3 _increment = Vector3.one * 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BallCanBeBigger())
            {
                IncrementObjectScale(other.gameObject);
                CameraShake.Instance.Shake(0.1f, 0.2f);
                OnBallBigger.TriggerEvent();
            }
            _ballBlockCounter.ResetBlockCounter();
        }
    }

    bool BallCanBeBigger()
    {
        return _ballBlockCounter.GetBlockCounter() >= _minimumBlocksToMakeBigger;
    }

    void IncrementObjectScale(GameObject objectToIncrementScale)
    {
        objectToIncrementScale.transform.localScale = objectToIncrementScale.transform.localScale + _increment;
    }

    public void SetBlockCounter(BlockCounter blockCounter)
    {
        _ballBlockCounter = blockCounter;
    }
}
