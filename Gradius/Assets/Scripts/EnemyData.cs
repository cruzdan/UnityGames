using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private int type;
    [SerializeField] private bool upgrade;
    public void SetType(int newType) { type = newType; }
    public void SetUpgrade(bool newUpgrade) { upgrade = newUpgrade; }
    public int GetEnemyType() { return type; }
    
    public bool GetUpgrade() { return upgrade; }
}
