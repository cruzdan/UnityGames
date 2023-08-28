using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : MonoBehaviour
{
    [SerializeField] private Transform _charactersLineTransform;
    [SerializeField] private float _centerCharacterPosition;
    [SerializeField] private float _distanceBetweenCharacters;
    [SerializeField] private float _targetYPosition;
    [SerializeField] private int _maxCharactersOnLine;

    [SerializeField] private FollowTargetPosition _playerTarget;
    [SerializeField] private CharacterAnimator _playerAnimator;
    private RotateToForward _playerRotateToForward;
    private TargetAutomaticShoot _playerAutomaticShoot;
    private ConstantLookAtTarget _playerConstantLookAtTarget;

    [SerializeField] private Transform _playerCharactersParent;
    [SerializeField] private Transform _mapObjectsTransform;

    [SerializeField] private GameEvent OnPartnersReady;

    private readonly List<Transform> _playerChildCharacters = new();
    private Collider _playerCollider;
    private int _currentCharactersOnLine;
    private int _characterLinesCreated;
    private Vector3 _positionToFollow;
    private int _charactersReady = 0;
    private int _totalCharacters;

    FollowTargetPosition _characterFollowTarget;

    private void Start()
    {
        _positionToFollow = new Vector3(0, _targetYPosition, 0);
        _playerCollider = _playerTarget.GetComponent<Collider>();
        _playerRotateToForward = _playerTarget.GetComponent<RotateToForward>();
        _playerAutomaticShoot = _playerTarget.GetComponent<TargetAutomaticShoot>();
        _playerConstantLookAtTarget = _playerTarget.GetComponent<ConstantLookAtTarget>();
    }

    public void StartEndPhase()
    {
        InitPhase();
        CalculateZPositionToFollow();
        StartPlayerEndPhase();
        StartCharactersEndPhase();
    }

    void InitPhase()
    {
        _playerChildCharacters.Clear();
        _currentCharactersOnLine = 0;
        _characterLinesCreated = 0;
        _positionToFollow.z = 0;
        _charactersReady = 0;
    }

    void StartPlayerEndPhase()
    {
        _positionToFollow.x = _centerCharacterPosition;
        SetTargetPositionToPlayer();
        SetPlayerRotateToFrontWhenReachTarget();
        SetPlayerIncrementCharactersReadyWhenCompleteRotation();
        SetPlayerIdleStateWhenCompleteRotation();
        EnablePlayerTarget();
        IncrementCharactersWithTarget();
    }

    void StartCharactersEndPhase()
    {
        _totalCharacters = _playerCharactersParent.childCount;
        FillPlayerChildCharactersList();
        ClearCharacterEvents();
        SetPlayerCharactersChildsOfMapObjectsTransform();
        DisablePartnersColliders();
        InitAndEnableCharacterTargets();
    }

    void SetTargetPositionToPlayer()
    {
        _playerTarget.SetTarget(_positionToFollow);
    }

    void SetPlayerRotateToFrontWhenReachTarget()
    {
        _playerTarget.SubscribeToOnTargetReached(EnablePlayerRotateToForward);
    }

    void SetPlayerIncrementCharactersReadyWhenCompleteRotation()
    {
        _playerRotateToForward.SubscribeToOnCompletedRotation(IncrementCharactersReady);
    }

    void EnablePlayerRotateToForward()
    {
        _playerRotateToForward.enabled = true;
    }

    void SetPlayerIdleStateWhenCompleteRotation()
    {
        _playerRotateToForward.SubscribeToOnCompletedRotation(SetPlayerInIdleState);
    }

    void SetPlayerInIdleState()
    {
        _playerAnimator.PassFromWalkToIdle();
    }

    void CalculateZPositionToFollow()
    {
        _positionToFollow.z = _charactersLineTransform.position.z - _characterLinesCreated * _distanceBetweenCharacters;
    }

    void CalculateNextXPositionToFollow()
    {
        if(_currentCharactersOnLine % 2 == 1)
        {
            _positionToFollow.x = _centerCharacterPosition - (_currentCharactersOnLine / 2 + 1) * _distanceBetweenCharacters;
        }
        else
        {
            _positionToFollow.x = _centerCharacterPosition + (_currentCharactersOnLine / 2)  * _distanceBetweenCharacters;
        }
    }

    void IncrementCharactersWithTarget()
    {
        if(_currentCharactersOnLine ==_maxCharactersOnLine - 1)
        {
            _currentCharactersOnLine = 0;
            _characterLinesCreated++;
            _positionToFollow.z -= _distanceBetweenCharacters;
        }
        else
        {
            _currentCharactersOnLine++;
        }
    }

    void EnablePlayerTarget()
    {
        _playerTarget.enabled = true;
    }

    void FillPlayerChildCharactersList()
    {
        for(int i = 0; i < _totalCharacters; i++)
        {
            _playerChildCharacters.Add(_playerCharactersParent.GetChild(i));
        }
    }

    void ClearCharacterEvents()
    {
        foreach (var character in _playerChildCharacters)
        {
            character.GetComponent<RotateToForward>().ClearOnCompletedRotation();
            character.GetComponent<FollowTargetPosition>().ClearOnTargetReached();
        }
    }

    void SetPlayerCharactersChildsOfMapObjectsTransform()
    {
        foreach(var character in _playerChildCharacters)
        {
            character.SetParent(_mapObjectsTransform);
        }
    }

    void DisablePartnersColliders()
    {
        _playerCollider.enabled = false;
        DisableCharacterColliders();
    }

    void DisableCharacterColliders()
    {
        foreach (var character in _playerChildCharacters)
        {
            character.GetComponent<Collider>().enabled = false;
        }
    }

    void InitAndEnableCharacterTargets() 
    {
        foreach (var character in _playerChildCharacters)
        {
            _characterFollowTarget = character.GetComponent<FollowTargetPosition>();
            CalculateNextXPositionToFollow();
            _characterFollowTarget.SetTarget(_positionToFollow);
            _characterFollowTarget.enabled = true;
            IncrementCharactersWithTarget();
            InitCharacterEvents();
        }
    }

    void InitCharacterEvents()
    {
        RotateToForward characterRotateToForward = _characterFollowTarget.GetComponent<RotateToForward>();
        _characterFollowTarget.SubscribeToOnTargetReached(() => { characterRotateToForward.enabled = true; } );
        characterRotateToForward.SubscribeToOnCompletedRotation(IncrementCharactersReady);
        characterRotateToForward.SubscribeToOnCompletedRotation(() =>
        {
            characterRotateToForward.GetComponent<CharacterAnimator>().PassFromWalkToIdle();
        });
    }

    void IncrementCharactersReady()
    {
        _charactersReady++;
        if(HasReachedTotalCharactersReady())
        {
            EnableCollidersToPlayerAndCharacters();
            OnPartnersReady.TriggerEvent();
        }
    }

    bool HasReachedTotalCharactersReady()
    {
        return _charactersReady > _totalCharacters;
    }

    public void OnPassLevel()
    {
        ClearPlayerEvents();
        ClearCharacterEvents();
    }

    public void OnRestartLevel()
    {
        ClearPlayerEvents();
    }

    void EnableCollidersToPlayerAndCharacters()
    {
        _playerCollider.enabled = true;
        EnableCharacterColliders();
    }

    void EnableCharacterColliders()
    {
        foreach(var character in _playerChildCharacters)
        {
            character.GetComponent<Collider>().enabled = true;
        }
    }

    void ClearPlayerEvents()
    {
        _playerRotateToForward.ClearOnCompletedRotation();
        _playerTarget.ClearOnTargetReached();
    }

    public List<Transform> GetPlayerChildCharacters()
    {
        return _playerChildCharacters;
    }

    public void EnablePartnerAutomaticShoots(bool value)
    {
        _playerAutomaticShoot.enabled = value;
        EnableCharacterShoots(value);
    }

    void EnableCharacterShoots(bool value)
    {
        foreach (var character in _playerChildCharacters)
        {
            character.GetComponent<TargetAutomaticShoot>().enabled = value;
        }
    }

    public void EnablePartnerConstantLookAtTarget(bool value)
    {
        _playerConstantLookAtTarget.enabled = value;
        EnableCharacterConstantLookAtTarget(value);
    }

    void EnableCharacterConstantLookAtTarget(bool value)
    {
        foreach (var character in _playerChildCharacters)
        {
            character.GetComponent<ConstantLookAtTarget>().enabled = value;
        }
    }
}
