using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BlockClasses;
using UnityEngine;

public class LevelLayoutManager : MonoBehaviour
{
	[Header ("Size of level ")]
	[SerializeField] LayerMask _blocksLM;
	[SerializeField] LayerMask _botLM;
	[SerializeField] Vector3 _levelBounds;
	[SerializeField] float _offset = 0.5f;
	Collider[] _colliders = new Collider[4];

	[Header ("Prefab list ")]
	[Tooltip ("Make sure they are in the same order as your block type enums!")]
	[SerializeField] List<Block> _blockList;
	[SerializeField] Bot _bot;

	[Header ("Parent classes list ")]
	[SerializeField] Transform _blockHolder;
	[SerializeField] Transform _botHolder;

	[HideInInspector] public string _levelName;

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

	private void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube (transform.position + transform.up * Mathf.RoundToInt (_levelBounds.y / 2), _levelBounds);
		for (int x = 0; x < _levelBounds.x; x++)
		{
			for (int y = 0; y < _levelBounds.y; y++)
			{
				for (int z = 0; z < _levelBounds.z; z++)
				{
					Vector3 pos = new Vector3 (x - _levelBounds.x / 2 + _offset, y + _offset, z - _levelBounds.z / 2 + _offset);
					int numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _blocksLM);
					if (numCols != 0)
					{
						Gizmos.color = Color.red / 2;
						Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
						continue;
					}
					else
					{
						numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _botLM);
						if (numCols != 0)
						{
							Gizmos.color = Color.blue / 2;
							Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
							continue;
						}
					}

					Gizmos.color = Color.white / 60;
					Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
				}
			}
		}
	}

	public void SaveLevel ()
	{
		string testLevel = "Assets/levelInfo.dat";

		Debug.Log ("Saving.");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = new FileStream (testLevel, FileMode.Create);
		List<LevelData> levelData = GetChildrenLevelData ();
		bf.Serialize (file, levelData);
		file.Close ();
	}

	public void LoadLevel ()
	{
		string testLevel = "Assets/levelInfo.dat";

		Debug.Log ("Loading");
		if (File.Exists (testLevel))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = new FileStream (testLevel, FileMode.Open);
			List<LevelData> levelDataList = (List<LevelData>) bf.Deserialize (file);
			file.Close ();
			GenerateLevel (levelDataList);
		}
		else Debug.Log ("Error. File does not exist.");
	}

	private void GenerateLevel (List<LevelData> levelDataList_)
	{
		// Delete all bots and blocks.
		foreach (Transform child in _botHolder)
			DestroyImmediate (child);

		foreach (Transform child in _blockHolder)
			DestroyImmediate (child);

		foreach (LevelData ld in levelDataList_)
		{
			Vector3 pos = GetVectorFromData (ld);
			Vector3 rot = new Vector3 (0, ld._rotationY, 0);

			if (ld._isBot)
			{
				Instantiate (_bot, pos, Quaternion.Euler (rot), _botHolder);
			}
			else
			{
				Instantiate (_blockList[(int) ld._blockType], pos, Quaternion.Euler (rot), _blockHolder);
			}
		}
	}

	private void OnValidate ()
	{
		_levelBounds = new Vector3 (
			Mathf.RoundToInt (_levelBounds.x),
			Mathf.RoundToInt (_levelBounds.y),
			Mathf.RoundToInt (_levelBounds.z)
		);
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