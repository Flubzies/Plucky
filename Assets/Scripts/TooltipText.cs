using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TooltipText : MonoBehaviour
{
	[Header ("Fields")]
	[SerializeField] TextMeshPro _buttonTwoField;
	[SerializeField] TextMeshPro _touchpadField;
	[SerializeField] TextMeshPro _triggerField;
	[SerializeField] TextMeshPro _gripField;

	[Header ("Strings")]
	[MultiLineProperty][SerializeField] string _buttonTwoText;
	[MultiLineProperty][SerializeField] string _touchpadText;
	[MultiLineProperty][SerializeField] string _triggerText;
	[MultiLineProperty][SerializeField] string _gripText;

	private void OnValidate ()
	{
		_buttonTwoField.text = _buttonTwoText;
		_touchpadField.text = _touchpadText;
		_triggerField.text = _triggerText;
		_gripField.text = _gripText;
	}

}