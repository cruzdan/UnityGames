public interface IPlayerInputSource
{
    float GetHorizontalMovement();
    bool GetRun();
    bool GetJump();
    bool GetShoot();
}
