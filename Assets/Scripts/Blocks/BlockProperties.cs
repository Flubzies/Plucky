using UnityEngine;

namespace BlockClasses
{
	[CreateAssetMenu (fileName = "BlockProperties", menuName = "Plucky/BlockProperties", order = 0)]
	public class BlockProperties : ScriptableObject
	{
		public MeshFilter _blockMesh;
	}
}