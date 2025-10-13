using UnityEngine;
using UnityEngine.UI;

public class MobilePlayerControls : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Shoot playerShoot;

    [Header("UI Controles")]
    [SerializeField] private Joystick joystick; // Asume que usas un componente Joystick
    [SerializeField] private Button shootButton;

    private void Start()
    {
        //if (shootButton != null)
        //    shootButton.onClick.AddListener(OnShootButtonPressed);
    }

    private void Update()
    {
        //Debug.Log("Player Movement: " + playerMovement);
        //if (playerMovement != null && joystick != null)
        //{
        //    Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        //    //playerMovement.SetMobileInput(moveInput);
        //}
    }

    private void OnShootButtonPressed()
    {
        if (playerShoot != null)
            playerShoot.ShootCurrentWeapon();
    }
}