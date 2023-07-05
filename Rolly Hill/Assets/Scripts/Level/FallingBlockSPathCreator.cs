using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockSPathCreator : MonoBehaviour
{
    [SerializeField] private float _inititalPositionY;
    [SerializeField] private float _finalPositionY;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _timeToGenerateBlock;
    [SerializeField] private int _blocksAmount;
    [SerializeField] private float _centerPositionX;
    [SerializeField] private float _xDistanceMultiplier;
    [SerializeField] private float _zPositionIncrement;
    [SerializeField] private float _initialZPosition;

    private GameObject _auxiliarObject;
    private Vector3 _auxiliarPosition = new();
    private FallOnYAxis _auxiliarMoveOnY;

    private float _timer;
    private int _blocksCreated;
    private float _currentZPosition;

    [SerializeField] private float _sinIncrement;
    private float _sinCurrentValue = 0;
    
    private void Start()
    {
        _auxiliarPosition.y = _inititalPositionY;
    }
    public void ResetSPathValues()
    {
        _timer = 0;
        _blocksCreated = 0;
        _currentZPosition = _initialZPosition;
        _sinCurrentValue = 0;
        _auxiliarPosition.x = 0;
        _auxiliarPosition.z = 0;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _timeToGenerateBlock)
        {
            GenerateBlockOnSPath();
            _timer = 0;
            _blocksCreated++;
            if(HasCreatedAllBlocks())
            {
                this.enabled = false;
            }
        }
    }
    void GenerateBlockOnSPath()
    {
        _auxiliarObject = BlockPool.Instance.GetObjectFromPool();
        UpdateSPosition();
        AddFallScript();
        InitFallScript();
    }

    void UpdateSPosition()
    {
        _auxiliarPosition.x = _xDistanceMultiplier * Mathf.Sin(_sinCurrentValue) + _centerPositionX;
        _auxiliarPosition.z = _currentZPosition;
        _currentZPosition += _zPositionIncrement;
        _sinCurrentValue += _sinIncrement;
        _auxiliarObject.transform.position = _auxiliarPosition;
    }

    void AddFallScript()
    {
        _auxiliarMoveOnY = _auxiliarObject.AddComponent<FallOnYAxis>();
    }

    void InitFallScript()
    {
        _auxiliarMoveOnY.InitPosition();
        _auxiliarMoveOnY.SetFinalPositionY(_finalPositionY);
        _auxiliarMoveOnY.SetFallSpeed(_fallSpeed);
    }

    bool HasCreatedAllBlocks()
    {
        return _blocksCreated >= _blocksAmount;
    }
}
