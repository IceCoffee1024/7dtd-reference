using System;
using System.Collections.Generic;

// Token: 0x020008AF RID: 2223
public class TraderData
{
	// Token: 0x06003FF1 RID: 16369 RVA: 0x00192170 File Offset: 0x00190370
	public TraderData Clone()
	{
		TraderData traderData = new TraderData();
		traderData.CopyFrom(this);
		return traderData;
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x00192180 File Offset: 0x00190380
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

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0019222A File Offset: 0x0019042A
	public void SetModified(ITrader _trader)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTraderData>().Setup(_trader), false);
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x0019224F File Offset: 0x0019044F
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

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x00192268 File Offset: 0x00190468
	public float FullTime
	{
		get
		{
			return (float)((this.TraderInfo != null) ? this.TraderInfo.ResetIntervalInTicks : 0);
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x00192281 File Offset: 0x00190481
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

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x001922B6 File Offset: 0x001904B6
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

	// Token: 0x06003FF8 RID: 16376 RVA: 0x001922D8 File Offset: 0x001904D8
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

	// Token: 0x06003FF9 RID: 16377 RVA: 0x00192350 File Offset: 0x00190550
	public void AddToPrimaryInventory(ItemStack stack, bool addedByPlayer)
	{
		for (int i = 0; i < this.PrimaryInventory.Count; i++)
		{
			if (stack.itemValue.type == this.PrimaryInventory[i].Item.itemValue.type && this.PrimaryInventory[i].AddedByPlayer == addedByPlayer)
			{
				ItemClass forId = ItemClass.GetForId(stack.itemValue.type);
				if (forId.CanStack())
				{
					int num = Math.Min(stack.count, forId.MaxCount - this.PrimaryInventory[i].Item.count);
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

	// Token: 0x06003FFA RID: 16378 RVA: 0x0019244C File Offset: 0x0019064C
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

	// Token: 0x06003FFB RID: 16379 RVA: 0x001924B0 File Offset: 0x001906B0
	public void Read(PooledBinaryReader _br)
	{
		this.TraderID = _br.ReadInt32();
		this.lastInventoryUpdate = _br.ReadUInt64();
		byte b = _br.ReadByte();
		this.ReadInventoryData(b, _br);
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x001924E4 File Offset: 0x001906E4
	public void ReadInventoryData(byte _readVersion, PooledBinaryReader _br)
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

	// Token: 0x06003FFD RID: 16381 RVA: 0x0019260F File Offset: 0x0019080F
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.TraderID);
		_bw.Write(this.lastInventoryUpdate);
		_bw.Write(TraderData.FileVersion);
		this.WriteInventoryData(_bw);
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x0019263C File Offset: 0x0019083C
	public void WriteInventoryData(PooledBinaryWriter _bw)
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

	// Token: 0x040033E6 RID: 13286
	public List<TraderData.Entry> PrimaryInventory = new List<TraderData.Entry>();

	// Token: 0x040033E7 RID: 13287
	public List<ItemStack[]> TierItemGroups = new List<ItemStack[]>();

	// Token: 0x040033E8 RID: 13288
	public ulong lastInventoryUpdate;

	// Token: 0x040033E9 RID: 13289
	public int TraderID = -1;

	// Token: 0x040033EA RID: 13290
	public int AvailableMoney;

	// Token: 0x040033EB RID: 13291
	public static byte FileVersion = 2;

	// Token: 0x020008B0 RID: 2224
	public class Entry
	{
		// Token: 0x06004001 RID: 16385 RVA: 0x0000640C File Offset: 0x0000460C
		public Entry()
		{
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x001926F5 File Offset: 0x001908F5
		public Entry(ItemStack _item, sbyte _markup = 0, bool _addedByPlayer = false)
		{
			this.Item = _item;
			this.Markup = _markup;
			this.AddedByPlayer = _addedByPlayer;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00192712 File Offset: 0x00190912
		public TraderData.Entry Clone()
		{
			return new TraderData.Entry(this.Item.Clone(), this.Markup, this.AddedByPlayer);
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00192730 File Offset: 0x00190930
		public void IncreaseMarkup()
		{
			if (this.Markup < 100)
			{
				this.Markup += 1;
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0019274B File Offset: 0x0019094B
		public void DecreaseMarkup()
		{
			if (this.Markup > -4)
			{
				this.Markup -= 1;
			}
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x00192766 File Offset: 0x00190966
		public void Read(PooledBinaryReader _br)
		{
			this.Item = new ItemStack().Read(_br);
			this.Markup = _br.ReadSByte();
			this.AddedByPlayer = _br.ReadBoolean();
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00192791 File Offset: 0x00190991
		public void Write(PooledBinaryWriter _bw)
		{
			this.Item.Write(_bw);
			_bw.Write(this.Markup);
			_bw.Write(this.AddedByPlayer);
		}

		// Token: 0x040033EC RID: 13292
		public ItemStack Item;

		// Token: 0x040033ED RID: 13293
		public sbyte Markup;

		// Token: 0x040033EE RID: 13294
		public bool AddedByPlayer;
	}
}
