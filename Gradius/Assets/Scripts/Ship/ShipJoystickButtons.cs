using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class is used to add the joystick buttons to the ship
 Attach this class to a ship and add the shoot and select buttons
 */
public class ShipJoystickButtons : MonoBehaviour
{
    [SerializeField] private List<KeyCode> buttons = new List<KeyCode>();
    private UpgradeRectsManager upgradeRects;
    private Ship ship;
    public void AddButton(KeyCode button)
    {
        buttons.Add(button);
    }
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Ship>();
        upgradeRects = ship.GetUpgradeRects();
    }

    // Update is called once per frame
    void Update()
    {
        if(buttons.Count > 1)
        {
            if (Input.GetKey(buttons[0]))
            {
                ship.Shoot();
            }
            if (Input.GetKey(buttons[1]))
            {
                upgradeRects.SelectUpgrade();
            }
        }
    }
}
