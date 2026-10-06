using System;

// Token: 0x0200058A RID: 1418
public class PreferenceTracker
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x00127EB4 File Offset: 0x001260B4
	// (set) Token: 0x06002EB9 RID: 11961 RVA: 0x00127EBC File Offset: 0x001260BC
	public int PlayerID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06002EBA RID: 11962 RVA: 0x00127EC5 File Offset: 0x001260C5
	// (set) Token: 0x06002EBB RID: 11963 RVA: 0x00127ECD File Offset: 0x001260CD
	public ItemStack[] toolbelt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06002EBC RID: 11964 RVA: 0x00127ED6 File Offset: 0x001260D6
	// (set) Token: 0x06002EBD RID: 11965 RVA: 0x00127EDE File Offset: 0x001260DE
	public ItemStack[] bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06002EBE RID: 11966 RVA: 0x00127EE7 File Offset: 0x001260E7
	// (set) Token: 0x06002EBF RID: 11967 RVA: 0x00127EEF File Offset: 0x001260EF
	public ItemValue[] equipment
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x00127EF8 File Offset: 0x001260F8
	public bool AnyPreferences
	{
		get
		{
			return this.toolbelt != null || this.bag != null || this.equipment != null;
		}
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x00127F15 File Offset: 0x00126115
	public PreferenceTracker(int playerId)
	{
		this.PlayerID = playerId;
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x00127F24 File Offset: 0x00126124
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

	// Token: 0x06002EC3 RID: 11971 RVA: 0x00127FB9 File Offset: 0x001261B9
	public void CopyFrom(PreferenceTracker _other)
	{
		this.PlayerID = _other.PlayerID;
		this.toolbelt = _other.toolbelt;
		this.bag = _other.bag;
		this.equipment = _other.equipment;
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x00127FEC File Offset: 0x001261EC
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

	// Token: 0x06002EC5 RID: 11973 RVA: 0x0012804C File Offset: 0x0012624C
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

	// Token: 0x06002EC6 RID: 11974 RVA: 0x001280BC File Offset: 0x001262BC
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

	// Token: 0x06002EC7 RID: 11975 RVA: 0x0012811C File Offset: 0x0012631C
	public void Clear()
	{
		this.toolbelt.ClearItemStackArray();
		this.bag.ClearItemStackArray();
		this.equipment.ClearItemValueArray();
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x00128140 File Offset: 0x00126340
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

	// Token: 0x06002EC9 RID: 11977 RVA: 0x001281E0 File Offset: 0x001263E0
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
