public interface IPlayerInputSource
{
    float GetHorizontalMovement();
    float GetVerticalMovement();
    bool GetCrouch();
    bool GetRun();
    bool GetStopRun();
    bool GetJump();
}
