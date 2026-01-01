using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
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

    // Start is called before the first frame update
    void Start()
    {
        inputSource = playerInputSource.GetResolvedInput();
        body = GetComponent<Rigidbody2D>();
        body.linearDamping = drag;

        speed = SquaresResolution.TotalSquaresInclined / 3.0f;
        originalSpeed = speed;
        maxSpeed = 5.0f * speed;

        originalAngularSpeed = angularSpeed;
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
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            body.AddForce(transform.up * speed);
            if(body.linearVelocity.magnitude > maxSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    private void Rotate()
    {
        if(rotate != 0)
        {
            transform.Rotate(0, 0, -angularSpeed * rotate * Time.deltaTime);
        }
    }

    public void Restart()
    {
        transform.position = Vector2.zero;
        transform.eulerAngles = Vector3.zero;
        body.linearVelocity = Vector2.zero;
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
