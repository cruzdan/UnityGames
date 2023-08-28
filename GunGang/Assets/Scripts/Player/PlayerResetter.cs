using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResetter : MonoBehaviour
{
    [SerializeField] private Vector3 _initialPossition;

    public void RestartPosition()
    {
        transform.position = _initialPossition;
    }

    public void RestartRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
