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
    #region Shake
    public const float DAMAGE_SHAKE_DURATION = 0.2f;
    #endregion
    #region Barrel Bullet
    public const float BARREL_DURATION = 5f;
    #endregion
    #region Weapon Levels
    public const int WEAPON_MAX_LEVEL = 10;
    public const int WEAPON_BASE_XP_REQUIRED = 100;
    public const int WEAPON_XP_INCREASE_PER_LEVEL = 50;
    public const float WEAPON_DAMAGE_INCREASE_PER_LEVEL = 5f;
    public const float WEAPON_TIME_TO_SHOOT_DECREASE_PER_LEVEL = 0.02f;
    public const float WEAPON_MAX_DISTANCE_INCREASE_PER_LEVEL = 2f;
    public const int WEAPON_XP_PER_KILL = 25;
    #endregion
    #region Saves
    public const string SAVE_INTRO = "WeaponSaveType_";
    public const string SAVE_CURRENT_LEVEL = "_CurrentLevel";
    public const string SAVE_CURRENT_XP = "_CurrentXP";
    public const string SAVE_TOTAL_DAMAGE_UPGRADES = "_TotalDamageUpgrades";
    public const string SAVE_TOTAL_TIME_TO_SHOOT_UPGRADES = "_TotalTimeToShootUpgrades";
    public const string SAVE_TOTAL_MAX_DISTANCE_UPGRADES = "_TotalMaxDistanceUpgrades";
    #endregion
}