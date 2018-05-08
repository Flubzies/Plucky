using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Singleton that manages the application. Primarily for scene management.
namespace ManagerClasses
{
	public class ApplicationManager : MonoBehaviour
	{
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

		public void LoadExit ()
		{
			Application.Quit ();
		}

	}
}