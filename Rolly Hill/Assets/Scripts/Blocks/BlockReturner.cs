using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReturner : MonoBehaviour
{
    public void ReturnAllBlocksToThePool()
    {
        GameObject[] blocksInGame = GameObject.FindGameObjectsWithTag("Block");
        BlockDrop blockDropped;
        foreach (GameObject block in blocksInGame)
        {
            blockDropped = block.GetComponent<BlockDrop>();
            blockDropped.ResetLayer();
            BlockPool.Instance.ReturnObjectToPool(block);
            block.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            block.transform.localScale = Vector3.one * 0.3f;
            blockDropped.RestartDropValues();
        }
    }
}
