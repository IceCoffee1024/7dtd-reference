using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000122 RID: 290
[Preserve]
public class BlockHay : Block
{
	// Token: 0x0600079F RID: 1951 RVA: 0x000361BB File Offset: 0x000343BB
	public BlockHay()
	{
		this.IsCheckCollideWithEntity = true;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x000361CC File Offset: 0x000343CC
	public override void GetCollisionAABB(BlockValue _blockValue, int _x, int _y, int _z, float _distortedY, List<Bounds> _result)
	{
		float num = 0.0625f;
		_result.Add(BoundsUtils.BoundsForMinMax((float)_x + num, (float)_y, (float)_z + num, (float)(_x + 1) - num, (float)(_y + 1) - num, (float)(_z + 1) - num));
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000355A7 File Offset: 0x000337A7
	public override IList<Bounds> GetClipBoundsList(BlockValue _blockValue, Vector3 _blockPos)
	{
		Block.staticList_IntersectRayWithBlockList.Clear();
		this.GetCollisionAABB(_blockValue, (int)_blockPos.x, (int)_blockPos.y, (int)_blockPos.z, 0f, Block.staticList_IntersectRayWithBlockList);
		return Block.staticList_IntersectRayWithBlockList;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00036209 File Offset: 0x00034409
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _e)
	{
		_e.fallDistance = Mathf.Max(_e.fallDistance - 5f, 0f);
		return true;
	}
}
