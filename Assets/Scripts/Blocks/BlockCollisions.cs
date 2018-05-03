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

		/// <summary>
		/// Returns true if any blocks are adjacent to it.
		/// </summary>
		/// <param name="changeMaterialColor_">Changes the material color if it has an adjacent block or not.</param>
		/// <param name="posNoCheck_">Doesn't check for collisions against this position.</param>
		/// <returns>Returns true if any blocks are adjacent to it.</returns>
		public bool HasAdjacentBlock (bool changeMaterialColor_ = false, Vector3? posNoCheck_ = null)
		{
			_adjacentPositions = BlockManager.instance.GetAdjacentPositions (transform);
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
	}
}