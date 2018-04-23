using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
	public class BlockBuild : Block
	{
		
	}
}
// *Might* use this if I decide to have multiple blocks placeable at the same time.

// List<IPlaceable> pList = new List<IPlaceable> ();

// public List<IPlaceable> GetConnectedBlocks ()
// {
// 	_colliders = GetAdjacentSpaces (transform);
// 	pList.Clear ();

// 	for (int i = 0; i < _colliders.Count; i++)
// 	{
// 		Collider[] cols = Physics.OverlapSphere (_colliders[i], 0.2f, _blocksLM);
// 		if (cols.Length != 0)
// 		{
// 			IPlaceable temp = cols[0].transform.GetComponent<IPlaceable> ();
// 			if (temp != null) pList.Add (temp);
// 		}
// 	}

// 	return pList;
// }