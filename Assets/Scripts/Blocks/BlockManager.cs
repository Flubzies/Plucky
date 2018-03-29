using System.Collections;
using ManagerClasses;
using UnityEditor;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	[SerializeField] LayerMask _blocksLM;
	bool _isHolding;
	IPlaceable _currentBlock;

	[Header ("VR Mode")]
	[SerializeField] Transform _playerHead;
	[SerializeField] bool _VRMode;

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

	private void Update ()
	{
		if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown (KeyCode.E))
		{
			if (!_isHolding) Grab ();
			else
			if (_isHolding) Place ();
		}
	}

	void Grab ()
	{
		Debug.Log ("Attempting to Grab");

		if (_VRMode)
		{
			ray.origin = _playerHead.position;
			ray.direction = _playerHead.forward;
		}
		else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
		{
			IPlaceable temp = hit.transform.GetComponent<IPlaceable> ();
			if (temp != null)
			{
				Debug.Log ("Grabbing.");
				_isHolding = true;
				_currentBlock = temp;
				_currentBlock.GetGhostBlock ().gameObject.SetActive (true);
				StartCoroutine (UpdateGhostBlock ());
			}
			else
				Debug.Log ("You cannot grab this!");
		}
	}

	IEnumerator UpdateGhostBlock ()
	{
		while (_isHolding)
		{
			if (_VRMode)
			{
				ray.origin = _playerHead.position;
				ray.direction = _playerHead.forward;
			}
			else ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
			{
				Vector3 newPos = hit.transform.position + hit.normal;
				_currentBlock.GetGhostBlock ().position = newPos;
			}

			yield return new WaitForSeconds (0.1f);
		}
	}

	void Place ()
	{
		Debug.Log ("Placing.");
		_isHolding = false;
		StopCoroutine (UpdateGhostBlock ());
		_currentBlock.PlaceBlock (_currentBlock.GetGhostBlock ().position);
		_currentBlock.GetGhostBlock ().gameObject.SetActive (false);
	}

}