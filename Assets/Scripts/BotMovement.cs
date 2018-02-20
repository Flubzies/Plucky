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
	float _damping = 50.0f;

	private void Start ()
	{
		StartCoroutine (MoveTo ());
	}

	IEnumerator MoveTo ()
	{
		while (!_moving)
		{
			CheckColliders ();
			yield return new WaitForSeconds (1.0f);
		}
	}

	private void CheckColliders ()
	{
		_moving = true;
		Collider[] cols = Physics.OverlapSphere (_colliders[0].position, 0.2f, _cubeLM);
		if (cols.Length == 0)
		{
			Turn ();
			return;
		}
		else
		{
			int[] configuration = new int[4];
			for (int i = 1; i < _colliders.Count; i++)
			{
				cols = Physics.OverlapSphere (_colliders[i].position, 0.2f, _cubeLM);
				if (cols.Length == 0) configuration[i - 1] = (i + 4);
				else configuration[i - 1] = i;
			}
			ActOnColliders (String.Join ("", configuration.Select (p => p.ToString ()).ToArray ()));
		}
	}

	void ActOnColliders (string configuration_)
	{
		switch (configuration_)
		{
			case "5678":
			case "5674":
			case "5638":
			case "5634":
				Move ();
				break;
			case "1678":
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