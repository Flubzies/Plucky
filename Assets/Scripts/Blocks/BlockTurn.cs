using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTurn : Block, IPlaceable
{
	[Space (1.0f)]

	[SerializeField] Transform _blockGhost;
	[SerializeField] LayerMask _blocksLM;

	public override bool BlockEffect (Transform bot_)
	{
		bot_.RotateTowardsVector (transform.forward);
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