using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    bool isHeld;
    bool downThisFrame;
    bool upThisFrame;

    public bool IsHeld => isHeld;
    public bool ButtonDown => downThisFrame;
    public bool ButtonUp => upThisFrame;

    void LateUpdate()
    {
        downThisFrame = false;
        upThisFrame = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHeld) return;
        isHeld = true;
        downThisFrame = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHeld) return;
        isHeld = false;
        upThisFrame = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isHeld) return;
        isHeld = false;
        upThisFrame = true;
    }
}
