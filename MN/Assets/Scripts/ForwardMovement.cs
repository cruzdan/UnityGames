using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.TransformDirection(Vector3.forward * speed);
    }
}
