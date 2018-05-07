using System;
using UnityEngine;

namespace BlockClasses
{
	public class BlockSelection : MonoBehaviour
	{
		public Block _CurrentlySelectedBlock { get; private set; }
		BlockGhostMesh _blockGhost;

		[SerializeField] Transform _selectionBlock;
		MeshFilter _selectionBlockMF;

		private void Awake ()
		{
			_selectionBlock.gameObject.SetActive (false);
		}

		void SelectBlock (Block _block)
		{
			_selectionBlock.gameObject.SetActive (true);
			_selectionBlockMF.mesh = _CurrentlySelectedBlock.GetMeshFilter.mesh;
			_selectionBlock.position = _CurrentlySelectedBlock.GetTransform.position;
		}

		private void OnTriggerEnter (Collider other)
		{
			_blockGhost = other.GetComponent<BlockGhostMesh> ();
			if (_blockGhost) SelectBlock (_blockGhost._Block);
		}

		private void OnTriggerExit (Collider other)
		{
			DeselectBlock ();
		}

		private void DeselectBlock ()
		{
			_CurrentlySelectedBlock = null;
			_selectionBlock.gameObject.SetActive (false);
		}

		private void OnValidate ()
		{
			_selectionBlockMF = _selectionBlock.GetComponent<MeshFilter> ();
		}

	}
}