using UnityEngine;
using Unity.Netcode;
public enum PlayerAnimState
{
    Idle,
    Walk,
    Run,
    Jump
}

public class PlayerAnimationController : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string idleClipName = "Soldier Idle";
    [SerializeField] private string walkClipName = "Soldier Walking";
    [SerializeField] private string runClipName = "Soldier Running";
    [SerializeField] private string jumpClipName = "Soldier Falling";

    [Header("Sincronization")]
    public NetworkVariable<PlayerAnimState> CurrentState =
        new NetworkVariable<PlayerAnimState>(
            PlayerAnimState.Idle,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

    private void Start()
    {
        // Listen for state changes (all clients)
        CurrentState.OnValueChanged += OnStateChanged;
        // Play the initial state
        OnStateChanged(CurrentState.Value, CurrentState.Value);
    }

    private void OnStateChanged(PlayerAnimState prev, PlayerAnimState next)
    {
        PlayAnimation(next);
    }

    private void PlayAnimation(PlayerAnimState state)
    {
        switch (state)
        {
            case PlayerAnimState.Idle:
                animator.Play(idleClipName);
                break;
            case PlayerAnimState.Walk:
                animator.Play(walkClipName);
                break;
            case PlayerAnimState.Run:
                animator.Play(runClipName);
                break;
            case PlayerAnimState.Jump:
                animator.Play(jumpClipName);
                break;
        }
    }

    // Methods from the movement script (the owner updates the state)
    public void SetIdle() { if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) CurrentState.Value = PlayerAnimState.Idle; }
    public void SetWalk() { if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) CurrentState.Value = PlayerAnimState.Walk; }
    public void SetRun() { if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) CurrentState.Value = PlayerAnimState.Run; }
    public void SetJump() { if (GameNetwork.IsOwnerOfflineOrOnline(NetworkObject)) CurrentState.Value = PlayerAnimState.Jump; }
}
