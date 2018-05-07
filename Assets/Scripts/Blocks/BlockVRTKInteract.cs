// using System.Collections;
// using UnityEngine;
// using VRTK;

// namespace BlockClasses
// {
// 	public class BlockVRTKInteract : VRTK_InteractableObject
// 	{
// 		Block _block;
// 		VRTKBlockInteraction _tempBlockInteraction;

// 		protected override void Awake ()
// 		{
// 			base.Awake ();
// 			_block = GetComponentInParent<Block> ();
// 		}

// 		public override void OnInteractableObjectTouched (InteractableObjectEventArgs e)
// 		{
// 			_tempBlockInteraction = e.interactingObject.GetComponent<VRTKBlockInteraction> ();
// 			if (_tempBlockInteraction) _tempBlockInteraction._currentlyTouching = _block;
// 		}

// 		public override void OnInteractableObjectUntouched (InteractableObjectEventArgs e)
// 		{
// 		}

// 		public override void StartUsing (VRTK_InteractUse usingObject_)
// 		{
// 			base.StartUsing ();
// 		}

// 		public override void StopUsing (VRTK_InteractUse usingObject_)
// 		{
// 			base.StopUsing ();
// 		}
// 	}
// }