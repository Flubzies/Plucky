using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailReset : MonoBehaviour
{
	[SerializeField] Animator _animator;
	[SerializeField] float _minSpeed, _maxSpeed;

	private void Start ()
	{
		_animator.speed = Random.Range (_minSpeed, _maxSpeed);
	}

}