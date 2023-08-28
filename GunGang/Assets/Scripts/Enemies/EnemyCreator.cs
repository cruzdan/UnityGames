using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    #region Enemy creation
    [Header("Enemy creation")]
    [SerializeField] private int _totalEnemiesToAppear;
    [SerializeField] private int _enemiesIncrement;
    [SerializeField] private int _maxEnemiesToAppear;
    [SerializeField] private float _timeToGenerateEnemy;
    private int _currentEnemiesCreated;
    #endregion

    #region Position to Generate enemies
    [Header("First enemies position")]
    [SerializeField] private Transform _centerTransformToSpawnEnemies;
    [SerializeField] private float _errorDistance;
    private Vector3 _errorVector = new();
    #endregion

    #region Init Enemies
    [Header("Init variables")]
    [SerializeField] private Enemy _enemyScriptableObject;
    [SerializeField] private EndPhase _endPhase;
    [SerializeField] private Transform _entranceTransform;
    [SerializeField] private GameObject _player;
    [SerializeField] private TargetAutomaticShoot _playerAutomaticShoot;
    [SerializeField] private ConstantLookAtTarget _playerConstantLookAtTarget;
    private List<GameObject> _activeEnemies = new();
    private List<Transform> _playerChildCharacters = new();
    int _currentTargetIndex = 0;
    #endregion

    [SerializeField] private GameEvent OnAllPartnersDead;
    [SerializeField] private GameEvent OnAllEnemiesDead;
    private int _currentDeadEnemies;
    
    private float _timer;

    public void StartCreateEnemies()
    {
        _playerChildCharacters = _endPhase.GetPlayerChildCharacters();
        _timer = 0;
        _currentDeadEnemies = 0;
        SetRandomAliveTargetIndex();
        GenerateEnemy();
        InitLastEnemyOfEnemiesList();
        _currentEnemiesCreated = 1;
        ChangePartnerAutomaticShootTargetsForFirstActiveEnemy();
        ChangePartnerConstantLookAtTargetsForFirstActiveEnemy();
    }

    public void IncrementTotalEnemiesToAppear()
    {
        _totalEnemiesToAppear = Mathf.Clamp(_totalEnemiesToAppear + _enemiesIncrement, 1, _maxEnemiesToAppear);
    }

    private void Update()
    {
        if(HasCreatedAllEnemies())
        {
            enabled = false;
            return;
        }
        _timer += Time.deltaTime;
        if(CanGenerateEnemy())
        {
            GenerateEnemy();
            InitLastEnemyOfEnemiesList();
            _currentEnemiesCreated++;
            _timer = 0;
        }
    }

    bool HasCreatedAllEnemies()
    {
        return _currentEnemiesCreated >= _totalEnemiesToAppear;
    }

    bool CanGenerateEnemy()
    {
        return _timer >= _timeToGenerateEnemy;
    }

    void GenerateEnemy()
    {
        _activeEnemies.Add(ObjectPool.Instance.GetObjectFromPool(ObjectPool.PoolObjectType.Enemy, GetErrorEnemyPosition()));
        print("generating enemey: " + _currentEnemiesCreated);
    }

    void InitLastEnemyOfEnemiesList()
    {
        InitEnemy(_activeEnemies[^1]);
    }

    void SetEnemyTransformAsPartnerAutomaticShootTargets(Transform target)
    {
        _playerAutomaticShoot.SetTarget(target);
        SetEnemyTransformAsCharacterAutomaticShootTargets(target);
    }

    void SetEnemyTransformAsPartnerConstantLookAtTargets(Transform target)
    {
        _playerConstantLookAtTarget.SetTarget(target);
        SetEnemyTransformAsCharacterConstantLookAtTargets(target);
    }

    void SetEnemyTransformAsCharacterAutomaticShootTargets(Transform target)
    {
        foreach(var character in _playerChildCharacters)
        {
            character.GetComponent<TargetAutomaticShoot>().SetTarget(target);
        }
    }

    void SetEnemyTransformAsCharacterConstantLookAtTargets(Transform target)
    {
        foreach (var character in _playerChildCharacters)
        {
            character.GetComponent<ConstantLookAtTarget>().SetTarget(target);
        }
    }

    Transform GetFirstEnemyTransformOfEnemiesList()
    {
        return _activeEnemies[0].transform;
    }

    void InitEnemy(GameObject enemy)
    {
        FollowTargetPosition enemyFollowTargetPosition = enemy.GetComponent<FollowTargetPosition>();
        ClearEnemyEvents(enemyFollowTargetPosition, enemy);
        EnableEnemyFollowTarget(enemyFollowTargetPosition);
        SetEntranceAsEnemyTarget(enemyFollowTargetPosition);
        SetCurrentTargetAsEnemyFollowTargetEvent(enemyFollowTargetPosition);
        SetEventOnTouchTargetToEnemy(enemy);
        SetInitialEnemyLife(enemy);
        SetEnemyOnDeadEvent(enemy);
    }

    void ClearEnemyEvents(FollowTargetPosition enemyFollowTargetPosition, GameObject enemy)
    {
        enemyFollowTargetPosition.ClearOnTargetReached();
        enemy.GetComponent<EnemyCollisions>().ClearOnCharacterTouched();
        enemy.GetComponent<EnemyBehaviour>().ClearOnEnemyDead();
    }

    void EnableEnemyFollowTarget(FollowTargetPosition enemyFollowTargetPosition)
    {
        enemyFollowTargetPosition.enabled = true;
    }

    void SetEntranceAsEnemyTarget(FollowTargetPosition enemyFollowTargetPosition)
    {
        enemyFollowTargetPosition.SetTarget(_entranceTransform.position);
    }

    void SetCurrentTargetAsEnemyFollowTargetEvent(FollowTargetPosition enemyFollowTargetPosition)
    {
        enemyFollowTargetPosition.SubscribeToOnTargetReached(() => {
            SetCurrentTargetToEnemyFollowTarget(enemyFollowTargetPosition);
        });
    }

    void SetCurrentTargetToEnemyFollowTarget(FollowTargetPosition enemyFollowTargetPosition)
    {
        enemyFollowTargetPosition.ClearOnTargetReached();
        enemyFollowTargetPosition.SetTarget(GetTargetWithCurrentTargetIndex());
        enemyFollowTargetPosition.enabled = true;
    }

    void SetEventOnTouchTargetToEnemy(GameObject enemy)
    {
        EnemyCollisions collision = enemy.GetComponent<EnemyCollisions>();
        collision.SubscribeToOnCharacterTouched(() =>
        {
            OnEnemyTouchTarget(collision);
        });
    }

    void SetInitialEnemyLife(GameObject enemy)
    {
        enemy.GetComponent<EnemyBehaviour>().SetLife(_enemyScriptableObject.GetLife());
    }

    void SetEnemyOnDeadEvent(GameObject enemy)
    {
        enemy.GetComponent<EnemyBehaviour>().SubscribeToOnEnemyDead(() => {
            OnEnemyDead(enemy);
        });
    }

    void OnEnemyDead(GameObject enemy)
    {
        _activeEnemies.Remove(enemy);
        if (TransformIsPartnerTarget(enemy.transform))
        {
            if(HaveActiveEnemies())
            {
                ChangePartnerAutomaticShootTargetsForFirstActiveEnemy();
                ChangePartnerConstantLookAtTargetsForFirstActiveEnemy();
            }
        }
        _currentDeadEnemies++;
        print("OnEnemyDead, current of total: " + _currentDeadEnemies + " of " + _totalEnemiesToAppear);
        if (AllEnemiesAreDead())
        {
            OnAllEnemiesDead.TriggerEvent();
        }
    }

    bool TransformIsPartnerTarget(Transform targetTransform)
    {
        if (_playerAutomaticShoot.GetTarget().Equals(targetTransform))
        {
            return true;
        }
        return false;
    }

    bool HaveActiveEnemies()
    {
        return _activeEnemies.Count > 0;
    }

    void ChangePartnerAutomaticShootTargetsForFirstActiveEnemy()
    {
        SetEnemyTransformAsPartnerAutomaticShootTargets(GetFirstEnemyTransformOfEnemiesList());
    }

    void ChangePartnerConstantLookAtTargetsForFirstActiveEnemy()
    {
        SetEnemyTransformAsPartnerConstantLookAtTargets(GetFirstEnemyTransformOfEnemiesList());
    }

    bool AllEnemiesAreDead()
    {
        return _currentDeadEnemies >= _totalEnemiesToAppear;
    }

    void OnEnemyTouchTarget(EnemyCollisions collision)
    {
        if (!AllTargetsAreDisabled())
        {
            SetRandomAliveTargetIndex();
            ChangeActiveEnemyTargets();
            collision.enabled = true;
        }
        else
        {
            OnAllPartnersDead.TriggerEvent();
        }
    }

    bool AllTargetsAreDisabled()
    {
        if(_playerChildCharacters.Count > 0)
        {
            foreach (var character in _playerChildCharacters)
            {
                if (character.gameObject.activeSelf)
                {
                    return false;
                }
            }
        }
        if (_player.activeSelf)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void ChangeActiveEnemyTargets()
    {
        foreach(var activeEnemy in _activeEnemies)
        {
            SetCurrentTargetToEnemyFollowTarget(activeEnemy.GetComponent<FollowTargetPosition>());
        }
    }

    Vector3 GetErrorEnemyPosition()
    {
        GenerateErrorVectorInErrorDistance();
        return _centerTransformToSpawnEnemies.position + _errorVector;
    }

    void GenerateErrorVectorInErrorDistance()
    {
        _errorVector.x = Random.Range(-_errorDistance, _errorDistance);
        _errorVector.z = Random.Range(-_errorDistance, _errorDistance);
    }

    Vector3 GetTargetWithCurrentTargetIndex()
    {
        if(_currentTargetIndex == 0)
        {
            return _player.transform.position;
        }
        else
        {
            return _playerChildCharacters[_currentTargetIndex - 1].position;
        }
    }

    void SetRandomAliveTargetIndex()
    {
        do
        {
            _currentTargetIndex = Random.Range(0, _playerChildCharacters.Count + 1);
        } while (!TargetIsAlive(_currentTargetIndex));
    }

    bool TargetIsAlive(int index)
    {
        if(index == 0)
        {
            if (_player.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return _playerChildCharacters[index - 1].gameObject.activeSelf;
    }

    public void ReturnActiveEnemiesToPool()
    {
        print("ReturnActiveEnemiesToPool");
        foreach(var activeEnemy in _activeEnemies)
        {
            ObjectPool.Instance.ReturnObjectToPool(activeEnemy, ObjectPool.PoolObjectType.Enemy);
        }
    }

    public void ClearActiveEnemies()
    {
        _activeEnemies.Clear();
    }

    public void StopActiveEnemyMovements()
    {
        foreach (var activeEnemy in _activeEnemies)
        {
            activeEnemy.GetComponent<FollowTargetPosition>().enabled = false;
        }
    }
}
