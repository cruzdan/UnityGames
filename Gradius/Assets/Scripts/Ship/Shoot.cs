using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
	[SerializeField] private EnemyManager enemyManager;
	//pools to: 0 -> forward, 1 -> inclined, 2 -> laser
	[SerializeField] private ObjectPool[] pools;
    private GameObject forwardBullet;
	//auxiliar variables to generate new bullets
	private BoundsPoolObject bound;
	private CollisionBulletToEnemy collision;
	private CollisionBulletToMap collisionMap;
	private Missile mis;

	public void SetPools(ObjectPool[] newPools)
    {
		pools = newPools;
    }
    public void SetEnemyManager(EnemyManager e) { enemyManager = e; }
	//x,y are the center position of the object, w = local scale.x
	public void ShootForwardBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = pools[0].GetObjectFromPool();
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 0.0f);
		SetForwardBounds(0f, 0);
		collision = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		SetCollisionInfo(1, 0, shipIndex);
		SetCollisionMapPool(0);
	}

	public void ShootInclinedBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = pools[1].GetObjectFromPool();
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 45.0f);
		SetForwardBounds(25f, 1);
		collision = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		SetCollisionInfo(1, 1, shipIndex);
		SetCollisionMapPool(1);
	}

	public void ShootLaserBullet(float speed, float x, float y, float w, int shipIndex)
	{
		forwardBullet = pools[2].GetObjectFromPool();
		forwardBullet.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(forwardBullet) / 2.0f, y);
		forwardBullet.GetComponent<ForwardMovement>().Init(speed, 0.0f);
		SetForwardBounds(0f, 2);
		collision = forwardBullet.GetComponent<CollisionBulletToEnemy>();
		SetCollisionInfo(2, 2, shipIndex);
		SetCollisionMapPool(2);
	}

	public void ShootMissile(GameObject missile, float x, float y, float w, int shipIndex)
    {
		missile.SetActive(true);
		missile.transform.position = new Vector2(x + w * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f + SpriteBounds.GetSpriteWidth(missile) / 2.0f, y);
		mis = missile.GetComponent<Missile>();
		mis.SetSpeedX(SquaresResolution.TotalSquaresX / (3.6f));
		mis.SetSpeedY(-SquaresResolution.TotalSquaresY / (2.4f));
		mis.SetState(1);
		collision = missile.GetComponent<CollisionBulletToEnemy>();
		collision.SetDead(false);
	}

	void SetCollisionInfo(int damage, int poolIndex, int shipIndex)
	{
		collision.SetDamage(damage);
		if (!collision.HasEnemyManager())
			collision.SetEnemyManager(enemyManager);
		if (!collision.HasObjectPool())
		{
			collision.SetObjectPool(pools[poolIndex]);
		}
		collision.SetShipIndex(shipIndex);
		collision.SetDead(false);
	}
	void SetCollisionMapPool(int poolIndex)
    {
		collisionMap = forwardBullet.GetComponent<CollisionBulletToMap>();
		if (!collisionMap.HasObjectPool())
			collisionMap.SetObjectPool(pools[poolIndex]);
	}

	void SetForwardBounds(float angle, int poolIndex)
    {
		bound = forwardBullet.GetComponent<BoundsPoolObject>();
		bound.Init(angle, SpriteBounds.GetSpriteWidth(forwardBullet), SpriteBounds.GetSpriteHeight(forwardBullet));
		if (!bound.HasObjectPool())
		{
			bound.SetObjectPool(pools[poolIndex]);
		}
	}
}

