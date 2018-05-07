using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
	public class BlockGhostMesh : MonoBehaviour
	{
		MeshRenderer _mr;
		MeshFilter _mf;
		public BlockCollisions _BlockCollisions { get; private set; }
		public Block _Block { get; private set; }

		void Awake ()
		{
			_mr = GetComponent<MeshRenderer> ();
			_mf = GetComponent<MeshFilter> ();
			_BlockCollisions = GetComponent<BlockCollisions> ();
			_mr.enabled = false;
		}

		public void SetupGhostMesh (MeshRenderer mr_, MeshFilter mf_, Block block_)
		{
			_mf.mesh = mf_.mesh;

			List<Material> temp = new List<Material> ();
			for (int i = 0; i < mr_.materials.Length; i++) temp.Add (_mr.material);
			_mr.materials = temp.ToArray ();

			_Block = block_;
		}

		public void MeshRenderer (bool _enable)
		{
			_mr.enabled = _enable;
		}

	}
}