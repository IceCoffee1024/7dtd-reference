using System;
using System.IO;

// Token: 0x02000904 RID: 2308
public class PowerRangedTrap : PowerConsumer
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x060042DB RID: 17115 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.RangedTrap;
		}
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x001A15A4 File Offset: 0x0019F7A4
	public PowerRangedTrap()
	{
		this.Stacks = new ItemStack[3];
		for (int i = 0; i < this.Stacks.Length; i++)
		{
			this.Stacks[i] = ItemStack.Empty;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x060042DD RID: 17117 RVA: 0x001A15EB File Offset: 0x0019F7EB
	// (set) Token: 0x060042DE RID: 17118 RVA: 0x001A15F3 File Offset: 0x0019F7F3
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

	// Token: 0x060042DF RID: 17119 RVA: 0x001A1608 File Offset: 0x0019F808
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

	// Token: 0x060042E0 RID: 17120 RVA: 0x001A16BC File Offset: 0x0019F8BC
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation))
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

	// Token: 0x060042E1 RID: 17121 RVA: 0x001A170A File Offset: 0x0019F90A
	public void SetSlots(ItemStack[] _stacks)
	{
		this.Stacks = _stacks;
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x001A1713 File Offset: 0x0019F913
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isLocked = _br.ReadBoolean();
		this.SetSlots(GameUtils.ReadItemStack(_br));
		this.TargetType = (PowerRangedTrap.TargetTypes)_br.ReadInt32();
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x001A1741 File Offset: 0x0019F941
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isLocked);
		GameUtils.WriteItemStack(_bw, this.Stacks);
		_bw.Write((int)this.TargetType);
	}

	// Token: 0x040035CE RID: 13774
	public ItemStack[] Stacks;

	// Token: 0x040035CF RID: 13775
	public PowerRangedTrap.TargetTypes TargetType = PowerRangedTrap.TargetTypes.Strangers | PowerRangedTrap.TargetTypes.Zombies;

	// Token: 0x040035D0 RID: 13776
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isLocked;

	// Token: 0x02000905 RID: 2309
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x040035D2 RID: 13778
		None = 0,
		// Token: 0x040035D3 RID: 13779
		Self = 1,
		// Token: 0x040035D4 RID: 13780
		Allies = 2,
		// Token: 0x040035D5 RID: 13781
		Strangers = 4,
		// Token: 0x040035D6 RID: 13782
		Zombies = 8
	}
}
