using System;
using System.IO;

// Token: 0x02000575 RID: 1397
public class InventoryOperation
{
	// Token: 0x06002DAC RID: 11692 RVA: 0x001229B0 File Offset: 0x00120BB0
	public static InventoryOperation CreateSetAbsolute(ItemStack _stack, int _index)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetAbsolute,
			Stack = _stack.Clone(),
			Index = _index
		};
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x001229D1 File Offset: 0x00120BD1
	public static InventoryOperation CreateSetRelative(ItemStack _stack, int _index)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetRelative,
			Stack = _stack.Clone(),
			Index = _index
		};
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x001229F2 File Offset: 0x00120BF2
	public static InventoryOperation CreateSetAll(ItemStack[] _newStacks)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetAll,
			NewStacks = ItemStack.Clone(_newStacks)
		};
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x00122A0C File Offset: 0x00120C0C
	public static InventoryOperation Read(BinaryReader _br)
	{
		switch (_br.ReadInt16())
		{
		case 0:
		{
			ItemStack itemStack = ItemStack.Empty.Read(_br);
			int num = _br.ReadInt32();
			return InventoryOperation.CreateSetAbsolute(itemStack, num);
		}
		case 1:
		{
			ItemStack itemStack2 = ItemStack.Empty.Read(_br);
			int num2 = _br.ReadInt32();
			return InventoryOperation.CreateSetRelative(itemStack2, num2);
		}
		case 2:
			return InventoryOperation.CreateSetAll(ItemStack.ReadArray(_br));
		default:
			return null;
		}
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x00122A74 File Offset: 0x00120C74
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((short)this.Operation);
		InventoryOperation.EnumOperation operation = this.Operation;
		if (operation <= InventoryOperation.EnumOperation.SetRelative)
		{
			this.Stack.Write(_bw);
			_bw.Write(this.Index);
			return;
		}
		if (operation != InventoryOperation.EnumOperation.SetAll)
		{
			return;
		}
		ItemStack.WriteArray(_bw, this.NewStacks);
	}

	// Token: 0x040023A5 RID: 9125
	public InventoryOperation.EnumOperation Operation;

	// Token: 0x040023A6 RID: 9126
	public ItemStack Stack;

	// Token: 0x040023A7 RID: 9127
	public int Index;

	// Token: 0x040023A8 RID: 9128
	public ItemStack[] NewStacks;

	// Token: 0x02000576 RID: 1398
	public enum EnumOperation
	{
		// Token: 0x040023AA RID: 9130
		SetAbsolute,
		// Token: 0x040023AB RID: 9131
		SetRelative,
		// Token: 0x040023AC RID: 9132
		SetAll
	}
}
