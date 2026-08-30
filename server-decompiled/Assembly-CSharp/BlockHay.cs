using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200011F RID: 287
[Preserve]
public class BlockHay : Block
{
	// Token: 0x0600078F RID: 1935 RVA: 0x00035E17 File Offset: 0x00034017
	public BlockHay()
	{
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00035E28 File Offset: 0x00034028
	public override void GetCollisionAABB(BlockValue _blockValue, int _x, int _y, int _z, float _distortedY, List<Bounds> _result)
	{
		float num = 0.0625f;
		_result.Add(BoundsUtils.BoundsForMinMax((float)_x + num, (float)_y, (float)_z + num, (float)(_x + 1) - num, (float)(_y + 1) - num, (float)(_z + 1) - num));
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00035203 File Offset: 0x00033403
	public override IList<Bounds> GetClipBoundsList(BlockValue _blockValue, Vector3 _blockPos)
	{
		Block.staticList_IntersectRayWithBlockList.Clear();
		this.GetCollisionAABB(_blockValue, (int)_blockPos.x, (int)_blockPos.y, (int)_blockPos.z, 0f, Block.staticList_IntersectRayWithBlockList);
		return Block.staticList_IntersectRayWithBlockList;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00035E65 File Offset: 0x00034065
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _e)
	{
		_e.fallDistance = Mathf.Max(_e.fallDistance - 5f, 0f);
		return true;
	}
}
