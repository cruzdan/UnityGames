using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    private int backgroundIndex = -1;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(SquaresResolution.TotalSquaresX, SquaresResolution.TotalSquaresY);
        backgroundIndex = GenerateNewBackgroundIndex();
        meshRenderer = GetComponent<MeshRenderer>();
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
}
