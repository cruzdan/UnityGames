using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Private Variables
    private bool isPressed = false;
    #endregion
    #region Public Properties
    public bool IsPressed { get { return isPressed; } }
    #endregion
    #region Functions
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
    #endregion
}