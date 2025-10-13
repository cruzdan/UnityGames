using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonYOrganizer : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<RectTransform> buttonsInCanvas;
    [SerializeField] private float spacing = 100f;
    public void OrganizeButtonsY()
    {
        if (canvas == null || buttonsInCanvas == null || buttonsInCanvas.Count == 0) return;
        float totalButtonsHeight = 0f;
        foreach (var button in buttonsInCanvas)
        {
            totalButtonsHeight += button.rect.height;
        }
        float startY = (totalButtonsHeight + spacing * (buttonsInCanvas.Count - 1)) / 2f;
        for (int i = 0; i < buttonsInCanvas.Count; i++)
        {
            var button = buttonsInCanvas[i];
            float posY = startY - (button.rect.height / 2f) - i * (button.rect.height + spacing);
            button.anchoredPosition = new Vector2(button.anchoredPosition.x, posY);
        }
        Debug.Log("Buttons organized vertically.");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonYOrganizer))]
public class ButtonYOrganizerEditor : Editor
{
    private ButtonYOrganizer buttonYOrganizer;
    private void OnEnable()
    {
        buttonYOrganizer = (ButtonYOrganizer)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Organize Buttons Y"))
        {
            buttonYOrganizer.OrganizeButtonsY();
        }
    }
}
#endif