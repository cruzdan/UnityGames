using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderWithCharacterCreator : MonoBehaviour
{
    [SerializeField] private float _leftPositionX = -3;
    [SerializeField] private float _rightPositionX = 3;
    [SerializeField] private Transform _cylinderParent;
    [SerializeField] private CylinderManager _cylinderManager;
    private int _level;
    CylinderObstacle _auxiliarCylinderObs;
    GameObject _auxiliarObject;
    private Vector3 _obstacleVector = Vector3.zero;


    public void SetLevel(int level)
    {
        _level = level;
    }

    public void GenerateAloneCylinderAndCharacter(int side, float posZ)
    {
        SetObstacleVectorValues(GetXPositionFromSide(side), 1.25f, posZ);
        CreateAndSaveCylinderInAuxiliarCylinder();
        InitSavedCylinderLifes();
        SetObstacleVectorValues(GetXPositionFromSide(side), 3, posZ);
        CreateAndSaveCharacterInAuxiliarObject();
        InitAuxiliarCylinderAndCharacter();
    }

    void SetObstacleVectorValues(float posX, float posY, float posZ)
    {
        _obstacleVector.x = posX;
        _obstacleVector.y = posY;
        _obstacleVector.z = posZ;
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

    void CreateAndSaveCylinderInAuxiliarCylinder()
    {
        _auxiliarCylinderObs = ObjectPool.Instance.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Cylinder, _obstacleVector,
            _cylinderParent).GetComponent<CylinderObstacle>();
    }

    void CreateAndSaveCharacterInAuxiliarObject()
    {
        _auxiliarObject = ObjectPool.Instance.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Character, _obstacleVector,
            _cylinderParent);
    }

    void InitAuxiliarCylinderAndCharacter()
    {
        _auxiliarObject.GetComponent<CharacterBehaviour>().RestartVars();
        _auxiliarCylinderObs.SetCharacter(_auxiliarObject.GetComponent<CharacterBehaviour>());
        SetObstacleVectorValues(0, 180, 0);
        _auxiliarObject.transform.eulerAngles = _obstacleVector;
    }

    public void GenerateCylinderLineAndCharacters(float posZ)
    {
        int total = 5;
        for (int i = 0; i < total; i++)
        {
            SetObstacleVectorValues(-4 + (i * 2), 1.25f, posZ);
            CreateAndSaveCylinderInAuxiliarCylinder();
            InitSavedCylinderLifes();
            GenerateCharacterToCylinderOnLine();
        }
    }

    void InitSavedCylinderLifes()
    {
        _auxiliarCylinderObs.SetLife(_cylinderManager.GetLifesInLevelWithDispersion(_level));
    }

    void GenerateCharacterToCylinderOnLine()
    {
        if (Random.Range(0, 10) < 5)
        {
            _obstacleVector.y = 3;
            CreateAndSaveCharacterInAuxiliarObject();
            InitAuxiliarCylinderAndCharacter();
        }
        else
        {
            _auxiliarCylinderObs.RestartVars();
        }
    }
}
