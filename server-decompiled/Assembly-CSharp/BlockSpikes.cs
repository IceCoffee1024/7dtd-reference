using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000154 RID: 340
[Preserve]
public class BlockSpikes : BlockDamage
{
	// Token: 0x0600095B RID: 2395 RVA: 0x00040FA0 File Offset: 0x0003F1A0
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockSpikes.PropDontDamageOnTouch))
		{
			this.bDontDamageOnTouch = StringParsers.ParseBool(base.Properties.Values[BlockSpikes.PropDontDamageOnTouch], 0, -1, true);
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00040FF0 File Offset: 0x0003F1F0
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

	// Token: 0x0600095D RID: 2397 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return true;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00041064 File Offset: 0x0003F264
	public override float GetStepHeight(IBlockAccess world, Vector3i blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return 1f;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0004106C File Offset: 0x0003F26C
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck) && _world.GetBlock(_blockPos - Vector3i.up).Block.shape.IsSolidCube;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000410AC File Offset: 0x0003F2AC
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

	// Token: 0x040009BF RID: 2495
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDontDamageOnTouch = "DontDamageOnTouch";

	// Token: 0x040009C0 RID: 2496
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDontDamageOnTouch;
}
