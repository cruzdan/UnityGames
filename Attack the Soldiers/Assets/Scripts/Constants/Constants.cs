using System.Collections.Generic;

public class Constants
{
    #region Scenes
    public static string OFFLINE_SCENE_PREFIX = "Offline Map ";
    public static string ONLINE_SCENE_PREFIX = "TitleScene";
    #endregion
    #region Input Keys
    public static string INPUT_KEY_RUN = "Run";
    public static string INPUT_KEY_RIGHT = "Right";
    public static string INPUT_KEY_LEFT = "Left";
    public static string INPUT_KEY_PAUSE = "Pause";
    public static string INPUT_KEY_JUMP = "Jump";
    public static string INPUT_KEY_SHOOT = "Shoot";
    #endregion
    #region Pool IDs
    public static string NETWORK_OBJECT_POOL_BULLET = "Bullet";
    public static string NETWORK_OBJECT_POOL_BOX = "Box";
    public static string NETWORK_OBJECT_POOL_FLAME = "Flame";
    public static string NETWORK_OBJECT_POOL_BARREL = "Barrel";
    #endregion
    #region Commands
    public const string COMMAND_CHEATS = "cheats";
    #endregion
    #region Enemy
    public static List<string> ENEMY_NAMES = new List<string> { "BarrelThrower", "Faster", "FlameThrower", "GunMan", "Knockout" };
    #endregion
}
