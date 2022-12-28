using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform forwardPoint;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Sprite firstSprite;
    [SerializeField] private MapManager mapManager;
    private bool pause;
    [SerializeField] private float speed;
    //0->up, 1->right, 2->down, 3-> left
    private byte direction = 1;
    private byte nextDirection = 1;

    PlayerInput playerInput;
    bool onTunnel = false;
    const float maxDistanceToTurn = 0.2f;

    //Auxiliar variables
    //Distance from the center of msPacMan to the center of the tile
    float centerTileDistance;
    bool nextTileIsCollision;
    float nextPosition;
    Vector2Int currentPosition;
    float timer;
    float timeToTurn = 0.2f;
    bool dead = false;
    byte deadTurns = 0;

    //slow movement
    [SerializeField] private float normalSpeed;
    //speed for 3 frames when a power pellet is eated
    [SerializeField] private float powerPelletSpeed;
    //speed for 1 frame when a pac dot is eated
    [SerializeField] private float dotSpeed;
    int framesSlow;

    Color visibleColor = Color.white;

    bool fright = false;
    float frightTimer;

    //animator variable
    bool notMoving;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InitLevelVariables();
        ChangePacmanDirection(direction);
    }
    void Update()
    {
        if (!pause)
        {
            if (playerInput.actions["Up"].WasPressedThisFrame())
            {
                if (direction == 2)
                {
                    ChangePacmanDirection(0);
                }
                nextDirection = 0;
            }
            if (playerInput.actions["Down"].WasPressedThisFrame())
            {
                if (direction == 0)
                {
                    ChangePacmanDirection(2);
                }
                nextDirection = 2;
            }
            if (playerInput.actions["Right"].WasPressedThisFrame())
            {
                if (direction == 3)
                {
                    ChangePacmanDirection(1);
                }
                nextDirection = 1;
            }
            if (playerInput.actions["Left"].WasPressedThisFrame())
            {
                if (direction == 1)
                {
                    ChangePacmanDirection(3);
                }
                nextDirection = 3;
            }
            notMoving = nextTileIsCollision = Physics2D.OverlapPoint(forwardPoint.position, wallMask) != null;
            MoveAndChangeDirection();
            if (notMoving)
            {
                animator.speed = 0;
            }
            else
            {
                animator.speed = 1;
            }
            if (fright)
            {
                if(frightTimer > 0)
                {
                    frightTimer -= Time.deltaTime;
                }
                else
                {
                    fright = false;
                    normalSpeed = LevelInformation.Instance.PacManSpeed;
                    speed = normalSpeed;
                }
            }
        }
        else if (dead)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if(deadTurns < 2)
                {
                    if (direction + 1 < 4)
                    {
                        ChangePacmanDirection((byte)(direction + 1));
                    }
                    else
                    {
                        deadTurns++;
                        ChangePacmanDirection(0);
                    }
                    timer = timeToTurn;
                }
                else
                {
                    deadTurns = 0;
                    dead = false;
                    GameManager.Instance.DecrementLifes();
                }
            }
        }
    }
    void MoveAndChangeDirection()
    {
        if (!nextTileIsCollision)
        {
            switch (direction)
            {
                case 0:
                    nextPosition = transform.position.y + speed * Time.deltaTime;
                    transform.position = new(transform.position.x, nextPosition);
                    break;
                case 1:
                    nextPosition = transform.position.x + speed * Time.deltaTime;
                    transform.position = new(nextPosition, transform.position.y);
                    break;
                case 2:
                    nextPosition = transform.position.y - speed * Time.deltaTime;
                    transform.position = new(transform.position.x, nextPosition);
                    break;
                case 3:
                    nextPosition = transform.position.x - speed * Time.deltaTime;
                    transform.position = new(nextPosition, transform.position.y);
                    break;
            }
            if (framesSlow > 0)
            {
                framesSlow--;
                if (framesSlow <= 0)
                {
                    speed = normalSpeed;
                }
            }
        }
        switch (direction)
        {
            case 0:
            case 2:
                CheckNextHorizontalDirection();
                break;
            case 1:
            case 3:
                CheckNextVerticalDirection();
                break;
        }
    }
    void SetCurrentPosition()
    {
        currentPosition.x = (int)transform.position.x;
        currentPosition.y = -(int)transform.position.y;
    }
    void CheckNextHorizontalDirection()
    {
        if (nextDirection == 1 || nextDirection == 3)
        {
            centerTileDistance = Mathf.Abs(-transform.position.y + (int)transform.position.y - 0.5f);
            if (centerTileDistance <= maxDistanceToTurn)
            {
                SetCurrentPosition();
                if ((nextDirection == 1 && !mapManager.GetRightMapCollision(currentPosition))
                    || (nextDirection == 3 && !mapManager.GetLeftMapCollision(currentPosition)))
                {
                    ChangePacmanDirection(nextDirection);
                    notMoving = false;
                    transform.position = new Vector2(transform.position.x, (int)transform.position.y - 0.5f);
                }
            }
        }
    }
    void CheckNextVerticalDirection()
    {
        if ((nextDirection == 0 || nextDirection == 2) && !onTunnel)
        {
            centerTileDistance = Mathf.Abs(transform.position.x - (int)transform.position.x - 0.5f);
            if (centerTileDistance <= maxDistanceToTurn)
            {
                SetCurrentPosition();
                if ((nextDirection == 0 && !mapManager.GetUpMapCollision(currentPosition))
                    || (nextDirection == 2 && !mapManager.GetDownMapCollision(currentPosition)))
                {
                    ChangePacmanDirection(nextDirection);
                    notMoving = false;
                    transform.position = new Vector2((int)transform.position.x + 0.5f, transform.position.y);
                }
            }
        }
    }
    public void InitLevelVariables()
    {
        normalSpeed = LevelInformation.Instance.PacManSpeed;
        powerPelletSpeed = LevelInformation.Instance.FrightPacManDotsSpeed;
        dotSpeed = LevelInformation.Instance.PacManDotsSpeed;
    }
    public void StartDead()
    {
        dead = true;
        ChangePacmanDirection(0);
        timer = timeToTurn;
        deadTurns = 0;
    }
    public void EnterFrightMode()
    {
        fright = true;
        frightTimer = LevelInformation.Instance.FrightTime;
        normalSpeed = LevelInformation.Instance.FrightPacManSpeed;
    }
    public void SetFright(bool value)
    {
        fright = value;
    }
    public void ChangePacmanDirection(byte newDirection)
    {
        direction = newDirection;
        nextDirection = newDirection;
        switch (direction)
        {
            case 0:
                transform.localEulerAngles = Vector3.zero;
                sp.flipX = false;
                break;
            case 1:
                transform.localEulerAngles = new Vector3(0, 0, 270);
                sp.flipX = false;
                break;
            case 2:
                transform.localEulerAngles = new Vector3(0, 0, 180);
                sp.flipX = false;
                break;
            case 3:
                transform.localEulerAngles = new Vector3(0, 0, 90);
                sp.flipX = true;
                break;
        }
    }
    public void SetActiveAnimator(bool value)
    {
        animator.enabled = value;
    }
    public void SetPause(bool value)
    {
        pause = value;
        SetActiveAnimator(!value);
    }
    public void SetOnTunnel(bool value)
    {
        onTunnel = value;
    }
    public void SetFirstSprite()
    {
        sp.sprite = firstSprite;
    }
    public void SetLowSpeed(int index)
    {
        switch (index)
        {
            case 0:
                speed = powerPelletSpeed;
                framesSlow = 3;
                break;
            case 1:
                speed = dotSpeed;
                framesSlow = 1;
                break;
        }
    }
    public void RestartSpeed()
    {
        normalSpeed = LevelInformation.Instance.PacManSpeed;
        speed = normalSpeed;
        framesSlow = 0;
    }
    public int GetDirection()
    {
        return direction;
    }
    public void SetVisible(bool value)
    {
        if (value)
        {
            visibleColor.a = 255;
        }
        else
        {
            visibleColor.a = 0;
        }
        sp.color = visibleColor;
    }
}
