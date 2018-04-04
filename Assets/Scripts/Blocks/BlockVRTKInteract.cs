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

		private void Start ()
		{

		}

		public override void StartUsing (VRTK_InteractUse usingObject)
		{
			base.StartUsing (usingObject);
			Debug.Log ("Using");

			// if (BlockManager.instance._IsHolding) return;
			// else BlockManager.instance.Grab (_block);
		}

		public override void StopUsing (VRTK_InteractUse usingObject)
		{
			base.StopUsing (usingObject);
			
		}

		// public override void Ungrabbed (VRTK_InteractGrab previousGrabbingObject)
		// {
		// 	Debug.Log("Ungrabbed");
		// }

		// IEnumerator UpdateGhostBlock ()
		// {
		// 	while (_IsHolding)
		// 	{
		// 		_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		// 		RaycastHit hit;
		// 		if (Physics.Raycast (_ray, out hit, 100f, _blocksLM))
		// 		{
		// 			Vector3 newPos = hit.transform.position + hit.normal;
		// 			_currentBlock.GetGhostBlock.position = newPos;
		// 		}

		// 		yield return new WaitForSeconds (0.1f);
		// 	}
		// }
	}
}