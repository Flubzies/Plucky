using System.Collections.Generic;
using BlockClasses;
using DG.Tweening;
using ManagerClasses;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ControllerMenu : MonoBehaviour
{
	[SerializeField] Transform _menuObject;
	[SerializeField] Text _levelText;

	[Header ("Level Preview ")]
	[SerializeField] Transform _previewHolder;
	[SerializeField] Transform _previewCube;
	[Tooltip ("Color of each preview block, corresponds with BlockType enums")]
	[SerializeField] Color[] _previewColorArray;
	[SerializeField] float _cubePosDiv = 100.0f;
	bool _canOpenMenu = true;
	bool _menuOpen;

	int _levelIndex;
	List<string> _savedLevels;

	private void Start ()
	{
		GetComponent<VRTK_ControllerEvents> ().ButtonTwoPressed += new ControllerInteractionEventHandler (Menu);
		_savedLevels = LevelLayoutManager.instance.GetSavedLevels ();
		_menuObject.gameObject.SetActive (false);
	}

	private void Menu (object sender, ControllerInteractionEventArgs e)
	{
		if (_menuOpen) DoMenuOff ();
		if (!_canOpenMenu) return;

		_menuOpen = true;
		_canOpenMenu = false;
		_menuObject.gameObject.SetActive (true);

		transform.localScale = Vector3.zero;
		Tweener t = transform.DOScale (Vector3.one, 0.5f);
		t.SetEase (Ease.InBounce);

		_levelText.text = LevelLayoutManager.instance._CurrentLevelName;
		GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelText.text));
		UpdateIndex ();
	}

	private void UpdateIndex ()
	{
		for (int i = 0; i < _savedLevels.Count; i++)
			if (_levelText.text == _savedLevels[i])
			{
				_levelIndex = i;
				break;
			}
		else _levelIndex = 0;
	}

	private void DoMenuOff ()
	{
		Tweener t = transform.DOScale (Vector3.zero, 0.5f);
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
			_levelText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelText.text));
		}
	}

	public void PrevLevel ()
	{
		UpdateIndex ();
		int temp = _levelIndex - 1;
		if (temp >= 0 && _savedLevels[temp] != null)
		{
			_levelText.text = _savedLevels[temp];
			GenerateLevelPreview (LevelLayoutManager.instance.GetLevelDataFromResources (_levelText.text));
		}
	}

	void GenerateLevelPreview (List<LevelData> levelDataList_)
	{
		foreach (Transform child in _previewHolder)
			if (child != null) Destroy (child.gameObject);

		if (levelDataList_ != null)
			foreach (LevelData ld in levelDataList_)
			{
				Vector3 pos = LevelLayoutManager.instance.GetVectorFromData (ld);
				Transform clone = Instantiate (_previewCube, _previewHolder.transform.position + (pos / _cubePosDiv), Quaternion.identity, _previewHolder);
				clone.GetComponent<MeshRenderer> ().material.color = _previewColorArray[(int) ld._blockType + 1];
			}
	}

	public void RestartLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelText.text);
	}

	public void LoadLevel ()
	{
		LevelLayoutManager.instance.LoadLevel (_levelText.text);
	}

}