using UnityEngine;
//Class in charge of general enemy detection of players
public class EnemyDetection : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected float detectionRange;
    #endregion
    #region Public Properties
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public float DetectionRange { get => detectionRange; set => detectionRange = value; }
    #endregion
    #region Functions
    public virtual void Idle()
    {
        Player nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            enemy.PlayerTarget = nearestPlayer;
            enemy.StartChase();
        }
    }

    public virtual void StartDetection()
    {
    }

    public Player GetNearestPlayer()
    {
        if (!GameNetwork.Instance.IsOnline)
        {
            return GetNearestPlayerOffline();
        }
        else
        {
            return GetnearestPlayerOnline();
        }
    }

    public Player GetnearestPlayerOnline()
    {
        Player nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;
        foreach (var player in PlayerManager.Instance.Players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }

    public Player GetNearestPlayerOffline()
    {
        Player nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;
        foreach (Player player in enemy.PlayerManager.Players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
    #endregion
}
