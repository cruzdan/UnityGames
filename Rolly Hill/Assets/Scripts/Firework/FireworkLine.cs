using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkLine : MonoBehaviour
{
    [SerializeField] private float _maxDistanceToTravel;
    [SerializeField] private float _speed;
    [SerializeField] private FireworkBlock _fireworkBlock;
    [SerializeField] private Transform _spawnTransform;
    private float _distanceTraveled;

    private void Update()
    {
        MoveToLocalUp();
        if (HasTraveledMaxDistance())
        {
            ActivateFireworkBlockWithCurrentPositionAndRotation();
            Disable();
        }
    }

    void MoveToLocalUp()
    {
        transform.position += Time.deltaTime * _speed * transform.up.normalized;
        _distanceTraveled += Time.deltaTime * _speed;
    }

    bool HasTraveledMaxDistance()
    {
        return _distanceTraveled >= _maxDistanceToTravel;
    }

    void ActivateFireworkBlockWithCurrentPositionAndRotation()
    {
        _fireworkBlock.enabled = true;
        _fireworkBlock.gameObject.SetActive(true);
        _fireworkBlock.Init(transform.position, transform.up.normalized);
    }

    void Disable()
    {
        enabled = false;
        gameObject.SetActive(false);
    }

    public void Init()
    {
        _distanceTraveled = 0;
        Enable();
        transform.position = _spawnTransform.position + Vector3.up;
    }

    void Enable()
    {
        enabled = true;
        gameObject.SetActive(true);
    }
}
