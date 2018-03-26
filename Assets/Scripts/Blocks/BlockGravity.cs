using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGravity : Block
{
    // [SerializeField] string _defaultParentTag = "BlockBuildsHolder";

    [SerializeField] List<Vector3> _colliders;
    [SerializeField] LayerMask _blocksLM;

    [SerializeField] int _maxHeight = 3;
    [SerializeField] int _currHeightPos = 0;
    Vector3 _currMoveDir = Vector3.up;

    public override bool BlockEffect (Transform bot_)
    {
        if (_currHeightPos == _maxHeight) _currMoveDir = Vector3.down;
        else
        if (_currHeightPos == 0) _currMoveDir = Vector3.up;

        MoveDirection (bot_, _currMoveDir);
        return true;
    }

    void MoveDirection (Transform bot_, Vector3 vec_)
    {
        if (vec_ == Vector3.up)
        {
            Collider[] cols = Physics.OverlapSphere (bot_.position + Vector3.up, 0.2f, _blocksLM);
            if (cols.Length != 0) MoveDirection (bot_, Vector3.down);
            _currHeightPos++;
        }
        else if (vec_ == Vector3.down)
        {
            Collider[] cols = Physics.OverlapSphere (transform.position + Vector3.down, 0.2f, _blocksLM);
            if (cols.Length != 0) MoveDirection (bot_, Vector3.up);
            _currHeightPos--;
        }

        transform.Translate (vec_);
        bot_.Translate (vec_);
    }

    public Transform GetGhostBlock ()
    {
        return _blockGhost;
    }

    public void PlaceBlock (Vector3 pos_)
    {
        transform.position = pos_;
    }

    private void OnValidate ()
    {
        if (_currHeightPos > _maxHeight) _currHeightPos = _maxHeight;
    }

}