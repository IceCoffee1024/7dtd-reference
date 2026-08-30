using System;
using System.IO;

// Token: 0x02000570 RID: 1392
public class InventoryOperation
{
	// Token: 0x06002D81 RID: 11649 RVA: 0x001218F4 File Offset: 0x0011FAF4
	public static InventoryOperation CreateSetAbsolute(ItemStack _stack, int _index)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetAbsolute,
			Stack = _stack.Clone(),
			Index = _index
		};
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x00121915 File Offset: 0x0011FB15
	public static InventoryOperation CreateSetRelative(ItemStack _stack, int _index)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetRelative,
			Stack = _stack.Clone(),
			Index = _index
		};
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x00121936 File Offset: 0x0011FB36
	public static InventoryOperation CreateSetAll(ItemStack[] _newStacks)
	{
		return new InventoryOperation
		{
			Operation = InventoryOperation.EnumOperation.SetAll,
			NewStacks = ItemStack.Clone(_newStacks)
		};
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x00121950 File Offset: 0x0011FB50
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

	// Token: 0x06002D85 RID: 11653 RVA: 0x001219B8 File Offset: 0x0011FBB8
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

	// Token: 0x04002373 RID: 9075
	public InventoryOperation.EnumOperation Operation;

	// Token: 0x04002374 RID: 9076
	public ItemStack Stack;

	// Token: 0x04002375 RID: 9077
	public int Index;

	// Token: 0x04002376 RID: 9078
	public ItemStack[] NewStacks;

	// Token: 0x02000571 RID: 1393
	public enum EnumOperation
	{
		// Token: 0x04002378 RID: 9080
		SetAbsolute,
		// Token: 0x04002379 RID: 9081
		SetRelative,
		// Token: 0x0400237A RID: 9082
		SetAll
	}
}
