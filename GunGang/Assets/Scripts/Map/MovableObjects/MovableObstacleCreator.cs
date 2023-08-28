using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacleCreator : MonoBehaviour
{
    [SerializeField] private int _totalMovableObjects = 7;
    [SerializeField] private Transform _obstacleParent;
    [SerializeField] private float _leftPositionX = -3;
    [SerializeField] private float _rightPositionX = 3;
    private Vector3 _obstaclePosition = Vector3.zero;
    GameObject _movableObject;
    public void GenerateRandomMovableObject(float posZ)
    {
        int movableObjectIndex = GetRandomMovableObjectIndex();
        SetAuxiliarVectorValuesOfMovableObject(movableObjectIndex, posZ);
        _movableObject = ObjectPool.Instance.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Movable1 +
            movableObjectIndex, _obstaclePosition, _obstacleParent);
        InitMovableObjectIfNeeded(_movableObject, movableObjectIndex);
    }

    int GetRandomMovableObjectIndex()
    {
        return Random.Range(0, _totalMovableObjects);
    }

    void SetAuxiliarVectorValuesOfMovableObject(int movableIndex, float posZ)
    {
        switch (movableIndex)
        {
            case 0:
                //Pendulum
                SetObstaclePosition(0, 1, posZ);
                break;
            case 1:
                //Vertical Rotating Line
                SetObstaclePosition(0, 1.5f, posZ);
                break;
            case 2:
                //Rotating Cylinders
                SetObstaclePosition(0, 2.5f, posZ);
                break;
            case 3:
                //Rotating Line
                SetObstaclePosition(GetXPositionFromSide(GetHorizontalSideIndexWithoutRepetition(3)), 1, posZ);
                break;
            case 4:
                //Rotating Square
                SetObstaclePosition(GetXPositionFromSide(GetHorizontalSideIndexWithoutRepetition(3)), 1.5f, posZ);
                break;
            case 5:
                //Car
                SetObstaclePosition(0, 1.37f, posZ);
                break;
            case 6:
                //Movable Ball
                SetObstaclePosition(0, 2, posZ);
                break;
        }
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

    void SetObstaclePosition(float posX, float posY, float posZ)
    {
        _obstaclePosition.x = posX;
        _obstaclePosition.y = posY;
        _obstaclePosition.z = posZ;
    }

    void InitMovableObjectIfNeeded(GameObject movableObject, int movableObjectIndex)
    {
        switch (movableObjectIndex)
        {
            case 2:
                MovableCylinders movableCylinders =
                movableObject.GetComponent<MovableCylinders>();
                movableCylinders.ResetCylinderZPositions();
                movableCylinders.InitCylinderMovements();
                break;
            case 3:
            case 4:
                movableObject.GetComponentInChildren<ConstantRotationPoint>().Init();
                break;
            case 5:
            case 6:
                movableObject.GetComponentInChildren<MoveOnAxisChanger>().InitMovements();
                break;
        }
    }
}
