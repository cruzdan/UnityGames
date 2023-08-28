using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainObstacleCreator : MonoBehaviour
{
    [SerializeField] private MountainObstaclePositions[] _obstacleMountainPositions;
    private int _totalMountains;

    private void OnEnable()
    {
        _totalMountains = _obstacleMountainPositions.Length;
    }

    public void CreateRandomMountain(float posZ)
    {
        int mountainIndex = Random.Range(0, _totalMountains);
        _obstacleMountainPositions[mountainIndex].CreateObstacleMountain(posZ);
    }
}
