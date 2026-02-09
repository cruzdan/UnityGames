using UnityEngine;

public class TargetAutomaticShootActions : MonoBehaviour
{
    [SerializeField] private TargetAutomaticShoot targetAutomaticShoot;

    void Awake()
    {
        targetAutomaticShoot.OnShoot += SetExplosionOnGun;
    }

    void SetExplosionOnGun(Transform characterTransform)
    {
        GunExplosion.Instance.SetExplosionOnGunPosition(characterTransform);
    }
}
