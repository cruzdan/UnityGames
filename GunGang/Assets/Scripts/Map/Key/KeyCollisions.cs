using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollisions : MonoBehaviour
{
    [SerializeField] private Score _score;
    private DeleteMapObject _deleteMapObject;
    private void Awake()
    {
        _deleteMapObject = GetComponent<DeleteMapObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
            case "Character":
                IncrementScore();
                ReturnToPool();
                break;
        }
    }

    void IncrementScore()
    {
        _score.IncrementScore(200);
    }

    void ReturnToPool()
    {
        _deleteMapObject.ReturnObjectToPool();
    }
}
