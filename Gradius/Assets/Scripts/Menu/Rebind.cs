using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class Rebind : MonoBehaviour
{
    [SerializeField] private InputActionReference[] actionsToRemap;
    [SerializeField] private Text controlText;
    private int totalActions = 6;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    
    private int shipIndex = 0;
    private int keyIndex = 0;
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
    public void PressButton()
    {
        Debug.Log("Pressing rebind, shipindex, control, keyindex, numberAction: " + shipIndex + "," + controlText + "," + keyIndex + "," + (shipIndex * totalActions + keyIndex));
        Debug.Log("first element: " + actionsToRemap[0].action);
    }
    public void StartRebinding()
    {
        EventSystem.current.SetSelectedGameObject(null);
        controlText.text = "PRESS ANY BUTTON";
        actionsToRemap[shipIndex * totalActions + keyIndex].action.Disable();
        rebindingOperation = actionsToRemap[shipIndex * totalActions + keyIndex].action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f).
            OnComplete(
            operation =>
            {
                controlText.text = InputControlPath.ToHumanReadableString(actionsToRemap[shipIndex * totalActions + keyIndex].action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                rebindingOperation.Dispose();
                actionsToRemap[shipIndex * totalActions + keyIndex].action.Enable();
            }
            ).Start();
    }
}
