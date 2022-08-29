using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayerToEnemy : MonoBehaviour
{
    [SerializeField] private MNManager manager;
    private int collisions = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            manager.Restart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                collisions++;
                if (collisions > 1)
                {
                    collisions = 0;
                    manager.Restart();
                }
                break;
            case "Finish":
                manager.Restart();
                break;
        }
    }
}
