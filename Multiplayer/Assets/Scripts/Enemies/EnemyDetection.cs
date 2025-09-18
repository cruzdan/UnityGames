using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private float detectionRange;
    [SerializeField] private PlayerManager playerManager;
    public virtual void Idle()
    {

    }

    public Player GetNearestPlayer()
    {
        playerManager = playerManager != null ? playerManager : FindObjectOfType<PlayerManager>();
        Player[] players = playerManager.Players.ToArray();
        Player nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;
        foreach (Player player in players)
        {
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
