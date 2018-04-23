using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockVRTKInteract : VRTK_InteractableObject
	{
		Block _block;
		bool _isBeingUsed;

		protected override void Awake ()
		{
			base.Awake ();
			_block = GetComponentInParent<Block> ();
		}

		public override void StartTouching (VRTK_InteractTouch currentTouchingObject = null)
		{
			base.StartTouching ();
			VRTKBlockInteraction.instance._currentlyTouching = _block;
		}

		public override void StartUsing (VRTK_InteractUse usingObject_)
		{
			Debug.Log ("Started Using");
			base.StartUsing ();
			// _isBeingUsed = true;
			// _block.OnStartUsingBlock ();
			// StartCoroutine (UpdateGhostMesh (usingObject_.transform));
		}

		public override void StopUsing (VRTK_InteractUse usingObject_)
		{
			Debug.Log ("Stopped Using");
			base.StopUsing ();
			// _isBeingUsed = false;
			// _block.OnStopUsingBlock ();
		}

		// IEnumerator UpdateGhostMesh (Transform usingObject_)
		// {
		// 	while (_isBeingUsed)
		// 	{
		// 		Debug.Log ("Is Using");
		// 		_block._BlockGhostMesh.transform.position = usingObject_.position.ToInt ();
		// 		yield return new WaitForSeconds (0.5f);
		// 	}
		// }
	}
}