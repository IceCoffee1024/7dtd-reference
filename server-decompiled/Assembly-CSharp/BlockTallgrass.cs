using System;
using UnityEngine.Scripting;

// Token: 0x02000159 RID: 345
[Preserve]
public class BlockTallgrass : BlockPlant
{
	// Token: 0x06000981 RID: 2433 RVA: 0x00041D2C File Offset: 0x0003FF2C
	public BlockTallgrass()
	{
		this.IsRandomlyTick = false;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00041D3B File Offset: 0x0003FF3B
	public override bool CheckPlantAlive(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!this.CanPlantStay(_world, _blockPos, _blockValue))
		{
			_world.SetBlockRPC(_blockPos, BlockValue.Air);
			return false;
		}
		return true;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00041D5C File Offset: 0x0003FF5C
	public override void OnBlockPlaceBefore(WorldBase _world, ref BlockPlacement.Result _bpResult, EntityAlive _ea, GameRandom _rnd)
	{
		_bpResult.blockValue.meta2and1 = this.CalcMeta(_rnd);
		_bpResult.blockValue.rotation = (byte)_rnd.RandomRange(32);
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00041D86 File Offset: 0x0003FF86
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.meta2and1 = this.CalcMeta(_rnd);
		_blockValue.rotation = (byte)_rnd.RandomRange(32);
		return _blockValue;
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00041DA9 File Offset: 0x0003FFA9
	[PublicizedFrom(EAccessModifier.Private)]
	public byte CalcMeta(GameRandom _rnd)
	{
		return (byte)(_rnd.RandomRange(6) | (_rnd.RandomRange(256) & -8));
	}
}
