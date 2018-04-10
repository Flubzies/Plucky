using UnityEngine;

namespace BlockClasses
{
	[CreateAssetMenu (fileName = "BlockProperties", menuName = "Plucky/BlockProperties", order = 0)]
	public class BlockProperties : ScriptableObject
	{
		[SerializeField] BlockType _blockType;

		// [Header ("Audio")]
		// [SerializeField] AudioClip _blockEffect;
		// [SerializeField] AudioClip _blockPlaceEffect;
		// [SerializeField] AudioClip _blockIdleEffect;
	}

	public enum BlockType
	{
		BlockBuild,
		BlockDamage,
		BlockEmpty,
		BlockExit,
		BlockGravity,
		BlockTurn
	}
}