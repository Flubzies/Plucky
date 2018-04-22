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
		[SerializeField] public VRMessage _VRMessage;
		public static ApplicationManager instance = null;

		void Awake ()
		{
			if (instance == null) instance = this;
			else if (instance != this) Destroy (gameObject);
			DontDestroyOnLoad (gameObject);
		}

		private void Start ()
		{
			_sceneFader = GetComponentInChildren<SceneFader> ();
			_VRMessage = GetComponentInChildren<VRMessage> ();
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnSceneLoaded (Scene scene_, LoadSceneMode mode_)
		{
			_sceneFader.FadeFromScene ();
		}

		public void LoadMainMenu ()
		{
			SettingsMenu.instance.ToggleSettings (false);
			_sceneFader.FadeToScene ("MainMenu");
		}

		public void ReLoadScene ()
		{
			SettingsMenu.instance.ToggleSettings (false);
			_sceneFader.FadeToScene (SceneManager.GetActiveScene ().name);
		}

		public void LoadExit ()
		{
			Application.Quit ();
		}

		public void LoadLevel01 ()
		{
			_sceneFader.FadeToScene ("Level01");
		}
	}
}