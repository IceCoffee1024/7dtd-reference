using System;

// Token: 0x02000591 RID: 1425
public class PreferenceTracker
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x00128AA8 File Offset: 0x00126CA8
	// (set) Token: 0x06002EC7 RID: 11975 RVA: 0x00128AB0 File Offset: 0x00126CB0
	public int PlayerID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x00128AB9 File Offset: 0x00126CB9
	// (set) Token: 0x06002EC9 RID: 11977 RVA: 0x00128AC1 File Offset: 0x00126CC1
	public ItemStack[] toolbelt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06002ECA RID: 11978 RVA: 0x00128ACA File Offset: 0x00126CCA
	// (set) Token: 0x06002ECB RID: 11979 RVA: 0x00128AD2 File Offset: 0x00126CD2
	public ItemStack[] bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06002ECC RID: 11980 RVA: 0x00128ADB File Offset: 0x00126CDB
	// (set) Token: 0x06002ECD RID: 11981 RVA: 0x00128AE3 File Offset: 0x00126CE3
	public ItemValue[] equipment
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06002ECE RID: 11982 RVA: 0x00128AEC File Offset: 0x00126CEC
	public bool AnyPreferences
	{
		get
		{
			return this.toolbelt != null || this.bag != null || this.equipment != null;
		}
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x00128B09 File Offset: 0x00126D09
	public PreferenceTracker(int playerId)
	{
		this.PlayerID = playerId;
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x00128B18 File Offset: 0x00126D18
	public PreferenceTracker Clone()
	{
		PreferenceTracker preferenceTracker = new PreferenceTracker(this.PlayerID);
		preferenceTracker.toolbelt = ((this.toolbelt != null) ? ItemStack.Clone(this.toolbelt) : null);
		preferenceTracker.bag = ((this.bag != null) ? ItemStack.Clone(this.bag) : null);
		if (this.equipment != null)
		{
			preferenceTracker.equipment = new ItemValue[this.equipment.Length];
			for (int i = 0; i < this.equipment.Length; i++)
			{
				preferenceTracker.equipment[i] = this.equipment[i].Clone();
			}
		}
		return preferenceTracker;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x00128BAD File Offset: 0x00126DAD
	public void CopyFrom(PreferenceTracker _other)
	{
		this.PlayerID = _other.PlayerID;
		this.toolbelt = _other.toolbelt;
		this.bag = _other.bag;
		this.equipment = _other.equipment;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x00128BE0 File Offset: 0x00126DE0
	public void SetToolbelt(ItemStack[] _itemStacks, Predicate<ItemStack> _includeCondition)
	{
		if (_itemStacks == null || _itemStacks.Length == 0)
		{
			return;
		}
		this.toolbelt = new ItemStack[_itemStacks.Length];
		for (int i = 0; i < this.toolbelt.Length; i++)
		{
			if (_includeCondition(_itemStacks[i]))
			{
				this.toolbelt[i] = _itemStacks[i].Clone();
			}
			else
			{
				this.toolbelt[i] = new ItemStack();
			}
		}
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x00128C40 File Offset: 0x00126E40
	public void SetBag(ItemStackGrid _itemStacks, Predicate<ItemStack> _includeCondition)
	{
		if (_itemStacks == null || _itemStacks.Length == 0)
		{
			return;
		}
		this.bag = new ItemStack[_itemStacks.Length];
		for (int i = 0; i < this.bag.Length; i++)
		{
			if (_includeCondition(_itemStacks[i]))
			{
				this.bag[i] = _itemStacks[i].Clone();
			}
			else
			{
				this.bag[i] = new ItemStack();
			}
		}
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x00128CB0 File Offset: 0x00126EB0
	public void SetEquipment(ItemValue[] _itemValues, Predicate<ItemValue> _includeCondition)
	{
		if (_itemValues == null && _itemValues.Length != 0)
		{
			return;
		}
		this.equipment = new ItemValue[_itemValues.Length];
		for (int i = 0; i < this.equipment.Length; i++)
		{
			if (_includeCondition(_itemValues[i]))
			{
				this.equipment[i] = _itemValues[i].Clone();
			}
			else
			{
				this.equipment[i] = new ItemValue();
			}
		}
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x00128D10 File Offset: 0x00126F10
	public void Clear()
	{
		this.toolbelt.ClearItemStackArray();
		this.bag.ClearItemStackArray();
		this.equipment.ClearItemValueArray();
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x00128D34 File Offset: 0x00126F34
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.PlayerID);
		bool flag = this.toolbelt != null && this.toolbelt.Length != 0;
		_bw.Write(flag);
		if (flag)
		{
			GameUtils.WriteItemStack(_bw, this.toolbelt);
		}
		bool flag2 = this.equipment != null && this.equipment.Length != 0;
		_bw.Write(flag2);
		if (flag2)
		{
			GameUtils.WriteItemValueArray(_bw, this.equipment);
		}
		bool flag3 = this.bag != null && this.bag.Length != 0;
		_bw.Write(flag3);
		if (flag3)
		{
			GameUtils.WriteItemStack(_bw, this.bag);
		}
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x00128DD4 File Offset: 0x00126FD4
	public void Read(PooledBinaryReader _br)
	{
		this.PlayerID = _br.ReadInt32();
		if (_br.ReadBoolean())
		{
			this.toolbelt = GameUtils.ReadItemStack(_br);
		}
		if (_br.ReadBoolean())
		{
			this.equipment = GameUtils.ReadItemValueArray(_br);
		}
		if (_br.ReadBoolean())
		{
			this.bag = GameUtils.ReadItemStack(_br);
		}
	}
}
