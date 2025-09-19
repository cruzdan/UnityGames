using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private float speed;
    public Enemy Enemy { get => enemy; set => enemy = value; }
    public virtual void Chase()
    {

    }

    public virtual void StartChase()
    {
    }
}
