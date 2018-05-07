using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public abstract class Block : MonoBehaviour, ILevelObject
	{
		[Header ("Block ")]
		[Tooltip ("If the Y rotation is not important for the block.")]
		[SerializeField] bool _randomizeYRotation;
		public BlockProperties _blockProperties;

		[ToggleGroup ("_isPlaceable", order : 0, groupTitle: "Placeable")][SerializeField] bool _isPlaceable;
		[ToggleGroup ("_isPlaceable")][SerializeField] BlockGhostMesh _ghostMesh;

		public BlockGhostMesh _BlockGhostMesh { get; private set; }
		public bool _IsBeingHeld { get; private set; }

		MeshRenderer _meshRenderer;
		MeshFilter _meshFilter;

		protected virtual void Awake ()
		{
			_meshRenderer = GetComponent<MeshRenderer> ();
			_meshFilter = GetComponent<MeshFilter> ();
		}

		public virtual void OnGrabbed ()
		{
			_IsBeingHeld = true;
			_BlockGhostMesh.MeshRenderer (true);
		}

		public virtual void OnPlaced (bool cancel_ = false)
		{
			_IsBeingHeld = false;
			_BlockGhostMesh.MeshRenderer (false);
		}

		public virtual bool BlockEffect (IBot bot_)
		{
			// Default returns false since no effect takes place.
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

		public virtual void DeathEffect (float deathEffectTime_)
		{
			if (Application.isPlaying)
			{
				Tweener t = transform.DOScale (Vector3.zero, deathEffectTime_);
				t.SetEase (_blockProperties._blockSpawnEaseType);
			}
			SafeDestroy.DestroyGameObject (this, deathEffectTime_);
		}

		public virtual void InitializeILevelObject (float spawnEffectTime_)
		{
			_blockProperties = Instantiate (_blockProperties);
			if (_randomizeYRotation) RandYRot ();
			if (_isPlaceable)
			{
				_BlockGhostMesh = Instantiate (_ghostMesh, transform.position, transform.rotation, transform);
				_BlockGhostMesh.SetupGhostMesh (_meshRenderer, _meshFilter, this);
			}

			transform.localScale = Vector3.zero;
			Tweener t = transform.DOScale (Vector3.one, spawnEffectTime_);
			t.SetEase (_blockProperties._blockSpawnEaseType);
		}

		public MeshRenderer GetMeshRenderer { get { return _meshRenderer; } }
		public MeshFilter GetMeshFilter { get { return _meshFilter; } }
	}
}