using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManagerClasses
{
	public class GameManager : MonoBehaviour
	{
		static GameManager _instance;
		public static GameManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<GameManager> ();
				return _instance;
			}
		}

		public int _BotCount { get; set; }

		private void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Space)) SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		// public static void DestroyPlayer (Player _player, float delay)
		// {
		// 	Destroy (_player.gameObject, delay);
		// }

		public void Settings ()
		{
			SettingsMenu.instance.ToggleSettings (true);
		}
	}
}