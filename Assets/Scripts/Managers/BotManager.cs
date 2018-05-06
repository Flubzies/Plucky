using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManagerClasses
{
	public class BotManager : MonoBehaviour
	{
		static BotManager _instance;
		public static BotManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<BotManager> ();
				return _instance;
			}
		}

		public int _BotCount { get; private set; }

		public void DecrementBotCount ()
		{
			_BotCount--;
			if (_BotCount == 0) GameManager.instance.LevelComplete ();
		}

		public void ResetBotCount ()
		{
			_BotCount = 0;
		}

		public void IncrementBotCount ()
		{
			_BotCount++;
		}
	}
}