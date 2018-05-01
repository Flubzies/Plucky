using System;
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
        [SerializeField]
        List<Block> _blockList;
        [SerializeField] Bot _bot;

        [Header ("Parent classes list ")]
        [SerializeField]
        Transform _blockHolder;
        [SerializeField] Transform _botHolder;

        [HideInInspector] public bool _clearOnStart;
        [HideInInspector] public string _levelName;

        [SerializeField] float _loadWaitTime = 2.0f;
        bool _canLoad = true;
        List<ILevelObject> _currentLevelObjects = new List<ILevelObject> ();
        public string _CurrentLevelName { get; private set; }

        string _fileExtension = ".txt";

        static LevelLayoutManager _instance;
        public static LevelLayoutManager instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<LevelLayoutManager> ();
                return _instance;
            }
        }

        void Start ()
        {
            if (_clearOnStart)
            {
                ClearLevelImmediate ();
                LoadLevel (_levelName);
            }
        }

        private void SetupLevelObjectList ()
        {
            ILevelObject[] temp = GetComponentsInChildren<ILevelObject> ();
            _currentLevelObjects.Clear ();
            foreach (ILevelObject obj in temp) _currentLevelObjects.Add (obj);
        }

        private List<LevelData> GetDataFromCurrentLevel ()
        {
            SetupLevelObjectList ();
            List<LevelData> levelDataList = new List<LevelData> ();

            foreach (ILevelObject obj in _currentLevelObjects)
            {
                LevelData levelData = new LevelData ();
                levelData._blockType = obj.GetBlockType;
                levelData._rotationY = obj.GetTransform.localRotation.eulerAngles.y;
                levelData._isPlaceable = obj.IsPlaceable;
                SetPositionsToData (levelData, obj.GetTransform);
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

        public Vector3 GetVectorFromData (LevelData ld_)
        {
            Vector3 temp = Vector3.zero;
            temp.x = ld_._positionX;
            temp.y = ld_._positionY;
            temp.z = ld_._positionZ;
            return temp;
        }

        public string GetLevelPath ()
        {
            return Application.dataPath + "/Resources/Levels/";
        }

        public void SaveLevel (string levelName_ = "")
        {
            string levelPath;
            if (levelName_ == "") levelPath = GetLevelPath () + _levelName + _fileExtension;
            else levelPath = GetLevelPath () + levelName_ + _fileExtension;

            Debug.Log ("Saving. " + levelName_);
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = new FileStream (levelPath, FileMode.Create);
            List<LevelData> levelData = GetDataFromCurrentLevel ();
            bf.Serialize (file, levelData);
            file.Close ();
        }

        public List<LevelData> GetLevelDataFromResources (string levelName_ = "")
        {
            TextAsset asset;
            if (levelName_ == "") asset = Resources.Load ("Levels/" + _levelName) as TextAsset;
            else asset = Resources.Load ("Levels/" + levelName_) as TextAsset;

            if (asset != null)
            {
                Stream stream = new MemoryStream (asset.bytes);
                BinaryFormatter bf = new BinaryFormatter ();
                List<LevelData> levelDataList = bf.Deserialize (stream) as List<LevelData>;
                stream.Close ();
                return levelDataList;
            }
            else
            {
                Debug.LogError ("Level data not found!");
                return null;
            }
        }

        public void LoadLevel (string levelName_ = "")
        {
            if (_canLoad)
            {
                List<LevelData> levelDataList = GetLevelDataFromResources (levelName_);
                if (levelDataList != null)
                {
                    Debug.Log ("Loading " + levelName_);
                    _CurrentLevelName = levelName_;
                    ClearLevel ();
                    if (Application.isPlaying) StartCoroutine (WaitForLoad (levelDataList));
                    else GenerateLevel (levelDataList);
                }
                else Debug.LogError ("Level data is null!");
            }
        }

        IEnumerator WaitForLoad (List<LevelData> ld)
        {
            _canLoad = false;
            yield return new WaitForSeconds (_loadWaitTime);
            GenerateLevel (ld);
            yield return new WaitForSeconds (_loadWaitTime);
            _canLoad = true;
        }

        public List<string> GetSavedLevels ()
        {
            UnityEngine.Object[] assets = Resources.LoadAll ("Levels/");
            List<string> levelsSaved = new List<string> ();
            foreach (UnityEngine.Object asset in assets) levelsSaved.Add (asset.name);
            return levelsSaved;
        }

        private void GenerateLevel (List<LevelData> levelDataList_)
        {
            Debug.Log ("Generating");

            foreach (LevelData ld in levelDataList_)
            {
                Vector3 pos = GetVectorFromData (ld);
                Vector3 rot = new Vector3 (0, ld._rotationY, 0);
                if (ld._blockType == BlockType.Undefined)
                {
                    _currentLevelObjects.Add (Instantiate (_bot, _botHolder.transform.position + pos, Quaternion.Euler (rot), _botHolder));
                }
                else
                {
                    ILevelObject l = (Instantiate (_blockList[(int) ld._blockType], _blockHolder.transform.position + pos, Quaternion.Euler (rot), _blockHolder));
                    l.IsPlaceable = ld._isPlaceable;
                    _currentLevelObjects.Add (l);
                }
            }

            foreach (ILevelObject obj in _currentLevelObjects)
                if (Application.isPlaying && obj != null) obj.InitializeILevelObject ();
        }

        public void ClearLevel ()
        {
            GetDataFromCurrentLevel ();
            foreach (ILevelObject obj in _currentLevelObjects) obj.DeathEffect ();
            _currentLevelObjects.Clear ();
        }

        public void ClearLevelImmediate ()
        {
            GetDataFromCurrentLevel ();
            foreach (ILevelObject obj in _currentLevelObjects) SafeDestroy.DestroyGameObject (obj.GetTransform);
            _currentLevelObjects.Clear ();
        }

        public void DeleteLevel (string levelName_)
        {
            string levelPath;
            if (levelName_ == "") levelPath = GetLevelPath () + _levelName + _fileExtension;
            else levelPath = GetLevelPath () + levelName_ + _fileExtension;

            Debug.Log ("Deleting");
            if (File.Exists (levelPath)) File.Delete (levelPath);
            if (File.Exists (levelPath + ".meta")) File.Delete (levelPath + ".meta");
            else Debug.Log ("Error. File does not exist.");
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public BlockType _blockType;
        public float _positionX;
        public float _positionY;
        public float _positionZ;
        public float _rotationY;
        public bool _isPlaceable;
    }
}

public interface ILevelObject
{
    Transform GetTransform { get; }
    BlockType GetBlockType { get; }
    bool IsPlaceable { get; set; }
    void DeathEffect ();
    void InitializeILevelObject ();
}