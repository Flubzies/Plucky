using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockExit : Block
{
    [SerializeField] Transform _blockGhost;
    [SerializeField] LayerMask _blocksLM;

    public override bool BlockEffect (Transform bot_)
    {
        Debug.Log ("Level Complete!");
        return true;
    }

    public Transform GetGhostBlock ()
    {
        return _blockGhost;
    }

    public void PlaceBlock (Vector3 pos_)
    {
        transform.position = pos_;
    }

}