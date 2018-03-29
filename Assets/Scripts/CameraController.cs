using UnityEngine;

// PC Debugging class.
public class CameraController : MonoBehaviour
{
	[SerializeField] float _rotSpeed = 1.5f;
	[SerializeField] float _panSpeed = 1.0f;
	[SerializeField] float _zoomSensitivity = 10f;

	void Update ()
	{

		if (Input.GetKey (KeyCode.LeftShift))
		{
			float input = Input.GetAxisRaw ("Vertical");
			if (Mathf.Abs (input) > 0.1)
				transform.Translate (Vector3.up * input * _panSpeed * Time.deltaTime);
			return;
		}

		float horzInput = Input.GetAxisRaw ("Horizontal");
		if (Mathf.Abs (horzInput) > 0.1)
			transform.Rotate (0.0f, -horzInput * _rotSpeed, 0.0f, Space.World);

		float verInput = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs (verInput) > 0.1)
			transform.Rotate (verInput * _rotSpeed, 0.0f, -verInput * _rotSpeed, Space.Self);

		// Zooming. Doesn't work with OpenVR
		float fov = Camera.main.fieldOfView;
		fov += Input.GetAxis ("Mouse ScrollWheel") * _zoomSensitivity * -1;
		fov = Mathf.Clamp (fov, 15, 90);
		Camera.main.fieldOfView = fov;
	}
}