using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000141 RID: 321
[Preserve]
public class BlockBladeTrap : BlockPoweredTrap
{
	// Token: 0x060008C9 RID: 2249 RVA: 0x0003DC34 File Offset: 0x0003BE34
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

	// Token: 0x060008CA RID: 2250 RVA: 0x0003DCE0 File Offset: 0x0003BEE0
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

	// Token: 0x060008CB RID: 2251 RVA: 0x0003DD54 File Offset: 0x0003BF54
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

	// Token: 0x060008CC RID: 2252 RVA: 0x0003DDE8 File Offset: 0x0003BFE8
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSound);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSoundPartlyBroken);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSoundBroken);
		base.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003DE38 File Offset: 0x0003C038
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		Manager.Stop(_blockPos.ToVector3(), this.runningSound);
		Manager.Stop(_blockPos.ToVector3(), this.runningSoundPartlyBroken);
		Manager.Stop(_blockPos.ToVector3(), this.runningSoundBroken);
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
	}

	// Token: 0x0400098B RID: 2443
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "Electricity/BladeTrap/bladetrap_fire_lp";

	// Token: 0x0400098C RID: 2444
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSoundPartlyBroken = "Electricity/BladeTrap/bladetrap_dm1_lp";

	// Token: 0x0400098D RID: 2445
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSoundBroken = "Electricity/BladeTrap/bladetrap_dm2_lp";
}
