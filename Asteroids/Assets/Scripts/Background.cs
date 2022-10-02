using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    int backgroundIndex = -1;
    MeshRenderer ren;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(SquaresResolution.TotalSquaresX, SquaresResolution.TotalSquaresY);
        backgroundIndex = GenerateNewBackgroundIndex();
        ren = GetComponent<MeshRenderer>();
        ren.material = materials[backgroundIndex];
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
