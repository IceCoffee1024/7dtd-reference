using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000115 RID: 277
[Preserve]
public class BlockCactus : BlockDamage
{
	// Token: 0x06000745 RID: 1861 RVA: 0x0003426F File Offset: 0x0003246F
	public override void Init()
	{
		base.Init();
		this.IsTerrainDecoration = true;
		this.CanDecorateOnSlopes = false;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00034288 File Offset: 0x00032488
	public override void GetCollisionAABB(BlockValue _blockValue, int _x, int _y, int _z, float _distortedY, List<Bounds> _result)
	{
		base.GetCollisionAABB(_blockValue, _x, _y, _z, _distortedY, _result);
		Vector3 vector = new Vector3(0.15f, 0.05f, 0.15f);
		Block block = _blockValue.Block;
		if (block.isMultiBlock && block.multiBlockPos.dim.y == 1)
		{
			vector = new Vector3(0.15f, -0.75f, 0.15f);
		}
		for (int i = 0; i < _result.Count; i++)
		{
			Bounds bounds = _result[i];
			bounds.SetMinMax(bounds.min - vector, bounds.max + vector);
			_result[i] = bounds;
		}
	}
}
