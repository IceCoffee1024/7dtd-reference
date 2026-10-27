using System;

// Token: 0x0200091F RID: 2335
public class PowerRangedTrap : PowerConsumer
{
	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x060042F5 RID: 17141 RVA: 0x000816F5 File Offset: 0x0007F8F5
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.RangedTrap;
		}
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x001A4828 File Offset: 0x001A2A28
	public PowerRangedTrap()
	{
		this.Stacks = new ItemStack[3];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x060042F7 RID: 17143 RVA: 0x001A486F File Offset: 0x001A2A6F
	// (set) Token: 0x060042F8 RID: 17144 RVA: 0x001A4877 File Offset: 0x001A2A77
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

	// Token: 0x060042F9 RID: 17145 RVA: 0x001A488C File Offset: 0x001A2A8C
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

	// Token: 0x060042FA RID: 17146 RVA: 0x001A4940 File Offset: 0x001A2B40
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

	// Token: 0x060042FB RID: 17147 RVA: 0x001A498F File Offset: 0x001A2B8F
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
	}

	// Token: 0x060042FC RID: 17148 RVA: 0x001A4998 File Offset: 0x001A2B98
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isLocked = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.TargetType = (PowerRangedTrap.TargetTypes)_br.ReadInt32();
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x001A49C6 File Offset: 0x001A2BC6
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isLocked);
		GameUtils.WriteItemStack(_bw, this.Stacks);
		_bw.Write((int)this.TargetType);
	}

	// Token: 0x04003663 RID: 13923
	public ItemStack[] Stacks;

	// Token: 0x04003664 RID: 13924
	public PowerRangedTrap.TargetTypes TargetType = PowerRangedTrap.TargetTypes.Strangers | PowerRangedTrap.TargetTypes.Zombies;

	// Token: 0x04003665 RID: 13925
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isLocked;

	// Token: 0x02000920 RID: 2336
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x04003667 RID: 13927
		None = 0,
		// Token: 0x04003668 RID: 13928
		Self = 1,
		// Token: 0x04003669 RID: 13929
		Allies = 2,
		// Token: 0x0400366A RID: 13930
		Strangers = 4,
		// Token: 0x0400366B RID: 13931
		Zombies = 8
	}
}
