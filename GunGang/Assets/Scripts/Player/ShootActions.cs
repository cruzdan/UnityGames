using UnityEngine;

public class ShootActions : MonoBehaviour
{
    [SerializeField] private Shoot shoot;

    private void Awake()
    {
        shoot.OnShoot += SetExplosionOnGun;
    }

    void SetExplosionOnGun(Transform characterTransform)
    {
        GunExplosion.Instance.SetExplosionOnGunPosition(characterTransform);
    }
}
