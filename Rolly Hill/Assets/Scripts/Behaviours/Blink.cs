using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private float _paintingSpeed;
    private GameObject _blinkObject;
    private MeshRenderer _meshRenderer;
    private Color _currentColor;
    private bool _paintingDown = true;

    private void Start()
    {
        enabled = false;
    }

    public void BlinkPlatformBelowThisPosition()
    {
        AssignBelowBlinkObject();
        enabled = true;
        InitBlink();
    }

    void AssignBelowBlinkObject()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        SetBlinkObject(hit.transform.gameObject);
    }

    public void SetBlinkObject(GameObject blinkObject)
    {
        _blinkObject = blinkObject;
    }

    public void InitBlink()
    {
        _meshRenderer = _blinkObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (_paintingDown)
        {
            if (_meshRenderer.materials[2].color.r > 0)
            {
                UpdateCurrentColor();
                DecrementRedColor();
            }
            else
            {
                _paintingDown = false;
            }
        }
        else
        {
            if (_meshRenderer.materials[2].color.r < 1)
            {
                UpdateCurrentColor();
                IncrementRedColor(); 
            }
            else
            {
                _paintingDown = true;
            }
        }
    }

    void UpdateCurrentColor()
    {
        _currentColor = _meshRenderer.materials[2].color;
    }

    void IncrementRedColor()
    {
        _meshRenderer.materials[2].color = new Color(Mathf.Clamp01(_currentColor.r + _paintingSpeed * Time.deltaTime),
            _currentColor.g, _currentColor.b);
    }

    void DecrementRedColor()
    {
        _meshRenderer.materials[2].color = new Color(Mathf.Clamp01(_currentColor.r - _paintingSpeed * Time.deltaTime),
            _currentColor.g, _currentColor.b);
    }
}
