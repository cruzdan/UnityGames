using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private float detectionRange;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public virtual void Idle()
    {

    }
    public virtual void StartDetection()
    {
    }

    public Player GetNearestPlayer()
    {
        Player nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Player player = client.PlayerObject.GetComponent<Player>();
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance && distance <= detectionRange)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
}
