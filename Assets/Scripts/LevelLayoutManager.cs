using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BlockClasses;
using UnityEngine;

namespace ManagerClasses
{
	public class LevelLayoutManager : MonoBehaviour
	{
		[Header ("Prefab list ")]
		[Tooltip ("Make sure they are in the same order as your block type enums!")]
		[SerializeField] List<Block> _blockList;
		[SerializeField] Bot _bot;

		[Header ("Parent classes list ")]
		[SerializeField] Transform _blockHolder;
		[SerializeField] Transform _botHolder;
		[SerializeField] bool _clearOnStart;

		[HideInInspector] public string _levelName;

		void Start ()
		{
			if (_clearOnStart)
			{
				ClearLevel ();
				LoadLevel ("Level 1.dat");
			}
		}

		private List<LevelData> GetChildrenLevelData ()
		{
			List<LevelData> levelDataList = new List<LevelData> ();
			levelDataList.Clear ();
			Block[] childBlocks = GetComponentsInChildren<Block> ();
			foreach (Block block in childBlocks)
			{
				LevelData levelData = new LevelData ();
				levelData._blockType = block._blockProperties._blockType;
				levelData._rotationY = block.GetInitialYRot ();
				SetPositionsToData (levelData, block.transform);
				levelDataList.Add (levelData);
			}

			Bot[] childBots = GetComponentsInChildren<Bot> ();
			foreach (Bot bot in childBots)
			{
				LevelData levelData = new LevelData ();
				levelData._isBot = true;
				levelData._rotationY = bot.transform.localRotation.y;
				SetPositionsToData (levelData, bot.transform);
				levelDataList.Add (levelData);
			}

			return levelDataList;
		}

		private void SetPositionsToData (LevelData ld_, Transform t_)
		{
			ld_._positionX = t_.transform.localPosition.x;
			ld_._positionY = t_.transform.localPosition.y;
			ld_._positionZ = t_.transform.localPosition.z;
		}

		private Vector3 GetVectorFromData (LevelData ld_)
		{
			Vector3 temp = Vector3.zero;
			temp.x = ld_._positionX;
			temp.y = ld_._positionY;
			temp.z = ld_._positionZ;
			return temp;
		}

		string GetLevelPath ()
		{
#if UNITY_EDITOR
			return Application.dataPath + "/Levels/";
#else
			return Application.dataPath + "/Levels/";
#endif
		}

		public void SaveLevel (string str_ = "")
		{
			string levelName;
			if (str_ == "") levelName = GetLevelPath () + _levelName + ".dat";
			else levelName = GetLevelPath () + str_ + ".dat";

			Debug.Log ("Saving.");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = new FileStream (levelName, FileMode.Create);
			List<LevelData> levelData = GetChildrenLevelData ();
			bf.Serialize (file, levelData);
			file.Close ();
		}

		public void LoadLevel (string str_ = "")
		{
			string levelName;
			if (str_ == "") levelName = GetLevelPath () + _levelName + ".dat";
			else levelName = GetLevelPath () + str_;

			Debug.Log ("Loading");
			if (File.Exists (levelName))
			{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = new FileStream (levelName, FileMode.Open);
				List<LevelData> levelDataList = (List<LevelData>) bf.Deserialize (file);
				file.Close ();
				GenerateLevel (levelDataList);
			}
			else Debug.Log ("Error. File does not exist.");
		}

		public List<string> GetSavedLevels ()
		{
			List<string> levelsSaved = new List<string> ();

			DirectoryInfo di = new DirectoryInfo (GetLevelPath ());
			foreach (var fi in di.GetFiles ("*.dat")) levelsSaved.Add (fi.Name);

			return levelsSaved;
		}

		private void GenerateLevel (List<LevelData> levelDataList_)
		{
			ClearLevel ();

			foreach (LevelData ld in levelDataList_)
			{
				Vector3 pos = GetVectorFromData (ld);
				Vector3 rot = new Vector3 (0, ld._rotationY, 0);

				if (ld._isBot) Instantiate (_bot, _botHolder.transform.position + pos, Quaternion.Euler (rot), _botHolder);
				else Instantiate (_blockList[(int) ld._blockType], _blockHolder.transform.position + pos, Quaternion.Euler (rot), _blockHolder);
			}
		}

		public void ClearLevel ()
		{
			// Delete all bots and blocks.
#if UNITY_EDITOR
			Invoke("ClearEditorLevel", 2.0f);
#else
			Debug.Log ("Game Deleting.");
			for (int i = _blockHolder.childCount; i > 0; --i) Destroy (_blockHolder.GetChild (0).gameObject, 2.0f);
			for (int i = _botHolder.childCount; i > 0; --i) Destroy (_botHolder.GetChild (0).gameObject, 2.0f);
#endif
		}

		private void ClearEditorLevel ()
		{
			Debug.Log("ASD");
			for (int i = _blockHolder.childCount; i > 0; --i) DestroyImmediate (_blockHolder.GetChild (0).gameObject);
			for (int i = _botHolder.childCount; i > 0; --i) DestroyImmediate (_botHolder.GetChild (0).gameObject);
		}

		public void DeleteLevel (string str_)
		{
			string levelName;
			if (str_ == "") levelName = GetLevelPath () + _levelName + ".dat";
			else levelName = GetLevelPath () + str_;

			Debug.Log ("Deleting");
			if (File.Exists (levelName))
			{
				File.Delete (levelName);
				File.Delete (levelName + ".meta");
			}
			else Debug.Log ("Error. File does not exist.");
		}
	}

	[System.Serializable]
	class LevelData
	{
		public BlockType _blockType;
		public float _positionX;
		public float _positionY;
		public float _positionZ;
		public float _rotationY;
		public bool _isBot = false;
	}
}