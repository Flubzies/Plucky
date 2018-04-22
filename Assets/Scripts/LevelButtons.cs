using System.Collections;
using System.Collections.Generic;
using ManagerClasses;
using UnityEngine;
using VRTK;

public class LevelButtons : VRTK_InteractableObject
{
	[Header ("Button Properties")]
	[SerializeField] bool _buttonLeft;
	[SerializeField] bool _buttonRight;
	[SerializeField] bool _buttonRestart;
	[SerializeField] bool _buttonLoadLevel;
	[SerializeField] ControllerMenu _controllerMenu;

	public override void StartUsing (VRTK_InteractUse usingObject_)
	{
		base.StartUsing (usingObject);
		if (_buttonLeft) _controllerMenu.NextLevel ();
		if (_buttonRight) _controllerMenu.PrevLevel ();
		if (_buttonRestart) _controllerMenu.RestartLevel ();
		if (_buttonLoadLevel) _controllerMenu.LoadLevel ();
	}

}