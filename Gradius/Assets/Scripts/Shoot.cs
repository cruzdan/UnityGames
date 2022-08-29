using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    
    [SerializeField] private GameObject forwardBulletPrefab;
	[SerializeField] private GameObject inclinedBulletPrefab;
	[SerializeField] private GameObject laserBulletPrefab;
	[SerializeField] private GameObject missilePrefab;
	[SerializeField] private EnemyManager enemyManager;
    private GameObject forwardBullet;
	private GameObject missile;

	public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
	//x,y are the center position of the object, w = local scale.x
	public void ShootForwardBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = Instantiate(forwardBulletPrefab) as GameObject;
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 0.0f);
		forwardBullet.GetComponent<Bounds>().Init(0.0f, SpriteBounds.GetSpriteWidth(forwardBullet), SpriteBounds.GetSpriteHeight(forwardBullet));
		CollisionBulletToEnemy c = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		c.SetDamage(1);
		c.SetEnemyManager(enemyManager);
		c.SetShipIndex(shipIndex);
	}

	public void ShootInclinedBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = Instantiate(inclinedBulletPrefab) as GameObject;
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 45.0f);
		forwardBullet.GetComponent<Bounds>().Init(45.0f, SpriteBounds.GetSpriteWidth(forwardBullet), SpriteBounds.GetSpriteHeight(forwardBullet));
		CollisionBulletToEnemy c = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		c.SetDamage(1);
		c.SetEnemyManager(enemyManager);
		c.SetShipIndex(shipIndex);
	}

	public void ShootLaserBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = Instantiate(laserBulletPrefab) as GameObject;
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 0.0f);
		forwardBullet.GetComponent<Bounds>().Init(0.0f, SpriteBounds.GetSpriteWidth(forwardBullet), SpriteBounds.GetSpriteHeight(forwardBullet));
		CollisionBulletToEnemy c = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		c.SetDamage(2);
		c.SetEnemyManager(enemyManager);
		c.SetShipIndex(shipIndex);
	}

	
	public void ShootMissile(Ship ship, int id, float x, float y, float w, int shipIndex)
    {
		missile = Instantiate(missilePrefab) as GameObject;
		missile.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(missile) / 2.0f, y);
		missile.GetComponent<Missile>().SetShip(ship);
		missile.GetComponent<Missile>().SetID(id);
		CollisionBulletToEnemy c = missile.GetComponent<CollisionBulletToEnemy>();
		c.SetDamage(1);
		c.SetEnemyManager(enemyManager);
		c.SetShipIndex(shipIndex);
	}
}
