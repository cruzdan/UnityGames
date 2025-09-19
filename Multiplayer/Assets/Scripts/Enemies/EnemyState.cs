using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private EnemyStateEnum currentEnemyState;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public EnemyStateEnum CurrentEnemyState { get { return currentEnemyState; } set { currentEnemyState = value; } }
    public enum EnemyStateEnum
    {
        Idle = 0,
        Chasing = 1,
        Attacking = 2,
        Searching = 3
    }
}
