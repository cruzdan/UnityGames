using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransformYOrganizer : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<RectTransform> transformsInCanvas;
    [SerializeField] private float spacing = 100f;
    #endregion
    #region Functions
    public void OrganizeTransformsY()
    {
        if (canvas == null || transformsInCanvas == null || transformsInCanvas.Count == 0) return;
        float totalTransformsHeight = 0f;
        foreach (var transformInCanvas in transformsInCanvas)
        {
            totalTransformsHeight += transformInCanvas.rect.height;
        }
        float startY = (totalTransformsHeight + spacing * (transformsInCanvas.Count - 1)) / 2f;
        for (int i = 0; i < transformsInCanvas.Count; i++)
        {
            var transformInCanvas = transformsInCanvas[i];
            float posY = startY - (transformInCanvas.rect.height / 2f) - i * (transformInCanvas.rect.height + spacing);
            transformInCanvas.anchoredPosition = new Vector2(transformInCanvas.anchoredPosition.x, posY);
        }
    }
    #endregion
}
#if UNITY_EDITOR
[CustomEditor(typeof(TransformYOrganizer))]
public class TransformYOrganizerEditor : Editor
{
    private TransformYOrganizer transformYOrganizer;
    private void OnEnable()
    {
        transformYOrganizer = (TransformYOrganizer)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Organize Transforms Y"))
        {
            transformYOrganizer.OrganizeTransformsY();
        }
    }
}
#endif