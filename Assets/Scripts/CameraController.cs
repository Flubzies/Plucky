using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float _rotSpeed = 1.5f;
	void Update ()
	{
		float horzInput = Input.GetAxisRaw ("Horizontal");
		if (Mathf.Abs (horzInput) > 0.1)
		{
			Quaternion newRot = transform.rotation * Quaternion.Euler (0.0f, -horzInput * _rotSpeed, 0.0f);
			transform.rotation = newRot;
		}
	}
}