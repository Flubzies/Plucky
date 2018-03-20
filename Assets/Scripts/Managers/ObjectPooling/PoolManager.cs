using System.Collections;
using UnityEngine;

// In the tutorial he has PoolCreator(His PoolManager) as a singleton.
// I decided to make this the PoolManager instead so that I can have multiple pools in this singleton.
// Whereas with his method you could only have one pool in the PoolCreator Singleton.
// I've added functionality where it won't Enque and reuse an active gameobject. 
// This is quite useful because I don't want enemies to randomly disappear if we use all the objects in the pool.
namespace ObjectPooling
{
	public class PoolManager : MonoBehaviour
	{
		[Header ("Hit Effect:")]
		[SerializeField] PoolObject _hitEffect;
		[SerializeField] int _hitEffectsInPoolCount;
		[SerializeField] float _hitEffectTimer;
		public static PoolCreator _HitEffectPool { get; set; }

		[Header ("Explosion Effect:")]
		[SerializeField] PoolObject _explosionEffect;
		[SerializeField] int _explosionEffectsInPoolCount;
		[SerializeField] float _explosionEffectTimer;
		public static PoolCreator _ExplosionEffectPool { get; set; }

		static PoolManager _instance;
		public static PoolManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<PoolManager> ();
				return _instance;
			}
		}

		void Awake ()
		{
			// _HitEffectPool = new PoolCreator ();
			// _HitEffectPool.CreatePool (_hitEffect.gameObject, _hitEffectsInPoolCount);

			// _ExplosionEffectPool = new PoolCreator ();
			// _ExplosionEffectPool.CreatePool (_explosionEffect.gameObject, _explosionEffectsInPoolCount);
		}

		public void ReuseHitEffectPool (Vector3 position_, Quaternion rotation_)
		{
			GameObject g = _HitEffectPool.ReuseObject (_hitEffect.gameObject, position_, rotation_);
			if (g != null) StartCoroutine (SetInactive (g, _hitEffectTimer));
		}

		public void ReuseExplosionEffect (Vector3 position_, Quaternion rotation_)
		{
			GameObject g = _ExplosionEffectPool.ReuseObject (_explosionEffect.gameObject, position_, rotation_);
			if (g != null) StartCoroutine (SetInactive (g, _explosionEffectTimer));
		}

		IEnumerator SetInactive (GameObject g, float timeBeforeInactive_ = 0.0f)
		{
			yield return new WaitForSeconds (timeBeforeInactive_);
			g.SetActive (false);
		}

	}
}