using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManagerClasses
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] ParticleSystem _victoryEffect;

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

		public void LevelComplete ()
		{
			// TODO: Play victory noise
			_victoryEffect.Play ();
			// Load next level automatically.
		}
	}
}