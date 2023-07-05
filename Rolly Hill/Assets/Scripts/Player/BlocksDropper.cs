using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDropper : MonoBehaviour
{
    [SerializeField] private int _blocksDroppedPerScoreDistance;
    [SerializeField] private float _dropSpeed;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private SlowRotation _slowRotation;
    private float _distanceToDropBlock;
    private float _distanceTraveled = 0;

    Vector3 _childBlockPosition;
    Vector3 _dropForce;
    private BlockDrop _childBlock;

    void Start()
    {
        _distanceToDropBlock = 1f / _blocksDroppedPerScoreDistance;
        _dropForce = new Vector3(0, 3, 0);
    }

    void Update()
    {
        IncrementDistanceTraveled();
        if (HasTraveledDIstanceToDropBlock())
        {
            if(HasChilds())
            {
                _distanceTraveled = 0;
                DropFirstChildBlock();
            }
            else
            {
                SlowMovement();
            }
        }
    }

    void IncrementDistanceTraveled()
    {
        _distanceTraveled += _dropSpeed * Time.deltaTime;
    }

    bool HasTraveledDIstanceToDropBlock()
    {
        return _distanceTraveled >= _distanceToDropBlock;
    }

    bool HasChilds()
    {
        return transform.childCount > 0;
    }
    void DropFirstChildBlock()
    {
        _childBlock = transform.GetChild(0).GetComponent<BlockDrop>();
        SetChildBlockPositionOverAndFrontOfThisTransform();
        GenerateRandomDropForce();
        _childBlock.DropFromPositionWithForce(_childBlockPosition, _dropForce);
    }

    void SetChildBlockPositionOverAndFrontOfThisTransform()
    {
        _childBlockPosition.x = _childBlock.transform.position.x;
        _childBlockPosition.z = _childBlock.transform.position.z + 2;
        _childBlockPosition.y = transform.position.y + transform.localScale.y / 2 + _childBlock.transform.localScale.y;
    }

    void GenerateRandomDropForce()
    {
        _dropForce.x = Random.Range(-2f, 2f);
        _dropForce.z = Random.Range(4f, 7f);
    }

    void SlowMovement()
    {
        _playerMovement.InitStopMovement();
        _slowRotation.enabled = true;
        this.enabled = false;
    }
}
