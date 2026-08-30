using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000151 RID: 337
[Preserve]
public class BlockSpikes : BlockDamage
{
	// Token: 0x0600094B RID: 2379 RVA: 0x00040C60 File Offset: 0x0003EE60
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockSpikes.PropDontDamageOnTouch))
		{
			this.bDontDamageOnTouch = StringParsers.ParseBool(base.Properties.Values[BlockSpikes.PropDontDamageOnTouch], 0, -1, true);
		}
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00040CB0 File Offset: 0x0003EEB0
	public override void GetCollisionAABB(BlockValue _blockValue, int _x, int _y, int _z, float _distortedY, List<Bounds> _result)
	{
		base.GetCollisionAABB(_blockValue, _x, _y, _z, _distortedY, _result);
		Vector3 vector = new Vector3(-0.3f, -0.2f, -0.3f);
		for (int i = 0; i < _result.Count; i++)
		{
			Bounds bounds = _result[i];
			bounds.extents = Vector3.Max(bounds.extents + vector, Vector3.zero);
			_result[i] = bounds;
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return true;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00040D24 File Offset: 0x0003EF24
	public override float GetStepHeight(IBlockAccess world, Vector3i blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return 1f;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00040D2C File Offset: 0x0003EF2C
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck) && _world.GetBlock(_blockPos - Vector3i.up).Block.shape.IsSolidCube;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00040D6C File Offset: 0x0003EF6C
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _targetEntity)
	{
		if (!base.OnEntityCollidedWithBlock(_world, _blockPos, _blockValue, _targetEntity))
		{
			return false;
		}
		BlockValue block = _world.GetBlock(_blockPos);
		if (!this.SiblingBlock.isair)
		{
			block.type = this.SiblingBlock.type;
			block.damage = 0;
			_world.SetBlockRPC(_blockPos, block);
		}
		else
		{
			_world.SetBlockRPC(_blockPos, BlockValue.Air);
		}
		return true;
	}

	// Token: 0x040009BA RID: 2490
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDontDamageOnTouch = "DontDamageOnTouch";

	// Token: 0x040009BB RID: 2491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDontDamageOnTouch;
}
