using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    #region events
    [SerializeField] private GameEvent OnPassLevel;
    [SerializeField] private GameEvent OnRestart;
    #endregion

    #region Player
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GlobalRotation _playerRotation;
    [SerializeField] private BlocksDropper _blocksDropper;
    [SerializeField] private PlayerInput _detectKey;
    #endregion

    #region Camera
    [SerializeField] private GameObject _camera;
    private FollowTransformOnOneAxis _cameraFollow;
    #endregion

    [SerializeField] private int _fps = 30;

    [SerializeField] private Shop[] _shops;


    void OnEnable()
    {
        Application.targetFrameRate = _fps;
    }

    private void Start()
    {
        InitShops();
    }

    void InitShops()
    {
        int total = _shops.Length;
        for (int i = 0; i < total; i++)
        {
            _shops[i].InitShopItems();
        }
    }

    public void Restart()
    {
        OnRestart.TriggerEvent();
    }

    public void Loose()
    {
        EnableBallBlockColliders();
        OnRestart.TriggerEvent();
        ResetBall();
    }

    public void SetBeginScene()
    {
        _playerMovement.DisableHorizontalMovement();
        _playerRotation.enabled = false;
    }

    public void SetInGame()
    {
        _playerMovement.EnableForwardMovement();
        _playerMovement.EnableHorizontalMovement();
        _playerRotation.enabled = true;
        _playerRotation.ResetRotationX();
    }

    public void SetOnFloorPhase()
    {
        Destroy(_playerMovement.GetComponent<SkyPhase>());
        _playerRotation.enabled = true;
        _playerMovement.EnableForwardMovement();
        Destroy(_cameraFollow);
    }

    public void EndSkyPhase()
    {
        _playerMovement.gameObject.AddComponent<SkyPhase>().InitGoingDown();
        _playerMovement.DisableForwardMovement();
        _playerRotation.enabled = false;
        Destroy(_playerMovement.GetComponent<PlayerSkyRotation>());
        _cameraFollow = _camera.AddComponent<FollowTransformOnOneAxis>();
        _cameraFollow.SetAxisFollowing(1);
        _cameraFollow.SetFollowingTransform(_playerMovement.transform);
    }

    public void InitSkyPhase()
    {
        _playerMovement.gameObject.AddComponent<SkyPhase>().InitGoingUp();
        _playerMovement.DisableForwardMovement();
        _playerRotation.enabled = false;
        _cameraFollow = _camera.AddComponent<FollowTransformOnOneAxis>();
        _cameraFollow.SetAxisFollowing(1);
        _cameraFollow.SetFollowingTransform(_playerMovement.transform);
    }
    public void SetOnSkyPhase()
    {
        _playerMovement.EnableForwardMovement();
        _playerMovement.gameObject.AddComponent<PlayerSkyRotation>();
        Destroy(_cameraFollow);
        Destroy(_playerMovement.GetComponent<SkyPhase>());
    }
    
    public void InitBallDropBlocks()
    {
        _blocksDropper.enabled = true;
    }

    void ResetBall()
    {
        _playerMovement.transform.position = new Vector3(0, 1, 0);
        _playerMovement.transform.localScale = Vector3.one;
        _playerMovement.ResetOriginalSpeed();
        _playerMovement.DisableForwardMovement();
        _playerRotation.enabled = false;
        _blocksDropper.enabled = false;
        _detectKey.enabled = false;
    }

    public void StartLoosingPhase()
    {
        _playerMovement.DisableHorizontalMovement();
        _playerMovement.DisableForwardMovement();
        _playerRotation.enabled = false;
    }

    void EnableBallBlockColliders()
    {
        BoxCollider[] colliders = _playerMovement.gameObject.transform.GetComponentsInChildren<BoxCollider>(true);
        foreach(var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public void PassLevel()
    {
        Restart();
        OnPassLevel.TriggerEvent();
        ResetBall();
    }
}
