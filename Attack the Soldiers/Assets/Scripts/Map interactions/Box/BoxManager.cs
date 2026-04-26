using UnityEngine;
using Unity.Netcode;
//Class that manages the spawning of boxes in the game
public class BoxManager : NetworkBehaviour
{
    #region Serialized Variables
    [Header("General")]
    [SerializeField] private float timeToSpawnBox = 10;
    [SerializeField] private BulletInfoSO bulletInfoSO;
    [SerializeField] private BoxInfoSO boxInfoSO;
    #endregion
    #region Override
    [Header("Override")]
    [SerializeField] public bool overrideBoxType;
    [SerializeField] public BoxType boxTypeOverrideValue;
    [SerializeField] public bool overrideWeapon;
    [SerializeField] public Weapon weaponOverrideValue;
    #endregion
    #region Private Variables
    private BoxType boxType;
    private GameObject box;
    private float timer;
    private int totalTypeWeapons;
    private Weapon weaponType;
    private BoxInteractions boxInteractions;
    private bool hasStarted = false;
    #endregion
    #region Auxiliar Variables
    private BoxInfo boxInfo;
    #endregion
    #region Functions
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; return; }
    }
    public void Initialize()
    {
        timer = timeToSpawnBox;
        totalTypeWeapons = bulletInfoSO.BulletInfos.Count;
        hasStarted = true;
    }

    void Update()
    {
        if (!hasStarted) return;
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
        //boxInteractions.ownColor.Value = boxInfo.BoxColor;
        boxInteractions.BoxRenderer.sprite = boxInfo.BoxSprite;
        boxInteractions.SetIsUsed(false);
        //ChangeBoxColor();
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
        return GameNetwork.Instance.Spawn(Constants.NETWORK_OBJECT_POOL_BOX, Spawns.Instance.GetSpawnByType(SpawnType.Box).position, Quaternion.identity).gameObject;
    }

    Weapon GetRandomWeaponIfPossible()
    {
        if (overrideWeapon)
            return weaponOverrideValue;
        else
            // Exclude "Pistol", first weapon in enum
            return (Weapon)Random.Range(1, totalTypeWeapons);
    }

    void AddWeaponInfoToBoxIfNecessary()
    {
        if (boxType == BoxType.Weapon)
        {
            weaponType = GetRandomWeaponIfPossible();
            boxInteractions.SetWeaponType(weaponType);
            boxInteractions.SetWeaponBullets(bulletInfoSO.GetBulletInfoByWeapon(weaponType).TotalBullets);
        }
    }

    void ChangeBoxColor()
    {
        if (GameNetwork.Instance.IsOnline)
            boxInteractions.ChangeColorClientRpc(boxInfo.BoxColor);
        else
            boxInteractions.ChangeColor(boxInfo.BoxColor);
    }
    #endregion
}
