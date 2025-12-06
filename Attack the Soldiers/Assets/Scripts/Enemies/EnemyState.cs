using UnityEngine;
//Class in charge of maintaining the enemy state
public class EnemyState : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] protected Enemy enemy;
    [SerializeField] private EnemyStateEnum currentEnemyState;
    #endregion
    #region Public Properties
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public EnemyStateEnum CurrentEnemyState 
    { get { return currentEnemyState; } set { currentEnemyState = value; } }
    #endregion
}

public enum EnemyStateEnum
{
    Idle = 0,
    Chasing = 1,
    Attacking = 2,
    Searching = 3
}
