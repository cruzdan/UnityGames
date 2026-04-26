using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpritesheetCreator : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private GridLayoutGroup grid;

    public void CreateSpriteSheet()
    {
        ClearSpriteSheet();
        SetGridItemsSize();
        AddSpriteItems();
    }

    public void ClearSpriteSheet()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void SetGridItemsSize()
    {
        RectTransform gridRect = grid.GetComponent<RectTransform>();
        float itemWidth = gridRect.rect.width / columns;
        float itemHeight = gridRect.rect.height / rows;
        grid.cellSize = new Vector2(itemWidth, itemHeight);
    }

    void AddSpriteItems()
    {
        int totalSprites = rows * columns;
        for (int i = 0; i < totalSprites && i < sprites.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, grid.transform);
            Image itemImage = newItem.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = sprites[i];
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(SpritesheetCreator))]
public class SpritesheetCreatorEditor : Editor
{
    SpritesheetCreator creator;
    private void OnEnable()
    {
        creator = (SpritesheetCreator)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        creator = (SpritesheetCreator)target;
        if (GUILayout.Button("Create Sprite Sheet"))
        {
            creator.CreateSpriteSheet();
        }
        if (GUILayout.Button("Clear Sprite Sheet"))
        {
            creator.ClearSpriteSheet();
        }
    }
}
#endif