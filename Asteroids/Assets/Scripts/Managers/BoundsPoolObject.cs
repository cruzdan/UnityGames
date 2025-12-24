using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class return a PoolObject to its pool when it are out of the limits*/
public class BoundsPoolObject : MonoBehaviour
{
    private float endHorizontal = 0;
    private float endVertical = 0;
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
    public void Init(float angle, float width, float height)
    {
        float firstX = -SquaresResolution.TotalSquaresX / 2.0f;
        float endX = -firstX;
        float firstY = SquaresResolution.TotalSquaresY / 2.0f;
        float endY = -firstY;


        if (angle > 360)
            angle -= 360;
        if (angle < 90)
        {
            endHorizontal = endX + width / 2;
            endVertical = firstY + height / 2;
        }
        else if (angle < 180)
        {
            endHorizontal = firstX - width / 2;
            endVertical = firstY + height / 2;
        }
        else if (angle < 270)
        {
            endHorizontal = firstX - width / 2;
            endVertical = endY - height / 2;
        }
        else
        {
            endHorizontal = endX + width / 2;
            endVertical = endY - height / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (endHorizontal < 0)
        {
            if (transform.position.x < endHorizontal)
            {
                objectPool.ReturnObjectToPool(this.gameObject);
            }
        }
        else
        {
            if (transform.position.x > endHorizontal)
            {
                objectPool.ReturnObjectToPool(this.gameObject);
            }
        }
        if (endVertical < 0)
        {
            if (transform.position.y < endVertical)
            {
                objectPool.ReturnObjectToPool(this.gameObject);
            }
        }
        else
        {
            if (transform.position.y > endVertical)
            {
                objectPool.ReturnObjectToPool(this.gameObject);
            }
        }
    }
}
