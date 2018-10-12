#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

public class SDKSetupsTransPosEditor : MonoBehaviour
{
	[ToggleGroup ("_offsetPosition", order : 0, groupTitle: "Offset Position ")]
	[SerializeField] bool _offsetPosition;
	[ToggleGroup ("_offsetPosition", order : 0, groupTitle: "Offset Position ")]
	[SerializeField] Vector3 _newPositions;
	[ToggleGroup ("_offsetPosition", order : 0, groupTitle: "Offset Position ")]
	[SerializeField] Vector3 _newRotation;

	private void OnValidate ()
	{
		if (_offsetPosition)
		{
			transform.position = _newPositions;
			transform.rotation = Quaternion.Euler (_newRotation);
		}
		else
		{
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}

}
#endif