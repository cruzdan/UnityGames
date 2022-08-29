using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject shipInfoPrefab;
    private GameObject shipInfo;

    public void GenerateShipInfo(Vector2 position, Ship ship)
    {
        shipInfo = Instantiate(shipInfoPrefab) as GameObject;
        shipInfo.transform.SetParent(this.transform);
        shipInfo.GetComponent<RectTransform>().anchoredPosition = position;
        MenuShipInfo menuShipInfo = shipInfo.GetComponent<MenuShipInfo>();
        menuShipInfo.SetUpControlText(ship.GetUpKey().ToString());
        menuShipInfo.SetDownControlText(ship.GetDownKey().ToString());
        menuShipInfo.SetRightControlText(ship.GetRightKey().ToString());
        menuShipInfo.SetLeftControlText(ship.GetLeftKey().ToString());
        menuShipInfo.SetShootControlText(ship.GetShootKey().ToString());
        menuShipInfo.SetSelectControlText(ship.GetSelectKey().ToString());
    }
}
