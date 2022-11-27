using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeGameMenuCreator : MonoBehaviour
{
    [SerializeField] private GameObject upgradesPrefab;
    private GameObject upgrade;

    public void CreateUpgrade(Ship ship, KeyCode selectKey, float posX, float posY, float scaleY)
    {
        upgrade = Instantiate(upgradesPrefab) as GameObject;
        upgrade.transform.SetParent(transform);
        upgrade.GetComponent<UpgradeRectsManager>().Init(selectKey, ship);
        upgrade.transform.localScale = new Vector3(1, scaleY, 1);
        upgrade.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
    }
}
