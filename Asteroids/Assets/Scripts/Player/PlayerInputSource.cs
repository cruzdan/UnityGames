using UnityEngine;

public class PlayerInputSource : MonoBehaviour
{
    [SerializeField] private MonoBehaviour keyboardInputSource;
    [SerializeField] private MonoBehaviour mobileInputSource;
    [SerializeField] private DeviceChecker deviceChecker;

    public IPlayerInputSource Keyboard =>
        keyboardInputSource as IPlayerInputSource;

    public IPlayerInputSource Mobile =>
        mobileInputSource as IPlayerInputSource;

    public bool IsOnPC() => deviceChecker.IsOnPC();
    public bool IsOnMobile() => deviceChecker.IsOnMobile();
}
