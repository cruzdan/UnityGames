using UnityEngine;

public class SpawnPointCenter : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform floor;
    [SerializeField] private float targetHeight;
    #endregion
    #region Functions
    void Start()
    {
        

        transform.position = new Vector2(transform.position.x, floor.position.y + floor.GetComponent<SpriteRenderer>().size.y /*floor.localScale.y*/ / 2f + targetHeight / 2f);
        Destroy(this);
    }
    #endregion
}
