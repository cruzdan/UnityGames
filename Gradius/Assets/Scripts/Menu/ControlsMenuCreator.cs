using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuCreator : MonoBehaviour
{
    [SerializeField] private GameObject shipInfoPrefab;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private ChangeControlsManager selectKey;
    [SerializeField] private GameObject ship2Title;

    public void GenerateShipInfo(Vector2 position, Ship ship)
    {
        GameObject shipInfo;
        shipInfo = Instantiate(shipInfoPrefab) as GameObject;
        shipInfo.transform.SetParent(this.transform);
        shipInfo.transform.localScale = new(1f, 1f, 1f);
        shipInfo.GetComponent<RectTransform>().anchoredPosition = position;
        ShipControlsMenuController menuShipInfo = shipInfo.GetComponent<ShipControlsMenuController>();
        menuShipInfo.SetUpControlText(ship.GetUpKey().ToString());
        menuShipInfo.SetDownControlText(ship.GetDownKey().ToString());
        menuShipInfo.SetRightControlText(ship.GetRightKey().ToString());
        menuShipInfo.SetLeftControlText(ship.GetLeftKey().ToString());
        menuShipInfo.SetShootControlText(ship.GetShootKey().ToString());
        menuShipInfo.SetSelectControlText(ship.GetSelectKey().ToString());
        int total = shipInfo.transform.childCount;
        //KeyIndex and the ship.SetKeyIndex() controls are in the same order
        int keyIndex = 0;
        int shipIndex = ship.GetShipIndex();
        for (int i = 0; i < total; i++)
        {
            Button button;
            button = shipInfo.transform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                
                Text controlText;
                int newKeyIndex = keyIndex;
                //i - 1 is the index of the control text of button i
                controlText = shipInfo.transform.GetChild(i - 1).GetComponent<Text>();
                button.onClick.AddListener(() => selectKey.SetKeyIndex(newKeyIndex));
                button.onClick.AddListener(() => selectKey.SetControlText(controlText));
                button.onClick.AddListener(() => selectKey.SetShipIndex(shipIndex));
                button.onClick.AddListener(() => pauseManager.ChangeFromControlsToSelectMenu());
                keyIndex++;
            }
        }
    }

    public void SetActiveShip2Title(bool value)
    {
        ship2Title.SetActive(value);
    }
}
