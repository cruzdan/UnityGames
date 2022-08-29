using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePrefab;
    private GameObject upgrade;
    //remaining enemies to get an upgrade
    int[] lineEnemies0;
    int actualLine = 0;
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
        upgrade = Instantiate(upgradePrefab) as GameObject;
        upgrade.transform.position = new Vector2(posX, posY);
        SpriteBounds.SetScaleSquare(upgrade, Squares.totalSquaresX / 18f, Squares.totalSquaresY * 0.89f / 12f);
        upgrade.GetComponent<ForwardMovement>().Init(-Squares.totalSquaresX / 7f, 0f);
        upgrade.GetComponent<Bounds>().Init(180f, SpriteBounds.GetSpriteWidth(upgrade), SpriteBounds.GetSpriteHeight(upgrade));
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
            objects[i].GetComponent<Enemy5>().SetShip(ship);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }

        objects = GameObject.FindGameObjectsWithTag("Enemy5up");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Enemy5Up>().SetShip(ship);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }

        objects = GameObject.FindGameObjectsWithTag("Enemy7");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Enemy7>().SetShip(ship.transform);
            objects[i].GetComponent<ShootToShip>().SetShip(ship.transform);
        }
    }

    public void DeleteAllEnemies()
    {
        DeleteEnemiesWithTag("Enemy0");
        DeleteEnemiesWithTag("Enemy1");
        DeleteEnemiesWithTag("Enemy2");
        DeleteEnemiesWithTag("Enemy3");
        DeleteEnemiesWithTag("Enemy4");
        DeleteEnemiesWithTag("Enemy5");
        DeleteEnemiesWithTag("Enemy5up");
        DeleteEnemiesWithTag("Enemy6");
        DeleteEnemiesWithTag("Enemy7");
        DeleteEnemiesWithTag("Enemy8");
        DeleteEnemiesWithTag("Enemy9");
    }
    void DeleteEnemiesWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Enemy>().SetDead(true);
            Destroy(objects[i]);
        }
    }
}
