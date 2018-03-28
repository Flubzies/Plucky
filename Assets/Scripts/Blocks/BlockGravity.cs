using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGravity : Block
{
    // [SerializeField] string _defaultParentTag = "BlockBuildsHolder";

    [SerializeField] List<Vector3> _colliders;
    [SerializeField] LayerMask _blocksLM;

    [SerializeField] int _maxHeight = 3;

    public override bool BlockEffect (IBotMovement bot_)
    {
        bot_.BotDestination (transform.position + transform.up * _maxHeight);
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