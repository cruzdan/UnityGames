using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTextureAdder : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private Sprite environmentTexture;
    [Header("Outline")]
    [SerializeField, Tooltip("Outline is a spriteRenderers copy without collider. Outlines should be the same quantity of spriteRenderers. Order in layer: -1")] 
    private List<SpriteRenderer> spriteOutlineRenderers;
    [SerializeField] private float outlineWidth;
    [SerializeField] private float outlineheight;

    [ContextMenu("Add Environment Texture")]
    private void AddEnvironmentTexture()
    {
        float environmentWidth;
        float environmentHeight;
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                environmentWidth = spriteRenderer.transform.localScale.x;
                environmentHeight = spriteRenderer.transform.localScale.y;

                spriteRenderer.sprite = environmentTexture;
                spriteRenderer.drawMode = SpriteDrawMode.Tiled;
                spriteRenderer.size = new Vector2(environmentWidth, environmentHeight);
                spriteRenderer.transform.transform.localScale = Vector3.one;

                spriteRenderer.GetComponent<BoxCollider2D>().size = new Vector2(environmentWidth, environmentHeight);
            }
        }
    }

    [ContextMenu("Add Outline")]
    public void AddOutline()
    {
        float environmentWidth;
        float environmentHeight;

        for (int i = 0; i < spriteRenderers.Count;i++)
        {
            var spriteRenderer = spriteRenderers[i];
            if (spriteRenderer != null)
            {
                environmentWidth = spriteRenderer.size.x;
                environmentHeight = spriteRenderer.size.y;

                spriteOutlineRenderers[i].size = new Vector2(environmentWidth + outlineWidth, environmentHeight + outlineheight);
                spriteOutlineRenderers[i].drawMode = SpriteDrawMode.Tiled;
                spriteOutlineRenderers[i].transform.transform.localScale = Vector3.one;
            }
        }
    }
}
