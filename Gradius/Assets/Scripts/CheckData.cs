using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckData : MonoBehaviour
{
    [SerializeField] private EnemyGenerator enemyGenerator;
    [SerializeField] private Level level;
    //EnemyData layer
    [SerializeField] private int layer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layer)
        {
            switch (collision.tag)
            {
                case "EnemyData":
                    enemyGenerator.CheckEnemyData(collision);
                    break;
                case "PhaseData":
                    level.CheckPhaseData(collision);
                    break;
            }
        }
    }
}
