using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000142 RID: 322
[Preserve]
public class BlockSolarPanel : BlockPowerSource
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x0003E5BD File Offset: 0x0003C7BD
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

	// Token: 0x060008DA RID: 2266 RVA: 0x0003E5F4 File Offset: 0x0003C7F4
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

	// Token: 0x060008DB RID: 2267 RVA: 0x0003E635 File Offset: 0x0003C835
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string GetPowerSourceIcon()
	{
		return "electric_solar";
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003E63C File Offset: 0x0003C83C
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		Manager.BroadcastStop(_blockPos.ToVector3(), this.runningSound);
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003E65B File Offset: 0x0003C85B
	public override void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockUnloaded(_world, _blockPos, _blockValue);
		Manager.Stop(_blockPos.ToVector3(), this.runningSound);
	}

	// Token: 0x04000992 RID: 2450
	[PublicizedFrom(EAccessModifier.Private)]
	public string runningSound = "solarpanel_idle";
}
