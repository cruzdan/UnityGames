using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector2 direction;
    [SerializeField] private string onlinePoolTag = "Bullet";
    [SerializeField] private string offlinePoolTag = "Offline Bullet";
    #endregion
    #region Override Variables
    [Header("Override")]
    [SerializeField] private bool countDistance = true;
    #endregion
    #region Private Variables
    private float traveledDistance;
    private Rigidbody2D rb;
    #endregion
    #region Public Properties
    public string OnlinePoolTag { get { return onlinePoolTag; } set { onlinePoolTag = value; } }
    public string OfflinePoolTag { get { return offlinePoolTag; } set { offlinePoolTag = value; } }
    #endregion
    #region Functions
    public void SetDirection(Vector2 value) { direction = value; }
    public void SetSpeed(float value) { speed = value; }
    public void SetMaxDistance(float value) { maxDistance = value; }

    public void ReiniciateMovement()
    {
        traveledDistance = 0;
    }

    void Start()
    {
        if (!isOffline && !IsOwner) return; 
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isOffline && !IsOwner) return;
        float distance = speed * Time.fixedDeltaTime;
        rb.MovePosition((Vector2)transform.position + direction * distance);
        if (countDistance)
        {
            traveledDistance += distance;
            if (traveledDistance >= maxDistance)
            {
                if (!isOffline)
                    ReturnToPoolServerRpc();
                else
                    ReturnToPoolLocal();
            }
        }
    }

    void ReturnToPoolLocal()
    {
        ObjectPool.Singleton.ReturnObject(gameObject, offlinePoolTag);
    }

    [ServerRpc]
    void ReturnToPoolServerRpc()
    {
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), onlinePoolTag);
    }
    #endregion
}
