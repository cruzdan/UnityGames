using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScaleHoverAdder : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private Button[] buttonsWithoutScale;
    #endregion
    #region Functions
    private void Start()
    {
        Object[] buttonObjects = Resources.FindObjectsOfTypeAll(typeof(Button));
        foreach (var buttonObject in buttonObjects)
        {
            Button button = (Button)buttonObject;
            if (!buttonsWithoutScale.Contains(button))
            {
                UIButtonHoverScale buttonScaleHover = button.gameObject.AddComponent<UIButtonHoverScale>();
                buttonScaleHover.HoverScale = hoverScale;
            }
        }
    }
    #endregion
}
