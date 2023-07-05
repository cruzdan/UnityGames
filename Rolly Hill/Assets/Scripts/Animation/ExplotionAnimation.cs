using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionAnimation : MonoBehaviour
{
    #region Circle
    [SerializeField] Transform _centralCircle;
    [SerializeField] float _maxSize;
    [SerializeField] float _expandSpeed;
    #endregion

    #region Elements
    [SerializeField] Transform[] _elementsMovingUp;
    [SerializeField] private Transform[] _initialTransforms;
    private float _totalElements;
    private Vector3[] _initialPositions;
    #endregion

    [SerializeField] private Transform _targetTransform;
    Vector3 _expandSize = Vector3.zero;
    Vector3 _initialCircleSize = new(1, 0.1f, 1);
    void Start()
    {
        _totalElements = _elementsMovingUp.Length;
        AssignInitialPositions();
        _initialTransforms = null;
        PathCheckTrigger.OnBallBigger += Enable;
        PathCheckTrigger.OnBallBigger += ResetExplosion;
        PathCheckTrigger.OnBallBigger += EnableBigWord;
        SkyPhase.OnSkyPhase += Enable;
        SkyPhase.OnSkyPhase += ResetExplosion;
        SkyPhase.OnSkyPhase += DisableBigWord;
        SkyPhase.OnFloorPhase += Enable;
        SkyPhase.OnFloorPhase += ResetExplosion;
        SkyPhase.OnFloorPhase += DisableBigWord;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PathCheckTrigger.OnBallBigger -= Enable;
        PathCheckTrigger.OnBallBigger -= ResetExplosion;
        PathCheckTrigger.OnBallBigger -= EnableBigWord;
        SkyPhase.OnSkyPhase -= Enable;
        SkyPhase.OnSkyPhase -= ResetExplosion;
        SkyPhase.OnSkyPhase -= DisableBigWord;
        SkyPhase.OnFloorPhase -= Enable;
        SkyPhase.OnFloorPhase -= ResetExplosion;
        SkyPhase.OnFloorPhase -= DisableBigWord;
    }

    void AssignInitialPositions()
    {
        int total = _initialTransforms.Length;
        _initialPositions = new Vector3[total];
        for(int i = 0; i < total; i++)
        {
            _initialPositions[i] = _initialTransforms[i].localPosition;
        }
    }

    void Update()
    {
        ExpandCircle();
        MoveElements();
        if (HasReachedMaxSize())
        {
            enabled = false;
            gameObject.SetActive(false);
        }
    }

    void ExpandCircle()
    {
        _expandSize.x = _expandSpeed * Time.deltaTime;
        _expandSize.z = _expandSize.x;
        _centralCircle.localScale += _expandSize;
    }

    void MoveElements()
    {
        for(int i = 0; i < _totalElements; i++)
        {
            MoveElementToLocalUp(i);
        }
    }

    void MoveElementToLocalUp(int elementIndex)
    {
        _elementsMovingUp[elementIndex].localPosition += Time.deltaTime * _expandSpeed * _elementsMovingUp[elementIndex].up.normalized;
    }

    bool HasReachedMaxSize()
    {
        return _centralCircle.localScale.x >= _maxSize;
    }

    void ResetExplosion()
    {
        InitCentralCirclePosition();
        ResetElementPositons();
        transform.position = _targetTransform.position;
    }

    void InitCentralCirclePosition()
    {
        _centralCircle.localPosition = _initialPositions[0];
        _centralCircle.localScale = _initialCircleSize;
    }

    void ResetElementPositons()
    {
        int total = _elementsMovingUp.Length;
        for (int i = 0; i < total; i++)
        {
            _elementsMovingUp[i].localPosition = _initialPositions[i + 1];
        }
    }

    void Enable()
    {
        gameObject.SetActive(true);
        enabled = true;
    }

    void EnableBigWord()
    {
        _elementsMovingUp[12].gameObject.SetActive(true);
    }

    void DisableBigWord()
    {
        _elementsMovingUp[12].gameObject.SetActive(false);
    }
}
