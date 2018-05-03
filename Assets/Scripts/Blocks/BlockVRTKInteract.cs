using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockVRTKInteract : VRTK_InteractableObject
	{
		Block _block;

		protected override void Awake ()
		{
			base.Awake ();
			_block = GetComponentInParent<Block> ();
		}

		public override void OnInteractableObjectTouched (InteractableObjectEventArgs e)
		{
			VRTKBlockInteraction.instance._currentlyTouching = _block;
		}

		public override void StartUsing (VRTK_InteractUse usingObject_)
		{
			base.StartUsing ();
			// _isBeingUsed = true;
			// _block.OnStartUsingBlock ();
			// StartCoroutine (UpdateGhostMesh (usingObject_.transform));
		}

		public override void StopUsing (VRTK_InteractUse usingObject_)
		{
			base.StopUsing ();
			// _isBeingUsed = false;
			// _block.OnStopUsingBlock ();
		}
	}
}