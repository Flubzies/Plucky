using UnityEngine;

namespace BlockClasses
{
	[CreateAssetMenu (fileName = "BlockProperties", menuName = "Plucky/BlockProperties", order = 0)]
	public class BlockProperties : ScriptableObject
	{
		public BlockType _blockType;

		// [Header ("Audio")]
		// [SerializeField] AudioClip _blockEffect;
		// [SerializeField] AudioClip _blockPlaceEffect;
		// [SerializeField] AudioClip _blockIdleEffect;
	}

	public enum BlockType
	{
		Undefined = -1,
		BlockBuild = 0,
		BlockDamage = 1,
		BlockEmpty = 2,
		BlockExit = 3,
		BlockGravity = 4,
		BlockTurn = 5
	}
}