using System.Collections;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockManager : MonoBehaviour
	{
		[SerializeField] LayerMask _blocksLM;
		public bool _IsHolding { get; private set; }
		IPlaceable _currentBlock;

		[Header ("VR Mode")]
		[SerializeField] Transform _raycaster;
		[SerializeField] bool _VRMode;
		VRTK_ControllerEvents _controllerEvents;

		Ray ray = new Ray ();

		static BlockManager _instance;
		public static BlockManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<BlockManager> ();
				return _instance;
			}
		}

		private void Awake ()
		{
			_controllerEvents = _raycaster.GetComponent<VRTK_ControllerEvents> ();
		}

		private void Start ()
		{
			_controllerEvents.TriggerPressed += new ControllerInteractionEventHandler (ControllerGrab);
			_controllerEvents.TriggerReleased += new ControllerInteractionEventHandler (ControllerPlace);
			_controllerEvents.GripPressed += new ControllerInteractionEventHandler (ControllerCancel);
		}

		private void Update ()
		{
			if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown (KeyCode.E))
			{
				if (!_IsHolding) Grab ();
				else
				if (_IsHolding) Place ();
			}
		}

		void ControllerGrab (object sender, ControllerInteractionEventArgs e)
		{
			if (!_IsHolding) Grab ();
		}

		void ControllerPlace (object sender, ControllerInteractionEventArgs e)
		{
			if (_IsHolding) Place ();
		}

		void ControllerCancel (object sender, ControllerInteractionEventArgs e)
		{
			if (_IsHolding) Cancel ();
		}

		void Grab ()
		{
			Debug.Log ("Attempting to Grab");

			if (_VRMode)
			{
				ray.origin = _raycaster.position;
				ray.direction = _raycaster.forward;
			}
			else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
			{
				IPlaceable temp = hit.transform.GetComponent<IPlaceable> ();
				if (temp != null)
				{
					Debug.Log ("Grabbing.");
					_IsHolding = true;
					_currentBlock = temp;
					_currentBlock.GetGhostBlock.gameObject.SetActive (true);
					StartCoroutine (UpdateGhostBlock ());
				}
				else
					Debug.Log ("You cannot grab this!");
			}
		}

		IEnumerator UpdateGhostBlock ()
		{
			while (_IsHolding)
			{
				if (_VRMode)
				{
					ray.origin = _raycaster.position;
					ray.direction = _raycaster.forward;
				}
				else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
				{
					Vector3 newPos = hit.transform.position + hit.normal;
					_currentBlock.GetGhostBlock.position = newPos;
				}

				yield return new WaitForSeconds (0.1f);
			}
		}

		void Place ()
		{
			Debug.Log ("Placing.");
			_IsHolding = false;
			StopCoroutine (UpdateGhostBlock ());
			_currentBlock.PlaceBlock (_currentBlock.GetGhostBlock.position);
			_currentBlock.GetGhostBlock.gameObject.SetActive (false);
		}

		void Cancel ()
		{
			Debug.Log ("Cancelling");
			_IsHolding = false;
			StopCoroutine (UpdateGhostBlock ());
			_currentBlock.GetGhostBlock.position = transform.position;
			_currentBlock.GetGhostBlock.gameObject.SetActive (false);
		}

	}
}