using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200081D RID: 2077
[Preserve]
public class NetPackagePlayerInventory : NetPackage
{
	// Token: 0x06003CF4 RID: 15604 RVA: 0x00183678 File Offset: 0x00181878
	public NetPackagePlayerInventory Setup(EntityPlayerLocal _player, bool _changedToolbelt, bool _changedBag, bool _changedEquipment, bool _changedDragAndDropItem)
	{
		if (_changedToolbelt)
		{
			this.toolbelt = ((_player.AttachedToEntity != null && _player.saveInventory != null) ? ItemStack.Clone(_player.saveInventory.GetSlots()) : ItemStack.Clone(_player.inventory.GetSlots()));
		}
		if (_changedBag)
		{
			this.bag = _player.bag.Clone();
		}
		if (_changedEquipment)
		{
			this.equipment = _player.equipment.Clone();
		}
		if (_changedDragAndDropItem)
		{
			this.dragAndDropItem = _player.DragAndDropItem.Clone();
		}
		return this;
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06003CF5 RID: 15605 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x00183708 File Offset: 0x00181908
	public override void read(PooledBinaryReader _reader)
	{
		if (_reader.ReadBoolean())
		{
			this.toolbelt = GameUtils.ReadItemStack(_reader);
		}
		if (_reader.ReadBoolean())
		{
			this.bag = Bag.Read(_reader);
		}
		if (_reader.ReadBoolean())
		{
			ItemValue[] array = GameUtils.ReadItemValueArray(_reader);
			this.equipment = new Equipment();
			int num = Utils.FastMin(array.Length, this.equipment.GetSlotCount());
			for (int i = 0; i < num; i++)
			{
				this.equipment.SetSlotItemRaw(i, array[i]);
			}
			for (int j = 0; j < num; j++)
			{
				this.equipment.SetCosmeticSlot(j, _reader.ReadInt32());
			}
			int num2 = _reader.ReadInt32();
			for (int k = 0; k < num2; k++)
			{
				this.equipment.m_unlockedCosmetics.Add(_reader.ReadInt32());
			}
		}
		if (_reader.ReadBoolean())
		{
			ItemStack[] array2 = GameUtils.ReadItemStack(_reader);
			if (array2 != null && array2.Length != 0)
			{
				this.dragAndDropItem = array2[0];
			}
		}
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x001837FC File Offset: 0x001819FC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.toolbelt != null);
		if (this.toolbelt != null)
		{
			GameUtils.WriteItemStack(_writer, this.toolbelt);
		}
		_writer.Write(this.bag != null);
		if (this.bag != null)
		{
			this.bag.Write(_writer);
		}
		_writer.Write(this.equipment != null);
		if (this.equipment != null)
		{
			GameUtils.WriteItemValueArray(_writer, this.equipment.GetItems());
			int[] cosmeticIDs = this.equipment.GetCosmeticIDs();
			for (int i = 0; i < cosmeticIDs.Length; i++)
			{
				_writer.Write(cosmeticIDs[i]);
			}
			List<int> unlockedCosmetics = this.equipment.m_unlockedCosmetics;
			_writer.Write(unlockedCosmetics.Count);
			for (int j = 0; j < unlockedCosmetics.Count; j++)
			{
				_writer.Write(unlockedCosmetics[j]);
			}
		}
		_writer.Write(this.dragAndDropItem != null);
		if (this.dragAndDropItem != null)
		{
			GameUtils.WriteItemStack(_writer, new ItemStack[] { this.dragAndDropItem });
		}
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00183904 File Offset: 0x00181B04
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		PlayerDataFile latestPlayerData = base.Sender.latestPlayerData;
		if (this.toolbelt != null)
		{
			latestPlayerData.inventory = this.toolbelt;
		}
		if (this.bag != null)
		{
			latestPlayerData.bag = this.bag;
		}
		if (this.equipment != null)
		{
			latestPlayerData.equipment = this.equipment;
		}
		if (this.dragAndDropItem != null)
		{
			latestPlayerData.dragAndDropItem = this.dragAndDropItem;
		}
		latestPlayerData.bModifiedSinceLastSave = true;
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003142 RID: 12610
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] toolbelt;

	// Token: 0x04003143 RID: 12611
	[PublicizedFrom(EAccessModifier.Private)]
	public Bag bag;

	// Token: 0x04003144 RID: 12612
	[PublicizedFrom(EAccessModifier.Private)]
	public Equipment equipment;

	// Token: 0x04003145 RID: 12613
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack dragAndDropItem;
}
