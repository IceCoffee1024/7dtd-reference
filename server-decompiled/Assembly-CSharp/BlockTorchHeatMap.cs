using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200015E RID: 350
[Preserve]
public class BlockTorchHeatMap : BlockTorch
{
	// Token: 0x0600099B RID: 2459 RVA: 0x0004231D File Offset: 0x0004051D
	public BlockTorchHeatMap()
	{
		this.IsRandomlyTick = true;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0004232C File Offset: 0x0004052C
	public override bool UpdateTick(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bRandomTick, ulong _ticksIfLoaded, GameRandom _rnd)
	{
		base.UpdateTick(_world, _blockPos, _blockValue, _bRandomTick, _ticksIfLoaded, _rnd);
		if (this.HeatMapStrength > 0f)
		{
			AIDirector aidirector = _world.GetAIDirector();
			if (aidirector != null)
			{
				float num = 1f;
				num *= 0.4f;
				aidirector.NotifyActivity(EnumAIDirectorChunkEvent.Torch, _blockPos, this.HeatMapStrength * num, 720f);
			}
		}
		return true;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00042384 File Offset: 0x00040584
	public override void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformAfterActivated(_world, _blockPos, _blockValue, _ebcd);
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		IChunk chunkSync = chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), World.toChunkY(_blockPos.y), World.toChunkXZ(_blockPos.z));
		if (chunkSync == null)
		{
			return;
		}
		BlockEntityData blockEntity = chunkSync.GetBlockEntity(_blockPos);
		if (blockEntity == null || !blockEntity.bHasTransform)
		{
			return;
		}
		Transform transform = blockEntity.transform.FindInChildren("MainLight");
		if (transform)
		{
			LightLOD component = transform.GetComponent<LightLOD>();
			if (component)
			{
				component.SetBlockEntityData(blockEntity);
			}
		}
	}
}
