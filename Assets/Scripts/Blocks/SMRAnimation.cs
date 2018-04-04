using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMRAnimation : MonoBehaviour
{

	[SerializeField] float _startTime = 0.0f;
	[SerializeField] float _endTime = 1.0f;
	[SerializeField] float _animSpeed = 5.0f;
	[SerializeField] AnimationCurve _animCurve;
	SkinnedMeshRenderer _smr;

	private void Awake ()
	{
		_smr = GetComponent<SkinnedMeshRenderer> ();
	}

	private void Start ()
	{
		StartCoroutine (AnimStart ());
	}

	IEnumerator AnimStart ()
	{
		float t = _endTime;
		while (t > _startTime)
		{
			t -= Time.deltaTime * _animSpeed;
			float a = _animCurve.Evaluate (t);
			_smr.SetBlendShapeWeight (0, a * 100f);
			yield return 0;
		}
		StartCoroutine (AnimStop ());
	}

	IEnumerator AnimStop ()
	{
		float t = _startTime;
		while (t < _endTime)
		{
			t += Time.deltaTime * _animSpeed;
			float a = _animCurve.Evaluate (t);
			_smr.SetBlendShapeWeight (0, a * 100f);
			yield return 0;
		}
		StartCoroutine (AnimStart ());
	}

	private void OnValidate ()
	{
		if (_startTime > _endTime) _startTime = _endTime;
	}

}