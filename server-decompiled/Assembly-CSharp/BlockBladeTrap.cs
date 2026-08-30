using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200013E RID: 318
[Preserve]
public class BlockBladeTrap : BlockPoweredTrap
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x0003D8E8 File Offset: 0x0003BAE8
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("RunningSound"))
		{
			this.runningSound = base.Properties.Values["RunningSound"];
		}
		if (base.Properties.Values.ContainsKey("RunningSoundBreaking"))
		{
			this.runningSoundPartlyBroken = base.Properties.Values["RunningSoundBreaking"];
		}
		if (base.Properties.Values.ContainsKey("RunningSoundBroken"))
		{
			this.runningSoundBroken = base.Properties.Values["RunningSoundBroken"];
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0003D994 File Offset: 0x0003BB94
	public override bool ActivateTrap(BlockEntityData blockEntity, bool isOn)
	{
		SpinningBladeTrapController component = blockEntity.transform.gameObject.GetComponent<SpinningBladeTrapController>();
		if (component == null)
		{
			return false;
		}
		component.Init(base.Properties, this);
		component.BlockPosition = blockEntity.pos;
		component.HealthRatio = 1f - (float)blockEntity.blockValue.damage / (float)blockEntity.blockValue.Block.MaxDamage;
		component.IsOn = isOn;
		return true;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003DA08 File Offset: 0x0003BC08
	public override int OnBlockDamaged(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _damagePoints, int _entityIdThatDamaged, ItemActionAttack.AttackHitInfo _attackHitInfo, bool _bUseHarvestTool, bool _bBypassMaxDamage, int _recDepth = 0)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache != null)
		{
			IChunk chunkSync = chunkCache.GetChunkSync(_bvRef);
			if (chunkSync != null)
			{
				BlockEntityData blockEntity = chunkSync.GetBlockEntity(_bvRef);
				if (blockEntity != null && blockEntity.bHasTransform)
				{
					SpinningBladeTrapController component = blockEntity.transform.gameObject.GetComponent<SpinningBladeTrapController>();
					if (component != null)
					{
						component.HealthRatio = 1f - (float)blockEntity.blockValue.damage / (float)blockEntity.blockValue.Block.MaxDamage;
					}
				}
			}
		}
		return base.OnBlockDamaged(_world, _bvRef, _blockValue, _damagePoints, _entityIdThatDamaged, _attackHitInfo, _bUseHarvestTool, _bBypassMaxDamage, _recDepth);
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0003DA9C File Offset: 0x0003BC9C
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSound);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSoundPartlyBroken);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSoundBroken);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0003DAEC File Offset: 0x0003BCEC
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		Manager.Stop(_blockPos.ToVector3(), this.runningSound);
		Manager.Stop(_blockPos.ToVector3(), this.runningSoundPartlyBroken);
		Manager.Stop(_blockPos.ToVector3(), this.runningSoundBroken);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x04000986 RID: 2438
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "Electricity/BladeTrap/bladetrap_fire_lp";

	// Token: 0x04000987 RID: 2439
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSoundPartlyBroken = "Electricity/BladeTrap/bladetrap_dm1_lp";

	// Token: 0x04000988 RID: 2440
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSoundBroken = "Electricity/BladeTrap/bladetrap_dm2_lp";
}
