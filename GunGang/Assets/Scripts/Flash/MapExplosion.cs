using UnityEngine;

public class MapExplosion : MonoBehaviour
{
    #region Static Variables
    public static MapExplosion Instance;
    #endregion
    #region Functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateExplosionOnPosition(Vector3 explosionPosition)
    {
        GameObject flashObject = ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.MapExplosion, explosionPosition);
        Flash flash = flashObject.GetComponent<Flash>();
        flash.OnFlashEnd += () => ObjectPool.Instance.ReturnObjectToPool(flashObject, ObjectPool.PoolObjectType.MapExplosion);
        flash.StartFlash();
    }
    #endregion
}
