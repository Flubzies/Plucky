using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Singleton that manages the application. Primarily for scene management.
namespace ManagerClasses
{
	public class ApplicationManager : MonoBehaviour
	{
		[SerializeField] SceneFader _sceneFader;
		[SerializeField] public VRMessage _VRDebug;

		static ApplicationManager _instance;
		public static ApplicationManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<ApplicationManager> ();
				return _instance;
			}
		}

		private void Awake ()
		{
			_VRDebug = GetComponentInChildren<VRMessage> ();
		}

		// public void LoadMainMenu ()
		// {
		// 	SettingsMenu.instance.ToggleSettings (false);
		// 	_sceneFader.FadeToScene ("MainMenu");
		// }

		// public void ReLoadScene ()
		// {
		// 	SettingsMenu.instance.ToggleSettings (false);
		// 	_sceneFader.FadeToScene (SceneManager.GetActiveScene ().name);
		// }

		public void LoadExit ()
		{
			Application.Quit ();
		}

	}
}