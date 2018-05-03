using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
	public Material _material;
	
	private void OnRenderImage (RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit (src, dest, _material);
	}
}