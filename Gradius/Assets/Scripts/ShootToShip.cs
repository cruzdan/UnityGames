using UnityEngine;

public class ShootToShip : MonoBehaviour
{
    [SerializeField] private Transform ship;
    [SerializeField] private GameObject enemyBulletPrefab;
    private GameObject enemyBullet;
    float firstX;
    float firstY;
    [SerializeField] float timeToShoot = 1.0f;
    float timer = 0f;
    //number of shoots
    public int shoots;
    public void SetTimer(float newTimer) { timer = newTimer; }
    public void SetShip(Transform newShip) { ship = newShip; }
    public void SetTimeToShoot(float time) { timeToShoot = time; }
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToShoot;
        firstX = Squares.totalSquaresX / 2;
        firstY = Squares.totalSquaresY / 2;
        shoots = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ShootEnemyBullet();
            shoots++;
            timer = timeToShoot;
        }
    }

    void ShootEnemyBullet()
    {
        //ship position and transform position are calculated by the 0,0 coordinate on the left up side, x+ to right and y+ to down
        float distanceX = (firstX + ship.position.x) - (firstX + transform.position.x);
        float distanceY = (firstY - transform.position.y) - (firstY - ship.position.y);
        float angle = Mathf.Atan2(distanceY, distanceX);

        enemyBullet = Instantiate(enemyBulletPrefab) as GameObject;
        float speed = Squares.totalSquaresInclined / 3f;
        ForwardMovementRB forward = enemyBullet.GetComponent<ForwardMovementRB>();
        forward.Init();
        forward.SetSpeed(speed * Mathf.Cos(angle), speed * Mathf.Sin(angle));
        enemyBullet.transform.position = new Vector2(transform.position.x + SpriteBounds.GetSpriteWidth(enemyBullet) * Mathf.Cos(angle) * 1.5f,
            transform.position.y + SpriteBounds.GetSpriteHeight(enemyBullet) * Mathf.Sin(angle) * 1.5f);
        enemyBullet.GetComponent<Bounds>().Init(angle * Mathf.Rad2Deg, SpriteBounds.GetSpriteWidth(enemyBullet), SpriteBounds.GetSpriteHeight(enemyBullet));
    }
}
