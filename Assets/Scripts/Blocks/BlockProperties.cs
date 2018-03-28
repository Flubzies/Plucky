using UnityEngine;

[CreateAssetMenu (fileName = "BlockProperties", menuName = "Plucky/BlockProperties", order = 0)]
public class BlockProperties : ScriptableObject
{
	public MeshFilter _blockMesh;

	private void OnValidate ()
	{

	}
}