using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotation : MonoBehaviour
{
    [SerializeField] private float _slowXSpeed;
    [SerializeField] private GlobalRotation _playerRotation;

    void FixedUpdate()
    {
        _playerRotation.DecrementRotationSpeedX(_slowXSpeed * Time.fixedDeltaTime);
        if(_playerRotation.GetRotationSpeedX() <= 0)
        {
            _playerRotation.SetRotationSpeedX(0);
            this.enabled = false;
        }
    }
}
