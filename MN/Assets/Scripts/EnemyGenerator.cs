using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float timeToSpawnEnemy;
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyType;
    //0 -> left, 1 -> right
    [SerializeField] private int direction;
    [SerializeField] private float boundX;

    //group objects
    //0 -> car, 1-> zombie, 2 -> chicken, 3 -> fireball
    [SerializeField] private Transform[] enemyGroup;

    [SerializeField] private Transform target;
    //distance to generate enemies from the target
    [SerializeField] private float distance;

    private float timer;
    private GameObject enemy;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
    }
    public void SetEnemyGroup(int index, Transform parent)
    {
        enemyGroup[index] = parent;
    }
    public void SetTime(float time)
    {
        timeToSpawnEnemy = time;
    }
    public void SetEnemySpeed(float speed)
    {
        enemySpeed = speed;
    }
    public void SetEnemyType(int newType)
    {
        enemyType = newType;
    }
    //0 -> left, 1 -> right
    public void SetEnemyDirection(int dir)
    {
        direction = dir;
    }
    public void SetBoundX(float bound)
    {
        boundX = bound;
    }
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToSpawnEnemy / 5f;
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Mathf.Abs(transform.position.z - target.position.z);
        if (targetDistance <= distance)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timeToSpawnEnemy;
                GenerateEnemy();
            }
        }
    }

    void GenerateEnemy()
    {
        enemy = Instantiate(enemies[enemyType]) as GameObject;
        enemy.transform.position = transform.position;
        if(direction == 0)
        {
            enemy.transform.localEulerAngles = new Vector3(0, 270, 0);
            if (enemyType != 3)
            {
                enemy.GetComponent<ForwardMovement>().SetSpeed(enemySpeed);
            }
            else
            {
                enemy.GetComponent<FirebalMovement>().SetSpeed(-enemySpeed);
            }
        }
        else
        {
            enemy.transform.localEulerAngles = new Vector3(0, 90, 0);
            if (enemyType != 3)
            {
                enemy.GetComponent<ForwardMovement>().SetSpeed(enemySpeed);
            }
            else
            {
                enemy.GetComponent<FirebalMovement>().SetSpeed(enemySpeed);
            }
        }
        
        Bounds enemyBounds = enemy.GetComponent<Bounds>();
        enemyBounds.SetBoundX(boundX);
        enemyBounds.SetDirection(direction);
        enemy.transform.SetParent(enemyGroup[enemyType]);
    }
}
