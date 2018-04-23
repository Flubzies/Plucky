using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public abstract class Block : MonoBehaviour, ILevelObject
	{
		[Header ("Block ")]
		[Tooltip ("If the Y rotation is not important for the block.")]
		[SerializeField] bool _randomizeYRotation;

		[Header ("Placeable")]
		[SerializeField] bool _isPlaceable;
		[Tooltip ("Only required if the block is placeable.")]
		[SerializeField] BlockGhostMesh _ghostMesh;

		public BlockGhostMesh _BlockGhostMesh { get; private set; }

		public BlockProperties _blockProperties;
		ScalingAnimation _scalingAnimation;

		MeshRenderer _meshRenderer;
		MeshFilter _meshFilter;

		protected virtual void Awake ()
		{
			_scalingAnimation = GetComponent<ScalingAnimation> ();
			_meshRenderer = GetComponent<MeshRenderer> ();
			_meshFilter = GetComponent<MeshFilter> ();
		}

		public virtual void OnGrabbed ()
		{
			_BlockGhostMesh.MeshRenderer (true);
		}

		public virtual void OnPlaced (bool cancel_ = false)
		{
			_BlockGhostMesh.MeshRenderer (false);
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

		public float GetInitialYRot ()
		{
			return transform.localRotation.eulerAngles.y;
		}

		public Transform GetTransform { get { return transform; } }
		public BlockType GetBlockType { get { return _blockProperties._blockType; } }
		public bool IsPlaceable { get { return _isPlaceable; } set { _isPlaceable = value; } }

		public void DeathEffect ()
		{
			if (Application.isPlaying) _scalingAnimation.DeathEffect ();
			SafeDestroy.DestroyGameObject (this, 2.0f);
		}

		public virtual void InitializeILevelObject ()
		{
			_blockProperties = Instantiate (_blockProperties);
			if (_randomizeYRotation) RandYRot ();
			if (_isPlaceable)
			{
				_BlockGhostMesh = Instantiate (_ghostMesh, transform.position, transform.rotation, transform);
				_BlockGhostMesh.SetupGhostMesh (_meshRenderer, _meshFilter);
			}
		}

		public MeshRenderer GetMeshRenderer { get { return _meshRenderer; } }
		public MeshFilter GetMeshFilter { get { return _meshFilter; } }
	}
}