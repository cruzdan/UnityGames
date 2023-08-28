using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private event Action OnCharacterTouched;
    public void ClearOnCharacterTouched()
    {
        OnCharacterTouched = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Character":
                other.GetComponent<DeleteMapObject>().ReturnObjectToPool();
                OnCharacterTouched?.Invoke();
                break;
            case "Player":
                other.gameObject.SetActive(false);
                OnCharacterTouched?.Invoke();
                break;
        }
    }
    
    public void SubscribeToOnCharacterTouched(Action action)
    {
        OnCharacterTouched += action;
    }
}
