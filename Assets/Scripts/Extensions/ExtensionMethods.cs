using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I am reusing most of this script from GHAP 2017-2018.
public static class ExtensionMethods
{
	// I have several situations where I need to rotate towards a vector.
	// So I made a quick Extenstion method to handle it for any Transform.
	public static void RotateTowardsVector (this Transform trans_, Vector2 vel_, float rotSpeed_ = 4.0f, float angleOffset_ = 0.0f)
	{
		float angle = Mathf.Atan2 (vel_.y, vel_.x) * Mathf.Rad2Deg;
		trans_.rotation = Quaternion.Lerp (trans_.rotation, Quaternion.AngleAxis (angle + angleOffset_, Vector3.forward), Time.deltaTime * rotSpeed_);
	}

	public static Vector3 ToInt (this Vector3 vec_)
	{
		vec_.x = Mathf.RoundToInt (vec_.x);
		vec_.y = Mathf.RoundToInt (vec_.y);
		vec_.z = Mathf.RoundToInt (vec_.z);
		return vec_;
	}

	// I just use this really often so an Extension here seems reasonable.
	public static T RandomFromList<T> (this List<T> list_)
	{
		return list_[UnityEngine.Random.Range (0, list_.Count)];
	}

	public static T FindComponent<T> (this GameObject go_)
	{
		if (go_.GetComponent<T> () != null) return go_.GetComponent<T> ();
		else
		{
			Debug.LogError ("Compoenent of Type " + typeof (T).GetType ().ToString () + " not found! ");
			return default (T);
		}
	}
}