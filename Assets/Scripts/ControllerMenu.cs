using System.Collections.Generic;
using BlockClasses;
using DG.Tweening;
using ManagerClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ControllerMenu : MonoBehaviour
{
	[SerializeField] Transform _menuObject;
	[SerializeField] TextMeshPro _levelNameText;

	[Header ("Level Preview ")]
	[SerializeField] Transform _previewHolder;
	[SerializeField] Transform _previewCube;
	[Tooltip ("Color of each preview block, corresponds with BlockType enums")]
	[SerializeField] Color[] _previewColorArray;
	[SerializeField] float _cubeScale = 0.1f;
	[SerializeField] float _positionOffset = 0.1f;
	[SerializeField] float _previewCubeAlphaValue = 0.9f;
	bool _canOpenMenu = true;
	bool _menuOpen;
	VRTK_ControllerEvents _controllerEvents;

	int _levelIndex;
	List<string> _savedLevels;

	private void Awake ()
	{
		_controllerEvents = GetComponent<VRTK_ControllerEvents> ();
	}

	private void Start ()
	{
		_controllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler (Menu);
		_savedLevels = LevelLayoutManager.instance.GetSavedLevels ();
		_menuObject.gameObject.SetActive (false);
	}

	private void Update ()
	{
		if (_menuOpen)
		{
			_menuObject.position = transform.position;
			_menuObject.rotation = transform.rotation;
		}
	}

	private void Menu (object sender, ControllerInteractionEventArgs e)
	{
		if (_menuOpen) DoMenuOff ();
		if (!_canOpenMenu) return;

		_menuOpen = true;
		_canOpenMenu = false;
		_menuObject.gameObject.SetActive (true);

		_menuObject.localScale = Vector3.zero;
		Tweener t = _menuObject.DOScale (Vector3.one, 0.5f);
		t.SetEase (Ease.InBounce);
		t.OnComplete (OnMenuOpen);
		UpdateIndex ();
		DestroyPreviewCubes ();

		_levelNameText.text = LevelLayoutManager.instance._CurrentLevelName;
	}

	void OnMenuOpen ()
	{
		GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelNameText.text));
	}

	private void UpdateIndex ()
	{
		for (int i = 0; i < _savedLevels.Count; i++)
			if (_levelNameText.text == _savedLevels[i])
			{
				_levelIndex = i;
				break;
			}
		else _levelIndex = 0;
	}

	private void DoMenuOff ()
	{
		Tweener t = _menuObject.DOScale (Vector3.zero, 0.5f);
		t.SetEase (Ease.InBounce);
		t.OnComplete (MenuOff);
	}

	void MenuOff ()
	{
		_menuObject.gameObject.SetActive (false);
		_canOpenMenu = true;
		_menuOpen = false;
	}

	public void NextLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex + 1;
		if (temp < _savedLevels.Count && _savedLevels[temp] != null)
		{
			_levelNameText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelNameText.text));
		}
	}

	public void PrevLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex - 1;
		if (temp >= 0 && _savedLevels[temp] != null)
		{
			_levelNameText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelNameText.text));
		}
	}

	void GenerateLevelPreview (List<LevelData> levelDataList_)
	{
		DestroyPreviewCubes ();

		if (levelDataList_ != null)
			foreach (LevelData ld in levelDataList_)
			{
				Vector3 pos = LevelLayoutManager.instance.GetVectorFromData (ld);
				Transform clone = Instantiate (_previewCube, _previewHolder.transform.position + (pos * _positionOffset), Quaternion.identity, _previewHolder);
				clone.localScale = Vector3.one * _cubeScale;
				clone.GetComponent<MeshRenderer> ().material.color = _previewColorArray[(int) ld._blockType + 1];
				_previewColorArray[(int) ld._blockType + 1].a = _previewCubeAlphaValue;
			}
	}

	private void DestroyPreviewCubes ()
	{
		foreach (Transform child in _previewHolder)
			if (child != null) Destroy (child.gameObject);
	}

	public void RestartLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelNameText.text);
	}

	public void LoadLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelNameText.text);
	}

	private void OnDestroy ()
	{
		_controllerEvents.ButtonTwoPressed -= new ControllerInteractionEventHandler (Menu);
	}

}