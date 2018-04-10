using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTurnWind : MonoBehaviour
{
	ParticleSystem _particleSystem;
	Animator[] _animators;

	[SerializeField] float _minSpeed;
	[SerializeField] float _maxSpeed;

	private void Awake ()
	{
		_particleSystem = GetComponent<ParticleSystem> ();
		_animators = GetComponentsInChildren<Animator> ();
	}

	private void Start ()
	{
		_particleSystem.Play ();
		foreach (Animator anim in _animators)
			anim.speed = Random.Range (_minSpeed, _maxSpeed);
	}

}