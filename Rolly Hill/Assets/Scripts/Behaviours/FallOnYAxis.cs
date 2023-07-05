using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnYAxis : MonoBehaviour
{
    [SerializeField] private float _finalPositionY;
    private Vector3 _position = new();
    [SerializeField] private float _fallSpeed;
    public void SetFinalPositionY(float position)
    {
        _finalPositionY = position;
    }

    public void SetFallSpeed(float speed)
    {
        _fallSpeed = speed;
    }

    public void InitPosition()
    {
        _position = transform.position;
    }

    void Update()
    {
        LowerPosition();
        if(HasReachedTheFinalPosition())
        {
            SetPositionOnFinalPosition();
            Destroy(this);
        }
    }

    void LowerPosition()
    {
        _position.y -= _fallSpeed * Time.deltaTime;
        transform.position = _position;
    }

    bool HasReachedTheFinalPosition()
    {
        return _position.y <= _finalPositionY;
    }

    void SetPositionOnFinalPosition()
    {
        _position.y = _finalPositionY;
        transform.position = _position;
    }
}
