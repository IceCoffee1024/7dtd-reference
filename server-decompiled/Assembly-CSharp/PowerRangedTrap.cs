using System;
using System.IO;

// Token: 0x02000910 RID: 2320
public class PowerRangedTrap : PowerConsumer
{
	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06004340 RID: 17216 RVA: 0x0008121D File Offset: 0x0007F41D
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.RangedTrap;
		}
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x001A367C File Offset: 0x001A187C
	public PowerRangedTrap()
	{
		this.Stacks = new ItemStack[3];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06004342 RID: 17218 RVA: 0x001A36C3 File Offset: 0x001A18C3
	// (set) Token: 0x06004343 RID: 17219 RVA: 0x001A36CB File Offset: 0x001A18CB
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

	// Token: 0x06004344 RID: 17220 RVA: 0x001A36E0 File Offset: 0x001A18E0
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

	// Token: 0x06004345 RID: 17221 RVA: 0x001A3794 File Offset: 0x001A1994
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

	// Token: 0x06004346 RID: 17222 RVA: 0x001A37E3 File Offset: 0x001A19E3
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x001A37EC File Offset: 0x001A19EC
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isLocked = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.TargetType = (PowerRangedTrap.TargetTypes)_br.ReadInt32();
	}

	// Token: 0x06004348 RID: 17224 RVA: 0x001A381A File Offset: 0x001A1A1A
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isLocked);
		GameUtils.WriteItemStack(_bw, this.Stacks);
		_bw.Write((int)this.TargetType);
	}

	// Token: 0x0400364F RID: 13903
	public ItemStack[] Stacks;

	// Token: 0x04003650 RID: 13904
	public PowerRangedTrap.TargetTypes TargetType = PowerRangedTrap.TargetTypes.Strangers | PowerRangedTrap.TargetTypes.Zombies;

	// Token: 0x04003651 RID: 13905
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isLocked;

	// Token: 0x02000911 RID: 2321
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x04003653 RID: 13907
		None = 0,
		// Token: 0x04003654 RID: 13908
		Self = 1,
		// Token: 0x04003655 RID: 13909
		Allies = 2,
		// Token: 0x04003656 RID: 13910
		Strangers = 4,
		// Token: 0x04003657 RID: 13911
		Zombies = 8
	}
}
