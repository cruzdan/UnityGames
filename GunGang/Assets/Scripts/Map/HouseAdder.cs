using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAdder : MonoBehaviour
{
    [SerializeField] private float _rightPositionX;
    [SerializeField] private float _leftPositionX;
    [SerializeField] private float _houseSizeZ;
    [SerializeField] private float _firstHousePositionZ;
    [SerializeField] private int _totalHousePrefabs;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private Transform _houseParent;
    private Vector3 _housePosition;
    private Quaternion _houseQuaternion = Quaternion.identity;

    private void Start()
    {
        CreateMapHouses();
    }

    public void CreateMapHouses()
    {
        _housePosition.z = _firstHousePositionZ;
        int totalHousesToCreate = GetTotalHousesToCreate();
        CreateWallHouses(totalHousesToCreate);
    }

    int GetTotalHousesToCreate()
    {
        return ((int)(GetMapZScale() / _houseSizeZ) + 1);
    }

    float GetMapZScale()
    {
        return _mapGenerator.GetMapZScale();
    }

    void CreateWallHouses(int totalHousesToCreate)
    {
        CreateRightHouses(totalHousesToCreate);
        CreateLeftHouses(totalHousesToCreate);
    }

    void CreateRightHouses(int totalHousesToCreate)
    {
        AssignRightHouseValues();
        CreateHouses(totalHousesToCreate);
    }

    void AssignRightHouseValues()
    {
        _houseQuaternion[1] = 0;
        _houseQuaternion[3] = 1;
        _housePosition.x = _rightPositionX;
    }

    void CreateHouses(int totalHousesToCreate)
    {
        for (int i = 0; i < totalHousesToCreate; i++)
        {
            CalculateHousePositionZ(i);
            CreateRandomHouse();
        }
    }

    void CalculateHousePositionZ(int houseIndex)
    {
        _housePosition.z = _houseSizeZ * houseIndex + _firstHousePositionZ;
    }

    void CreateRandomHouse()
    {
        ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.House1 + Random.Range(0, _totalHousePrefabs),
            _housePosition, _houseQuaternion);
    }

    void CreateLeftHouses(int totalHousesToCreate)
    {
        AssignLeftHouseValues();
        CreateHouses(totalHousesToCreate);
    }

    void AssignLeftHouseValues()
    {
        _houseQuaternion[1] = -1;
        _houseQuaternion[3] = 0;
        _housePosition.x = _leftPositionX;
    }

    public void ReturnAllActiveHousesFromHouseParent()
    {
        int total = _houseParent.childCount;
        Transform house;
        for(int i = total -1; i >= 0; i--)
        {
            house = _houseParent.GetChild(i);
            if (house.gameObject.activeSelf)
            {
                _houseParent.GetChild(i).GetComponent<DeleteMapObject>().ReturnObjectToPool();
            }
        }
    }
}