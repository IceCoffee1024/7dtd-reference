using System;
using System.Collections.Generic;

// Token: 0x020008B9 RID: 2233
public class TraderData
{
	// Token: 0x06004014 RID: 16404 RVA: 0x00193218 File Offset: 0x00191418
	public TraderData Clone()
	{
		TraderData traderData = new TraderData();
		traderData.CopyFrom(this);
		return traderData;
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x00193228 File Offset: 0x00191428
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

	// Token: 0x06004016 RID: 16406 RVA: 0x001932D2 File Offset: 0x001914D2
	public void SetModified(ITrader _trader)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTraderData>().Setup(_trader), false);
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06004017 RID: 16407 RVA: 0x001932F7 File Offset: 0x001914F7
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

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06004018 RID: 16408 RVA: 0x00193310 File Offset: 0x00191510
	public float FullTime
	{
		get
		{
			return (float)((this.TraderInfo != null) ? this.TraderInfo.ResetIntervalInTicks : 0);
		}
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06004019 RID: 16409 RVA: 0x00193329 File Offset: 0x00191529
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

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x0600401A RID: 16410 RVA: 0x0019335E File Offset: 0x0019155E
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

	// Token: 0x0600401B RID: 16411 RVA: 0x00193380 File Offset: 0x00191580
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

	// Token: 0x0600401C RID: 16412 RVA: 0x001933F8 File Offset: 0x001915F8
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

	// Token: 0x0600401D RID: 16413 RVA: 0x001934F4 File Offset: 0x001916F4
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

	// Token: 0x0600401E RID: 16414 RVA: 0x00193558 File Offset: 0x00191758
	public void Read(PooledBinaryReader _br)
	{
		this.TraderID = _br.ReadInt32();
		this.lastInventoryUpdate = _br.ReadUInt64();
		byte b = _br.ReadByte();
		this.ReadInventoryData(b, _br);
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x0019358C File Offset: 0x0019178C
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

	// Token: 0x06004020 RID: 16416 RVA: 0x001936B7 File Offset: 0x001918B7
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.TraderID);
		_bw.Write(this.lastInventoryUpdate);
		_bw.Write(TraderData.FileVersion);
		this.WriteInventoryData(_bw);
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x001936E4 File Offset: 0x001918E4
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

	// Token: 0x04003406 RID: 13318
	public List<TraderData.Entry> PrimaryInventory = new List<TraderData.Entry>();

	// Token: 0x04003407 RID: 13319
	public List<ItemStack[]> TierItemGroups = new List<ItemStack[]>();

	// Token: 0x04003408 RID: 13320
	public ulong lastInventoryUpdate;

	// Token: 0x04003409 RID: 13321
	public int TraderID = -1;

	// Token: 0x0400340A RID: 13322
	public int AvailableMoney;

	// Token: 0x0400340B RID: 13323
	public static byte FileVersion = 2;

	// Token: 0x020008BA RID: 2234
	public class Entry
	{
		// Token: 0x06004024 RID: 16420 RVA: 0x0000640C File Offset: 0x0000460C
		public Entry()
		{
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0019379D File Offset: 0x0019199D
		public Entry(ItemStack _item, sbyte _markup = 0, bool _addedByPlayer = false)
		{
			this.Item = _item;
			this.Markup = _markup;
			this.AddedByPlayer = _addedByPlayer;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x001937BA File Offset: 0x001919BA
		public TraderData.Entry Clone()
		{
			return new TraderData.Entry(this.Item.Clone(), this.Markup, this.AddedByPlayer);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x001937D8 File Offset: 0x001919D8
		public void IncreaseMarkup()
		{
			if (this.Markup < 100)
			{
				this.Markup += 1;
			}
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x001937F3 File Offset: 0x001919F3
		public void DecreaseMarkup()
		{
			if (this.Markup > -4)
			{
				this.Markup -= 1;
			}
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0019380E File Offset: 0x00191A0E
		public void Read(PooledBinaryReader _br)
		{
			this.Item = new ItemStack().Read(_br);
			this.Markup = _br.ReadSByte();
			this.AddedByPlayer = _br.ReadBoolean();
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00193839 File Offset: 0x00191A39
		public void Write(PooledBinaryWriter _bw)
		{
			this.Item.Write(_bw);
			_bw.Write(this.Markup);
			_bw.Write(this.AddedByPlayer);
		}

		// Token: 0x0400340C RID: 13324
		public ItemStack Item;

		// Token: 0x0400340D RID: 13325
		public sbyte Markup;

		// Token: 0x0400340E RID: 13326
		public bool AddedByPlayer;
	}
}
