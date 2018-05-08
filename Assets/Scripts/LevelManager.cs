using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BlockClasses;
using Sirenix.OdinInspector;
using UnityEngine;

// I've refactored this class quite a few times. 
// First refactor - Application.persistentDataPath would not work in build because
// I wanted a few levels to be in the game to begin with.
// Second I used Resources which worked well.
// Third I just wanted rearrange able lists so I disabled the custom editor script.
// Probably should put everything in a Scriptable Object and leave it there.

namespace ManagerClasses
{
    public class LevelManager : MonoBehaviour
    {
        [Tooltip("Make sure they are in the same order as your block type enums!")]
        [Header("Prefab Holders")]
        [FoldoutGroup("Prefabs")]
        [SerializeField]
        Transform _blockHolder;
        [FoldoutGroup("Prefabs")] [SerializeField] Transform _botHolder;
        [Header("Prefabs")]
        [FoldoutGroup("Prefabs")]
        [SerializeField]
        List<Block> _blockList;
        [FoldoutGroup("Prefabs")] [SerializeField] Bot _bot;

        [Space(10.0f)]
        [SerializeField]
        float _loadWaitTime = 1.0f;
        [SerializeField] float _spawnTimer = 0.8f;
        [SerializeField] ParticleSystem _levelLoadedEffect;
        [SerializeField] bool _debug;
        [Space(10.0f)]

        [ToggleGroup("_clearOnStart", order: 0, groupTitle: "Initial level ")]
        [SerializeField]
        bool _clearOnStart;
        [ToggleGroup("_clearOnStart", order: 0, groupTitle: "Initial level ")] [SerializeField] string _initialLevel;
        [Space(10.0f)]

        [SerializeField]
        List<LevelData> _levels;
        [SerializeField] int _indexToSkipRename = 0;

        bool _canLoad = true;
        public List<Block> _PlaceableBlocks { get; private set; }
        public string _CurrentLevelName { get; private set; }

        string _fileExtension = ".txt";

        static LevelManager _instance;
        public static LevelManager instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<LevelManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            _PlaceableBlocks = new List<Block>();
        }

        void Start()
        {
            if (_clearOnStart)
            {
                ClearLevelImmediate();
                LoadLevel(_initialLevel);
            }
        }

        public List<ILevelObject> GetCurrentLevelObjects()
        {
            ILevelObject[] temp = GetComponentsInChildren<ILevelObject>();
            List<ILevelObject> _levelObjects = new List<ILevelObject>();
            foreach (ILevelObject obj in temp) _levelObjects.Add(obj);
            return _levelObjects;
        }

        private LevelData GetDataFromCurrentLevel()
        {
            LevelData LevelData = new LevelData();

            foreach (ILevelObject obj in GetCurrentLevelObjects())
            {
                LevelObjectData LevelObjectData = new LevelObjectData();
                LevelObjectData._blockType = obj.GetBlockType;
                LevelObjectData._rotationY = obj.GetTransform.localRotation.eulerAngles.y;
                LevelObjectData._isPlaceable = obj.IsPlaceable;
                SetPositionsToData(LevelObjectData, obj.GetTransform);
                LevelData._levelObjects.Add(LevelObjectData);
            }

            return LevelData;
        }

        private void SetPositionsToData(LevelObjectData ld_, Transform t_)
        {
            ld_._positionX = t_.transform.localPosition.x;
            ld_._positionY = t_.transform.localPosition.y;
            ld_._positionZ = t_.transform.localPosition.z;
        }

        public Vector3 GetVectorFromData(LevelObjectData ld_)
        {
            Vector3 temp = Vector3.zero;
            temp.x = ld_._positionX;
            temp.y = ld_._positionY;
            temp.z = ld_._positionZ;
            return temp;
        }

