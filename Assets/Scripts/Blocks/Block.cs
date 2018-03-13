using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
	protected static List<Vector3> GetAdjacentSpaces (Transform vec_)
	{
		List<Vector3> VecList = new List<Vector3> ();
		VecList.Add (vec_.up);
		VecList.Add (vec_.right * -1);
		VecList.Add (vec_.right);
		VecList.Add (vec_.forward);
		VecList.Add (vec_.forward * -1);
		return VecList;
	}

	public virtual bool BlockEffect (Transform bot_)
	{
		Debug.Log ("No Effect");
		return false;
	}
}

public interface IPlaceable
{
	Transform GetGhostBlock ();
	void PlaceBlock (Vector3 pos_);
}