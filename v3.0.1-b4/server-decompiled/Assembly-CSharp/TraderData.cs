using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200089E RID: 2206
public class TraderData
{
	// Token: 0x06003FF6 RID: 16374 RVA: 0x0018FF9C File Offset: 0x0018E19C
	public TraderData Clone()
	{
		TraderData traderData = new TraderData();
		traderData.CopyFrom(this);
		return traderData;
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x0018FFAC File Offset: 0x0018E1AC
	public void CopyFrom(TraderData other)
	{
		if (other == null)
		{
			return;
		}
		this.lastInventoryUpdate = other.lastInventoryUpdate;
		this.TraderID = other.TraderID;
		this.AvailableMoney = other.AvailableMoney;
		this.PrimaryInventory.Clear();
		for (int i = 0; i < other.PrimaryInventory.Count; i++)
		{
			this.PrimaryInventoryAdd(other.PrimaryInventory[i].Clone());
		}
		this.TierItemGroups.Clear();
		for (int j = 0; j < other.TierItemGroups.Count; j++)
		{
			this.TierItemGroups.Add(ItemStack.Clone(other.TierItemGroups[j]));
		}
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00190056 File Offset: 0x0018E256
	public void SetModified(ITrader _trader)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTraderData>().Setup(_trader), false);
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x0019007B File Offset: 0x0018E27B
	public TraderInfo TraderInfo
	{
		get
		{
			if (this.TraderID != -1)
			{
				return TraderInfo.traderInfoList[this.TraderID];
			}
			return null;
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06003FFA RID: 16378 RVA: 0x00190094 File Offset: 0x0018E294
	public float FullTime
	{
		get
		{
			return (float)((this.TraderInfo != null) ? this.TraderInfo.ResetIntervalInTicks : 0);
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06003FFB RID: 16379 RVA: 0x001900AD File Offset: 0x0018E2AD
	public float CurrentTime
	{
		get
		{
			if (this.lastInventoryUpdate == 0UL)
			{
				return 0f;
			}
			return (this.FullTime - (float)((int)(GameManager.Instance.World.GetWorldTime() - this.lastInventoryUpdate))) / 10f;
		}
	}

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x06003FFC RID: 16380 RVA: 0x001900E2 File Offset: 0x0018E2E2
	public ulong NextResetTime
	{
		get
		{
			if (this.TraderInfo == null)
			{
				return 0UL;
			}
			return this.lastInventoryUpdate + (ulong)((long)this.TraderInfo.ResetIntervalInTicks);
		}
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x00190104 File Offset: 0x0018E304
	public void PrimaryInventoryAdd(TraderData.Entry _entry)
	{
		if (_entry.Item.count <= 0 || _entry.Item.itemValue.type == 0)
		{
			Log.Warning("TraderData PrimaryInventoryAdd count {0}, type {1}", new object[]
			{
				_entry.Item.count,
				_entry.Item.itemValue.type
			});
			return;
		}
		this.PrimaryInventory.Add(_entry);
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x0019017C File Offset: 0x0018E37C
	public void AddToPrimaryInventory(ItemStack stack, bool addedByPlayer)
	{
		for (int i = 0; i < this.PrimaryInventory.Count; i++)
		{
			if (stack.itemValue.type == this.PrimaryInventory[i].Item.itemValue.type && this.PrimaryInventory[i].AddedByPlayer == addedByPlayer)
			{
				ItemClass forId = ItemClass.GetForId(stack.itemValue.type);
				if (forId.CanStack())
				{
					int num = Math.Min(stack.count, forId.Stacknumber.Value - this.PrimaryInventory[i].Item.count);
					stack.count -= num;
					this.PrimaryInventory[i].Item.count += num;
					if (stack.count == 0)
					{
						return;
					}
				}
			}
		}
		if (stack.count > 0)
		{
			this.PrimaryInventoryAdd(new TraderData.Entry(stack.Clone(), 0, addedByPlayer));
		}
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x0019027C File Offset: 0x0018E47C
	public int GetPrimaryItemCount(ItemValue itemValue)
	{
		int num = 0;
		for (int i = 0; i < this.PrimaryInventory.Count; i++)
		{
			if (itemValue.type == this.PrimaryInventory[i].Item.itemValue.type)
			{
				num += this.PrimaryInventory[i].Item.count;
			}
		}
		return num;
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x001902E0 File Offset: 0x0018E4E0
	public void Read(BinaryReader _br)
	{
		this.TraderID = _br.ReadInt32();
		this.lastInventoryUpdate = _br.ReadUInt64();
		byte b = _br.ReadByte();
		this.ReadInventoryData(b, _br);
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x00190314 File Offset: 0x0018E514
	public void ReadInventoryData(byte _readVersion, BinaryReader _br)
	{
		this.PrimaryInventory.Clear();
		this.TierItemGroups.Clear();
		if (_readVersion < 2)
		{
			ItemStack[] array = GameUtils.ReadItemStack(_br);
			int num = (int)_br.ReadByte();
			for (int i = 0; i < num; i++)
			{
				this.TierItemGroups.Add(GameUtils.ReadItemStack(_br));
			}
			this.AvailableMoney = _br.ReadInt32();
			int num2 = _br.ReadInt32();
			sbyte[] array2 = new sbyte[num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = _br.ReadSByte();
			}
			for (int k = 0; k < array.Length; k++)
			{
				sbyte b = ((k < array2.Length) ? array2[k] : 0);
				this.PrimaryInventoryAdd(new TraderData.Entry(array[k], b, false));
			}
			return;
		}
		int num3 = _br.ReadInt32();
		for (int l = 0; l < num3; l++)
		{
			TraderData.Entry entry = new TraderData.Entry();
			entry.Read(_br);
			this.PrimaryInventoryAdd(entry);
		}
		int num4 = (int)_br.ReadByte();
		for (int m = 0; m < num4; m++)
		{
			this.TierItemGroups.Add(GameUtils.ReadItemStack(_br));
		}
		this.AvailableMoney = _br.ReadInt32();
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x0019043F File Offset: 0x0018E63F
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.TraderID);
		_bw.Write(this.lastInventoryUpdate);
		_bw.Write(TraderData.FileVersion);
		this.WriteInventoryData(_bw);
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x0019046C File Offset: 0x0018E66C
	public void WriteInventoryData(BinaryWriter _bw)
	{
		_bw.Write(this.PrimaryInventory.Count);
		for (int i = 0; i < this.PrimaryInventory.Count; i++)
		{
			this.PrimaryInventory[i].Write(_bw);
		}
		_bw.Write((byte)this.TierItemGroups.Count);
		for (int j = 0; j < this.TierItemGroups.Count; j++)
		{
			GameUtils.WriteItemStack(_bw, this.TierItemGroups[j]);
		}
		_bw.Write(this.AvailableMoney);
	}

	// Token: 0x04003371 RID: 13169
	public List<TraderData.Entry> PrimaryInventory = new List<TraderData.Entry>();

	// Token: 0x04003372 RID: 13170
	public List<ItemStack[]> TierItemGroups = new List<ItemStack[]>();

	// Token: 0x04003373 RID: 13171
	public ulong lastInventoryUpdate;

	// Token: 0x04003374 RID: 13172
	public int TraderID = -1;

	// Token: 0x04003375 RID: 13173
	public int AvailableMoney;

	// Token: 0x04003376 RID: 13174
	public static byte FileVersion = 2;

	// Token: 0x0200089F RID: 2207
	public class Entry
	{
		// Token: 0x06004006 RID: 16390 RVA: 0x0000640C File Offset: 0x0000460C
		public Entry()
		{
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00190525 File Offset: 0x0018E725
		public Entry(ItemStack _item, sbyte _markup = 0, bool _addedByPlayer = false)
		{
			this.Item = _item;
			this.Markup = _markup;
			this.AddedByPlayer = _addedByPlayer;
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x00190542 File Offset: 0x0018E742
		public TraderData.Entry Clone()
		{
			return new TraderData.Entry(this.Item.Clone(), this.Markup, this.AddedByPlayer);
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x00190560 File Offset: 0x0018E760
		public void IncreaseMarkup()
		{
			if (this.Markup < 100)
			{
				this.Markup += 1;
			}
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0019057B File Offset: 0x0018E77B
		public void DecreaseMarkup()
		{
			if (this.Markup > -4)
			{
				this.Markup -= 1;
			}
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x00190596 File Offset: 0x0018E796
		public void Read(BinaryReader _br)
		{
			this.Item = new ItemStack().Read(_br);
			this.Markup = _br.ReadSByte();
			this.AddedByPlayer = _br.ReadBoolean();
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x001905C1 File Offset: 0x0018E7C1
		public void Write(BinaryWriter _bw)
		{
			this.Item.Write(_bw);
			_bw.Write(this.Markup);
			_bw.Write(this.AddedByPlayer);
		}

		// Token: 0x04003377 RID: 13175
		public ItemStack Item;

		// Token: 0x04003378 RID: 13176
		public sbyte Markup;

		// Token: 0x04003379 RID: 13177
		public bool AddedByPlayer;
	}
}
