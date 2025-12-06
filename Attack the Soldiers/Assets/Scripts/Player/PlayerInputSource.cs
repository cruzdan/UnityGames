using UnityEngine;

public class PlayerInputSource : MonoBehaviour
{
    [SerializeField] private MonoBehaviour keyboardInputSource;
    [SerializeField] private MonoBehaviour mobileInputSource;
    [SerializeField] private DeviceChecker deviceChecker;
    public IPlayerInputSource GetInputSource()
    {
        if (deviceChecker.IsOnPC())
            return keyboardInputSource as IPlayerInputSource;
        if (deviceChecker.IsOnMobile())
            return mobileInputSource as IPlayerInputSource;
        return keyboardInputSource as IPlayerInputSource;
    }
}