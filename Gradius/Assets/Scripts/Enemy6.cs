using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : MonoBehaviour
{
    [SerializeField] private float timeToShoot;
    [SerializeField] private bool up;
    [SerializeField] private EnemyGenerator enemyGenerator;
    private float timer = 0;

    public void SetTimeToShoot(float time) { timeToShoot = time; }
    public void SetUp(bool newUp) { up = newUp; }
    public void SetEnemyGenerator(EnemyGenerator e) { enemyGenerator = e; }
    // Start is called before the first frame update
    void Start()
    {
        if (up)
        {
            transform.rotation = new Quaternion(180f, 0f, 0f, 0f);
        }
        timer = timeToShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            enemyGenerator.GenerateEnemy7(up, transform.position.x, transform.position.y, SpriteBounds.GetSpriteHeight(gameObject));
            timer = timeToShoot;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
