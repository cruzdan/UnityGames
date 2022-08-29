using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private UpgradeRects upgradeRects;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int shipIndex = 0;
    private int lifes = 10;
    private Animator animator;
    private bool dead = false;
    public void SetUpgradeRect(UpgradeRects upgrade) { upgradeRects = upgrade; }
    public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
    public void SetShipIndex(int index) { shipIndex = index; }
    public void SetDead(bool value) { dead = value; }
    public int GetShipIndex() { return shipIndex; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public int GetLifes()
    {
        return lifes;
    }

    public void SetLifes(int newLifes)
    {
        lifes = newLifes;
        if(lifes <= 3)
        {
            animator.SetBool("Low", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dead)
        {
            Enemy enemy;
            switch (collision.gameObject.layer)
            {
                //enemy layer
                case 8:
                    enemy = collision.GetComponent<Enemy>();
                    if (!enemy.GetDead())
                    {
                        switch (collision.tag)
                        {
                            case "Enemy0":
                                collision.gameObject.GetComponent<Enemy0>().Dead();
                                EnemyDead(enemy, collision);
                                break;
                            case "Enemy9":
                                int enemyLifes = enemy.GetLifes();
                                if (enemyLifes <= lifes)
                                {
                                    collision.gameObject.GetComponent<Enemy9>().Dead();
                                }
                                EnemyDead(enemy, collision);
                                break;
                            default:
                                EnemyDead(enemy, collision);
                                break;
                        }
                    }
                    break;
                //enemy 4, 5down and 5up layer
                case 17:
                case 18:
                case 19:
                    enemy = collision.GetComponent<Enemy>();
                    if (!enemy.GetDead())
                    {
                        EnemyDead(enemy, collision);
                    }
                    break;
                //EnemyBullet layer
                case 7:
                    SetLifes(lifes - 1);
                    Destroy(collision.gameObject);
                    if (lifes <= 0)
                    {
                        dead = true;
                        upgradeRects.DecrementShieldUpgrade();
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }
    void EnemyDead(Enemy enemy, Collider2D collision)
    {
        int enemyLifes = enemy.GetLifes();
        if (enemyLifes <= lifes)
        {
            if (enemy.GetUpgrade())
            {
                enemyManager.GenerateUpgrade(collision.transform.position.x, collision.transform.position.y);
            }
            enemy.SetDead(true);
            enemy.AddScore(shipIndex);
            Destroy(collision.gameObject);
            SetLifes(lifes - enemyLifes);
            if(lifes <= 0)
            {
                dead = true;
                upgradeRects.DecrementShieldUpgrade();
                Destroy(this.gameObject);
            }
        }
        else
        {
            enemy.SetLifes(enemy.GetLifes() - lifes);
            dead = true;
            upgradeRects.DecrementShieldUpgrade();
            Destroy(this.gameObject);
        }
    }
}