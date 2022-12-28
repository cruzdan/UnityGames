using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostBehaviour : MonoBehaviour
{
    [Header("Dots")]
    [SerializeField] private int dotLimit;
    int dotCounter = 0;

    [SerializeField] private float speed;
    //0->up, 1->right, 2->down, 3->left
    [SerializeField] private int direction;
    [SerializeField] private Rigidbody2D rb;

    [Header("Initial Values")]
    [SerializeField] private int initialDirection;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private MovementType initialMovementType;

    [Header("On House")]
    [SerializeField] private float houseLimitUp;
    [SerializeField] private float houseLimitDown;
    bool movingToInitalPosition = false;
    
    Vector2 movement;
    bool pause = true;
    [SerializeField] protected MovementType movementType;

    [Header("Leaving House")]
    [SerializeField] private float centerY;
    [SerializeField] private float centerX;
    [SerializeField] private float exitY;
    [SerializeField] private float maxDistanceToCenter;
    bool isOnCenterY;
    bool isOnCenterX;

    [SerializeField] private MovementForGrid movementForGrid;
    [SerializeField] protected Vector2Int targetScatterPosition;

    bool fright;
    float frightTimer;

    [SerializeField] private Vector2Int housePosition;

    Vector2Int ghostPositionOnGrid = new();

    bool dead = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Sprite initialSprite;

    [SerializeField] protected GameManager gameManager;

    Color visibleColor = Color.white;
    public enum MovementType
    {
        OnHouse = 0,
        LeavingHouse = 1,
        Scatter = 2,
        Chase = 3,
        Fright = 4,
        ReturnToHouse = 5,
        RandomMovement = 6
    }

    private void Update()
    {
        if (!pause)
        {
            switch (movementType)
            {
                case MovementType.OnHouse:
                    OnHouse();
                    break;
                case MovementType.LeavingHouse:
                    LeavingHouse();
                    break;
                case MovementType.ReturnToHouse:
                    EnterToHouse();
                    break;
            }
            animator.SetFloat("Direction", direction);
        }
        else if (movementForGrid.isActiveAndEnabled)
        {
            animator.SetFloat("Direction", movementForGrid.GetDirection());
        }
        UpdateFrightTimer();
        switch (movementType)
        {
            //collide with ms pacman
            case MovementType.Scatter:
            case MovementType.Chase:
            case MovementType.RandomMovement:
                ghostPositionOnGrid.x = (int)transform.position.x;
                ghostPositionOnGrid.y = -(int)transform.position.y;
                if (gameManager.GetMsPacManPositionOnGrid().Equals(ghostPositionOnGrid))
                {
                    //ms Pac Man dead
                    gameManager.SetDeadState();
                }
                break;
            case MovementType.Fright:
                ghostPositionOnGrid.x = (int)transform.position.x;
                ghostPositionOnGrid.y = -(int)transform.position.y;
                if (gameManager.GetMsPacManPositionOnGrid().Equals(ghostPositionOnGrid))
                {
                    //ghost is eated on fright mode
                    ChangeMovementToReturnToHouse();
                    fright = false;
                    animator.SetBool("Dead", true);
                    animator.Play("Dead");
                    SetVisible(false);
                    gameManager.SetGhostEatedActions();
                    gameManager.SpawnGhostScore(transform.position);
                    animator.enabled = true;
                    dead = true;
                }
                break;
        }
    }
    void UpdateFrightTimer()
    {
        if (fright && movementType != MovementType.ReturnToHouse)
        {
            frightTimer -= Time.deltaTime;

            if (frightTimer >= 2)
            {
                animator.SetFloat("Time", 2);
            }
            else
            {
                animator.SetFloat("Time", 0);
            }
            if (frightTimer <= 0)
            {
                fright = false;
                animator.SetBool("Fright", fright);
                if (movementType == MovementType.Fright)
                {
                    if (gameManager.GetGhostModeIndex() % 2 == 0)
                    {
                        ChangeMovementToScatter();
                    }
                    else
                    {
                        ChangeMovementToChase();
                    }
                }
            }
        }
    }
    public virtual void ReturnFromFrightToScatter()
    {
        movementType = MovementType.Scatter;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.TargetMovement);
        movementForGrid.SetTargetPosition(targetScatterPosition);
    }
    public virtual void ReturnFromFrightToChase()
    {
        movementType = MovementType.Chase;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.GhostTarget);
    }
    public void OnHouse()
    {
        if (movingToInitalPosition)
        {
            if(transform.position.x < initialPosition.x)
            {
                movement.x = transform.position.x + speed * Time.deltaTime;
                movement.y = transform.position.y;
                rb.MovePosition(movement);
                if(transform.position.x >= initialPosition.x - maxDistanceToCenter)
                {
                    transform.position = initialPosition;
                    movingToInitalPosition = false;
                }
            }
            else
            {
                movement.x = transform.position.x - speed * Time.deltaTime;
                movement.y = transform.position.y;
                rb.MovePosition(movement);
                if (transform.position.x <= initialPosition.x + maxDistanceToCenter)
                {
                    transform.position = initialPosition;
                    movingToInitalPosition = false;
                }
            }
        }
        else
        {
            if (direction == 0)
            {
                if (transform.position.y >= houseLimitUp)
                {
                    direction = 2;
                }
                else
                {
                    movement.x = transform.position.x;
                    movement.y = transform.position.y + speed * Time.deltaTime;
                    rb.MovePosition(movement);
                }
            }
            else
            {
                if (transform.position.y <= houseLimitDown)
                {
                    direction = 0;
                }
                else
                {
                    movement.x = transform.position.x;
                    movement.y = transform.position.y - speed * Time.deltaTime;
                    rb.MovePosition(movement);
                }
            }
        }
    }
    public void SetVisible(bool value)
    {
        if (value)
            visibleColor.a = 255;
        else
            visibleColor.a = 0;
        sp.color = visibleColor;
    }
    public virtual void InitOnStartGameVariables() { }
    public virtual Vector2Int GetTargetPosition() { return Vector2Int.zero; }
    public void PauseMovement() 
    {
        pause = true;
        movementForGrid.enabled = false;
    }
    public void LeavingHouse()
    {
        if (!isOnCenterY)
        {
            if(Mathf.Abs(transform.position.y - centerY) <= maxDistanceToCenter)
            {
                isOnCenterY = true;
                transform.position = new Vector2(transform.position.x, centerY);
            }
            else
            {
                if(transform.position.y > centerY)
                {
                    movement.x = transform.position.x;
                    movement.y = transform.position.y - speed * Time.deltaTime;
                    rb.MovePosition(movement);
                }
                else
                {
                    movement.x = transform.position.x;
                    movement.y = transform.position.y + speed * Time.deltaTime;
                    rb.MovePosition(movement);
                }
            }
        }
        else if(!isOnCenterX)
        {
            if(Mathf.Abs(transform.position.x - centerX) <= maxDistanceToCenter)
            {
                isOnCenterX = true;
            }
            else
            {
                if(transform.position.x < centerX)
                {
                    movement.x = transform.position.x + speed * Time.deltaTime;
                    movement.y = transform.position.y;
                    rb.MovePosition(movement);
                }
                else
                {
                    movement.x = transform.position.x - speed * Time.deltaTime;
                    movement.y = transform.position.y;
                    rb.MovePosition(movement);
                }
            }
        }
        else
        {
            if(transform.position.y < exitY)
            {
                movement.x = transform.position.x;
                movement.y = transform.position.y + speed * Time.deltaTime;
                rb.MovePosition(movement);
            }
            else
            {
                //leaving ghost from house
                direction = 1;
                TurnInmediatly();
                movementForGrid.SetOnTunnel(false);
                if (fright)
                {
                    ChangeMovementToFright();
                }
                else
                {
                    if (gameManager.GetGhostModeIndex() % 2 == 0)
                    {
                        ChangeMovementToScatter();
                    }
                    else
                    {
                        ChangeMovementToChase();
                    }
                }
            }
        }
    }
    public void TurnInmediatly()
    {
        direction = (direction + 2) % 4;
        movementForGrid.RestartLastPositionChanged();
        movementForGrid.SetDirection(direction);
    }
    public virtual void ChangeGhostSpeed(float newSpeed)
    {
        speed = newSpeed;
        movementForGrid.SetSpeed(newSpeed);
    }
    public void TurnGrid()
    {
        movementForGrid.SetTurn(true);
    }
    public void EnterToHouse()
    {
        if (!isOnCenterY)
        {
            movement.x = transform.position.x;
            movement.y = transform.position.y - speed * Time.deltaTime;
            rb.MovePosition(movement);
            if (Mathf.Abs(transform.position.y - centerY) <= maxDistanceToCenter)
            {
                isOnCenterY = true;
                transform.position = new Vector2(transform.position.x, centerY);
                dead = false;
                animator.SetBool("Dead", dead);
                animator.SetBool("Fright", fright);
                if (!gameManager.GhostIsUsingGlobalCounter())
                {
                    if(dotCounter >= dotLimit)
                    {
                        ChangeMovementToLeavingHouse();
                    }
                    else
                    {
                        SetOnHouse();
                        if (!gameManager.IsCountingDotsForAGhost())
                        {
                            gameManager.SetCurrentCounterGhost(gameObject.name);
                            gameManager.RestartGhostTimer();
                        }
                    }
                }
                else
                {
                    if (!gameObject.name.Equals("Blinky"))
                    {
                        SetOnHouse();
                        if (!gameManager.IsCountingDotsForAGhost())
                        {
                            gameManager.SetCurrentCounterGhost(gameObject.name);
                            gameManager.RestartGhostTimer();
                        }
                    }
                    else
                    {
                        ChangeMovementToLeavingHouse();
                    }
                }
                if (gameManager.GhostIsOnEatingMode())
                {
                    SetActiveAnimation(false);
                    enabled = false;
                }
            }
        }
    }
    void SetOnHouse()
    {
        direction = initialDirection;
        movementType = MovementType.OnHouse;
        movingToInitalPosition = true;
        speed = LevelInformation.Instance.GhostSpeed;
        movementForGrid.SetSpeed(LevelInformation.Instance.GhostSpeed);
    }
    public void SetMovingToInitialPosition(bool value)
    {
        movingToInitalPosition = value;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetGridSpeed(float newSpeed)
    {
        movementForGrid.SetSpeed(newSpeed);
    }
    public void SetPause(bool value)
    {
        pause = value;
    }
    public MovementType GetMovementType()
    {
        return movementType;
    }
    public void InitLeavingHouseVariables()
    {
        isOnCenterY = false;
        isOnCenterX = false;
        
    }
    public void InitScatterVariables()
    {
        movementForGrid.SetMovementType(MovementForGrid.MovementType.TargetMovement);
        movementForGrid.SetTargetPosition(targetScatterPosition);
    }
    public void InitChaseVariables()
    {
        movementType = MovementType.Chase;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.GhostTarget);
    }
    public void SetInitialDirection()
    {
        direction = initialDirection;
    }
    public void SetInitialMovement()
    {
        movementType = initialMovementType;
    }
    public void SetActiveGridMovement(bool value)
    {
        movementForGrid.enabled = value;
    }
    public void InitGridMovement(MovementForGrid.MovementType newMovement, bool hasForbiddenTiles, bool isOnTunnel)
    {
        movementForGrid.SetMovementType(newMovement);
        movementForGrid.SetHasForbiddenTiles(hasForbiddenTiles);
        movementForGrid.SetOnTunnel(isOnTunnel);
        movementForGrid.RestartLastPositionChanged();
        movementForGrid.SetDirection(direction);
    }
    public void ChangeMovementToLeavingHouse()
    {
        movementType = MovementType.LeavingHouse;
        movementForGrid.enabled = false;
        pause = false;
        InitLeavingHouseVariables();
        speed = LevelInformation.Instance.GhostSpeed;
        movementForGrid.SetSpeed(LevelInformation.Instance.GhostSpeed);
    }
    public virtual void ChangeMovementToScatter()
    {
        movementType = MovementType.Scatter;
        movementForGrid.enabled = true;
        pause = true;
        InitScatterVariables();
        ChangeGhostSpeed(LevelInformation.Instance.GhostSpeed);
    }
    public void ChangeMovementToFright()
    {
        movementForGrid.enabled = true;
        pause = true;
        movementType = MovementType.Fright;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.RandomMovement);
        speed = LevelInformation.Instance.FrightGhostSpeed;
        movementForGrid.SetSpeed(LevelInformation.Instance.FrightGhostSpeed);
    }
    public void ChangeMovementToChase()
    {
        movementType = MovementType.Chase;
        movementForGrid.enabled = true;
        pause = true;
        InitChaseVariables();
        ChangeGhostSpeed(LevelInformation.Instance.GhostSpeed);
    }
    public void ChangeMovementToReturnToHouse()
    {
        movementForGrid.enabled = true;
        pause = true;
        movementType = MovementType.ReturnToHouse;
        movementForGrid.SetMovementType(MovementForGrid.MovementType.ReachTarget);
        movementForGrid.SetTargetPosition(housePosition);
        speed = 10;
        movementForGrid.SetSpeed(10);
    }
    public void ChangeModeToEnterHouse()
    {
        movementForGrid.enabled = false;
        pause = false;
        movementType = MovementType.ReturnToHouse;
        direction = 2;
        isOnCenterY = false;
    }
    public void SetFrightMode(float time)
    {
        fright = true;
        frightTimer = time;
        animator.SetBool("Fright", fright);
        animator.SetFloat("Time", frightTimer);
    }
    public void SetActiveAnimation(bool value)
    {
        animator.enabled = value;
    }
    public void SetInitialPosition()
    {
        transform.position = initialPosition;
    }
    public void SetDotLimit(int newLimit)
    {
        dotLimit = newLimit;
    }
    public int GetDotCounter()
    {
        return dotCounter;
    }
    public int GetDotLimit()
    {
        return dotLimit;
    }
    public void ResetDotCounter()
    {
        dotCounter = 0;
    }
    public void IncrementDotCounter()
    {
        dotCounter++;
    }
    public void SetDead(bool value)
    {
        dead = value;
        animator.SetBool("Dead", dead);
    }
    public bool GetDead()
    {
        return dead;
    }
    public bool IsFright()
    {
        return fright;
    }
    public void SetFright(bool value)
    {
        fright = value;
        animator.SetBool("Fright", fright);
    }
    public void SetFirstSprite()
    {
        sp.sprite = initialSprite;
    }
    public void SetGridTurnActive(bool value)
    {
        movementForGrid.SetTurn(value);
    }
    public void SetActiveGridMovementIfNeeded()
    {
        switch (movementType)
        {
            case MovementType.Scatter:
            case MovementType.Chase:
            case MovementType.Fright:
            case MovementType.ReturnToHouse:
            case MovementType.RandomMovement:
                movementForGrid.enabled = true;
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Tunnel") && speed != LevelInformation.Instance.GhostTunnelSpeed)
        {
            speed = LevelInformation.Instance.GhostTunnelSpeed;
            movementForGrid.SetSpeed(LevelInformation.Instance.GhostTunnelSpeed);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tunnel"))
        {
            switch (movementType)
            {
                case MovementType.Scatter:
                case MovementType.Chase:
                case MovementType.RandomMovement:
                    ChangeGhostSpeed(LevelInformation.Instance.GhostSpeed);
                    break;
                case MovementType.Fright:
                    speed = LevelInformation.Instance.FrightGhostSpeed;
                    movementForGrid.SetSpeed(LevelInformation.Instance.FrightGhostSpeed);
                    break;
            }
        }
    }
    public void SetOnTunnel(bool value)
    {
        movementForGrid.SetOnTunnel(value);
    }
    public void SetTargetScatterPosition(int x, int y)
    {
        Debug.Log("Setting scatter pos to " + gameObject.name + " " + x + ", " + y);
        targetScatterPosition.x = x;
        targetScatterPosition.y = y;
    }
}
