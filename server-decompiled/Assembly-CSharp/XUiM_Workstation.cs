using System;

// Token: 0x02001143 RID: 4419
public class XUiM_Workstation : XUiModel
{
	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06008BBA RID: 35770 RVA: 0x00352CB3 File Offset: 0x00350EB3
	public TileEntityWorkstation TileEntity
	{
		get
		{
			return this.tileEntity;
		}
	}

	// Token: 0x06008BBB RID: 35771 RVA: 0x00352CBB File Offset: 0x00350EBB
	public XUiM_Workstation(TileEntityWorkstation _te)
	{
		this.tileEntity = _te;
	}

	// Token: 0x06008BBC RID: 35772 RVA: 0x00352CCA File Offset: 0x00350ECA
	public bool GetIsBurning()
	{
		return this.tileEntity.IsBurning;
	}

	// Token: 0x06008BBD RID: 35773 RVA: 0x00352CD7 File Offset: 0x00350ED7
	public bool GetIsBesideWater()
	{
		return this.tileEntity.IsBesideWater;
	}

	// Token: 0x06008BBE RID: 35774 RVA: 0x00352CE4 File Offset: 0x00350EE4
	public void SetIsBurning(bool _isBurning)
	{
		this.tileEntity.IsBurning = _isBurning;
		this.tileEntity.ResetTickTime();
	}

	// Token: 0x06008BBF RID: 35775 RVA: 0x00352CFD File Offset: 0x00350EFD
	public ItemStack[] GetInputStacks()
	{
		return this.tileEntity.Input;
	}

	// Token: 0x06008BC0 RID: 35776 RVA: 0x00352D0A File Offset: 0x00350F0A
	public void SetInputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Input = _itemStacks;
	}

	// Token: 0x06008BC1 RID: 35777 RVA: 0x00352D18 File Offset: 0x00350F18
	public void SetInputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Input[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008BC2 RID: 35778 RVA: 0x00352D2D File Offset: 0x00350F2D
	public ItemStack[] GetOutputStacks()
	{
		return this.tileEntity.Output;
	}

	// Token: 0x06008BC3 RID: 35779 RVA: 0x00352D3A File Offset: 0x00350F3A
	public void SetOutputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Output = _itemStacks;
	}

	// Token: 0x06008BC4 RID: 35780 RVA: 0x00352D48 File Offset: 0x00350F48
	public void SetOutputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Output[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008BC5 RID: 35781 RVA: 0x00352D5D File Offset: 0x00350F5D
	public ItemStack[] GetToolStacks()
	{
		return this.tileEntity.Tools;
	}

	// Token: 0x06008BC6 RID: 35782 RVA: 0x00352D6A File Offset: 0x00350F6A
	public void SetToolStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Tools = _itemStacks;
	}

	// Token: 0x06008BC7 RID: 35783 RVA: 0x00352D78 File Offset: 0x00350F78
	public void SetToolInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Tools[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008BC8 RID: 35784 RVA: 0x00352D8D File Offset: 0x00350F8D
	public ItemStack[] GetFuelStacks()
	{
		return this.tileEntity.Fuel;
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x00352D9A File Offset: 0x00350F9A
	public void SetFuelStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Fuel = _itemStacks;
	}

	// Token: 0x06008BCA RID: 35786 RVA: 0x00352DA8 File Offset: 0x00350FA8
	public void SetFuelInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Fuel[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008BCB RID: 35787 RVA: 0x00352DBD File Offset: 0x00350FBD
	public float GetBurnTimeLeft()
	{
		if (this.tileEntity.BurnTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTimeLeft + 0.5f;
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x00352DE8 File Offset: 0x00350FE8
	public float GetTotalBurnTimeLeft()
	{
		if (this.tileEntity.BurnTotalTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTotalTimeLeft + 0.5f;
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x00352E13 File Offset: 0x00351013
	public RecipeQueueItem[] GetRecipeQueueItems()
	{
		return this.tileEntity.Queue;
	}

	// Token: 0x06008BCE RID: 35790 RVA: 0x00352E20 File Offset: 0x00351020
	public void SetRecipeQueueItems(RecipeQueueItem[] _queueStacks)
	{
		this.tileEntity.Queue = _queueStacks;
	}

	// Token: 0x06008BCF RID: 35791 RVA: 0x00352E2E File Offset: 0x0035102E
	public void SetQueueInSlot(int _idx, RecipeQueueItem _queueStack)
	{
		this.tileEntity.Queue[_idx] = _queueStack;
	}

	// Token: 0x06008BD0 RID: 35792 RVA: 0x00352E3E File Offset: 0x0035103E
	public void SetUserAccessing(bool _isUserAccessing)
	{
		this.tileEntity.SetUserAccessing(_isUserAccessing);
	}

	// Token: 0x06008BD1 RID: 35793 RVA: 0x00352E4C File Offset: 0x0035104C
	public string[] GetMaterialNames()
	{
		return this.tileEntity.MaterialNames;
	}

	// Token: 0x04006750 RID: 26448
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityWorkstation tileEntity;
}
