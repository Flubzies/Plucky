using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VRTK;

public class ControllerPointerSwitcher : SerializedMonoBehaviour
{
	[SerializeField] VRTK_Pointer _pointer;
	[SerializeField] VRTK_StraightPointerRenderer _pointerRenderer;
	VRTK_ControllerEvents _controllerEvents;
	VRTKBlockInteraction _blockInteraction;
	VRTK_InteractUse _interactUse;
	VRTK_InteractTouch _interactTouch;

	[FoldoutGroup ("Menu Settings")][SerializeField] Color _validColorMenu;
	[FoldoutGroup ("Menu Settings")][SerializeField] Color _invalidColorMenu;
	[FoldoutGroup ("Teleport Settings")][SerializeField] Color _validColorTeleport;
	[FoldoutGroup ("Teleport Settings")][SerializeField] Color _invalidColorTeleport;

	private void Awake ()
	{
		_controllerEvents = GetComponent<VRTK_ControllerEvents> ();
		_blockInteraction = GetComponent<VRTKBlockInteraction> ();
		_interactUse = GetComponent<VRTK_InteractUse> ();
		_interactTouch = GetComponent<VRTK_InteractTouch> ();
	}

	private void Start ()
	{
		_controllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler (CheckTouchpad);
		_controllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler (TouchEnd);
	}

	void PointerTeleportSettings ()
	{
		_pointer.enableTeleport = true;
		_interactUse.enabled = false;
		_interactTouch.enabled = false;
		_pointerRenderer.validCollisionColor = _validColorTeleport;
		_pointerRenderer.invalidCollisionColor = _invalidColorTeleport;
	}

	void PointerMenuSelectSettings ()
	{
		_pointer.enableTeleport = false;
		_interactUse.enabled = true;
		_interactTouch.enabled = true;
		_pointerRenderer.validCollisionColor = _validColorMenu;
		_pointerRenderer.invalidCollisionColor = _invalidColorMenu;
	}

	void CheckTouchpad (object sender, ControllerInteractionEventArgs e)
	{
		if (!_blockInteraction._IsHolding)
			if (e.touchpadAxis.y > 0) PointerTeleportSettings ();
			else PointerMenuSelectSettings ();
	}

	void TouchEnd (object sender, ControllerInteractionEventArgs e)
	{
		_interactUse.enabled = true;
		_interactTouch.enabled = true;
	}

	private void OnDestroy ()
	{
		_controllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler (CheckTouchpad);
	}

}