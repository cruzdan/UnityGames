using UnityEngine;
using Unity.Netcode;
//Class that manages the spawning of boxes in the game
public class BoxManager : NetworkBehaviour
{
    #region Serialized Variables
    [Header("General")]
    [SerializeField] private bool isOffline = false;
    [SerializeField] private float timeToSpawnBox = 10;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private BoxInfoSO boxInfoSO;
    #endregion
    #region Override
    [Header("Override")]
    [SerializeField] private bool overrideBoxType;
    [SerializeField] private BoxType boxTypeOverrideValue;
    [SerializeField] private bool overrideWeapon;
    [SerializeField] private Weapon weaponOverrideValue;
    #endregion
    #region Private Variables
    private BoxType boxType;
    private GameObject box;
    private float timer;
    private int totalTypeWeapons;
    private Weapon weaponType;
    private BoxInteractions boxInteractions;
    #endregion
    #region Auxiliar Variables
    private BoxInfo boxInfo;
    #endregion
    #region Functions
    public void Initialize()
    {
        timer = timeToSpawnBox;
        totalTypeWeapons = bulletInfoSO.BulletInfos.Count;
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        else
        {
            timer = timeToSpawnBox;
            SpawnRandomBox();
        }
    }

    void SpawnRandomBox()
    {
        boxType = GetRandomBoxTypeIfPossible();
        box = GetRandomBoxObjectIfPossible();
        boxInteractions = box.GetComponent<BoxInteractions>();
        boxInteractions.SetBoxType(boxType);
        AddWeaponInfoToBoxIfNecessary();
        boxInfo = boxInfoSO.GetBoxInfo(boxType);
        boxInteractions.ownColor.Value = boxInfo.BoxColor;
        boxInteractions.SetIsUsed(false);
        ChangeBoxColor();
    }

    BoxType GetRandomBoxTypeIfPossible()
    {
        if (overrideBoxType)
            return boxTypeOverrideValue;
        else
            return (BoxType)Random.Range(0, boxInfoSO.BoxInfos.Count);
    }

    GameObject GetRandomBoxObjectIfPossible()
    {
        if (isOffline)
            return ObjectPool.Singleton.GetObject("Offline Box", Spawns.Instance.GetBoxSpawnPoint().position, Quaternion.identity);
        else
            return NetworkObjectPool.Singleton.GetNetworkObject("Box", Spawns.Instance.GetBoxSpawnPoint().position, Quaternion.identity).gameObject;
    }

    Weapon GetRandomWeaponIfPossible()
    {
        if (overrideWeapon)
            return weaponOverrideValue;
        else
            return (Weapon)Random.Range(1, totalTypeWeapons);
    }

    void AddWeaponInfoToBoxIfNecessary()
    {
        if (boxType == BoxType.Weapon)
        {
            weaponType = GetRandomWeaponIfPossible();
            boxInteractions.SetWeaponIndex(weaponType);
            boxInteractions.SetWeaponBullets(bulletInfoSO.GetBulletInfoByWeapon(weaponType).TotalBullets);
        }
    }

    void ChangeBoxColor()
    {
        if (!isOffline)
            boxInteractions.ChangeColorClientRpc(boxInfo.BoxColor);
        else
            boxInteractions.ChangeColor(boxInfo.BoxColor);
    }
    #endregion
}
