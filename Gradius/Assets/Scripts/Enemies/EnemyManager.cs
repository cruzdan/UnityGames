using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool upgradePool;
    private GameObject upgrade;
    //remaining enemies to get an upgrade
    int[] lineEnemies0;
    int actualLine = 0;

    //auxiliar variable to create upgrades
    BoundsPoolObject bound;

    // Start is called before the first frame update
    void Start()
    {
        lineEnemies0 = new int[5];
    }
    public void SetActualLine(int value) { actualLine = value; }
    public void SetTotalLineEnemies(int total) { lineEnemies0[actualLine] = total; }
    public int GetActualLine() { return actualLine; }
    
    public void UpdateActualLine()
    {
        if (actualLine < 4)
            actualLine++;
        else
            actualLine = 0;
    }
    //When the 4 line enemies are dead, generate an upgrade on posX,posY
    public void UpdateLineEnemies0(int line, float posX, float posY)
    {
        lineEnemies0[line]--;
        if (lineEnemies0[line] <= 0)
        {
            GenerateUpgrade(posX, posY);
        }
    }

    public void GenerateUpgrade(float posX, float posY)
    {
        upgrade = upgradePool.GetObjectFromPool();
        upgrade.transform.position = new Vector2(posX, posY);
        upgrade.GetComponent<UpgradeDeadInfo>().dead = false;
        upgrade.GetComponent<ForwardMovement>().Init(-SquaresResolution.TotalSquaresX / 7f, 0f);
        bound = upgrade.GetComponent<BoundsPoolObject>();
        bound.Init(180f, SpriteBounds.GetSpriteWidth(upgrade), SpriteBounds.GetSpriteHeight(upgrade));
        if (!bound.HasObjectPool())
        {
            bound.SetObjectPool(upgradePool);
        }
    }

    public void ChangeShipTargetToEnemies(GameObject ship)
    {
        GameObject[] objects;

        objects = GameObject.FindGameObjectsWithTag("Enemy3");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
            objects[i].GetComponent<AimToShip>().SetShip(ship.transform);
        }

        objects = GameObject.FindGameObjectsWithTag("Enemy5");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Enemy5FloorMountainBehaviour>().SetShip(ship);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }

        objects = GameObject.FindGameObjectsWithTag("Enemy5up");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Enemy5FloorBehaviour>().SetShip(ship);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }

        objects = GameObject.FindGameObjectsWithTag("Enemy7");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<EnemyFromMachineBehaviour>().SetShip(ship.transform);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }
    }
}
