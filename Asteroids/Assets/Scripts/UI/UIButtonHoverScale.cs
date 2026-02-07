using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Serialized Variables
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float speed = 10f;
    #endregion
    #region Private Variables
    private Vector3 originalScale;
    private Vector3 targetScale;
    #endregion
    #region Public Properties
    public float HoverScale { get { return hoverScale; } set { hoverScale = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    #endregion
    #region Functions
    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            speed * Time.unscaledDeltaTime
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    private void OnDisable()
    {
        targetScale = originalScale;
        transform.localScale = originalScale;
    }
    #endregion
}
