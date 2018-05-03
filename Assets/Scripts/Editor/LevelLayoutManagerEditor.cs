using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ManagerClasses
{
	[CustomEditor (typeof (LevelLayoutManager))]
	public class LevelLayoutManagerEditor : Editor
	{
		List<string> _levelsList = new List<string> ();

		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();
			LevelLayoutManager LLM = (LevelLayoutManager) target;

			GUILayout.Space (10.0f);
			EditorGUILayout.LabelField ("Level Data Persistence", EditorStyles.boldLabel);

			LLM._clearOnStart = EditorGUILayout.Toggle (new GUIContent ("Reset Scene", "Clears the scene and loads the level name scene."), LLM._clearOnStart);
			LLM._levelName = EditorGUILayout.TextField ("Level Name", LLM._levelName);
			GUILayout.Space (10.0f);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (new GUIContent ("Clear", "Clears the scene."))) LLM.ClearLevel ();
			if (GUILayout.Button ("Save")) LLM.SaveLevel ();
			if (GUILayout.Button ("Load")) LLM.LoadLevel ();
			GUILayout.EndHorizontal ();
			GUILayout.Space (10.0f);

			_levelsList.Clear ();
			_levelsList = LLM.GetSavedLevels ();

			for (int i = 0; i < _levelsList.Count; i++)
			{
				GUILayout.BeginHorizontal ();

				// string levelName = levelName.Remove (levelName.Length - 4, 4);
				GUILayout.Label ((i + 1).ToString (), GUILayout.Width (10));
				GUILayout.Label (_levelsList[i], GUILayout.Width (180));

				if (GUILayout.Button ("Save")) LLM.SaveLevel (_levelsList[i]);
				if (GUILayout.Button ("Load")) LLM.LoadLevel (_levelsList[i]);
				GUI.skin.button.normal.textColor = new Color (1.0f, 0.2f, 0.2f, 1.0f);
				if (GUILayout.Button (new GUIContent ("Delete", "Deletes the level file permanently!"))) LLM.DeleteLevel (_levelsList[i]);
				GUI.skin.button.normal.textColor = new Color (0.9f, 0.9f, 0.9f, 1.0f);
				GUILayout.EndHorizontal ();
			}
		}
	}
}