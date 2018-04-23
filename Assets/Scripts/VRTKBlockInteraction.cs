using System.Collections;
using BlockClasses;
using UnityEngine;
using VRTK;

public class VRTKBlockInteraction : MonoBehaviour
{
	static VRTKBlockInteraction _instance;
	public static VRTKBlockInteraction instance
	{
		get
		{
			if (!_instance)
				_instance = FindObjectOfType<VRTKBlockInteraction> ();
			return _instance;
		}
	}

	[SerializeField] LayerMask _blocksLM;
	public bool _IsHolding { get; private set; }
	Block _currentBlock;

	VRTK_ControllerEvents _controllerEventsR;
	Collider[] _colliders = new Collider[10];

	[HideInInspector] public Block _currentlyTouching;

	private void Awake ()
	{
		_controllerEventsR = GetComponent<VRTK_ControllerEvents> ();
	}

	private void Start ()
	{
		_controllerEventsR.TriggerPressed += new ControllerInteractionEventHandler (ControllerGrab);
		_controllerEventsR.TriggerReleased += new ControllerInteractionEventHandler (ControllerPlace);
		_controllerEventsR.GripPressed += new ControllerInteractionEventHandler (ControllerCancel);
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

		if (_currentlyTouching != null)
		{
			_IsHolding = true;
			_currentBlock = _currentlyTouching;
			_currentBlock.OnStartUsingBlock ();
			StartCoroutine (UpdateGhostBlock ());
		}
		else Debug.Log ("Unable to grab!");
	}

	IEnumerator UpdateGhostBlock ()
	{
		while (_IsHolding)
		{
			Vector3 pos = transform.position.ToInt ();
			int numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _blocksLM);
			if (numCols == 0) _currentBlock._BlockGhostMesh.transform.position = pos;
			yield return new WaitForSeconds (0.5f);
		}
	}

	void Place ()
	{
		Debug.Log ("Placing.");
		_IsHolding = false;
		_currentBlock.OnStopUsingBlock ();
		_currentBlock.transform.position = _currentBlock._BlockGhostMesh.transform.position;
		_currentlyTouching = null;
	}

	void Cancel ()
	{
		Debug.Log ("Cancelling");
		_IsHolding = false;
		_currentBlock._BlockGhostMesh.transform.position = transform.position;
		_currentlyTouching = null;
	}

}

// [SerializeField] LayerMask _blocksLM;
// public bool _IsHolding { get; private set; }
// Block _currentBlock;

// [Header ("VR Mode")]
// [SerializeField] Transform _controller;
// [SerializeField] Transform _leftController;
// [SerializeField] bool _VRMode;

// VRTK_ControllerEvents _controllerEventsR;

// Ray ray = new Ray ();

// static VRTKBlockInteraction _instance;
// public static VRTKBlockInteraction instance
// {
// 	get
// 	{
// 		if (!_instance)
// 			_instance = FindObjectOfType<VRTKBlockInteraction> ();
// 		return _instance;
// 	}
// }

// private void Awake ()
// {
// 	_controllerEventsR = _controller.GetComponent<VRTK_ControllerEvents> ();
// }

// private void Start ()
// {
// 	_controllerEventsR.TriggerPressed += new ControllerInteractionEventHandler (ControllerGrab);
// 	_controllerEventsR.TriggerReleased += new ControllerInteractionEventHandler (ControllerPlace);
// 	_controllerEventsR.GripPressed += new ControllerInteractionEventHandler (ControllerCancel);
// }

// private void Update ()
// {
// 	if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown (KeyCode.E))
// 	{
// 		if (!_IsHolding) Grab ();
// 		else
// 		if (_IsHolding) Place ();
// 	}
// }

// void ControllerGrab (object sender, ControllerInteractionEventArgs e)
// {
// 	if (!_IsHolding) Grab ();
// }

// void ControllerPlace (object sender, ControllerInteractionEventArgs e)
// {
// 	if (_IsHolding) Place ();
// }

// void ControllerCancel (object sender, ControllerInteractionEventArgs e)
// {
// 	if (_IsHolding) Cancel ();
// }

// void Grab ()
// {
// 	Debug.Log ("Attempting to Grab");

// 	if (_VRMode)
// 	{
// 		ray.origin = _controller.position;
// 		ray.direction = _controller.forward;
// 	}
// 	else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

// 	RaycastHit hit;

// 	if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
// 	{
// 		Block block = hit.transform.GetComponent<Block> ();
// 		if (block != null)
// 		{
// 			Debug.Log ("Grabbing.");
// 			_IsHolding = true;
// 			_currentBlock = block;
// 			_currentBlock.GetGhostBlock.gameObject.SetActive (true);
// 			StartCoroutine (UpdateGhostBlock ());
// 		}
// 		else
// 			Debug.Log ("You cannot grab this!");
// 	}
// }

// IEnumerator UpdateGhostBlock ()
// {
// 	while (_IsHolding)
// 	{
// 		if (_VRMode)
// 		{
// 			ray.origin = _controller.position;
// 			ray.direction = _controller.forward;
// 		}
// 		else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

// 		RaycastHit hit;

// 		if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
// 		{
// 			Vector3 newPos = hit.transform.position + hit.normal;
// 			_currentBlock.GetGhostBlock.position = newPos;
// 		}

// 		yield return new WaitForSeconds (0.1f);
// 	}
// }

// void Place ()
// {
// 	Debug.Log ("Placing.");
// 	_IsHolding = false;
// 	StopCoroutine (UpdateGhostBlock ());
// 	_currentBlock.PlaceBlock (_currentBlock.GetGhostBlock.position);
// 	_currentBlock.GetGhostBlock.gameObject.SetActive (false);
// }

// void Cancel ()
// {
// 	Debug.Log ("Cancelling");
// 	_IsHolding = false;
// 	StopCoroutine (UpdateGhostBlock ());
// 	_currentBlock.GetGhostBlock.position = transform.position;
// 	_currentBlock.GetGhostBlock.gameObject.SetActive (false);
// }