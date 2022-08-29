using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject upgradesPrefab;
    private GameObject upgrade;

    public void Init1Player(Ship ship, KeyCode selectKey)
    {
        upgrade = Instantiate(upgradesPrefab) as GameObject;
        upgrade.transform.SetParent(transform);
        upgrade.GetComponent<UpgradeRects>().Init(selectKey, ship);
        upgrade.transform.localScale = new Vector3(1, 1.3f, 1);
        upgrade.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -230);
    }

    public void Init2Players(Ship ship1, Ship ship2, KeyCode selectKey1, KeyCode selectKey2)
    {
        upgrade = Instantiate(upgradesPrefab) as GameObject;
        upgrade.transform.SetParent(transform);
        upgrade.GetComponent<UpgradeRects>().Init(selectKey1, ship1);
        upgrade.transform.localScale = Vector3.one;
        upgrade.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -220);

        //second player upgrades
        upgrade = Instantiate(upgradesPrefab) as GameObject;
        upgrade.transform.SetParent(transform);
        upgrade.transform.localScale = Vector3.one;
        upgrade.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -240);
        upgrade.GetComponent<UpgradeRects>().Init(selectKey2, ship2);
    }
}
