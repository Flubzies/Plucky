using DG.Tweening;
using UnityEngine;

namespace BlockClasses
{
	[CreateAssetMenu (fileName = "BlockProperties", menuName = "Plucky/BlockProperties", order = 0)]
	public class BlockProperties : ScriptableObject
	{
		public BlockType _blockType;
		public Ease _blockSpawnEaseType;
		// public Color _blockColor;

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