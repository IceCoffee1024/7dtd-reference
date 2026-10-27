using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000832 RID: 2098
[Preserve]
public class NetPackagePlayerInventory : NetPackage
{
	// Token: 0x06003D36 RID: 15670 RVA: 0x001867A4 File Offset: 0x001849A4
	public NetPackagePlayerInventory Setup(EntityPlayerLocal _player, bool _changedToolbelt, bool _changedBag, bool _changedEquipment, bool _changedDragAndDropItem)
	{
		if (_changedToolbelt)
		{
			this.inventoryData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
			{
				_player.inventory.Write(pbw, StreamModeWrite.Persistency);
			});
		}
		if (_changedBag)
		{
			this.bagData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
			{
				_player.bag.Write(pbw, StreamModeWrite.Persistency);
			});
		}
		if (_changedEquipment)
		{
			this.equipmentData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
			{
				_player.equipment.Write(pbw, StreamModeWrite.Persistency);
			});
		}
		if (_changedDragAndDropItem)
		{
			this.dragAndDropItem = _player.DragAndDropItem.Clone();
		}
		return this;
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06003D37 RID: 15671 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x00186828 File Offset: 0x00184A28
	public override void read(PooledBinaryReader _reader)
	{
		if (_reader.ReadBoolean())
		{
			this.inventoryData = StreamUtils.ReadBlob(_reader);
		}
		if (_reader.ReadBoolean())
		{
			this.bagData = StreamUtils.ReadBlob(_reader);
		}
		if (_reader.ReadBoolean())
		{
			this.equipmentData = StreamUtils.ReadBlob(_reader);
		}
		if (_reader.ReadBoolean())
		{
			ItemStack[] array = GameUtils.ReadItemStack(_reader);
			if (array != null && array.Length != 0)
			{
				this.dragAndDropItem = array[0];
			}
		}
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x00186890 File Offset: 0x00184A90
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.inventoryData != null);
		if (this.inventoryData != null)
		{
			StreamUtils.WriteBlob(_writer, this.inventoryData);
		}
		_writer.Write(this.bagData != null);
		if (this.bagData != null)
		{
			StreamUtils.WriteBlob(_writer, this.bagData);
		}
		_writer.Write(this.equipmentData != null);
		if (this.equipmentData != null)
		{
			StreamUtils.WriteBlob(_writer, this.equipmentData);
		}
		_writer.Write(this.dragAndDropItem != null);
		if (this.dragAndDropItem != null)
		{
			GameUtils.WriteItemStack(_writer, new ItemStack[] { this.dragAndDropItem });
		}
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x0018693C File Offset: 0x00184B3C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		PlayerDataFile latestPlayerData = base.Sender.latestPlayerData;
		if (this.inventoryData != null)
		{
			latestPlayerData.inventoryData = this.inventoryData;
		}
		if (this.bagData != null)
		{
			latestPlayerData.bagData = this.bagData;
		}
		if (this.equipmentData != null)
		{
			latestPlayerData.equipmentData = this.equipmentData;
		}
		if (this.dragAndDropItem != null)
		{
			latestPlayerData.dragAndDropItem = this.dragAndDropItem;
		}
		latestPlayerData.bModifiedSinceLastSave = true;
	}

	// Token: 0x040031C7 RID: 12743
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream inventoryData;

	// Token: 0x040031C8 RID: 12744
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream bagData;

	// Token: 0x040031C9 RID: 12745
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream equipmentData;

	// Token: 0x040031CA RID: 12746
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack dragAndDropItem;
}
