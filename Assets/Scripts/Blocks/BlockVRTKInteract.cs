using UnityEngine;
using VRTK;

namespace BlockClasses
{
	[RequireComponent (typeof (IPlaceable))]
	public class BlockVRTKInteract : VRTK_InteractableObject
	{
		protected IPlaceable _block;

		protected override void Awake ()
		{
			base.Awake ();
			_block = transform.GetComponent<IPlaceable> ();
		}

		public override void StartUsing (VRTK_InteractUse usingObject)
		{
			base.StartUsing ();
			
		}

		public override void StopUsing (VRTK_InteractUse usingObject)
		{
			base.StopUsing ();

		}
	}
}