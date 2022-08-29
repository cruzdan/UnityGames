using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Transform cam;
    public bool isRestarting = false;
    public void Restart()
    {
        player.GetComponent<PlayerMovement>().Restart();
        player.transform.localEulerAngles = new Vector3(0, 0, 0);
        cam.position = new Vector3(0, 3.85f, -3f);
        DeleteObjectsWithTag("Enemy");
        DeleteObjectsWithTag("Wall");
        DeleteObjectsWithTag("Floor");
        DeleteObjectsWithTag("Spawner");
        DeleteObjectsWithTag("Trap");
        mapManager.GenerateMapLevel();
    }

    void DeleteObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }
}
