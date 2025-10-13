using UnityEngine;
using UnityEngine.EventSystems; // necesario para detectar eventos UI

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    public bool IsPressed { get { return isPressed; } }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}