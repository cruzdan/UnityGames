using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReturner : MonoBehaviour
{
    [SerializeField] private Transform _mapObjectsParent;

    public void ReturnAllCharactersFromMapObjectsParentToPool()
    {
        int totalMapObjects = _mapObjectsParent.childCount;
        Transform character;
        for (int i = 0; i < totalMapObjects; i++)
        {
            character = _mapObjectsParent.GetChild(i);
            if (character.CompareTag("Character"))
            {
                ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Explosion, _mapObjectsParent.GetChild(i).position);
                ObjectPool.Instance.ReturnObjectToPool(_mapObjectsParent.GetChild(i).gameObject, ObjectPool.PoolObjectType.Character);
            }
        }
    }
}
