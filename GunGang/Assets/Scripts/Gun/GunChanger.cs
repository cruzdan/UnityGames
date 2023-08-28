using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChanger : MonoBehaviour
{
    [SerializeField] private Transform _charactersParent;
    [SerializeField] private Transform _mapObjectsParent;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _gunPrefabs;

    public void ChangeAllCharacterGuns(int gunIndex)
    {
        DestroyAllCharacterGuns();
        CreateNewCharacterGunsWithIndex(gunIndex);
        UpdatePoolCharacter();
    }

    void DestroyAllCharacterGuns()
    {
        DestroyAllCharacterGunsInCharactersParent();
        DestroyAllSpawnedCharacterGuns();
        DestroyPlayerGun();
    }

    void DestroyAllCharacterGunsInCharactersParent()
    {
        int totalCharacters = _charactersParent.childCount;
        for (int i = totalCharacters - 1; i >= 0; i--)
        {
            DestroyGunOfCharacterTransform(_charactersParent.GetChild(i));
        }
    }

    void DestroyGunOfCharacterTransform(Transform characterTransform)
    {
        DestroyImmediate(GetCharacterGun(characterTransform).gameObject);
    }

    Transform GetCharacterGun(Transform characterTransform)
    {
        return GetCharacterHand(characterTransform).GetChild(3);
    }

    Transform GetCharacterHandFromWalkingCharacter(Transform walkingCharacterTransform)
    {
        return walkingCharacterTransform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
    }

    Transform GetCharacterHand(Transform characterTransform)
    {
        return GetCharacterHandFromWalkingCharacter(characterTransform.GetChild(0));
    }

    void DestroyAllSpawnedCharacterGuns()
    {
        int total = _mapObjectsParent.childCount;
        Transform mapChild;
        for (int i = 0; i < total; i++)
        {
            mapChild = _mapObjectsParent.GetChild(i);
            if (mapChild.CompareTag("Character"))
            {
                DestroyGunOfCharacterTransform(mapChild);
            }
        }
    }

    void DestroyPlayerGun()
    {
        DestroyImmediate(GetCharacterHandFromWalkingCharacter(_player.GetChild(3)).GetChild(3).gameObject);
    }

    void CreateNewCharacterGunsWithIndex(int gunIndex)
    {
        CreateNewCharacterGunsWithIndexInCharactersParent(gunIndex);
        CreateNewSpawnedCharacterGunsWithIndex(gunIndex);
        CreateNewPlayerGunWithIndex(gunIndex);
    }

    void CreateNewCharacterGunsWithIndexInCharactersParent(int gunIndex)
    {
        int totalCharacters = _charactersParent.childCount;
        for (int i = totalCharacters - 1; i >= 0; i--)
        {
            Instantiate(_gunPrefabs[gunIndex], GetCharacterHand(_charactersParent.GetChild(i)));
        }
    }

    void CreateNewSpawnedCharacterGunsWithIndex(int gunIndex)
    {
        int total = _mapObjectsParent.childCount;
        Transform mapChild;
        for (int i = 0; i < total; i++)
        {
            mapChild = _mapObjectsParent.GetChild(i);
            if (mapChild.CompareTag("Character"))
            {
                Instantiate(_gunPrefabs[gunIndex], GetCharacterHand(mapChild));
            }
        }
    }

    void CreateNewPlayerGunWithIndex(int gunIndex)
    {
        Instantiate(_gunPrefabs[gunIndex], GetCharacterHandFromWalkingCharacter(_player.GetChild(3)));
    }

    void UpdatePoolCharacter()
    {
        ObjectPool.Instance.ChangePrefab(ObjectPool.PoolObjectType.Character, _charactersParent.GetChild(0).gameObject);
    }
}
