using System.Collections;
using System.Collections.Generic;
using ManagerClasses;
using Sirenix.OdinInspector;
using UnityEngine;
using VRTK;

public class LevelButtons : VRTK_InteractableObject
{
	[Header ("Button Properties")]
	[Space (10.0f)]
	[FoldoutGroup ("Level Menu")][SerializeField] bool _buttonLeft;
	[FoldoutGroup ("Level Menu")][SerializeField] bool _buttonRight;
	[FoldoutGroup ("Level Menu")][SerializeField] bool _buttonRestart;
	[FoldoutGroup ("Level Menu")][SerializeField] bool _buttonLoadLevel;
	[FoldoutGroup ("Level Menu")][SerializeField] ControllerMenu _controllerMenu;

	[FoldoutGroup ("Help Menu")][SerializeField] bool _buttonLeftHelpMenu;
	[FoldoutGroup ("Help Menu")][SerializeField] bool _buttonRightHelpMenu;
	[FoldoutGroup ("Help Menu")][SerializeField] ControllerHelpMenu _controllerHelpMenu;

	public override void StartUsing (VRTK_InteractUse usingObject_)
	{
		base.StartUsing (usingObject);

		if (_buttonLeft) _controllerMenu.NextLevel ();
		if (_buttonRight) _controllerMenu.PrevLevel ();
		if (_buttonRestart) _controllerMenu.RestartLevel ();
		if (_buttonLoadLevel) _controllerMenu.LoadLevel ();

		if (_buttonLeftHelpMenu) _controllerHelpMenu.LeftButton ();
		if (_buttonRightHelpMenu) _controllerHelpMenu.RightButton ();
	}
}