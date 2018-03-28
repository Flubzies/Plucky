using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockExit : Block
{
    [SerializeField] LayerMask _blocksLM;

    new void Start () { }

    public override bool BlockEffect (IBotMovement bot_)
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