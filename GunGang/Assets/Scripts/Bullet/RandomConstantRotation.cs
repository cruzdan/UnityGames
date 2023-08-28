using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomConstantRotation : MonoBehaviour
{
    [SerializeField] private ConstantRotation _constantRotation;
    private void OnEnable()
    {
        _constantRotation.SetRotationVector(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
