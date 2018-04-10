using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlockClasses;
using DG.Tweening;
using ManagerClasses;
using UnityEngine;

public class Bot : MonoBehaviour, IBot
{
	[Header ("Bot")]
	[SerializeField] float _moveSpeed = 2.0f;

	[Space (10.0f)]
	[SerializeField] List<Transform> _collisionPositions;
	[SerializeField] Transform _currentGround;

	[Space (10.0f)]
	[SerializeField] LayerMask _blocksLM;

	[Space (10.0f)]
	[SerializeField] bool _debug;

	// Rigidbody _rb;
	Health _health;

	private void Awake ()
	{
		_health = GetComponent<Health> ();
		// _rb = GetComponent<Rigidbody> ();
	}

	private void Start ()
	{
		_health.DeathEvent += OnDeath;
		StartMoving ();
	}

	void StartMoving ()
	{
		StartCoroutine (Checks ());
	}

	IEnumerator Checks ()
	{
		if (CheckForBlockEffects ())
		{
			yield return new WaitForSeconds (_moveSpeed);
			CheckColliders ();
		}
		else CheckColliders ();
	}

	private bool CheckForBlockEffects ()
	{
		Collider[] cols = Physics.OverlapSphere (_currentGround.position, 0.2f, _blocksLM);
		if (cols.Length != 0)
		{
			if (cols[0].GetComponent<Block> ().BlockEffect (GetComponent<IBot> ())) return true;
			else return false;
		}
		return false;
	}

	private void CheckColliders ()
	{
		int config = 0;

		for (int i = 0; i < _collisionPositions.Count; i++)
		{
			Collider[] cols = Physics.OverlapSphere (_collisionPositions[i].position, 0.2f, _blocksLM);
			if (cols.Length != 0) config += (int) Math.Pow (2, i);
		}

		ActOnColliders (config);
	}

	void ActOnColliders (int configuration_)
	{
		if (_debug) Debug.Log (configuration_);
		switch (configuration_)
		{
			case 1:
			case 9:
			case 17:
			case 25:
				Drop ();
				break;
			case 2:
			case 3:
			case 10:
			case 11:
			case 18:
			case 19:
			case 26:
			case 28:
				Move ();
				break;
			case 4:
			case 5:
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
		Tweener t = transform.DORotateQuaternion (transform.rotation * Quaternion.Euler (0, 180, 0), _moveSpeed);
		t.OnComplete (StartMoving);
	}

	void Climb ()
	{
		Vector3[] vecPath = new Vector3[2];
		vecPath[0] = transform.up + transform.position;
		vecPath[1] = transform.forward + vecPath[0];

		Tweener t = transform.DOPath (vecPath, _moveSpeed);
		t.OnComplete (StartMoving);
	}

	void Move ()
	{
		Tweener t = transform.DOMove (transform.position + transform.forward, _moveSpeed);
		t.OnComplete (StartMoving);
	}

	void Drop ()
	{
		Vector3[] vecPath = new Vector3[2];
		vecPath[0] = transform.forward + transform.position;
		vecPath[1] = transform.up * -1 + vecPath[0];

		Tweener t = transform.DOPath (vecPath, _moveSpeed);
		t.OnComplete (StartMoving);
	}

	void OnDeath ()
	{
		gameObject.SetActive (false);
	}

	public void BotRotation (Quaternion rot_)
	{
		transform.DORotateQuaternion (rot_, _moveSpeed);
	}

	public void BotDestination (Vector3 dest_)
	{
		transform.DOMove (dest_, _moveSpeed);
	}

	public float GetMoveSpeed { get { return _moveSpeed; } }
	public Transform GetTransform { get { return transform; } }
	public Health GetHealth { get { return _health; } }

}

public interface IBot
{
	Transform GetTransform { get; }
	Health GetHealth { get; }
	float GetMoveSpeed { get; }
	void BotRotation (Quaternion rot_);
	void BotDestination (Vector3 dest_);
}

// Use this if we want to revert to the original 2 unit Bot.
// void ActOnColliders (int configuration_)
// {
//     if (_debug) Debug.Log (configuration_);
//     switch (configuration_)
//     {
//         case 1:
//         case 2:
//         case 3:
//         case 17:
//         case 18:
//         case 19:
//         case 33:
//         case 34:
//         case 50:
//         case 51:
//             Move ();
//             break;
//         case 4:
//         case 5:
//         case 6:
//         case 7:
//             Climb ();
//             break;
//         default:
//             Turn ();
//             break;
//     }
// }