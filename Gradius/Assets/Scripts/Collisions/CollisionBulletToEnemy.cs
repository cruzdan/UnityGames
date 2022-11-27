using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBulletToEnemy : MonoBehaviour
{
	[SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int damage;
	[SerializeField] private int shipIndex = 0;
	private bool dead = false;
	[SerializeField] private ObjectPool objectPool;
	public void SetObjectPool(ObjectPool obj)
	{
		objectPool = obj;
	}
	public ObjectPool GetObjectPool()
	{
		return objectPool;
	}
	public bool HasObjectPool()
	{
		return objectPool != null;
	}
	public void SetDamage(int newDamage) { damage = newDamage; }
	public void SetShipIndex(int index) { shipIndex = index; }
	public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
	public void SetDead(bool newDead) { dead = newDead; }
	public int GetDamage() { return damage; }
	public int GetShipIndex() { return shipIndex; }
	public bool HasEnemyManager() { return enemyManager != null; }

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
						string tag = collision.tag;
						switch (tag)
						{
							case "Enemy0":
								collision.gameObject.GetComponent<Enemy0Information>().Dead();
								EnemyDead(enemy, collision, false);
								break;
							case "Enemy9":
								int enemyLifes = enemy.GetLifes();
								if (enemyLifes <= damage)
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
			}
		}
	}

	void EnemyDead(EnemyInfo enemy, Collider2D collision, bool isEnemy9)
    {
		int enemyLife = enemy.GetLifes();
		if(enemyLife <= damage)
        {
			ParticleManager.Instance.PlayParticleSystem(collision.transform.position);
			enemy.SetDead(true);
			if (enemy.GetUpgrade())
			{
				enemyManager.GenerateUpgrade(collision.transform.position.x, collision.transform.position.y);
			}
			enemy.AddScore(shipIndex);
            if (!isEnemy9)
            {
				collision.gameObject.GetComponent<BoundsPoolObject>().GetObjectPool().ReturnObjectToPool(collision.gameObject);
            }
            else
            {
				collision.gameObject.SetActive(false);
			}
        }
        else
        {
			enemy.SetLifes(enemy.GetLifes() - damage);
        }
		if(enemyLife >= damage)
        {
			dead = true;
			Missile missile = GetComponent<Missile>();
			
			if (missile != null)
			{
				gameObject.SetActive(false);
            }
            else
            {
				objectPool.ReturnObjectToPool(this.gameObject);
			}
		}
	}
}
