using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000152 RID: 338
[Preserve]
public class BlockSpikes : BlockDamage
{
	// Token: 0x06000955 RID: 2389 RVA: 0x00040EC0 File Offset: 0x0003F0C0
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey(BlockSpikes.PropDontDamageOnTouch))
		{
			this.bDontDamageOnTouch = StringParsers.ParseBool(base.Properties.Values[BlockSpikes.PropDontDamageOnTouch], 0, -1, true);
		}
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00040F10 File Offset: 0x0003F110
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

	// Token: 0x06000957 RID: 2391 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMovementBlocked(IBlockAccess _world, Vector3i _blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return true;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x00040F84 File Offset: 0x0003F184
	public override float GetStepHeight(IBlockAccess world, Vector3i blockPos, BlockValue _blockValue, BlockFace crossingFace)
	{
		return 1f;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00040F8C File Offset: 0x0003F18C
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck) && _world.GetBlock(_blockPos - Vector3i.up).Block.shape.IsSolidCube;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00040FCC File Offset: 0x0003F1CC
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

	// Token: 0x040009BC RID: 2492
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropDontDamageOnTouch = "DontDamageOnTouch";

	// Token: 0x040009BD RID: 2493
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDontDamageOnTouch;
}
