using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000145 RID: 325
[Preserve]
public class BlockSolarPanel : BlockPowerSource
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x0003E905 File Offset: 0x0003CB05
	public override TileEntityPowerSource CreateTileEntity(Chunk chunk)
	{
		if (this.slotItem == null)
		{
			this.slotItem = ItemClass.GetItemClass(this.SlotItemName, false);
		}
		return new TileEntityPowerSource(chunk)
		{
			PowerItemType = PowerItem.PowerItemTypes.SolarPanel,
			SlotItem = this.slotItem
		};
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003E93C File Offset: 0x0003CB3C
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		if (!base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck))
		{
			return false;
		}
		Vector3i vector3i = _blockPos + Vector3i.up;
		ChunkCluster chunkCache = _world.ChunkCache;
		return chunkCache == null || chunkCache.GetLight(vector3i, Chunk.LIGHT_TYPE.SUN) >= 15;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0003E97D File Offset: 0x0003CB7D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "electric_solar";
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0003E984 File Offset: 0x0003CB84
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSound);
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0003E9A3 File Offset: 0x0003CBA3
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
		Manager.Stop(_blockPos.ToVector3(), this.runningSound);
	}

	// Token: 0x04000997 RID: 2455
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";
}
