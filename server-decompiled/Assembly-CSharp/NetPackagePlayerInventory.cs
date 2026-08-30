using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000828 RID: 2088
[Preserve]
public class NetPackagePlayerInventory : NetPackage
{
	// Token: 0x06003D53 RID: 15699 RVA: 0x00185A50 File Offset: 0x00183C50
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

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06003D54 RID: 15700 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00185AE0 File Offset: 0x00183CE0
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

	// Token: 0x06003D56 RID: 15702 RVA: 0x00185BD4 File Offset: 0x00183DD4
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

	// Token: 0x06003D57 RID: 15703 RVA: 0x00185CDC File Offset: 0x00183EDC
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

	// Token: 0x06003D58 RID: 15704 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040031C1 RID: 12737
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] toolbelt;

	// Token: 0x040031C2 RID: 12738
	[PublicizedFrom(EAccessModifier.Private)]
	public Bag bag;

	// Token: 0x040031C3 RID: 12739
	[PublicizedFrom(EAccessModifier.Private)]
	public Equipment equipment;

	// Token: 0x040031C4 RID: 12740
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack dragAndDropItem;
}
