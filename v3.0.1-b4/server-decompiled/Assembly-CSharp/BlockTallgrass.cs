using System;
using UnityEngine.Scripting;

// Token: 0x02000156 RID: 342
[Preserve]
public class BlockTallgrass : BlockPlant
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00041A2C File Offset: 0x0003FC2C
	public BlockTallgrass()
	{
		this.IsRandomlyTick = false;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00041A3B File Offset: 0x0003FC3B
	public override bool CheckPlantAlive(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (!this.CanPlantStay(_world, _blockPos, _blockValue))
		{
			_world.SetBlockRPC(_blockPos, BlockValue.Air);
			return false;
		}
		return true;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00041A5C File Offset: 0x0003FC5C
	public override void OnBlockPlaceBefore(WorldBase _world, ref BlockPlacement.Result _bpResult, EntityAlive _ea, GameRandom _rnd)
	{
		_bpResult.blockValue.meta2and1 = this.CalcMeta(_rnd);
		_bpResult.blockValue.rotation = (byte)_rnd.RandomRange(32);
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00041A86 File Offset: 0x0003FC86
	public override BlockValue OnBlockPlaced(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, GameRandom _rnd)
	{
		_blockValue.meta2and1 = this.CalcMeta(_rnd);
		_blockValue.rotation = (byte)_rnd.RandomRange(32);
		return _blockValue;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00041AA9 File Offset: 0x0003FCA9
	[PublicizedFrom(EAccessModifier.Private)]
	public byte CalcMeta(GameRandom _rnd)
	{
		return (byte)(_rnd.RandomRange(6) | (_rnd.RandomRange(256) & -8));
	}
}
