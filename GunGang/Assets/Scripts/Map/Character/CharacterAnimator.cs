using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _legsAnimator;
    public void PassFromIdleToWalk()
    {
        _legsAnimator.SetBool("Idle", false);
    }

    public void PassFromWalkToIdle()
    {
        _legsAnimator.SetBool("Idle", true);
    }

    public void SetLegsAnimator(Animator legsAnimator)
    {
        _legsAnimator = legsAnimator;
    }
}
