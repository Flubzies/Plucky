using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
public abstract class Block : MonoBehaviour
{
	[SerializeField] protected Transform _blockGhost;

	private void Start ()
	{
		SetupGhostMesh ();
	}

	void SetupGhostMesh ()
	{
		_blockGhost.GetComponent<MeshFilter> ().mesh = GetComponent<MeshFilter> ().mesh;
	}

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
		return false;
	}
}

public interface IPlaceable
{
	Transform GetGhostBlock ();
	void PlaceBlock (Vector3 pos_);
}