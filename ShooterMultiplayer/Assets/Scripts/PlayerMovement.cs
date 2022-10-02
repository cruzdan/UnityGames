using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float velX;
    [SerializeField] private float velZ;
    [SerializeField] private float maxVelocity;
    private CharacterController _charController;

    //jump
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -8.0f;
    public float minFall = -1.5f;
    private float vertSpeed;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        vertSpeed = minFall;
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        velX = Input.GetAxis("Horizontal") * maxVelocity;
        velZ = Input.GetAxis("Vertical") * maxVelocity;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(velX, 0f, velZ);
        Vector3.ClampMagnitude(movement, maxVelocity);



        // check when the character is on the ground
        bool hitGround = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
            Debug.Log("hit ground: " + hitGround);
        }


        //jump
        if (Input.GetKey(KeyCode.Space))
        {
            if (hitGround)
            {
                vertSpeed = jumpSpeed;
                hitGround = false;
            }
        }
        if (hitGround)
        {
            vertSpeed = minFall;
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }


        movement.y = vertSpeed;
        movement = transform.TransformDirection(movement);
        movement *= Time.fixedDeltaTime;
        characterController.Move(movement);
    }
}
