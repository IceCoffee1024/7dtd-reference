using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020008AA RID: 2218
public class TraderData
{
	// Token: 0x0600405B RID: 16475 RVA: 0x0019206C File Offset: 0x0019026C
	public TraderData Clone()
	{
		TraderData traderData = new TraderData();
		traderData.CopyFrom(this);
		return traderData;
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x0019207C File Offset: 0x0019027C
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

	// Token: 0x0600405D RID: 16477 RVA: 0x00192126 File Offset: 0x00190326
	public void SetModified(ITrader _trader)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTraderData>().Setup(_trader), false);
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x0600405E RID: 16478 RVA: 0x0019214B File Offset: 0x0019034B
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

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x0600405F RID: 16479 RVA: 0x00192164 File Offset: 0x00190364
	public float FullTime
	{
		get
		{
			return (float)((this.TraderInfo != null) ? this.TraderInfo.ResetIntervalInTicks : 0);
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06004060 RID: 16480 RVA: 0x0019217D File Offset: 0x0019037D
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

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06004061 RID: 16481 RVA: 0x001921B2 File Offset: 0x001903B2
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

	// Token: 0x06004062 RID: 16482 RVA: 0x001921D4 File Offset: 0x001903D4
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

	// Token: 0x06004063 RID: 16483 RVA: 0x0019224C File Offset: 0x0019044C
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

	// Token: 0x06004064 RID: 16484 RVA: 0x00192348 File Offset: 0x00190548
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

	// Token: 0x06004065 RID: 16485 RVA: 0x001923AC File Offset: 0x001905AC
	public void Read(BinaryReader _br)
	{
		this.TraderID = _br.ReadInt32();
		this.lastInventoryUpdate = _br.ReadUInt64();
		byte b = _br.ReadByte();
		this.ReadInventoryData(b, _br);
	}

	// Token: 0x06004066 RID: 16486 RVA: 0x001923E0 File Offset: 0x001905E0
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

	// Token: 0x06004067 RID: 16487 RVA: 0x0019250B File Offset: 0x0019070B
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.TraderID);
		_bw.Write(this.lastInventoryUpdate);
		_bw.Write(TraderData.FileVersion);
		this.WriteInventoryData(_bw);
	}

	// Token: 0x06004068 RID: 16488 RVA: 0x00192538 File Offset: 0x00190738
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

	// Token: 0x040033F2 RID: 13298
	public List<TraderData.Entry> PrimaryInventory = new List<TraderData.Entry>();

	// Token: 0x040033F3 RID: 13299
	public List<ItemStack[]> TierItemGroups = new List<ItemStack[]>();

	// Token: 0x040033F4 RID: 13300
	public ulong lastInventoryUpdate;

	// Token: 0x040033F5 RID: 13301
	public int TraderID = -1;

	// Token: 0x040033F6 RID: 13302
	public int AvailableMoney;

	// Token: 0x040033F7 RID: 13303
	public static byte FileVersion = 2;

	// Token: 0x020008AB RID: 2219
	public class Entry
	{
		// Token: 0x0600406B RID: 16491 RVA: 0x0000640C File Offset: 0x0000460C
		public Entry()
		{
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x001925F1 File Offset: 0x001907F1
		public Entry(ItemStack _item, sbyte _markup = 0, bool _addedByPlayer = false)
		{
			this.Item = _item;
			this.Markup = _markup;
			this.AddedByPlayer = _addedByPlayer;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0019260E File Offset: 0x0019080E
		public TraderData.Entry Clone()
		{
			return new TraderData.Entry(this.Item.Clone(), this.Markup, this.AddedByPlayer);
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0019262C File Offset: 0x0019082C
		public void IncreaseMarkup()
		{
			if (this.Markup < 100)
			{
				this.Markup += 1;
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x00192647 File Offset: 0x00190847
		public void DecreaseMarkup()
		{
			if (this.Markup > -4)
			{
				this.Markup -= 1;
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x00192662 File Offset: 0x00190862
		public void Read(BinaryReader _br)
		{
			this.Item = new ItemStack().Read(_br);
			this.Markup = _br.ReadSByte();
			this.AddedByPlayer = _br.ReadBoolean();
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0019268D File Offset: 0x0019088D
		public void Write(BinaryWriter _bw)
		{
			this.Item.Write(_bw);
			_bw.Write(this.Markup);
			_bw.Write(this.AddedByPlayer);
		}

		// Token: 0x040033F8 RID: 13304
		public ItemStack Item;

		// Token: 0x040033F9 RID: 13305
		public sbyte Markup;

		// Token: 0x040033FA RID: 13306
		public bool AddedByPlayer;
	}
}
