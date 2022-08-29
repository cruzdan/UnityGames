using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject highwayPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject invisibleWallPrefab;
    [SerializeField] private GameObject invisibleFloorPrefab;
    [SerializeField] private GameObject enemyGeneratorPrefab;
    [SerializeField] private GameObject trapPrefab;

    //group objects
    [SerializeField] private Transform wallGroup;
    [SerializeField] private Transform floorGroup;
    [SerializeField] private Transform highwayGroup;
    [SerializeField] private Transform spawnerGroup;
    [SerializeField] private Transform trapGroup;
    //0 -> car, 1-> zombie, 2 -> chicken, 3 -> fireball
    [SerializeField] private Transform[] enemyGroup;

    [SerializeField] private Transform target;

    private GameObject floor;
    private GameObject wall;
    private float invisibleFloorSize;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMapLevel();
    }

    public void GenerateMapLevel()
    {
        float sizeX = floorPrefab.transform.localScale.x;
        float sizeZ = floorPrefab.transform.localScale.z;
        float positionZ = 0f;
        invisibleFloorSize = 4f;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int size = Random.Range(2, 7);
                if (j == 0)
                {
                    for (int k = 0; k < size; k++)
                    {
                        floor = Instantiate(floorPrefab) as GameObject;
                        floor.transform.position = new Vector3(0f, 0f, positionZ);
                        floor.transform.SetParent(floorGroup);
                        positionZ += sizeZ;
                    }
                }
                else
                {
                    int enemyType = Random.Range(0, 5);
                    for (int k = 0; k < size; k++)
                    {
                        floor = Instantiate(highwayPrefab) as GameObject;
                        floor.transform.position = new Vector3(0f, 0f, positionZ);
                        floor.transform.SetParent(highwayGroup);
                        if (k % 2 == 0)
                        {
                            //0 -> left, 1 -> right
                            int direction = Random.Range(0, 2);
                            if (enemyType != 4)
                            {
                                AddEnemyGenerator(positionZ, sizeX, enemyType, direction);
                            }
                            else
                            {
                                AddTrap(floor, positionZ);
                            }
                        }
                        positionZ += sizeZ;
                    }
                }
            }
        }
        positionZ -= sizeZ;

        //first wall
        wall = Instantiate(invisibleWallPrefab) as GameObject;
        wall.transform.position = new Vector3(0f, 2f, -sizeZ / 2f - wall.transform.localScale.z / 2f);
        wall.transform.localScale = new Vector3(sizeX + sizeZ, 5f, 1f);
        wall.transform.SetParent(wallGroup);

        //left wall
        wall = Instantiate(wallPrefab) as GameObject;
        wall.transform.position = new Vector3(-sizeX / 2f - wall.transform.localScale.x / 2f, 2f, positionZ / 2f);
        wall.transform.localScale = new Vector3(sizeZ / 2f, 5f, positionZ + sizeZ + 2 * wall.transform.localScale.z);
        wall.transform.SetParent(wallGroup);
        //left floor
        floor = Instantiate(invisibleFloorPrefab) as GameObject;
        floor.transform.localScale = new Vector3(invisibleFloorSize, 1f, wall.transform.localScale.z);
        floor.transform.position = new Vector3(wall.transform.position.x - floor.transform.localScale.x / 2f + 0.5f, 0,
            wall.transform.position.z);
        floor.transform.SetParent(floorGroup);

        //right wall
        wall = Instantiate(wallPrefab) as GameObject;
        wall.transform.position = new Vector3(sizeX / 2f + wall.transform.localScale.x / 2f, 2f, positionZ / 2f);
        wall.transform.localScale = new Vector3(sizeZ / 2f, 5f, positionZ + sizeZ + 2 * wall.transform.localScale.z);
        wall.transform.SetParent(wallGroup);
        //right floor
        floor = Instantiate(invisibleFloorPrefab) as GameObject;
        floor.transform.localScale = new Vector3(invisibleFloorSize, 1f, wall.transform.localScale.z);
        floor.transform.position = new Vector3(wall.transform.position.x + floor.transform.localScale.x / 2f - 0.5f, 0,
            wall.transform.position.z);
        floor.transform.SetParent(floorGroup);

        //last wall
        wall = Instantiate(invisibleWallPrefab) as GameObject;
        wall.transform.position = new Vector3(0f, 2f, positionZ + sizeZ / 2f + wall.transform.localScale.z / 2f);
        wall.transform.localScale = new Vector3(sizeX + sizeZ, 5f, 1f);
        wall.transform.SetParent(wallGroup);
        wall.tag = "Finish";
        wall.GetComponent<BoxCollider>().isTrigger = true;
    }
    void AddEnemyGenerator(float posZ, float highwaySize, int enemyType, int direction)
    {
        GameObject generator = Instantiate(enemyGeneratorPrefab) as GameObject;
        if (direction == 0)
        {
            generator.transform.position = new Vector3(highwaySize / 2f + invisibleFloorSize / 2f,
                GetPositionYForEnemy(enemyType), posZ);
        }
        else
        {
            generator.transform.position = new Vector3(-highwaySize / 2f - invisibleFloorSize / 2f,
                GetPositionYForEnemy(enemyType), posZ);
        }
        EnemyGenerator enemyGenerator = generator.GetComponent<EnemyGenerator>();
        enemyGenerator.SetTime(GetTimeForEnemy(enemyType));
        enemyGenerator.SetEnemySpeed(GetSpeedForEnemy(enemyType));
        enemyGenerator.SetEnemyType(enemyType);
        enemyGenerator.SetEnemyDirection(direction);
        enemyGenerator.SetBoundX(GetBoundX(direction, highwaySize, enemyGenerator.transform.localScale.x));
        enemyGenerator.SetEnemyGroup(enemyType, enemyGroup[enemyType]);
        enemyGenerator.transform.SetParent(spawnerGroup);
        enemyGenerator.SetTarget(target);
        if(enemyType == 1)
        {
            //more distance to the zombie because he is slow 
            enemyGenerator.SetDistance(50f);
        }
        else
        {
            enemyGenerator.SetDistance(30f);
        }
    }

    void AddTrap(GameObject newFloor, float posZ)
    {
        GameObject trap = Instantiate(trapPrefab) as GameObject;
        trap.transform.position = new Vector3(0, 0, posZ);
        Trap tr = trap.GetComponent<Trap>();
        tr.SetFloor(newFloor);
        tr.SetTimeToActivateTrap(GetTimeForEnemy(4));
        tr.transform.SetParent(trapGroup);
        tr.SetDistance(30f);
        tr.SetTarget(target);
    }

    float GetTimeForEnemy(int enemyType)
    {
        float minTime = 0f;
        float maxTime = 0f;
        float time;
        switch (enemyType)
        {
            //car
            case 0:
                minTime = 2f;
                maxTime = 4f;
                break;
            //zombie    
            case 1:
                minTime = 3.0f;
                maxTime = 5.0f;
                break;
            //chicken
            case 2:
                minTime = 0.3f;
                maxTime = 0.5f;
                break;
            //fireballs
            case 3:
                minTime = 0.7f;
                maxTime = 1.0f;
                break;
            //trap
            case 4:
                minTime = 2f;
                maxTime = 3f;
                break;
        }
        time = Random.Range(minTime, maxTime);
        return time;
    }

    float GetPositionYForEnemy(int enemyType)
    {
        float pos = 0f;
        switch (enemyType)
        {
            //car
            case 0:
                pos = 0.5f;
                break;
            //zombie    
            case 1:
                pos = 0.6f;
                break;
            //chicken
            case 2:
                pos = 0.55f;
                break;
            //fireballs
            case 3:
                pos = 2.7f;
                break;
        }
        return pos;
    }

    float GetSpeedForEnemy(int enemyType)
    {
        float minSpeed = 0f;
        float maxSpeed = 0f;
        float speed;
        switch (enemyType)
        {
            //car
            case 0:
                minSpeed = 5.5f;
                maxSpeed = 8.5f;
                break;
            //zombie    
            case 1:
                minSpeed = 1.3f;
                maxSpeed = 1.7f;
                break;
            //chicken
            case 2:
                minSpeed = 9.0f;
                maxSpeed = 10.0f;
                break;
            //fireballs
            case 3:
                minSpeed = 11.5f;
                maxSpeed = 12.5f;
                break;
        }
        speed = Random.Range(minSpeed, maxSpeed);
        return speed;
    }

    float GetBoundX(int direction, float highwaySize, float sizeX)
    {
        float bound;
        if(direction == 0)
        {
            bound = -highwaySize / 2f - invisibleFloorSize / 2f - sizeX / 2f;
        }
        else
        {
            bound = highwaySize / 2f + invisibleFloorSize / 2f + sizeX / 2f;
        }
        return bound;
    }
}
