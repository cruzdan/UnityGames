using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainObstaclePositions : MonoBehaviour
{
    [SerializeField] private Transform[] _cubeTransforms;
    [SerializeField] private Transform[] _coneTransforms;
    [SerializeField] private Transform[] _triangleTransforms;
    [SerializeField] private Transform[] _sphereTransforms;
    [SerializeField] private Transform _obstacleParent;
    private ObjectPool _objectPool;
    private Vector3 _objectPosition = Vector3.zero;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    public void CreateObstacleMountain(float posZ)
    {
        CreateCubes(posZ);
        CreateCones(posZ);
        CreateTriangles(posZ);
        CreateSpheres(posZ);
    }

    void CreateCubes(float posZ)
    {
        int total = _cubeTransforms.Length;
        for (int i = 0; i < total; i++)
        {
            CalculateObjectPosition(_cubeTransforms[i].position, posZ);
            CreateCube();
        }
    }

    void CreateCones(float posZ)
    {
        int total = _coneTransforms.Length;
        for (int i = 0; i < total; i++)
        {
            CalculateObjectPosition(_coneTransforms[i].position, posZ);
            CreateCone();
        }
    }

    void CreateTriangles(float posZ)
    {
        int total = _triangleTransforms.Length;
        for (int i = 0; i < total; i++)
        {
            CalculateObjectPosition(_triangleTransforms[i].position, posZ);
            CreateTriangle();
        }
    }

    void CreateSpheres(float posZ)
    {
        int total = _sphereTransforms.Length;
        for (int i = 0; i < total; i++)
        {
            CalculateObjectPosition(_sphereTransforms[i].position, posZ);
            CreateSphere();
        }
    }

    void CalculateObjectPosition(Vector3 objectPosition, float posZ)
    {
        _objectPosition = objectPosition;
        _objectPosition.z += posZ;
    }

    void CreateCube()
    {
        _objectPool.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Cube, _objectPosition,
                _obstacleParent);
    }

    void CreateCone()
    {
        _objectPool.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Cone, _objectPosition,
                _obstacleParent);
    }

    void CreateTriangle()
    {
        _objectPool.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Triangle, _objectPosition,
                _obstacleParent);
    }

    void CreateSphere()
    {
        _objectPool.GetObjectFromPoolWithParent(ObjectPool.PoolObjectType.Sphere, _objectPosition,
                _obstacleParent);
    }
}
