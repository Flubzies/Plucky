using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
	[SerializeField] protected Transform _blockGhost;

	protected void Start ()
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

	public virtual bool BlockEffect (IBotMovement bot_)
	{
		return false;
	}

	protected void RandomXRotation ()
	{
		int x = Random.Range (0, 3);
		transform.rotation = Quaternion.AngleAxis (x * 90, Vector3.up);
	}

}

public interface IPlaceable
{
	Transform GetGhostBlock ();
	void PlaceBlock (Vector3 pos_);
}