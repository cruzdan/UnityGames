using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebalMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angleSpeed;
    private Rigidbody rb;
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetAngleSpeed(float newSpeed)
    {
        angleSpeed = newSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        movement.x = speed * Time.deltaTime;
        movement += transform.position;
        rb.MovePosition(movement);
        transform.Rotate(0, angleSpeed, 0);
    }
}
