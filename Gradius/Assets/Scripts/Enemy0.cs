using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : MonoBehaviour
{
    [SerializeField] private int line;
    [SerializeField] private EnemyManager enemyManager;
    
    public void SetLine(int newLine) { line = newLine; }
    public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
    public int GetLine() { return line; }
    public void Dead()
    {
        enemyManager.UpdateLineEnemies0(line, transform.position.x, transform.position.y);
    }
}
