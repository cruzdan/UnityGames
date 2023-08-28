using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private Transform _charactersParent;
    [SerializeField] private Transform _mapObjectsParent;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _walkingCharacterPrefabs;
	
    public void ChangeAllCharactersForPrefabWithIndex(int characterIndex)
    {
        DestroyAllWalkingCharacters();
        AddToAllCharactersNewWalkingCharacterWithIndex(characterIndex);
        UpdatePoolCharacter();
    }

    void DestroyAllWalkingCharacters()
    {
        DestroyAllWalkingCharactersInCharactersParent();
        DestroyAllSpawnedWalkingCharacters();
        DestroyPlayerWalkingCharacter();
    }

    void DestroyAllWalkingCharactersInCharactersParent()
    {
        int totalCharacters = _charactersParent.childCount;
        for (int i = totalCharacters - 1; i >= 0; i--)
        {
            DestroyFirstChildOfTransform(_charactersParent.GetChild(i));
        }
    }

    void DestroyFirstChildOfTransform(Transform targetTransform)
    {
        DestroyImmediate(targetTransform.GetChild(0).gameObject);
    }

    void DestroyAllSpawnedWalkingCharacters()
    {
        int total = _mapObjectsParent.childCount;
        Transform mapChild;
        for(int i = 0; i < total; i++)
        {
            mapChild = _mapObjectsParent.GetChild(i);
            if (mapChild.CompareTag("Character"))
            {
                DestroyFirstChildOfTransform(mapChild);
            }
        }
    }

    void DestroyPlayerWalkingCharacter()
    {
        DestroyImmediate(_player.GetChild(3).gameObject);
    }

    void AddToAllCharactersNewWalkingCharacterWithIndex(int characterIndex)
    {
        AddNewWalkingCharactersWithIndexInCharactersParent(characterIndex);
        AddNewWalkingCharactersToSpawnedCharactersWithIndex(characterIndex);
        AddNewWalkingCharacterToPlayer(characterIndex);
    }

    void AddNewWalkingCharactersWithIndexInCharactersParent(int characterIndex)
    {
        int totalCharacters = _charactersParent.childCount;
        for (int i = 0; i < totalCharacters; i++)
        {
            AddNewWalkingCharacterToTransform(characterIndex, _charactersParent.GetChild(i));
        }
    }

    void AddNewWalkingCharactersToSpawnedCharactersWithIndex(int characterIndex)
    {
        int totalMapObjects = _mapObjectsParent.childCount;
        Transform character;
        for (int i = 0; i < totalMapObjects; i++)
        {
            character = _mapObjectsParent.GetChild(i);
            if (character.CompareTag("Character"))
            {
                AddNewWalkingCharacterToTransform(characterIndex, character);
            }
        }
    }

    void AddNewWalkingCharacterToPlayer(int characterIndex)
    {
        AddNewWalkingCharacterToTransform(characterIndex, _player);
    }

    void AddNewWalkingCharacterToTransform(int characterIndex, Transform targetTransform)
    {
        Animator characterLegsAnimator = Instantiate(_walkingCharacterPrefabs[characterIndex],
                targetTransform).GetChild(3).GetComponent<Animator>();
        targetTransform.GetComponent<CharacterAnimator>().SetLegsAnimator(characterLegsAnimator);
    }

    void UpdatePoolCharacter()
    {
        ObjectPool.Instance.ChangePrefab(ObjectPool.PoolObjectType.Character, _charactersParent.GetChild(0).gameObject);
    }
}
