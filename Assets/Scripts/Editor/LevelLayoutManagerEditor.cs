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

			LLM._levelName = EditorGUILayout.TextField ("Level Name :", LLM._levelName);
			this.Repaint ();

			// LLM._levelName = GUI.TextField (new Rect (10, 10, 200, 20), LLM._levelName, 25);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (new GUIContent ("Clear", "Clears the scene."))) LLM.ClearLevel ();
			if (GUILayout.Button ("Save")) LLM.SaveLevel ();
			if (GUILayout.Button ("Load")) LLM.LoadLevel ();
			GUILayout.EndHorizontal ();

			GUILayout.Space (10.0f);
			GUILayout.BeginHorizontal ();
			bool loadSavedLevelsButton = GUILayout.Button ("Load Saved Levels");
			GUILayout.EndHorizontal ();
			GUILayout.Space (10.0f);

			if (loadSavedLevelsButton)
			{
				_levelsList.Clear ();
				_levelsList = LLM.GetSavedLevels ();
			}

			foreach (string levelName in _levelsList)
			{
				GUILayout.BeginHorizontal ();

				string temp = levelName.Remove (levelName.Length - 4, 4);
				EditorGUILayout.LabelField ("Level Name: ", GUILayout.Width (80));
				GUILayout.Label (temp, GUILayout.Width (60));

				if (GUILayout.Button ("Save"))
				{
					LLM.SaveLevel (levelName);
				}
				if (GUILayout.Button ("Load"))
				{
					LLM.LoadLevel (levelName);
				}
				GUI.skin.button.normal.textColor = new Color (1.0f, 0.2f, 0.2f, 1.0f);
				if (GUILayout.Button ("Delete"))
				{
					LLM.DeleteLevel (levelName);
				}
				GUI.skin.button.normal.textColor = new Color (0.8f, 0.8f, 0.8f, 1.0f);
				GUILayout.EndHorizontal ();
			}
		}
	}
}