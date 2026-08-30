using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000112 RID: 274
[Preserve]
public class BlockCactus : BlockDamage
{
	// Token: 0x06000735 RID: 1845 RVA: 0x00033EBB File Offset: 0x000320BB
	public override void Init()
	{
		base.Init();
		this.IsTerrainDecoration = true;
		this.CanDecorateOnSlopes = false;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00033ED4 File Offset: 0x000320D4
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
