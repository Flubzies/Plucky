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
	List<string> _savedLevelNames;

	private void Awake ()
	{
		_controllerEvents = GetComponent<VRTK_ControllerEvents> ();
	}

	private void Start ()
	{
		_controllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler (Menu);
		_savedLevelNames = LevelManager.instance.GetSavedLevelNames ();
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

		_levelNameText.text = LevelManager.instance._CurrentLevelName;
	}

	void OnMenuOpen ()
	{
		GenerateLevelPreview (LevelManager.instance.GetLevelData (_levelNameText.text));
	}

	private void UpdateIndex ()
	{
		for (int i = 0; i < _savedLevelNames.Count; i++)
			if (_levelNameText.text == _savedLevelNames[i])
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
		if (temp < _savedLevelNames.Count && _savedLevelNames[temp] != null)
		{
			_levelNameText.text = _savedLevelNames[temp];
			GenerateLevelPreview (LevelManager.instance.GetLevelData (_levelNameText.text));
		}
	}

	public void PrevLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex - 1;
		if (temp >= 0 && _savedLevelNames[temp] != null)
		{
			_levelNameText.text = _savedLevelNames[temp];
			GenerateLevelPreview (LevelManager.instance.GetLevelData (_levelNameText.text));
		}
	}

	void GenerateLevelPreview (LevelData levelData_)
	{
		DestroyPreviewCubes ();

		if (levelData_ != null)
			foreach (LevelObjectData ld in levelData_._levelObjects)
			{
				Vector3 pos = LevelManager.instance.GetVectorFromData (ld);
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
		LevelManager.instance.LoadLevel (_levelNameText.text);
	}

	public void LoadLevel ()
	{
		LevelManager.instance.LoadLevel (_levelNameText.text);
	}

	private void OnDestroy ()
	{
		_controllerEvents.ButtonTwoPressed -= new ControllerInteractionEventHandler (Menu);
	}

}