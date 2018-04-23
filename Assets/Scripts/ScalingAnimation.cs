using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAnimation : MonoBehaviour
{
	[SerializeField] float _animSpeed = 1.0f;
	[SerializeField] AnimationCurve _curve;

	[Space (10.0f)]
	[SerializeField] bool _disableRB;
	[SerializeField] Rigidbody _rb;

	void Awake ()
	{
		if (_disableRB) _rb.isKinematic = true;
		StartCoroutine (ScaleUp ());
	}

	public void DeathEffect ()
	{
		StartCoroutine (ScaleDown ());
	}

	IEnumerator ScaleUp ()
	{
		float curveTime = 0f;
		float curveAmount = _curve.Evaluate (curveTime);

		transform.localScale = Vector3.zero;

		while (curveAmount < 1.0f)
		{
			curveTime += Time.deltaTime * _animSpeed;
			curveAmount = _curve.Evaluate (curveTime);
			transform.localScale = Vector3.one * curveAmount;
			yield return null;
		}

		transform.localScale = Vector3.one;
		if (_disableRB) _rb.isKinematic = false;
	}

	IEnumerator ScaleDown ()
	{
		float curveTime = 0.0f;
		float curveAmount = _curve.Evaluate (curveTime);
		transform.localScale = Vector3.one;

		while (curveAmount < 1.0f)
		{
			curveTime += Time.deltaTime * _animSpeed;
			curveAmount = _curve.Evaluate (curveTime);
			transform.localScale = Vector3.one * Mathf.Abs (curveAmount - 1.01f);
			yield return null;
		}

		transform.localScale = Vector3.one * 0.001f;
	}

}