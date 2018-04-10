using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayoutManager : MonoBehaviour
{
	[Header ("Size of Level ")]
	[SerializeField] LayerMask _blocksLM;
	[SerializeField] LayerMask _botLM;
	[SerializeField] Vector3 _levelBounds;
	[SerializeField] float _offset = 0.5f;
	Collider[] _colliders = new Collider[4];

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube (transform.position + transform.up * Mathf.RoundToInt (_levelBounds.y / 2), _levelBounds);
		for (int x = 0; x < _levelBounds.x; x++)
		{
			for (int y = 0; y < _levelBounds.y; y++)
			{
				for (int z = 0; z < _levelBounds.z; z++)
				{
					Vector3 pos = new Vector3 (x - _levelBounds.x / 2 + _offset, y + _offset, z - _levelBounds.z / 2 + _offset);
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

					Gizmos.color = Color.white / 60;
					Gizmos.DrawWireCube (pos, Vector3.one * 0.9f);
				}
			}
		}
	}

	private void OnValidate ()
	{
		_levelBounds = new Vector3 (
			Mathf.RoundToInt (_levelBounds.x),
			Mathf.RoundToInt (_levelBounds.y),
			Mathf.RoundToInt (_levelBounds.z)
		);
	}

}

// // Uncomment the following line after replacing "MyScript" with your script name:
// [CustomEditor (typeof (LevelLayoutManager))]
// [CanEditMultipleObjects]
// public class LevelEditor : Editor
// {
// 	// //This is the value of the Slider
// 	// float _val;

// 	// public override void OnInspectorGUI ()
// 	// {
// 	// 	base.OnInspectorGUI ();
// 	// 	//This is the Label for the Slider
// 	// 	GUI.Label (new Rect (0, 300, 100, 30), "Rectangle Width");
// 	// 	//This is the Slider that changes the size of the Rectangle drawn
// 	// 	_val = GUI.HorizontalSlider (new Rect (100, 300, 100, 30), _val, 1.0f, 250.0f);

// 	// 	//The rectangle is drawn in the Editor (when MyScript is attached) with the width depending on the value of the Slider
// 	// 	EditorGUI.DrawRect (new Rect (50, 350, _val, 70), Color.green);
// 	// }
// }