using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class UpgradeRectsManager : MonoBehaviour
{
	private int actualUpgrade = -1;
	private int[] gettedUpgrades;
	private int[] maxUpgrades;
	private Ship ship;
	/*
	 * SPRITES:
	0->Speed up
	1->Missile
	2->Double
	3->Laser
	4->Option
	5->Shield
	6->Nothing
	 */
	[SerializeField] private Sprite[] blueSprites;
	[SerializeField] private Sprite[] yellowSprites;

	//upgrade rects on the UI
	[SerializeField] private Image[] spriteRects;

	[SerializeField] private KeyCode selectKey;
	PlayerInput playerInput;

	public void SetActualUpgrade(int value) { actualUpgrade = value; }
	public void SetSelectKey(KeyCode key)
    {
		selectKey = key;
    }
	public KeyCode GetSelectKey() { return selectKey; }
	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		maxUpgrades = new int[6];
		gettedUpgrades = new int[6];

		maxUpgrades[0] = 10;
		maxUpgrades[1] = 1;
		maxUpgrades[2] = 1;
		maxUpgrades[3] = 1;
		maxUpgrades[4] = 2;
		maxUpgrades[5] = 1;

		RestartGettedUpgrades();
	}
	public void SetInputDevice(InputDevice inputDevice)
	{
		// Pair the gamepad or joystick to a user.
		var user = InputUser.PerformPairingWithDevice(inputDevice);
		user.AssociateActionsWithUser(GetComponent<PlayerInput>().currentActionMap);
	}
	public void RestartGettedUpgrades()
    {
		for (int i = 0; i < 6; i++)
			gettedUpgrades[i] = 0;
	}
	public void InitRects()
    {
		for(int i = 0; i < 6; i++)
        {
			spriteRects[i].sprite = blueSprites[i];
		}
	}

	public void Init(KeyCode select, Ship newShip)
    {
		selectKey = select;
		ship = newShip;
		ship.GetComponent<Ship>().SetUpgradeRect(this);
    }
	void Update()
	{
		if (playerInput.actions["Select"].WasPressedThisFrame() || Input.GetKeyDown(selectKey))
		{
			SelectUpgrade();
		}
	}

	public void SelectUpgrade()
    {
		if (actualUpgrade > -1)
		{
			if (gettedUpgrades[actualUpgrade] < maxUpgrades[actualUpgrade])
			{
				switch (actualUpgrade)
				{
					case 0:
						ship.IncrementSpeed();
						break;
					case 1:
						ship.SetMissileUpgrade(true);
						break;
					case 2:
						ship.SetLaserUpgrade(true);
						break;
					case 3:
						ship.SetDoubleUpgrade(true);
						break;
					case 4:
						ship.AddOption();
						break;
					case 5:
						ship.CreateShield();
						break;
				}
				gettedUpgrades[actualUpgrade]++;
				if (gettedUpgrades[actualUpgrade] == maxUpgrades[actualUpgrade])
				{
					spriteRects[actualUpgrade].sprite = blueSprites[6];
				}
				else
				{
					spriteRects[actualUpgrade].sprite = blueSprites[actualUpgrade];
				}
				actualUpgrade = -1;
			}
		}
	}

	public void AddUpgrade()
    {
		if (actualUpgrade > -1)
		{
			if (gettedUpgrades[actualUpgrade] < maxUpgrades[actualUpgrade])
			{
				spriteRects[actualUpgrade].sprite = blueSprites[actualUpgrade];
			}
			else
			{
				spriteRects[actualUpgrade].sprite = blueSprites[6];
			}
		}
		if (actualUpgrade < 5)
		{
			actualUpgrade++;
		}
		else
		{
			actualUpgrade = 0;
		}
		if (gettedUpgrades[actualUpgrade] < maxUpgrades[actualUpgrade])
		{
			spriteRects[actualUpgrade].sprite = yellowSprites[actualUpgrade];
		}
		else
		{
			spriteRects[actualUpgrade].sprite = yellowSprites[6];
		}
	}

	public void DecrementShieldUpgrade()
	{
		gettedUpgrades[5]--;
		if (actualUpgrade != 5)
			spriteRects[5].sprite = blueSprites[5];
		else
			spriteRects[5].sprite = yellowSprites[5];
	}
}
