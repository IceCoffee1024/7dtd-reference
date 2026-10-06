using System;

// Token: 0x02000915 RID: 2325
public class PowerRangedTrap : PowerConsumer
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x060042D2 RID: 17106 RVA: 0x000814D1 File Offset: 0x0007F6D1
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.RangedTrap;
		}
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x001A3780 File Offset: 0x001A1980
	public PowerRangedTrap()
	{
		this.Stacks = new ItemStack[3];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x060042D4 RID: 17108 RVA: 0x001A37C7 File Offset: 0x001A19C7
	// (set) Token: 0x060042D5 RID: 17109 RVA: 0x001A37CF File Offset: 0x001A19CF
	public bool IsLocked
	{
		get
		{
			return this.isLocked;
		}
		set
		{
			if (this.isLocked != value)
			{
				this.isLocked = value;
			}
		}
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x001A37E4 File Offset: 0x001A19E4
	public bool TryStackItem(ItemStack itemStack)
	{
		int num = 0;
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			num = itemStack.count;
			if (this.Stacks[i].IsEmpty())
			{
				this.Stacks[i] = itemStack.Clone();
				itemStack.count = 0;
				return true;
			}
			if (this.Stacks[i].itemValue.type == itemStack.itemValue.type && this.Stacks[i].CanStackPartly(ref num))
			{
				this.Stacks[i].count += num;
				itemStack.count -= num;
				if (itemStack.count == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x001A3898 File Offset: 0x001A1A98
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation, -1))
		{
			return false;
		}
		if (!this.isLocked)
		{
			for (int i = 0; i < this.Stacks.Length; i++)
			{
				if (this.Stacks[i].IsEmpty())
				{
					this.Stacks[i] = _itemStack;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x001A38E7 File Offset: 0x001A1AE7
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x001A38F0 File Offset: 0x001A1AF0
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isLocked = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.TargetType = (PowerRangedTrap.TargetTypes)_br.ReadInt32();
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x001A391E File Offset: 0x001A1B1E
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isLocked);
		GameUtils.WriteItemStack(_bw, this.Stacks);
		_bw.Write((int)this.TargetType);
	}

	// Token: 0x04003643 RID: 13891
	public ItemStack[] Stacks;

	// Token: 0x04003644 RID: 13892
	public PowerRangedTrap.TargetTypes TargetType = PowerRangedTrap.TargetTypes.Strangers | PowerRangedTrap.TargetTypes.Zombies;

	// Token: 0x04003645 RID: 13893
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isLocked;

	// Token: 0x02000916 RID: 2326
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x04003647 RID: 13895
		None = 0,
		// Token: 0x04003648 RID: 13896
		Self = 1,
		// Token: 0x04003649 RID: 13897
		Allies = 2,
		// Token: 0x0400364A RID: 13898
		Strangers = 4,
		// Token: 0x0400364B RID: 13899
		Zombies = 8
	}
}
