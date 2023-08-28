using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    #region CharacterBehaviours
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ReachTarget _rt;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private FollowTransformOnXZAxis _followXZ;
    [SerializeField] private RotateToForward _rotateToForward;
    [SerializeField] private Shoot _shoot;
    [SerializeField] private CharacterAnimator _legsAnimator;
    #endregion

    Transform _transform;
    bool _isFollowingPlayer;
    bool _isOnFloor;
    bool _isPartner;

    private void OnEnable()
    {
        _rotateToForward.SubscribeToOnCompletedRotation(EnableShoot);
    }

    private void OnDisable()
    {
        _rotateToForward.ClearOnCompletedRotation();
    }

    private void Start()
    {
        _transform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Floor":
                FloorTrigger();
                break;
            case "Player":
                FollowTargetTrigger();
                _isPartner = true;
                break;
            case "Character":
                if (other.GetComponent<CharacterBehaviour>().IsPartner())
                {
                    FollowTargetTrigger();
                    _isPartner = true;
                }
                break;
        }
    }

    void FloorTrigger()
    {
        if (!_isOnFloor)
        {
            _isOnFloor = true;
            StopRBVelocity();
            SetPositionOnFloor();
            if (!_isFollowingPlayer)
            {
                TouchesFloorButNotPlayer();
            }
            else if (_followXZ.enabled)
            {
                TouchesFloorAterTouchesPlayer();
            }
            _legsAnimator.PassFromIdleToWalk();
        }
    }

    void StopRBVelocity()
    {
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }

    void SetPositionOnFloor()
    {
        _transform.position = new Vector3(_transform.position.x, 1.5f, _transform.position.z);
    }

    void TouchesFloorButNotPlayer()
    {
        _rt.enabled = true;
    }

    void TouchesFloorAterTouchesPlayer()
    {
        _followXZ.enabled = false;
        _transform.SetParent(GameManager.Instance.GetPlayerCharacterParent());
        _rb.isKinematic = true;
        gameObject.layer = 0;
    }

    void FollowTargetTrigger()
    {
        if (!_isFollowingPlayer)
        {
            _rt.enabled = false;
            _isFollowingPlayer = true;
            if (_isOnFloor)
            {
                TouchesPlayerAfterTouchesFloor();
            }
            else
            {
                TouchesPlayerButNotTouchesFloor();
            }
            _rotateToForward.enabled = true;
        }
    }

    void TouchesPlayerAfterTouchesFloor()
    {
        _transform.SetParent(GameManager.Instance.GetPlayerCharacterParent());
        _rb.isKinematic = true;
    }

    void TouchesPlayerButNotTouchesFloor()
    {
        _followXZ.enabled = true;
        _followXZ.RestartVars();
        gameObject.layer = 6;
    }

    public void SetTrigger(bool value)
    {
        _collider.isTrigger = value;
    }

    public void RestartVars()
    {
        _isFollowingPlayer = false;
        _isOnFloor = false;
        _rotateToForward.enabled = false;
        _rotateToForward.ResetVariables();
        _shoot.enabled = false;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _collider.isTrigger = false;
        _rt.enabled = false;
        _followXZ.enabled = false;
        _isPartner = false;
    }

    public void EnableShoot()
    {
        _shoot.enabled = true;
    }

    public bool IsPartner()
    {
        return _isPartner;
    }
}