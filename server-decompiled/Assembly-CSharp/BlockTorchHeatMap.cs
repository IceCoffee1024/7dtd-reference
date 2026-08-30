using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200015B RID: 347
[Preserve]
public class BlockTorchHeatMap : BlockTorch
{
	// Token: 0x0600098B RID: 2443 RVA: 0x00041FE1 File Offset: 0x000401E1
	public BlockTorchHeatMap()
	{
		this.IsRandomlyTick = true;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00041FF0 File Offset: 0x000401F0
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

	// Token: 0x0600098D RID: 2445 RVA: 0x00042048 File Offset: 0x00040248
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
