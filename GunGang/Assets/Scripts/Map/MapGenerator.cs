using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int _level = 1;

    [SerializeField] private Transform _floor;
    [SerializeField] private Transform[] _walls;
    [SerializeField] private Transform _endBuilding;
    [SerializeField] private Transform _mapObjectsParentTransform;
    [SerializeField] private float _endDistance;
    [SerializeField] private float _distanceBetweenLastObjectAndEndBuilding;
    [SerializeField] private float _distanceToAddObjects;
    [SerializeField] private int _maxSpawns;

    #region Map Object Creators
    [SerializeField] private MovableObstacleCreator _movableObstacleCreator;
    [SerializeField] private MountainObstacleCreator _obstacleMountainCreator;
    [SerializeField] private StaticObstacleCreator _staticObstacleCreator;
    [SerializeField] private CylinderWithCharacterCreator _cylinderWithCharacterCreator;
    #endregion

    

    List<int> _mapObjectSpawns = new();
    Vector3 _auxiliarVector = new();
    GameObject _auxiliarObject;
    int _minimumCylinderLines;
    int _totalSpawns;
    byte _lineIndex;

    

    public enum MapObject
    {
        StaticObstacle = 0,
        AloneCylinder = 1,
        Key = 2,
        Blocks = 3,
        Line = 4,
        Movable = 5
    }

    private void Start()
    {
        InitLevelFromPlayerPrefs();
        GenerateMap();
    }

    void InitLevelFromPlayerPrefs()
    {
        _level = PlayerPrefs.GetInt("Level", 1);
    }

    public void IncrementLevel()
    {
        _level++;
        SaveLevel();
    }

    void SaveLevel()
    {
        PlayerPrefs.SetInt("Level", _level);
        PlayerPrefs.Save();
    }

    public void GenerateMap()
    {
        CalculateTotalSpawnsOnMapBetween0AndMaxSpawns();
        CalculateMinimumCylinderLines();
        ResetSpawns();
        SelectLineIndexesWithoutCountinousRepetition();
        CompleteSpawnIndexes();
        _cylinderWithCharacterCreator.SetLevel(_level);
        AddMapObjectsAndMapTriggers(3);
        AdjustFloor();
        AdjustWalls();
        SetFinalBuildPositionOnFloor();
    }

    void CalculateTotalSpawnsOnMapBetween0AndMaxSpawns()
    {
        int levelPhase = (_level - 1) / 5;
        _totalSpawns = Mathf.Clamp(Random.Range(0, 4) + 5 + levelPhase, 0, _maxSpawns);
    }

    void CalculateMinimumCylinderLines()
    {
        _minimumCylinderLines = _totalSpawns / 3;
    }

    void ResetSpawns()
    {
        _mapObjectSpawns.Clear();
        for (int i = 0; i < _totalSpawns; i++)
        {
            _mapObjectSpawns.Add(6);
        }
    }

    void SelectLineIndexesWithoutCountinousRepetition()
    {
        for (int i = 0; i < _minimumCylinderLines; i++)
        {
            do
            {
                SelectLineIndexWithoutRepetition();
            } while (MapObjectIndexIsBetweenTheSameObject(_lineIndex, MapObject.Line));
            _mapObjectSpawns[_lineIndex] = (byte)MapObject.Line;
        }
    }

    void SelectLineIndexWithoutRepetition()
    {
        _lineIndex = (byte)Random.Range(0,_totalSpawns);
        if (_mapObjectSpawns[_lineIndex] != (byte)MapObject.Line)
        {
            return;
        }
        SelectLineIndexWithoutRepetition();
    }

    void CompleteSpawnIndexes()
    {
        MapObject auxiliarMapObject;
        for (int i = 0; i < _totalSpawns; i++)
        {
            if (_mapObjectSpawns[i] == 6)
            {
                do
                {
                    auxiliarMapObject = GetRandomMapObject();
                } while (MapObjectIndexIsBetweenTheSameObject(i, auxiliarMapObject));
                _mapObjectSpawns[i] = (byte)auxiliarMapObject;
            }
        }
    }

    MapObject GetRandomMapObject()
    {
        byte number = (byte)Random.Range(0, 100);
        if (number < 25)
        {
            return MapObject.StaticObstacle;
        }
        else if (number < 50)
        {
            return MapObject.AloneCylinder;
        }
        else if (number < 51)
        {
            return MapObject.Key;
        }
        else if (number < 70)
        {
            return MapObject.Blocks;
        }
        else if (number < 90)
        {
            return MapObject.Line;
        }
        return MapObject.Movable;
    }

    bool MapObjectIndexIsBetweenTheSameObject(int index, MapObject mapObjectIndex)
    {
        if (IndexIsFirstSpawn(index))
        {
            if (NextObjectIsMapObjectIndex(index, mapObjectIndex))
            {
                return false;
            }
        }
        else if (IndexIsLastSpawn(index))
        {
            if (PreviousObjectIsMapObjectIndex(index, mapObjectIndex))
            {
                return false;
            }
        }
        else if (PreviousObjectIsMapObjectIndex(index, mapObjectIndex) && NextObjectIsMapObjectIndex(index, mapObjectIndex))
        {
            return false;
        }
        return true;
    }
   
    bool IndexIsFirstSpawn(int index)
    {
        return index == 0;
    }

    bool NextObjectIsMapObjectIndex(int index, MapObject mapObjectIndex)
    {
        return _mapObjectSpawns[index + 1] != (byte)mapObjectIndex;
    }

    bool IndexIsLastSpawn(int index)
    {
        return index == _totalSpawns - 1;
    }

    bool PreviousObjectIsMapObjectIndex(int index, MapObject mapObjectIndex)
    {
        return _mapObjectSpawns[index - 1] != (byte)mapObjectIndex;
    }

    public void AddMapObjectsAndMapTriggers(int mapObjectsAmount)
    {
        CreateFirstMapObjects(mapObjectsAmount);
        AddMapTriggersFromStartIndex(mapObjectsAmount);
    }

    void CreateFirstMapObjects(int total)
    {
        for(int i = 0; i < total; i++)
        {
            CreateMapObject(_mapObjectSpawns[i], i + 1);
        }
    }

    public void CreateMapObject(int mapIndex, int objectIndex)
    {
        switch (mapIndex)
        {
            case 0:
                GenerateRandomStaticObstacle(objectIndex);
                break;
            case 1:
                GenerateAloneCylinderAndCharacter(GetHorizontalSideIndexWithoutRepetition(3), 
                    objectIndex);
                break;
            case 2:
                GenerateKey(objectIndex);
                break;
            case 3:
                GenerateRandomBlockMountain(objectIndex);
                break;
            case 4:
                GenerateCylinderLineAndCharacters(objectIndex);
                break;
            case 5:
                GenerateRandomMovableObject(objectIndex);
                break;
        }
    }

    void AddMapTriggersFromStartIndex(int startIndex)
    {
        for (int i = startIndex; i < _totalSpawns; i++)
        {
            CreateMapTrigger(i + 1, _mapObjectSpawns[i]);
        }
    }

    void CreateMapTrigger(int objectIndex, int spawnNumber)
    {
        SetAuxiliarVectorValues(0, 1, _distanceToAddObjects * objectIndex);
        ObjectPool.Instance.GetObjectFromPoolWithParent(
            ObjectPool.PoolObjectType.MapObjectTrigger, _auxiliarVector, _mapObjectsParentTransform).
            GetComponent<SpawnMapObjectInfo>().SetSpawnIndexAndObjectIndex(spawnNumber, objectIndex);
    }

    byte GetHorizontalSideIndexWithoutRepetition(byte side)
    {
        byte newSide;
        do
        {
            newSide = (byte)Random.Range(0, 3);
        } while (newSide == side);
        return newSide;
    }

    void GenerateRandomStaticObstacle(int objectIndex)
    {
        int number = Random.Range(0, 100);
        if (number < 30)
        {
            GenerateStaticObstacleAndCylinder(objectIndex);
            return;
        }
        _staticObstacleCreator.GenerateRandomObstacle(objectIndex * _distanceToAddObjects);
    }

    void GenerateStaticObstacleAndCylinder(int objectIndex)
    {
        byte side = GetHorizontalSideIndexWithoutRepetition(1);
        _staticObstacleCreator.CreateRandomObstacleOnSide(side, objectIndex * _distanceToAddObjects);
        GenerateAloneCylinderAndCharacter(GetHorizontalSideIndexWithoutRepetition(side), objectIndex);
    }

    void SetAuxiliarVectorValues(float posX, float posY, float posZ)
    {
        _auxiliarVector.x = posX;
        _auxiliarVector.y = posY;
        _auxiliarVector.z = posZ;
    }

    void GenerateAloneCylinderAndCharacter(int side, int objectIndex)
    {
        _cylinderWithCharacterCreator.GenerateAloneCylinderAndCharacter(side, objectIndex * _distanceToAddObjects);
    }

    void GenerateKey(int objectIndex)
    {
        SetAuxiliarVectorValues(0, 2, _distanceToAddObjects * objectIndex);
        ObjectPool.Instance.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Key, _auxiliarVector, _mapObjectsParentTransform);
    }

    void GenerateRandomBlockMountain(int objectIndex)
    {
        _obstacleMountainCreator.CreateRandomMountain(_distanceToAddObjects * objectIndex);
    }

    void GenerateCylinderLineAndCharacters(int objectIndex)
    {
        _cylinderWithCharacterCreator.GenerateCylinderLineAndCharacters(objectIndex * _distanceToAddObjects);
    }

    void GenerateRandomMovableObject(int objectIndex)
    {
        _movableObstacleCreator.GenerateRandomMovableObject(objectIndex * _distanceToAddObjects);
    }

    void AdjustFloor()
    {
        AssignMapFloorScale();
        AssignMapFloorPosition();
    }

    void AssignMapFloorScale()
    {
        SetAuxiliarVectorValues(10, 1, _totalSpawns * _distanceToAddObjects + _distanceBetweenLastObjectAndEndBuilding + _endDistance);
        _floor.localScale = _auxiliarVector;
    }

    void AssignMapFloorPosition()
    {
        SetAuxiliarVectorValues(0, 0, _auxiliarVector.z / 2f);
        _floor.position = _auxiliarVector;
    }

    void AdjustWalls()
    {
        AssignMapWallScale();
        AssignMapWallPosition();
    }

    void AssignMapWallScale()
    {
        SetAuxiliarVectorValues(1, 3.5f, _totalSpawns * _distanceToAddObjects + _distanceBetweenLastObjectAndEndBuilding + _endDistance);
        _walls[0].localScale = _auxiliarVector;
        _walls[1].localScale = _auxiliarVector;
    }

    public float GetMapZScale()
    {
        return _totalSpawns* _distanceToAddObjects +_distanceBetweenLastObjectAndEndBuilding + _endDistance;
    }

    void AssignMapWallPosition()
    {
        SetAuxiliarVectorValues(-_floor.localScale.x / 2f - .5f, _walls[0].localScale.y / 2f - .5f, _walls[0].localScale.z / 2f);
        _walls[0].position = _auxiliarVector;
        _auxiliarVector.x *= -1;
        _walls[1].position = _auxiliarVector;
    }

    void SetFinalBuildPositionOnFloor()
    {
        float buildingOffset = 2.5f;
        SetAuxiliarVectorValues(0, 0.55f, _totalSpawns * _distanceToAddObjects + buildingOffset + _distanceBetweenLastObjectAndEndBuilding);
        _endBuilding.position = _auxiliarVector;
    }

    public void ReturnMapObjectsToPool()
    {
        int total = _mapObjectsParentTransform.childCount;
        for(int i = total - 1; i >= 0; i--)
        {
            _auxiliarObject = _mapObjectsParentTransform.GetChild(i).gameObject;
            ObjectPool.Instance.ReturnObjectToPoolInPoolParent(_auxiliarObject,
                _auxiliarObject.GetComponent<DeleteMapObject>().GetPoolObjectType());
        }
    }
}