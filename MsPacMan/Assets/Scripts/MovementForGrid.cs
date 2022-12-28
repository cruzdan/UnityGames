using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementForGrid : MonoBehaviour
{
    [SerializeField] private Transform[] forwardPoints;
    //0->up, 1->right, 2-> down, 3-> left
    int direction = 1;
    [SerializeField] private float speed;
    Vector2 movement;
    [SerializeField] private LayerMask wallMask;
    int movementType;

    [SerializeField] private MapManager mapManager;
    public enum MovementType
    {
        RandomMovement = 0,
        TargetMovement = 1,
        GhostTarget = 2,
        ReachTarget = 3
    }

    //Change direction
    bool nextTileIsCollision;
    float centerTileDistance;
    //distance to change direction
    float maxCenterDistance = 0.2f;
    //up, right, down, left
    bool[] adjacentTileCollisions = new bool[4];
    Vector2Int lastPositionChanged = new Vector2Int(-10, -10);
    //auxiliar variable to change direction
    int nextDirection;

    //Target
    private Vector2Int targetPosition = new();
    int[] euclideanDistances = new int[4];
    //auxiliar variable to set the minimum distance to the target
    int min;
    Vector2Int offset = Vector2Int.zero;

    bool hasForbiddenTiles;
    [SerializeField] Vector2Int[] forbiddenTilePositions;
    int forbiddenDirection;

    Rigidbody2D rb;
    bool onTunnel = false;

    //Ghost target
    [SerializeField] private GhostBehaviour ghostBehaviour;
    bool turn = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (onTunnel)
        {
            if (turn)
            {
                turn = false;
                direction = (direction + 2) % 4;
                lastPositionChanged.x = Mathf.Clamp((int)transform.position.x, 0, 27);
                lastPositionChanged.y = -(int)transform.position.y;
            }
            MoveToDirection();
        }
        else
        {
            nextTileIsCollision = Physics2D.OverlapPoint(forwardPoints[direction].position, wallMask) != null;
            if (!nextTileIsCollision)
            {
                MoveToDirection();
            }
            if (lastPositionChanged.x != (int)transform.position.x || lastPositionChanged.y != -(int)transform.position.y)
            {
                CalculateCenterTileDistance();
                if (centerTileDistance <= maxCenterDistance)
                {
                    //on center tile
                    if (turn)
                    {
                        turn = false;
                        direction = (direction + 2) % 4;
                        lastPositionChanged.x = Mathf.Clamp((int)transform.position.x, 0, 27);
                        lastPositionChanged.y = -(int)transform.position.y;
                    }
                    else
                    {
                        CheckTileCollisions();
                        switch (movementType)
                        {
                            case 0:
                                //random movement
                                SetRandomDirection();
                                break;
                            case 1:
                                //follow target
                                SetBestDirection();
                                break;
                            case 2:
                                //ghost target
                                targetPosition = ghostBehaviour.GetTargetPosition();
                                SetBestDirection();
                                break;
                            case 3:
                                //ghost House, reach target
                                if (IsOnTarget())
                                {
                                    ghostBehaviour.ChangeModeToEnterHouse();
                                }
                                else
                                {
                                    SetBestDirection();
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
    public void RestartLastPositionChanged()
    {
        lastPositionChanged = new Vector2Int(-10, -10);
    }
    void MoveToDirection()
    {
        switch (direction)
        {
            case 0:
                //up
                movement.x = transform.position.x;
                movement.y = transform.position.y + speed * Time.fixedDeltaTime;
                break;
            case 1:
                //right
                movement.x = transform.position.x + speed * Time.fixedDeltaTime;
                movement.y = transform.position.y;
                break;
            case 2:
                //down
                movement.x = transform.position.x;
                movement.y = transform.position.y - speed * Time.fixedDeltaTime;
                break;
            case 3:
                //left
                movement.x = transform.position.x - speed * Time.fixedDeltaTime;
                movement.y = transform.position.y;
                break;
        }
        rb.MovePosition(movement);
    }
    void CalculateCenterTileDistance()
    {
        switch (direction)
        {
            case 0:
                centerTileDistance = Mathf.Abs(-transform.position.y + (int)transform.position.y - 0.5f);
                break;
            case 1:
                centerTileDistance = Mathf.Abs(transform.position.x - (int)transform.position.x - 0.5f);
                break;
            case 2:
                centerTileDistance = Mathf.Abs(-transform.position.y + (int)transform.position.y - 0.5f);
                break;
            case 3:
                centerTileDistance = Mathf.Abs(transform.position.x - (int)transform.position.x - 0.5f);
                break;
        }
    }
    void CheckTileCollisions()
    {
        lastPositionChanged.x = Mathf.Clamp((int)transform.position.x, 0, 27);
        lastPositionChanged.y = -(int)transform.position.y;

        adjacentTileCollisions[0] = mapManager.GetUpMapCollision(lastPositionChanged);
        adjacentTileCollisions[1] = mapManager.GetRightMapCollision(lastPositionChanged);
        adjacentTileCollisions[2] = mapManager.GetDownMapCollision(lastPositionChanged);
        adjacentTileCollisions[3] = mapManager.GetLeftMapCollision(lastPositionChanged);
        if (hasForbiddenTiles)
        {
            for (int i = 0; i < forbiddenTilePositions.Length; i++)
            {
                forbiddenDirection = GetAdjacentForbiddenTileIndex(forbiddenTilePositions[i], lastPositionChanged);
                if (forbiddenDirection > -1)
                {
                    adjacentTileCollisions[forbiddenDirection] = true;
                    break;
                }
            }
        }
        //the previous tile traversed is a collision
        adjacentTileCollisions[(direction + 2) % 4] = true;

    }
    //get forbidden adjacent tile: 0 up, 1 right, 2 down, 3 left, -1 if does not have forbidden adjacent tile
    int GetAdjacentForbiddenTileIndex(Vector2Int forbiddenTilePosition, Vector2Int currentPosition)
    {
        if(forbiddenTilePosition.x == currentPosition.x)
        {
            if(forbiddenTilePosition.y == currentPosition.y - 1)
            {
                //up = forbidden
                return 0;
            }
            else if(forbiddenTilePosition.y == currentPosition.y + 1)
            {
                //down = forbidden
                return 2;
            }
        }
        if (forbiddenTilePosition.y == currentPosition.y)
        {
            if (forbiddenTilePosition.x == currentPosition.x - 1)
            {
                //left = forbidden
                return 3;
            }
            else if (forbiddenTilePosition.x == currentPosition.x + 1)
            {
                //right = forbidden
                return 1;
            }
        }
        return -1;
    }
    void SetRandomDirection()
    {
        do
        {
            nextDirection = Random.Range(0, 4);
        } while (adjacentTileCollisions[nextDirection]);
        CenterPosition();
        direction = nextDirection;
    }
    void SetBestDirection()
    {
        min = 100000;
        for(int i = 0; i < 4; i++)
        {
            if (!adjacentTileCollisions[i])
            {
                offset = GetOffsetByDirection(i);
                euclideanDistances[i] = (int)Mathf.Pow(offset.x + lastPositionChanged.x - targetPosition.x, 2) +
                    (int)Mathf.Pow(offset.y + lastPositionChanged.y - targetPosition.y, 2);
                if(euclideanDistances[i] < min)
                {
                    min = euclideanDistances[i];
                    direction = i;
                }
            }
        }
    }
    Vector2Int GetOffsetByDirection(int index)
    {
        return index switch
        {
            0 => new Vector2Int(0, -1),
            1 => new Vector2Int(1, 0),
            2 => new Vector2Int(0, 1),
            3 => new Vector2Int(-1, 0),
            _ => Vector2Int.zero,
        };
    }
    void CenterPosition()
    {
        if (direction % 2 != nextDirection % 2)
        {
            switch (direction)
            {
                case 0:
                case 2:
                    transform.position = new Vector2(transform.position.x, (int)transform.position.y - 0.5f);
                    break;
                case 1:
                case 3:
                    transform.position = new Vector2((int)transform.position.x + 0.5f, transform.position.y);
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tunnel"))
        {
            onTunnel = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tunnel"))
        {
            onTunnel = false;
        }
    }
    public void SetOnTunnel(bool value)
    {
        onTunnel = value;
    }
    public void SetTargetPosition(Vector2Int newPosition)
    {
        targetPosition = newPosition;
    }
    public void SetMovementType(MovementType newMovement)
    {
        movementType = (int)newMovement;
    }
    public void SetHasForbiddenTiles(bool value)
    {
        hasForbiddenTiles = value;
    }
    public void SetForbiddenTiles(Vector2Int[] newForbiddenTilePositions)
    {
        forbiddenTilePositions = newForbiddenTilePositions;
    }
    public void SetDirection(int newDirection) 
    {
        direction = newDirection;
    }
    public void SetTurn(bool value)
    {
        turn = value;
    }
    public int GetDirection()
    {
        return direction;
    }
    public bool IsOnTarget()
    {
        return lastPositionChanged.Equals(targetPosition);
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
