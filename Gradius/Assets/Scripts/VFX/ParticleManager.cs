using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private ObjectPool particlePool;
    GameObject particleObject;

    public void PlayParticleSystem(Vector2 position)
    {
        particleObject = particlePool.GetObjectFromPool();
        particleObject.transform.position = position;
        particleObject.GetComponent<ParticleSystem>().Play();
        particleObject.GetComponent<ParticleExplosion>().Restart();
    }
    public void ReturnParticleObject(GameObject particleToReturn)
    {
        particlePool.ReturnObjectToPool(particleToReturn);
    }
    public void ReturnAllParticles()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Particle");
        for (int i = 0; i < objects.Length; i++)
        {
            particlePool.ReturnObjectToPool(objects[i]);
        }
    }
}
