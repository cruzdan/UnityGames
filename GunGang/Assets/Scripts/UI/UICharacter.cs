using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    [SerializeField] private Animator _legsAnimator;
    private void Start()
    {
        _legsAnimator.SetBool("Idle", false);
    }

}
