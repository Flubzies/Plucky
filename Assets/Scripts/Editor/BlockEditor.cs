// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// namespace BlockClasses
// {
// 	[CustomEditor (typeof (Block), true)]
// 	public class BlockEditor : Editor
// 	{
// 		private bool _isPlaceable = false;
// 		private Block _target = null;

// 		void OnEnable ()
// 		{
// 			_target = (Block) target;
// 		}

// 		public override void OnInspectorGUI ()
// 		{
// 			base.OnInspectorGUI ();

// 			EditorGUILayout.LabelField ("Block Placeable", EditorStyles.boldLabel);
// 			GUILayout.BeginHorizontal ();
// 			GUILayout.Label ("Is Placeable", GUILayout.Width (160));
// 			_isPlaceable = EditorGUILayout.Toggle (_isPlaceable);
// 			GUILayout.EndHorizontal ();

// 			if (_isPlaceable)
// 			{
// 				GUILayout.Space (5);
// 				GUILayout.BeginHorizontal ();
// 				GUILayout.Label ("Placeable Block Prefab", GUILayout.Width (160));
// 				_target._blockPlaceable = EditorGUILayout.ObjectField (_target._blockPlaceable, typeof (Transform), false) as Transform;
// 				GUILayout.EndHorizontal ();
// 			}
// 		}
// 	}
// }