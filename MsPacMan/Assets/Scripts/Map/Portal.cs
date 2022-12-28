using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Vector2 nextPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Fruit":
                collision.gameObject.SetActive(false);
                break;
            case "Ghost":
                collision.transform.position = nextPosition;
                collision.GetComponent<GhostBehaviour>().SetOnTunnel(true);
                break;
            case "Player":
                collision.transform.position = nextPosition;
                break;
        }
    }
}
