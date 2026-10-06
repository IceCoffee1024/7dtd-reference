using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000828 RID: 2088
[Preserve]
public class NetPackagePlayerInventory : NetPackage
{
	// Token: 0x06003D13 RID: 15635 RVA: 0x001856A0 File Offset: 0x001838A0
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

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06003D14 RID: 15636 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x00185724 File Offset: 0x00183924
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

	// Token: 0x06003D16 RID: 15638 RVA: 0x0018578C File Offset: 0x0018398C
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

	// Token: 0x06003D17 RID: 15639 RVA: 0x00185838 File Offset: 0x00183A38
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

	// Token: 0x040031A7 RID: 12711
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream inventoryData;

	// Token: 0x040031A8 RID: 12712
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream bagData;

	// Token: 0x040031A9 RID: 12713
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream equipmentData;

	// Token: 0x040031AA RID: 12714
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack dragAndDropItem;
}
