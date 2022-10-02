using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeControlsManager : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    private List<Ship> ships = new List<Ship>();
    private int shipIndex = 0;
    private int keyIndex = 0;
    private Text controlText;
    public void AddShip(Ship ship)
    {
        ships.Add(ship);
    }
    public void SetShipIndex(int index)
    {
        shipIndex = index;
    }
    public void SetControlText(Text control)
    {
        controlText = control;
    }
    public void SetKeyIndex(int index)
    {
        keyIndex = index;
    }
    public void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode != KeyCode.None && e.keyCode != KeyCode.Escape)
                {
                    controlText.text = e.keyCode.ToString();
                    if(keyIndex != 5)
                    {
                        //ship buttons
                        ChangeKey(e.keyCode);
                    }
                    else
                    {
                        //select button
                        ships[shipIndex].GetUpgradeRects().SetSelectKey(e.keyCode);
                    }
                    pauseManager.ChangeFromSelectToControlsMenu();
                    return;
                }
            }
        }
    }

    void ChangeKey(KeyCode key)
    {
        ships[shipIndex].SetKey(keyIndex, key);
    }
}
