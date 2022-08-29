using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private GameObject cam;
	public float speed;
	public float walkSpeed = 6.0f;
	public float crouchSpeed = 3.0f;
	public float runSpeed = 12.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -8.0f;
	public float minFall = -1.5f;
	public float rotSpeed = 15.0f;

	private bool crouch = false;
	private bool running = false;

	private CharacterController _charController;
	private CapsuleCollider capsule;
	private Animator anim;
	private float vertSpeed;

	void Start()
	{
		anim = GetComponent<Animator>();
		speed = walkSpeed;
		vertSpeed = minFall;
		_charController = GetComponent<CharacterController>();
		capsule = GetComponent<CapsuleCollider>();
	}

	void Update()
	{
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed;
		float posX = transform.position.x;
		float posZ = transform.position.z;
		float animationSpeed;
		
        if (crouch)
        {
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				Crouch(false);
				Run(true);
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				Crouch(false);
			}
        }
		else if (!running)
        {
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				Run(true);
            }
            else if(Input.GetKeyDown(KeyCode.LeftControl))
            {
				Crouch(true);
			}
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
        {
			Run(false);
		}
		
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed);

		// check when the character is on the ground
		bool hitGround = false;
		RaycastHit hit;
		if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
		{
			float check = (_charController.height + _charController.radius) / 1.9f;
			hitGround = hit.distance <= check;
		}

		//jump
		if (hitGround)
        {
			if (Input.GetButtonDown("Jump"))
            {
				vertSpeed = jumpSpeed;
			}
            else
            {
				vertSpeed = minFall;
			}
        }
        else
        {
			vertSpeed += gravity * 5 * Time.deltaTime;
			if(vertSpeed < terminalVelocity)
            {
				vertSpeed = terminalVelocity;
            }
        }
		movement.y = vertSpeed;
		movement *= Time.deltaTime;
		_charController.Move(movement);

		//rotation
		movement.y = 0;
		if(deltaX != 0 || deltaZ != 0)
        {
			Quaternion direction = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
			animationSpeed = speed;
		}
        else
        {
			animationSpeed = 0f;
        }
		anim.SetFloat("Vel", animationSpeed);

		//cam movement
		float angle = cam.transform.localEulerAngles.x;
		cam.transform.localEulerAngles = Vector3.zero;
		cam.transform.Translate(new Vector3(transform.position.x - posX, 0, transform.position.z - posZ));
		cam.transform.localEulerAngles = new Vector3(angle, 0f, 0f);
	}

	void Crouch(bool value)
    {
		crouch = value;
		if (value)
		{
			speed = crouchSpeed;
			
			capsule.center = new Vector3(capsule.center.x, 0.67f, capsule.center.z);
			capsule.height = 1.43f;
			_charController.center = new Vector3(capsule.center.x, 0.67f, capsule.center.z);
			_charController.height = 1.43f;
		}
		else
		{
			speed = walkSpeed;

			capsule.center = new Vector3(capsule.center.x, 0.92f, capsule.center.z);
			capsule.height = 1.83f;
			_charController.center = new Vector3(capsule.center.x, 0.67f, capsule.center.z);
			_charController.height = 1.43f;
		}
		anim.SetBool("Crouch", value);
	}

	void Run(bool value)
    {
		running = value;
		if (value)
		{
			speed = runSpeed;
		}
		else
		{
			speed = walkSpeed;
		}
		anim.SetBool("Running", value);
	}

	public void Restart()
    {
		transform.position = new Vector3(0, 0.58f, 0f);
		speed = walkSpeed;
		vertSpeed = minFall;
		Crouch(false);
		running = false;
		anim.SetFloat("Vel", 0f);
		anim.SetBool("Crouch", false);
		anim.SetBool("Running", false);
		anim.Play("Idle");
	}
}
