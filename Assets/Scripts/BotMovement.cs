using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
	[SerializeField] List<Transform> _colliders;
	[SerializeField] LayerMask _cubeLM;
	bool _moving;

	private void Start ()
	{
		StartCoroutine (MoveTo ());
	}

	IEnumerator MoveTo ()
	{
		while (!_moving)
		{
			CheckColliders ();
			yield return new WaitForSeconds (2.0f);
		}
	}

	private void CheckColliders ()
	{
		_moving = true;
		int config = 0;

		for (int i = 0; i < _colliders.Count; i++)
		{
			Collider[] cols = Physics.OverlapSphere (_colliders[i].position, 0.2f, _cubeLM);
			if (cols.Length != 0) config += (int) Math.Pow (2, i);
		}

		ActOnColliders (config);
	}

	void ActOnColliders (int configuration_)
	{
		Debug.Log (configuration_);
		switch (configuration_)
		{
			case 1:
			case 2:
			case 3:
			case 18:
			case 19:
			case 34:
			case 50:
			case 51:
				Move ();
				break;
			case 6:
			case 7:
				Climb ();
				break;
			default:
				Turn ();
				break;
		}
	}

	void Turn ()
	{
		Debug.Log ("Turning.");

		Quaternion newRot = transform.rotation * Quaternion.Euler (0.0f, 90.0f, 0.0f);
		transform.rotation = newRot;
		_moving = false;
	}

	void Climb ()
	{
		Debug.Log ("Climbing.");
		transform.Translate (Vector3.forward + Vector3.up);
		_moving = false;
	}

	void Move ()
	{
		Debug.Log ("Moving.");
		transform.Translate (Vector3.forward);
		_moving = false;
	}

}