using UnityEngine;

public class Background : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Material[] materials;
    [SerializeField] private MeshRenderer meshRenderer;
    #endregion
    #region Private Varibales
    private int backgroundIndex = -1;
    #endregion
    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(SquaresResolution.TotalSquaresX * 1.2f, SquaresResolution.TotalSquaresY * 1.2f);
    }

    public void ChangeBackground()
    {
        backgroundIndex = GenerateNewBackgroundIndex();
        meshRenderer.material = materials[backgroundIndex];
    }

    int GenerateNewBackgroundIndex()
    {
        int totalBackgrounds = materials.Length;
        int newIndex = Random.Range(0, totalBackgrounds);
        while(newIndex == backgroundIndex)
        {
            newIndex = Random.Range(0, totalBackgrounds);
        }
        return newIndex;
    }
    #endregion
}
