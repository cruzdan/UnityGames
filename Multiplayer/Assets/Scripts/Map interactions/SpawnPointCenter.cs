using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCenter : MonoBehaviour
{
    [SerializeField] private Transform floor;
    [SerializeField] private float targetHeight;
    void Start()
    {
        transform.position = new Vector2(transform.position.x, floor.position.y + floor.localScale.y / 2f + targetHeight / 2f);
        Destroy(this);
    }
}
