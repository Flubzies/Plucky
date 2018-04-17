using System.Collections;
using UnityEngine;
using VRTK;

namespace BlockClasses
{
	public class BlockManager : MonoBehaviour
	{
		static BlockManager _instance;
		public static BlockManager instance
		{
			get
			{
				if (!_instance)
					_instance = FindObjectOfType<BlockManager> ();
				return _instance;
			}
		}

		

	}
}