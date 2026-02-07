using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private ShootFlash shootFlash;
    #region Input
    [Header("Input")]
    private IPlayerInputSource inputSource;
    [SerializeField] private PlayerInputSource playerInputSource;
    #endregion
    Rigidbody2D body;
    float originalSpeed;
    float originalAngularSpeed;
    public float speed;
    public float maxSpeed;
    //stop the ship with time
    public float drag = 1.0f;
    public float angularSpeed = 3.0f;
    bool move;
    float rotate;
    #region Rotation
    [SerializeField] private float smooth = 8f;
    private float targetRotation;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        inputSource = playerInputSource.GetResolvedInput();
        body = GetComponent<Rigidbody2D>();
        body.linearDamping = drag;
        originalSpeed = speed;
        originalAngularSpeed = angularSpeed;
        body.angularDamping = 5f;
    }

    public void InitSpeed()
    {
        speed = originalSpeed;
    }

    public void InitRotation()
    {
        angularSpeed = originalAngularSpeed;
    }

    void Update()
    {
        if (pauseManager.pause)
        {
            move = false;
        }
        else
        {
            if (inputSource.GetVerticalMovement() > 0)
            {
                move = true;
            }
            else
            {
                move = false;
            }
            rotate = inputSource.GetHorizontalMovement();
            targetRotation -= rotate * angularSpeed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        RotateIfPossible();
        if (move)
        {
            body.AddForce(transform.up * speed);
            if(body.linearVelocity.magnitude > maxSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    private void RotateIfPossible()
    {
        if (!pauseManager.pause)
        {
            Quaternion target = Quaternion.Euler(0f, 0f, targetRotation);
            body.MoveRotation(
                Quaternion.Lerp(
                    Quaternion.Euler(0f, 0f, body.rotation),
                    target,
                    smooth * Time.fixedDeltaTime
                )
            );
        }
    }

    public void Restart()
    {
        transform.position = Vector2.zero;
        transform.eulerAngles = Vector3.zero;
        body.linearVelocity = Vector2.zero;
        targetRotation = 0;
        rotate = 0;
        shootFlash.ResetFlash();
    }

    public void SetSpeedByPercentage(float percentage)
    {
        float mul = percentage / 100.0f;
        speed = originalSpeed * mul;
    }

    public void SetAngularSpeedByPercentage(float percentage)
    {
        float mul = percentage / 100.0f;
        angularSpeed = originalAngularSpeed * mul;
    }
}
