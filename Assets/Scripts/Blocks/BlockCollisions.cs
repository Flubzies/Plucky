using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
	public class BlockCollisions : MonoBehaviour
	{
		[SerializeField] LayerMask _blocksLM;
		[SerializeField] Color _acceptablePosCol;
		[SerializeField] Color _unacceptablePosCol;

		Collider[] _colliders = new Collider[10];
		Vector3[] _adjacentPositions = new Vector3[6];

		Material[] _materials;
		MeshRenderer _mr;

		void Awake ()
		{
			_mr = GetComponent<MeshRenderer> ();
			_materials = _mr.materials;
		}

		void UpdateAdjacentPositions ()
		{
			_adjacentPositions[0] = (transform.position + transform.up);
			_adjacentPositions[1] = (transform.position + transform.up * -1);
			_adjacentPositions[2] = (transform.position + transform.right * -1);
			_adjacentPositions[3] = (transform.position + transform.right);
			_adjacentPositions[4] = (transform.position + transform.forward);
			_adjacentPositions[5] = (transform.position + transform.forward * -1);
		}

		/// <summary>
		/// Returns true if any blocks are adjacent to it.
		/// </summary>
		/// <param name="changeMaterialColor_">Changes the material color if it has an adjacent block or not.</param>
		/// <param name="posNoCheck_">Doesn't check for collisions against this position.</param>
		/// <returns>Returns true if any blocks are adjacent to it.</returns>
		public bool HasAdjacentBlock (bool changeMaterialColor_ = false, Vector3? posNoCheck_ = null)
		{
			UpdateAdjacentPositions ();
			int numCols = 0;

			for (int i = 0; i < _adjacentPositions.Length; i++)
			{
				if (posNoCheck_.HasValue && _adjacentPositions[i] == posNoCheck_) continue;
				numCols += Physics.OverlapSphereNonAlloc (_adjacentPositions[i], 0.2f, _colliders, _blocksLM);
			}

			if (numCols == 0)
			{
				if (changeMaterialColor_) ChangeMaterialColor (false);
				return false;
			}

			if (changeMaterialColor_) ChangeMaterialColor (true);
			return true;
		}

		void ChangeMaterialColor (bool isAcceptable_)
		{
			if (isAcceptable_)
				foreach (Material mat in _materials) mat.color = _acceptablePosCol;
			else
				foreach (Material mat in _materials) mat.color = _unacceptablePosCol;
		}

		List<Block> GetAdjacentBlocks ()
		{
			UpdateAdjacentPositions ();

			Block tempBlock = null;
			List<Block> tempColliderList = new List<Block> ();
			Collider[] cols;

			for (int i = 0; i < _adjacentPositions.Length; i++)
			{
				cols = Physics.OverlapSphere (_adjacentPositions[i], 0.2f, _blocksLM);
				tempBlock = cols[0].GetComponent<Block> ();
				if (!tempColliderList.Contains (tempBlock)) tempColliderList.Add (tempBlock);
			}

			return tempColliderList;
		}

		bool AreAdjacentBlocksConnectedToRoot (List<Block> adjacentBlocks_)
		{
			foreach (Block block in adjacentBlocks_)
			{
				if (!block.IsPlaceable) return true;
			}
			return false;
		}

		// tempBlock = col.GetComponent<Block> ();
		// if (tempBlock != null && !tempBlock.IsPlaceable)
		// {
		// 	return true;
		// }
	}
}