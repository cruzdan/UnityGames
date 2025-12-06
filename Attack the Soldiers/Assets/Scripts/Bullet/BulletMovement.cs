using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : NetworkBehaviour
{
    #region General Variables
    [Header("General")]
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector2 direction;
    [SerializeField] private string poolTag = "Bullet";
    [SerializeField] private Rigidbody2D rb;
    #endregion
    #region Override Variables
    [Header("Override")]
    [SerializeField] private bool countDistance = true;
    #endregion
    #region Private Variables
    private float traveledDistance;
    #endregion
    #region Public Properties
    public string PoolTag { get { return poolTag; } set { poolTag = value; } }
    #endregion
    #region Functions
    public void SetDirection(Vector2 value) { direction = value; }
    public void SetSpeed(float value) { speed = value; }
    public void SetMaxDistance(float value) { maxDistance = value; }

    private void Start()
    {
        if (GameNetwork.Instance.IsOnline && !IsServer) enabled = false;
    }

    void FixedUpdate()
    {
        float distance = speed * Time.fixedDeltaTime;
        rb.MovePosition((Vector2)transform.position + direction * distance);
        if (countDistance)
        {
            traveledDistance += distance;
            if (traveledDistance >= maxDistance)
            {
                GameNetwork.Instance.Despawn(NetworkObject, poolTag);
            }
        }
    }

    public void ReiniciateMovement()
    {
        traveledDistance = 0;
    }
    #endregion
}
