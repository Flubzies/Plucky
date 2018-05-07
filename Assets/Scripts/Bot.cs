using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlockClasses;
using DG.Tweening;
using ManagerClasses;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bot : SerializedMonoBehaviour, IBot, ILevelObject
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

	[FoldoutGroup ("Tween Ease Settings")][SerializeField] Ease _moveEase;
	[FoldoutGroup ("Tween Ease Settings")][SerializeField] Ease _rotEase;
	[FoldoutGroup ("Tween Ease Settings")][SerializeField] Ease _climbEase;
	[FoldoutGroup ("Tween Ease Settings")][SerializeField] Ease _dropEase;
	[FoldoutGroup ("Tween Ease Settings")][SerializeField] Ease _spawnEase;

	Health _health;
	Collider[] _colliders = new Collider[10];
	bool _initialized;

	private void Awake ()
	{
		_health = GetComponent<Health> ();
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
		int numCols = Physics.OverlapSphereNonAlloc (_currentGround.position, 0.2f, _colliders, _blocksLM);
		if (numCols != 0)
		{
			if (_colliders[0].GetComponent<Block> ().BlockEffect (GetComponent<IBot> ())) return true;
			else return false;
		}
		return false;
	}

	private void CheckColliders ()
	{
		int config = 0;

		for (int i = 0; i < _collisionPositions.Count; i++)
		{
			int numCols = Physics.OverlapSphereNonAlloc (_collisionPositions[i].position, 0.2f, _colliders, _blocksLM);
			if (numCols != 0) config += (int) Math.Pow (2, i);
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
		t.SetEase (_climbEase);
		t.OnComplete (StartMoving);
	}

	void Move ()
	{
		Tweener t = transform.DOMove (transform.position + transform.forward, _moveSpeed);
		t.SetEase (_moveEase);
		t.OnComplete (StartMoving);
	}

	void Drop ()
	{
		Vector3[] vecPath = new Vector3[2];
		vecPath[0] = transform.forward + transform.position;
		vecPath[1] = transform.up * -1 + vecPath[0];

		Tweener t = transform.DOPath (vecPath, _moveSpeed);
		t.SetEase (_dropEase);
		t.OnComplete (StartMoving);
	}

	void OnDeath ()
	{
		_health.DeathEvent -= OnDeath;
	}

	public void BotRotation (Quaternion rot_)
	{
		Tweener t = transform.DORotateQuaternion (rot_, _moveSpeed);
		t.SetEase (_rotEase);
	}

	public void BotDestination (Vector3 dest_)
	{
		Tweener t = transform.DOMove (dest_, _moveSpeed);
		t.SetEase (_moveEase);
	}

	public Transform GetTransform { get { return transform; } }
	public Health GetHealth { get { return _health; } }
	public float GetMoveSpeed { get { return _moveSpeed; } }
	public BlockType GetBlockType { get { return BlockType.Undefined; } }
	public bool IsPlaceable { get { return false; } set { return; } }

	public void InitializeILevelObject (float spawnEffectTime_)
	{
		_health.DeathEvent += OnDeath;
		transform.localScale = Vector3.zero;
		Tweener t = transform.DOScale (Vector3.one, spawnEffectTime_);
		t.SetEase (_spawnEase);
		StartMoving ();
	}

	public void DeathEffect (float deathEffectTime_)
	{
		if (Application.isPlaying)
		{
			Tweener t = transform.DOScale (Vector3.zero, deathEffectTime_);
			t.SetEase (_spawnEase);
			OnDeath ();
		}
		SafeDestroy.DestroyGameObject (this, 2.0f);
	}

}

public interface IBot
{
	void BotRotation (Quaternion rot_);
	void BotDestination (Vector3 dest_);
	Transform GetTransform { get; }
	Health GetHealth { get; }
	float GetMoveSpeed { get; }
	void DeathEffect (float deathEffectTime_);
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