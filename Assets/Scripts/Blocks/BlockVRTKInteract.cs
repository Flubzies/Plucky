using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockVRTKInteract : VRTK_InteractableObject
	{
		public override void StartUsing (VRTK_InteractUse usingObject)
		{
			base.StartUsing (usingObject);
			if (BlockManager.instance._IsHolding) return;
			else BlockManager.instance.
		}

		public override void StopUsing (VRTK_InteractUse usingObject)
		{
			base.StopUsing (usingObject);

		}
	}
}