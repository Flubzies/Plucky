using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (LevelLayoutManager))]
public class LevelLayoutManagerEditor : Editor
{
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
		if (GUILayout.Button ("Save")) LLM.SaveLevel ();
		if (GUILayout.Button ("Load")) LLM.LoadLevel ();
		GUILayout.EndHorizontal ();
	}
}