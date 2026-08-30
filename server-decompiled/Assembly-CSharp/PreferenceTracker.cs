using System;

// Token: 0x02000588 RID: 1416
public class PreferenceTracker
{
	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06002E60 RID: 11872 RVA: 0x00126DFD File Offset: 0x00124FFD
	// (set) Token: 0x06002E61 RID: 11873 RVA: 0x00126E05 File Offset: 0x00125005
	public int PlayerID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06002E62 RID: 11874 RVA: 0x00126E0E File Offset: 0x0012500E
	// (set) Token: 0x06002E63 RID: 11875 RVA: 0x00126E16 File Offset: 0x00125016
	public ItemStack[] toolbelt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06002E64 RID: 11876 RVA: 0x00126E1F File Offset: 0x0012501F
	// (set) Token: 0x06002E65 RID: 11877 RVA: 0x00126E27 File Offset: 0x00125027
	public ItemStack[] bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06002E66 RID: 11878 RVA: 0x00126E30 File Offset: 0x00125030
	// (set) Token: 0x06002E67 RID: 11879 RVA: 0x00126E38 File Offset: 0x00125038
	public ItemValue[] equipment
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06002E68 RID: 11880 RVA: 0x00126E41 File Offset: 0x00125041
	public bool AnyPreferences
	{
		get
		{
			return this.toolbelt != null || this.bag != null || this.equipment != null;
		}
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x00126E5E File Offset: 0x0012505E
	public PreferenceTracker(int playerId)
	{
		this.PlayerID = playerId;
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x00126E70 File Offset: 0x00125070
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

	// Token: 0x06002E6B RID: 11883 RVA: 0x00126F08 File Offset: 0x00125108
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

	// Token: 0x06002E6C RID: 11884 RVA: 0x00126F68 File Offset: 0x00125168
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

	// Token: 0x06002E6D RID: 11885 RVA: 0x00126FC8 File Offset: 0x001251C8
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

	// Token: 0x06002E6E RID: 11886 RVA: 0x00127028 File Offset: 0x00125228
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

	// Token: 0x06002E6F RID: 11887 RVA: 0x001270C8 File Offset: 0x001252C8
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
