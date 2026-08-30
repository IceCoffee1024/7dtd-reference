using System;
using UnityEngine.Scripting;

// Token: 0x02000111 RID: 273
[Preserve]
public class BlockBarbed : BlockDamage
{
	// Token: 0x06000733 RID: 1843 RVA: 0x00033E40 File Offset: 0x00032040
	public override bool OnEntityCollidedWithBlock(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, Entity _targetEntity)
	{
		if (!base.OnEntityCollidedWithBlock(_world, _blockPos, _blockValue, _targetEntity))
		{
			return false;
		}
		byte meta = _blockValue.meta;
		_blockValue.meta = meta + 1;
		if (_blockValue.meta == 15)
		{
			this.DamageBlock(_world, _blockPos, _blockValue, _blockValue.Block.MaxDamage, (_targetEntity != null) ? _targetEntity.entityId : (-1), null, false, false);
		}
		else
		{
			_world.SetBlockRPC(_blockPos, _blockValue);
		}
		return true;
	}
}
