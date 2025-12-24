public interface IPlayerInputSource
{
    float GetHorizontalMovement();
    float GetVerticalMovement();
    bool GetShoot();
    bool GetRun();
    bool GetJump();
}