        public void SaveLevel(string levelName_ = null)
        {
            if (String.IsNullOrEmpty(levelName_)) return;
            if (_debug) Debug.Log("Saving. " + levelName_);

            LevelData levelData = GetDataFromCurrentLevel();
            levelData._levelName = levelName_;
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_levels[i]._levelName == levelName_)
                {
                    _levels[i] = levelData;
                    return;
                }
            }
            _levels.Add(levelData);
        }

        public LevelData GetLevelData(string levelName_ = null)
        {
            // TextAsset asset;
            // if (levelName_ == "") asset = Resources.Load ("Levels/" + _levelName) as TextAsset;
            // else asset = Resources.Load ("Levels/" + levelName_) as TextAsset;

            // if (asset != null)
            // {
            // 	Stream stream = new MemoryStream (asset.bytes);
            // 	BinaryFormatter bf = new BinaryFormatter ();
            // 	LevelData levelData = bf.Deserialize (stream) as LevelData;
            // 	stream.Close ();
            // 	return levelData;
            // }
            // else
            // {
            // 	Debug.LogError ("Level data not found!");
            // 	return null;
            // }

            foreach (LevelData ld in _levels)
                if (ld._levelName == levelName_) return ld;

            Debug.LogError("Unable to find the file!");
            return null;
        }

        public void LoadLevel(string levelName_ = "")
        {
            if (_canLoad)
            {
                LevelData levelData = GetLevelData(levelName_);
                if (levelData != null)
                {
                    if (_debug) Debug.Log("Loading " + levelName_);
                    _CurrentLevelName = levelName_;
                    if (Application.isPlaying)
                    {
                        ClearLevel();
                        StartCoroutine(WaitForLoad(levelData));
                    }
                    else
                    {
                        ClearLevelImmediate();
                        GenerateLevel(levelData);
                    }
                }
                else Debug.LogError("Level data is null!");
            }
        }

        IEnumerator WaitForLoad(LevelData levelData_)
        {
            _canLoad = false;
            yield return new WaitForSeconds(_loadWaitTime);
            GenerateLevel(levelData_);
            yield return new WaitForSeconds(_loadWaitTime);
            _canLoad = true;
        }

        List<LevelData> GetSavedLevels()
        {
            // UnityEngine.Object[] assets = Resources.LoadAll ("Levels/");
            // List<string> levelNames = new List<string> ();
            // foreach (UnityEngine.Object asset in assets) levelNames.Add (asset.name);
            // _levels.Clear ();
            // foreach (string levelName in levelNames)
            // 	_levels.Add (GetLevelData (levelName));
            // return _levels;
            return _levels;
        }

        public List<string> GetSavedLevelNames()
        {
            List<string> levelNames = new List<string>();
            foreach (var item in _levels)
                levelNames.Add(item._levelName);
            return levelNames;
        }

        void GenerateLevel(LevelData levelData_)
        {
            if (_debug) Debug.Log("Generating");
            if (_PlaceableBlocks != null) _PlaceableBlocks.Clear();
            BotManager.instance.ResetBotCount();
            List<ILevelObject> _tempILevelObjectList = new List<ILevelObject>();

            foreach (LevelObjectData lod in levelData_._levelObjects)
            {
                Vector3 pos = GetVectorFromData(lod);
                Vector3 rot = new Vector3(0, lod._rotationY, 0);
                if (lod._blockType == BlockType.Undefined)
                {
                    _tempILevelObjectList.Add(Instantiate(_bot, _botHolder.transform.position + pos, Quaternion.Euler(rot), _botHolder));
                    BotManager.instance.IncrementBotCount();
                }
                else
                {
                    ILevelObject iLevelObj = (Instantiate(_blockList[(int)lod._blockType], _blockHolder.transform.position + pos, Quaternion.Euler(rot), _blockHolder));
                    iLevelObj.IsPlaceable = lod._isPlaceable;
                    _tempILevelObjectList.Add(iLevelObj);
                    if (Application.isPlaying && iLevelObj.IsPlaceable) _PlaceableBlocks.Add(iLevelObj.GetTransform.GetComponent<Block>());
                }
            }

            foreach (ILevelObject obj in _tempILevelObjectList)
                if (Application.isPlaying && obj != null) obj.InitializeILevelObject(_spawnTimer);

            _levelLoadedEffect.Play();
        }

        void ClearLevel()
        {
            foreach (ILevelObject obj in GetCurrentLevelObjects()) obj.DeathEffect(_spawnTimer);
        }

        [HorizontalGroup("Buttons")]
        [Button]
        void ClearLevelImmediate()
        {
            foreach (ILevelObject obj in GetCurrentLevelObjects()) SafeDestroy.DestroyGameObject(obj.GetTransform);
        }

        [HorizontalGroup("Buttons")]
        [Button]
        public void RenameLevelsToIndex()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_indexToSkipRename == i) continue;
                _levels[i]._levelName = "Level " + (i);
            }
        }

        public string GetLevelPath()
        {
            return Application.persistentDataPath + "/Levels/";
        }

        void SaveAllLevelsToResources(string levelName_ = null)
        {
            string levelPath;
            BinaryFormatter bf;
            FileStream file;

            foreach (LevelData ld in _levels)
            {
                levelPath = GetLevelPath() + ld._levelName + _fileExtension;
                bf = new BinaryFormatter();
                file = new FileStream(levelPath, FileMode.Create);
                bf.Serialize(file, ld);
                file.Close();
            }
        }

        // public void DeleteLevel (string levelName_)
        // {
        // 	string levelPath;
        // 	if (levelName_ == "") levelPath = GetLevelPath () + _levelName + _fileExtension;
        // 	else levelPath = GetLevelPath () + levelName_ + _fileExtension;

        // 	if(_debug)Debug.Log ("Deleting");
        // 	if (File.Exists (levelPath)) File.Delete (levelPath);
        // 	if (File.Exists (levelPath + ".meta")) File.Delete (levelPath + ".meta");
        // 	else if(_debug)Debug.Log ("Error. File does not exist.");
        // }

    }

    [System.Serializable]
    public class LevelObjectData
    {
        public BlockType _blockType;
        public float _positionX;
        public float _positionY;
        public float _positionZ;
        public float _rotationY;
        public bool _isPlaceable;
    }

    [System.Serializable]
    public class LevelData
    {
        [HorizontalGroup("Level Data Persistence Options")] public string _levelName;
        [HideInInspector] public List<LevelObjectData> _levelObjects = new List<LevelObjectData>();

        bool _levelCompleted;

#if UNITY_EDITOR
        [HorizontalGroup("Level Data Persistence Options")]
        [Button]
        public void SaveLevel() { LevelManager.instance.SaveLevel(_levelName); }

        [HorizontalGroup("Level Data Persistence Options")]
        [Button]
        public void LoadLevel() { LevelManager.instance.LoadLevel(_levelName); }

        // [HorizontalGroup ("Level Data Persistence Options"), ]
        // [Button] public void DeleteLevel () { LevelManager.instance.DeleteLevel (_levelName); }
#endif
    }

}

public interface ILevelObject
{
    Transform GetTransform { get; }
    BlockType GetBlockType { get; }
    bool IsPlaceable { get; set; }
    void DeathEffect(float deathEffectTime_ = 0.8f);
    void InitializeILevelObject(float spawnEffectTime_ = 0.8f);
}