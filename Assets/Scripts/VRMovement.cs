using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovement : MonoBehaviour
{
	[SerializeField] float _translateSpeed = 1.5f;

	void Update ()
	{
		float verInput = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs (verInput) > 0.1)
		{
			transform.Translate (Vector3.up * verInput * _translateSpeed);
		}
	}
}