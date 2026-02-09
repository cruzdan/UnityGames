using UnityEngine;

public class GunExplosion : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private string gunFlashObjectName = "Gun Flash";
    #endregion
    #region Static Variables
    public static GunExplosion Instance;
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

    public void SetExplosionOnGunPosition(Transform characterTransform)
    {
        GameObject flashObject = ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.ShootExplosion,
            GetTransformWithName(gunFlashObjectName, characterTransform).position);
        Flash flash = flashObject.GetComponent<Flash>();
        flash.OnFlashEnd += () => ObjectPool.Instance.ReturnObjectToPool(flashObject, ObjectPool.PoolObjectType.ShootExplosion);
        flash.StartFlash();
    }

    Transform GetTransformWithName(string name, Transform characterTransform)
    {
        Transform[] children = characterTransform.GetComponentsInChildren<Transform>();
        int total = children.Length;
        for (int i = 0; i < total; i++)
        {
            if (children[i].name.Contains(name))
            {
                return children[i];
            }
        }
        return null;
    }

    public void CreateBulletExplosionOnPosition(Vector3 explosionPosition)
    {
        GameObject flashObject = ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.ShootExplosion, explosionPosition);
        Flash flash = flashObject.GetComponent<Flash>();
        flash.OnFlashEnd += () => ObjectPool.Instance.ReturnObjectToPool(flashObject, ObjectPool.PoolObjectType.ShootExplosion);
        flash.StartFlash();
    }
    #endregion
}
