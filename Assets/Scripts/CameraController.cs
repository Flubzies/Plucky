using UnityEngine;

// PC Debugging class.
public class CameraController : MonoBehaviour
{
	[SerializeField] float _rotSpeed = 1.5f;
	[SerializeField] float _panSpeed = 1.0f;
	[SerializeField] float _zoomSensitivity = 10f;
	float _tempFloat = 0.0f;

	void Update ()
	{

		if (Input.GetKey (KeyCode.LeftShift))
		{
			_tempFloat = Input.GetAxisRaw ("Horizontal");
			if (Mathf.Abs (_tempFloat) > 0.1) transform.Translate (Vector3.right * _tempFloat * _panSpeed * Time.deltaTime);
			_tempFloat = Input.GetAxisRaw ("Vertical");
			if (Mathf.Abs (_tempFloat) > 0.1) transform.Translate (Vector3.up * _tempFloat * _panSpeed * Time.deltaTime);
			return;
		}

		_tempFloat = Input.GetAxisRaw ("Horizontal");
		if (Mathf.Abs (_tempFloat) > 0.1)
			transform.Rotate (0.0f, -_tempFloat * _rotSpeed, 0.0f, Space.World);

		_tempFloat = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs (_tempFloat) > 0.1)
			transform.Rotate (_tempFloat * _rotSpeed, 0.0f, -_tempFloat * _rotSpeed, Space.Self);

		// Zooming. Doesn't work with OpenVR
		_tempFloat = Camera.main.fieldOfView;
		_tempFloat += Input.GetAxis ("Mouse ScrollWheel") * _zoomSensitivity * -1;
		_tempFloat = Mathf.Clamp (_tempFloat, 15, 90);
		Camera.main.fieldOfView = _tempFloat;
	}
}