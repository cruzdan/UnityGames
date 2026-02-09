using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Transform _transformToFollow;
    [SerializeField] private Transform _camera;
    #endregion
    #region Private Variables
    private Vector3 _position;
    #endregion
    #region Functions
    private void Start()
    {
        _position = _transformToFollow.position;
        _camera.SetParent(_transformToFollow);
    }

    void Update()
    {
        _position.z = transform.position.z;
        _transformToFollow.position = _position;
    }
    #endregion
}
