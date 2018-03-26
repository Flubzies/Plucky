using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SkinnedMeshRenderer))]
public class SMRAnimation : MonoBehaviour
{

	[SerializeField] float _startTime;
	[SerializeField] float _endTime;
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
		float t = 1f;
		while (t > 0f)
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
		float t = 0.0f;
		while (t < 1f)
		{
			t += Time.deltaTime * _animSpeed;
			float a = _animCurve.Evaluate (t);
			_smr.SetBlendShapeWeight (0, a * 100f);
			yield return 0;
		}
		StartCoroutine (AnimStart ());
	}

}