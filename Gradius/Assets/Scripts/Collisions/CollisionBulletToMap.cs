using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBulletToMap : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    public void SetObjectPool(ObjectPool obj)
    {
        objectPool = obj;
    }
    public ObjectPool GetObjectPool()
    {
        return objectPool;
    }
    public bool HasObjectPool()
    {
        return objectPool != null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        switch (tag)
        {
            case "Floor":
            case "Mountain":
                objectPool.ReturnObjectToPool(this.gameObject);
                break;
        }
    }
}
