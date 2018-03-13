using System.Collections;
using UnityEngine;

public class BlockBuildManager : MonoBehaviour
{
	[SerializeField] LayerMask _blocksLM;
	bool _isHolding;
	IPlaceable _currentBlock;

	static BlockBuildManager _instance;
	public static BlockBuildManager instance
	{
		get
		{
			if (!_instance)
				_instance = FindObjectOfType<BlockBuildManager> ();
			return _instance;
		}
	}

	private void Update ()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			if (!_isHolding) Grab ();
			else
			if (_isHolding) Place ();
		}
	}

	void Grab ()
	{
		Debug.Log ("Grabbing.");

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f, _blocksLM))
		{
			IPlaceable temp = hit.transform.GetComponent<IPlaceable> ();
			if (temp != null)
			{
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
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
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