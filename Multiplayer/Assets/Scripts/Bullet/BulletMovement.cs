using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector2 direction;
    private float traveledDistance;
    private Rigidbody2D rb;
    public void SetDirection(Vector2 value) { direction = value; }
    public void SetSpeed(float value) { speed = value; }
    public void SetMaxDistance(float value) { maxDistance = value; }
    public void ReiniciateMovement()
    {
        traveledDistance = 0;
    }

    void Start()
    {
        if (!IsOwner) return; 
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (!IsOwner) return;
        float distance = speed * Time.fixedDeltaTime;
        traveledDistance += distance;
        rb.MovePosition((Vector2)transform.position + direction * distance);
        
        if (traveledDistance >= maxDistance)
        {
            NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), "Bullet");
        }
    }
}
