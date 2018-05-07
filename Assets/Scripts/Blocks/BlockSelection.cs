using System.Collections;
using UnityEngine;

namespace BlockClasses
{
	public class BlockSelection : MonoBehaviour
	{
		public Block _CurrentlySelectedBlock { get; private set; }
		BlockGhostMesh _blockGhost;

		[SerializeField] Transform _selectionBlock;
		MeshFilter _selectionBlockMF;

		bool _deselecting;

		private void Awake ()
		{
			_selectionBlock.gameObject.SetActive (false);
			_selectionBlockMF = _selectionBlock.GetComponent<MeshFilter> ();
		}

		void SelectBlock (Block block_)
		{
			_selectionBlock.gameObject.SetActive (true);
			_CurrentlySelectedBlock = block_;
			_selectionBlock.transform.rotation = _CurrentlySelectedBlock.transform.rotation;
			_selectionBlockMF.mesh = _CurrentlySelectedBlock.GetMeshFilter.mesh;
			_selectionBlock.position = _CurrentlySelectedBlock.GetTransform.position;
		}

		private void OnTriggerStay (Collider other_)
		{
			if (_CurrentlySelectedBlock != null && _CurrentlySelectedBlock._IsBeingHeld) return;
			_blockGhost = other_.GetComponent<BlockGhostMesh> ();
			if (_blockGhost) SelectBlock (_blockGhost._Block);
		}

		private void OnTriggerExit (Collider other_)
		{
			if (!_deselecting) StartCoroutine (DeselectBlock ());
		}

		IEnumerator DeselectBlock ()
		{
			_deselecting = true;

			while (_CurrentlySelectedBlock != null && _CurrentlySelectedBlock._IsBeingHeld)
				yield return null;

			_CurrentlySelectedBlock = null;
			_selectionBlock.gameObject.SetActive (false);
			_deselecting = false;
		}
	}
}