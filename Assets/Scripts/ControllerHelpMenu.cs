using System.Collections;
using System.Collections.Generic;
using BlockClasses;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using VRTK;

public class ControllerHelpMenu : MonoBehaviour
{
	[SerializeField] Transform _menuHelpObject;
	[SerializeField] TextMeshPro _titleText;
	[SerializeField] TextMeshPro _descriptionField;
	[SerializeField] Transform _viveTooltipL;
	[SerializeField] Transform _viveTooltipR;
	[SerializeField] Transform _prefabHolder;

	[SerializeField] List<HelpMenuDescriptions> _descriptions;

	VRTK_ControllerEvents _controllerEvents;

	int _indexTracker;
	bool _canOpenMenu = true;
	bool _menuOpen;

	private void Awake ()
	{
		_controllerEvents = GetComponent<VRTK_ControllerEvents> ();
	}

	void Start ()
	{
		_controllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler (Menu);
		_descriptionField.text = _descriptions[_indexTracker]._description;
		_menuHelpObject.gameObject.SetActive (false);
		_viveTooltipL.gameObject.SetActive (false);
		_viveTooltipR.gameObject.SetActive (false);
	}

	private void Update ()
	{
		if (_menuOpen)
		{
			_menuHelpObject.position = transform.position;
			_menuHelpObject.rotation = transform.rotation;
		}
	}

	private void Menu (object sender, ControllerInteractionEventArgs e)
	{
		if (_menuOpen) DoMenuOff ();
		if (!_canOpenMenu) return;

		_menuOpen = true;
		_canOpenMenu = false;

		TweenIn (_menuHelpObject);
		TweenIn (_viveTooltipL);
		Tweener t = TweenIn (_viveTooltipR);
		t.OnComplete (DoMenuOn);
	}

	Tweener TweenIn (Transform trans_, Ease ease_ = Ease.InBounce)
	{
		trans_.gameObject.SetActive (true);
		trans_.localScale = Vector3.zero;
		Tweener t = trans_.DOScale (Vector3.one, 0.5f);
		t.SetEase (ease_);
		return t;
	}

	Tweener TweenOut (Transform trans_, Ease ease_ = Ease.InBounce)
	{
		Tweener t = trans_.DOScale (Vector3.zero, 0.5f);
		t.SetEase (ease_);
		return t;
	}

	void DoMenuOn ()
	{
		SetupIndex ();
	}

	private void DoMenuOff ()
	{
		TweenOut (_menuHelpObject);
		TweenOut (_viveTooltipL);
		Tweener t = TweenOut (_viveTooltipR);
		t.OnComplete (MenuOff);
	}

	void MenuOff ()
	{
		_menuHelpObject.gameObject.SetActive (false);
		_viveTooltipL.gameObject.SetActive (false);
		_viveTooltipR.gameObject.SetActive (false);

		_canOpenMenu = true;
		_menuOpen = false;
	}

	public void LeftButton ()
	{
		if (_indexTracker + 1 < _descriptions.Count)
		{
			_indexTracker++;
			SetupIndex ();
		}
	}

	public void RightButton ()
	{
		if (_indexTracker - 1 >= 0)
		{
			_indexTracker--;
			SetupIndex ();
		}
	}

	void SetupIndex ()
	{
		foreach (Transform child in _prefabHolder)
			if (child != null) Destroy (child.gameObject);

		Instantiate (_descriptions[_indexTracker]._previewPrefab, _prefabHolder.position, _prefabHolder.rotation, _prefabHolder);
		CleanPrefab (_prefabHolder);
		_titleText.text = _descriptions[_indexTracker]._title.ToUpper ();
		_descriptionField.text = _descriptions[_indexTracker]._description;
	}

	private void OnDestroy ()
	{
		_controllerEvents.ButtonTwoPressed -= new ControllerInteractionEventHandler (Menu);
	}

	void CleanPrefab (Transform trans_)
	{
		LayerMask l = transform.gameObject.layer;
		DeleteComponentsInChild<TrailRenderer> (_prefabHolder);
		DeleteComponentsInChild<Rigidbody> (_prefabHolder);
		DeleteComponentsInChild<Collider> (_prefabHolder);
		DeleteComponentsInChild<Block> (_prefabHolder);
		DeleteComponentsInChild<Bot> (_prefabHolder);
	}

	void DeleteComponentsInChild<T> (Transform trans_) where T : Component
	{
		Component[] comps = trans_.GetComponentsInChildren<T> ();
		foreach (T comp in comps)
			if (comp) Destroy (comp);
	}

#if UNITY_EDITOR
	[Button]
	private void CheckCharacterCount ()
	{
		foreach (HelpMenuDescriptions item in _descriptions)
			Debug.Log (item._title + "  " + item._description.Length);
	}
#endif
}

[System.Serializable]
class HelpMenuDescriptions
{
	public string _title = null;
	public Transform _previewPrefab = null;
	[MultiLineProperty] public string _description = null;
}