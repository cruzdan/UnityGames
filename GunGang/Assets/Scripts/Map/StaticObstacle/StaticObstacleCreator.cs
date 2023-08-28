using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacleCreator : MonoBehaviour
{
    [SerializeField] private int _totalObstacles;
    [SerializeField] private float _leftPositionX = -3;
    [SerializeField] private float _rightPositionX = 3;
    [SerializeField] private float[] _positionsY;
    [SerializeField] private Transform _obstacleParent;
    Vector3 _obstaclePosition = Vector3.zero;

    public void GenerateRandomObstacle(float posZ)
    {
        int number = Random.Range(0, 100);
        if (number < 50)
        {
            GenerateDoubleObstacle(posZ);
        }
        else
        {
            GenerateDoubleObstacle(posZ);
        }
    }

    void GenerateDoubleObstacle(float posZ)
    {
        CreateRandomObstacleOnXZPositionAndSide(_leftPositionX, posZ, 0);
        CreateRandomObstacleOnXZPositionAndSide(_rightPositionX, posZ, 2);
    }

    void CreateRandomObstacleOnXZPositionAndSide(float posX, float posZ, int side)
    {
        int obstacleIndex = GetRandomObstacleIndex();
        SetObstaclePosition(posX, _positionsY[obstacleIndex], posZ);
        CreateObstacleOnSide(obstacleIndex, side);
    }

    void SetObstaclePosition(float posX, float posY, float posZ)
    {
        _obstaclePosition.x = posX;
        _obstaclePosition.y = posY;
        _obstaclePosition.z = posZ;
    }

    void CreateObstacleOnSide(int obstacleIndex, int side)
    {
        ObjectPool.Instance.GetObjectFromPoolWithParentAndRotation(ObjectPool.PoolObjectType.StaticObstacle1 + obstacleIndex,
            _obstaclePosition, GetObstacleQuaternionBySide(side), _obstacleParent);
    }

    Quaternion GetObstacleQuaternionBySide(int side)
    {
        Quaternion obstacleQuaternion = Quaternion.identity;
        if (side == 2)
        {
            obstacleQuaternion.eulerAngles = Vector3.up * 180;
        }
        return obstacleQuaternion;
    }

    int GetRandomObstacleIndex()
    {
        return Random.Range(0, _totalObstacles);
    }

    public void CreateRandomObstacleOnSide(byte side, float posZ)
    {
        int obstacleIndex = GetRandomObstacleIndex();
        SetObstaclePosition(GetXPositionFromSide(side), _positionsY[obstacleIndex], posZ);
        CreateObstacleOnSide(obstacleIndex, side);
    }

    void GenerateObstacle(float posZ)
    {
        int obstacleIndex = GetRandomObstacleIndex();
        int side = GetHorizontalSideIndexWithoutRepetition(1);
        SetObstaclePosition(GetXPositionFromSide(side), _positionsY[obstacleIndex], posZ);
        CreateObstacleOnSide(obstacleIndex, side);
    }

    float GetXPositionFromSide(int side)
    {
        return side switch
        {
            0 => _leftPositionX,
            1 => 0,
            2 => _rightPositionX,
            _ => 0,
        };
    }

    int GetHorizontalSideIndexWithoutRepetition(int side)
    {
        int newSide;
        do
        {
            newSide = Random.Range(0, 3);
        } while (newSide == side);
        return newSide;
    }
}
