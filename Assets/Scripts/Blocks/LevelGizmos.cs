using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I wrote this script then forgot why I wrote it.

public class LevelGizmos : MonoBehaviour
{
	[Header ("Size of level ")]
	[SerializeField] LayerMask _blocksLM;
	[SerializeField] LayerMask _botLM;
	[SerializeField] Vector3 _levelBounds;
	[SerializeField] float _offset = 0.5f;
	Collider[] _colliders = new Collider[4];

	[SerializeField] Color _emptyCubeColor;

	private void OnDrawGizmosSelected ()
	{
		Vector3 levelPos =
			new Vector3 (
				transform.position.x - _offset,
				(transform.position.y + 1 * Mathf.RoundToInt (_levelBounds.y / 2)) - _offset,
				transform.position.z - _offset
			);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube (levelPos, _levelBounds);

		for (int x = 0; x < _levelBounds.x; x++)
		{
			for (int y = 0; y < _levelBounds.y; y++)
			{
				for (int z = 0; z < _levelBounds.z; z++)
				{
					Vector3 pos = new Vector3 (x - _levelBounds.x / 2, y, z - _levelBounds.z / 2);
					int numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _blocksLM);
					if (numCols != 0)
					{
						Gizmos.color = Color.red / 2;
						Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
						continue;
					}
					else
					{
						numCols = Physics.OverlapSphereNonAlloc (pos, 0.2f, _colliders, _botLM);
						if (numCols != 0)
						{
							Gizmos.color = Color.blue / 2;
							Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
							continue;
						}
					}

					Gizmos.color = _emptyCubeColor;
					Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
				}
			}
		}
	}

	private void OnValidate ()
	{
		_levelBounds = _levelBounds.ToInt ();
	}
}