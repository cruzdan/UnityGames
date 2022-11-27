using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private UpgradeRectsManager upgradeRects;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int shipIndex = 0;
    private int lifes = 10;
    private Animator animator;
    private bool dead = false;
    public void SetUpgradeRect(UpgradeRectsManager upgrade) { upgradeRects = upgrade; }
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
            EnemyInfo enemy;
            switch (collision.gameObject.layer)
            {
                //enemy layer
                case 8:
                    enemy = collision.GetComponent<EnemyInfo>();
                    if (!enemy.GetDead())
                    {
                        switch (collision.tag)
                        {
                            case "Enemy0":
                                collision.gameObject.GetComponent<Enemy0Information>().Dead();
                                EnemyDead(enemy, collision, false);
                                break;
                            case "Enemy9":
                                int enemyLifes = enemy.GetLifes();
                                if (enemyLifes <= lifes)
                                {
                                    collision.gameObject.GetComponent<BossBehaviour>().Dead();
                                }
                                EnemyDead(enemy, collision, true);
                                break;
                            default:
                                EnemyDead(enemy, collision, false);
                                break;
                        }
                    }
                    break;
                //enemy 4, 5down and 5up layer
                case 17:
                case 18:
                case 19:
                    enemy = collision.GetComponent<EnemyInfo>();
                    if (!enemy.GetDead())
                    {
                        EnemyDead(enemy, collision, false);
                    }
                    break;
                //EnemyBullet layer
                case 7:
                    SetLifes(lifes - 1);
                    collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
                    if (lifes <= 0)
                    {
                        dead = true;
                        upgradeRects.DecrementShieldUpgrade();
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
    void EnemyDead(EnemyInfo enemy, Collider2D collision, bool isEnemy9)
    {
        int enemyLifes = enemy.GetLifes();
        if (enemyLifes <= lifes)
        {
            ParticleManager.Instance.PlayParticleSystem(collision.transform.position);
            if (enemy.GetUpgrade())
            {
                enemyManager.GenerateUpgrade(collision.transform.position.x, collision.transform.position.y);
            }
            enemy.SetDead(true);
            enemy.AddScore(shipIndex);
            if (!isEnemy9)
            {
                collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
            SetLifes(lifes - enemyLifes);
            if(lifes <= 0)
            {
                dead = true;
                upgradeRects.DecrementShieldUpgrade();
                gameObject.SetActive(false);
            }
        }
        else
        {
            enemy.SetLifes(enemy.GetLifes() - lifes);
            dead = true;
            upgradeRects.DecrementShieldUpgrade();
            gameObject.SetActive(false);
        }
    }
}