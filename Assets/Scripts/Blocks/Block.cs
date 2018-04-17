using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public abstract class Block : MonoBehaviour
	{
		[Header ("Block ")]
		[Tooltip("If the Y rotation is not important for the block.")]
		[SerializeField] bool _randomizeYRotation;
		public BlockProperties _blockProperties;
		protected Transform _blockGhost;

		protected virtual void Awake ()
		{
			transform.parent = GameObject.FindGameObjectWithTag ("BlockManager").transform;
			if (GetComponent<IPlaceable> () != null) SetupGhostMesh ();
		}

		protected virtual void Start ()
		{
			if (_randomizeYRotation) RandYRot ();
		}

		void SetupGhostMesh ()
		{
			_blockGhost = transform.Find ("GhostMesh");
			if (_blockGhost == null) Debug.LogError ("No Ghost Block on a IPlaceable GameObject!");
			_blockGhost.GetComponent<MeshFilter> ().mesh = GetComponent<MeshFilter> ().mesh;
			MeshRenderer mr = _blockGhost.GetComponent<MeshRenderer> ();

			List<Material> temp = new List<Material> ();
			for (int i = 0; i < GetComponent<MeshRenderer> ().materials.Length; i++)
				temp.Add (_blockGhost.GetComponent<MeshRenderer> ().material);

			mr.materials = temp.ToArray ();
		}

		public virtual bool BlockEffect (IBot bot_)
		{
			// Default no effect takes place.
			return false;
		}

		protected void RandYRot ()
		{
			int x = Random.Range (0, 4);
			transform.rotation = Quaternion.AngleAxis (x * 90, Vector3.up);
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

		public float GetInitialYRot()
		{
			return transform.localRotation.y;
		}

	}

	public interface IPlaceable
	{
		Transform GetGhostBlock { get; }
		void PlaceBlock (Vector3 pos_);
	}
}