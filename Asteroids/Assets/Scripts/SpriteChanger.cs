using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    //0->ship, 1->asteroid, 2->bullet
    [SerializeField] private SpriteRenderer[] spriteObjects;
    [SerializeField] private Sprite[] shipSprites;
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private Sprite[] bulletSprites;

    private void Start()
    {
        Init();
    }
    public void ChangeSprite(int objectIndex, int spriteIndex)
    {
        switch (objectIndex)
        {
            case 0:
                spriteObjects[objectIndex].sprite = shipSprites[spriteIndex];
                break;
            case 1:
                spriteObjects[objectIndex].sprite = asteroidSprites[spriteIndex];
                break;
            case 2:
                spriteObjects[objectIndex].sprite = bulletSprites[spriteIndex];
                break;
        }
    }

    public void Init()
    {
        spriteObjects[0].sprite = shipSprites[0];
        spriteObjects[1].sprite = asteroidSprites[0];
        spriteObjects[2].sprite = bulletSprites[0];
    }
}
