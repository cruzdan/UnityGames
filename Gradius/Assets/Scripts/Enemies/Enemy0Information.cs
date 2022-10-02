using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0Information : MonoBehaviour
{
    [SerializeField] private int line;
    [SerializeField] private EnemyManager enemyManager;
    
    public void SetLine(int newLine) { line = newLine; }
    public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
    public int GetLine() { return line; }
    public bool HasEnemyManager() { return enemyManager != null; }
    public void Dead()
    {
        enemyManager.UpdateLineEnemies0(line, transform.position.x, transform.position.y);
    }
}
