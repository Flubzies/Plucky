using System.Collections;
using UnityEngine;

namespace ManagerClasses
{
	public class PlayerPrefsManager : MonoBehaviour
	{
		static PlayerPrefsManager _instance;
		public static PlayerPrefsManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<PlayerPrefsManager> ();
				return _instance;
			}
		}

		public void DeleteAllKeys ()
		{
			PlayerPrefs.DeleteAll ();
			PlayerPrefs.Save ();
		}

	}
}