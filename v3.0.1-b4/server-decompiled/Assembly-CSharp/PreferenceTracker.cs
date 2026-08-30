using System;

// Token: 0x02000583 RID: 1411
public class PreferenceTracker
{
	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06002E32 RID: 11826 RVA: 0x00125D0D File Offset: 0x00123F0D
	// (set) Token: 0x06002E33 RID: 11827 RVA: 0x00125D15 File Offset: 0x00123F15
	public int PlayerID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06002E34 RID: 11828 RVA: 0x00125D1E File Offset: 0x00123F1E
	// (set) Token: 0x06002E35 RID: 11829 RVA: 0x00125D26 File Offset: 0x00123F26
	public ItemStack[] toolbelt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06002E36 RID: 11830 RVA: 0x00125D2F File Offset: 0x00123F2F
	// (set) Token: 0x06002E37 RID: 11831 RVA: 0x00125D37 File Offset: 0x00123F37
	public ItemStack[] bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06002E38 RID: 11832 RVA: 0x00125D40 File Offset: 0x00123F40
	// (set) Token: 0x06002E39 RID: 11833 RVA: 0x00125D48 File Offset: 0x00123F48
	public ItemValue[] equipment
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06002E3A RID: 11834 RVA: 0x00125D51 File Offset: 0x00123F51
	public bool AnyPreferences
	{
		get
		{
			return this.toolbelt != null || this.bag != null || this.equipment != null;
		}
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x00125D6E File Offset: 0x00123F6E
	public PreferenceTracker(int playerId)
	{
		this.PlayerID = playerId;
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x00125D80 File Offset: 0x00123F80
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

	// Token: 0x06002E3D RID: 11837 RVA: 0x00125E18 File Offset: 0x00124018
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

	// Token: 0x06002E3E RID: 11838 RVA: 0x00125E78 File Offset: 0x00124078
	public void SetBag(ItemStack[] _itemStacks, Predicate<ItemStack> _includeCondition)
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

	// Token: 0x06002E3F RID: 11839 RVA: 0x00125ED8 File Offset: 0x001240D8
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

	// Token: 0x06002E40 RID: 11840 RVA: 0x00125F38 File Offset: 0x00124138
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

	// Token: 0x06002E41 RID: 11841 RVA: 0x00125FD8 File Offset: 0x001241D8
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
