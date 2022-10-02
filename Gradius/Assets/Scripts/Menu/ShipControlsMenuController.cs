using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControlsMenuController : MonoBehaviour
{
    [SerializeField] private Text upControl;
    [SerializeField] private Text downControl;
    [SerializeField] private Text rightControl;
    [SerializeField] private Text leftControl;
    [SerializeField] private Text shootControl;
    [SerializeField] private Text selectControl;
    
    public void SetUpControlText(string control) { upControl.text = control; }
    public void SetDownControlText(string control) { downControl.text = control; }
    public void SetRightControlText(string control) { rightControl.text = control; }
    public void SetLeftControlText(string control) { leftControl.text = control; }
    public void SetShootControlText(string control) { shootControl.text = control; }
    public void SetSelectControlText(string control) { selectControl.text = control; }
}
