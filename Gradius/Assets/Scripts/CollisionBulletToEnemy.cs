using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBulletToEnemy : MonoBehaviour
{
	[SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int damage;
	[SerializeField] private int shipIndex = 0;
	private bool dead = false;

    public void SetDamage(int newDamage) { damage = newDamage; }
	public void SetShipIndex(int index) { shipIndex = index; }
	public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
	public void SetDead(bool newDead) { dead = newDead; }
	public int GetDamage() { return damage; }
	public int GetShipIndex() { return shipIndex; }

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
						string tag = collision.tag;
						switch (tag)
						{
							case "Enemy0":
								collision.gameObject.GetComponent<Enemy0>().Dead();
								EnemyDead(enemy, collision);
								break;
							case "Enemy9":
								int enemyLifes = enemy.GetLifes();
								if(enemyLifes <= damage)
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
			}
		}
	}

	void EnemyDead(Enemy enemy, Collider2D collision)
    {
		int enemyLife = enemy.GetLifes();
		if(enemyLife <= damage)
        {
			enemy.SetDead(true);
			if (enemy.GetUpgrade())
			{
				enemyManager.GenerateUpgrade(collision.transform.position.x, collision.transform.position.y);
			}
			enemy.AddScore(shipIndex);
			Destroy(collision.gameObject);
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
				missile.ActiveShipMissile();
			}
			Destroy(this.gameObject);
		}
	}
}
