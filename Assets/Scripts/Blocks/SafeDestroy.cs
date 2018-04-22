using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Got the code here.
// https://answers.unity.com/questions/894959/addingremoving-objects-in-editor-mode.html
// Safe Destroy makes destroying easier in Editor.
public static class SafeDestroy
{
	public static T DestroyObject<T> (T obj_, float deathTimer_) where T : Object
	{
		if (!Application.isPlaying) Object.DestroyImmediate (obj_);
		else Object.Destroy (obj_, deathTimer_);
		return null;
	}

	public static T DestroyGameObject<T> (T component_, float deathTimer_ = 0.0f) where T : Component
	{
		if (component_ != null) DestroyObject (component_.gameObject, deathTimer_);
		return null;
	}
}