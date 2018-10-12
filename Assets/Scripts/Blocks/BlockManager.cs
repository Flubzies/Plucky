using System.Collections;
using System.Collections.Generic;
using ManagerClasses;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockManager : MonoBehaviour
	{
		[SerializeField] LayerMask _blocksLM;
		Vector3[] _adjacentPositions = new Vector3[6];
		List<Block> _checkingBlocks = new List<Block> ();
		int _blocksCheckedCount;

		static BlockManager _instance;
		public static BlockManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<BlockManager> ();
				return _instance;
			}
		}

		public Vector3[] GetAdjacentPositions (Transform trans_)
		{
			Vector3[] adj = new Vector3[6];
			adj[0] = (trans_.position + trans_.up);
			adj[1] = (trans_.position + trans_.up * -1);
			adj[2] = (trans_.position + trans_.right * -1);
			adj[3] = (trans_.position + trans_.right);
			adj[4] = (trans_.position + trans_.forward);
			adj[5] = (trans_.position + trans_.forward * -1);
			return adj;
		}

		public void CheckForRoot ()
		{
			List<Block> placeableBlocks = LevelManager.instance._PlaceableBlocks;
			foreach (Block placeableBlock in placeableBlocks)
			{
				_checkingBlocks.Clear ();
				_blocksCheckedCount = 0;
				_checkingBlocks.Add (placeableBlock);
				FindRoot (_checkingBlocks);
			}
		}

		void FindRoot (List<Block> blocksAdjSpaces_)
		{
			_blocksCheckedCount = _checkingBlocks.Count;

			Collider[] cols;
			Block blockNullCheck;

			for (int i = 0; i < blocksAdjSpaces_.Count; i++)
			{
				_adjacentPositions = GetAdjacentPositions (blocksAdjSpaces_[i].transform);
				blockNullCheck = null;

				for (int j = 0; j < _adjacentPositions.Length; j++)
				{
					cols = Physics.OverlapSphere (_adjacentPositions[j], 0.2f, _blocksLM);
					foreach (Collider col in cols)
						if (col != null) blockNullCheck = col.GetComponent<Block> ();
					if (blockNullCheck != null && !_checkingBlocks.Contains (blockNullCheck)) _checkingBlocks.Add (blockNullCheck);
				}
			}

			if (!AreAdjacentBlocksConnectedToRoot (_checkingBlocks))
			{
				if (_checkingBlocks.Count != _blocksCheckedCount)
					FindRoot (_checkingBlocks);
				else DestroyBlocks ();
			}
		}

		void DestroyBlocks ()
		{
			for (int i = _checkingBlocks.Count - 1; i >= 0; i--) _checkingBlocks[i].DeathEffect (0.8f);
			_checkingBlocks.Clear ();
		}

		bool AreAdjacentBlocksConnectedToRoot (List<Block> blocksToCheck_)
		{
			foreach (Block block in blocksToCheck_)
			{
				if (!block.IsPlaceable)
					return true;
			}
			return false;
		}
	}
}