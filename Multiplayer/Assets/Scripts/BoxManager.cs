using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BoxManager : NetworkBehaviour
{
    [SerializeField] private float timeToSpawnBox = 10;
    
    //pistol, shotgun, machine gun, sniper
    [SerializeField] private int[] weaponBullets;
    //health, Speed, Weapon
    [SerializeField] private Color[] boxColors;
    int boxIndex;
    private GameObject box;
    float timer;
    BoxInteractions boxInteractions;

    private int totalTypeBoxes;
    private int totalTypeWeapons;
    int weaponIndex;

    void Start()
    {
        timer = timeToSpawnBox;
        totalTypeBoxes = boxColors.Length;
        totalTypeWeapons = weaponBullets.Length;
        SpawnRandomBox();
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
        boxIndex = Random.Range(0, totalTypeBoxes);
        box = NetworkObjectPool.Singleton.GetNetworkObject("Box", Spawns.Instance.GetBoxSpawnPoint().position, Quaternion.identity).gameObject;
        boxInteractions = box.GetComponent<BoxInteractions>();
        boxInteractions.SetUpgradeIndex(boxIndex);
        if (boxIndex == 2)
        {
            weaponIndex = Random.Range(1, totalTypeWeapons);
            boxInteractions.SetWeaponIndex(weaponIndex);
            boxInteractions.SetWeaponBullets(weaponBullets[weaponIndex]);
        }
        boxInteractions.ownColor.Value = boxColors[boxIndex];
        boxInteractions.SetDestroyed(false);
        boxInteractions.ChangeColorClientRpc(boxColors[boxIndex]);
    }
}
