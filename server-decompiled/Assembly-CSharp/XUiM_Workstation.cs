using System;

// Token: 0x0200112A RID: 4394
public class XUiM_Workstation : XUiModel
{
	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x06008AC8 RID: 35528 RVA: 0x0034E47F File Offset: 0x0034C67F
	public TileEntityWorkstation TileEntity
	{
		get
		{
			return this.tileEntity;
		}
	}

	// Token: 0x06008AC9 RID: 35529 RVA: 0x0034E487 File Offset: 0x0034C687
	public XUiM_Workstation(TileEntityWorkstation _te)
	{
		this.tileEntity = _te;
	}

	// Token: 0x06008ACA RID: 35530 RVA: 0x0034E496 File Offset: 0x0034C696
	public bool GetIsBurning()
	{
		return this.tileEntity.IsBurning;
	}

	// Token: 0x06008ACB RID: 35531 RVA: 0x0034E4A3 File Offset: 0x0034C6A3
	public bool GetIsBesideWater()
	{
		return this.tileEntity.IsBesideWater;
	}

	// Token: 0x06008ACC RID: 35532 RVA: 0x0034E4B0 File Offset: 0x0034C6B0
	public void SetIsBurning(bool _isBurning)
	{
		this.tileEntity.IsBurning = _isBurning;
		this.tileEntity.ResetTickTime();
	}

	// Token: 0x06008ACD RID: 35533 RVA: 0x0034E4C9 File Offset: 0x0034C6C9
	public ItemStack[] GetInputStacks()
	{
		return this.tileEntity.Input;
	}

	// Token: 0x06008ACE RID: 35534 RVA: 0x0034E4D6 File Offset: 0x0034C6D6
	public void SetInputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Input = _itemStacks;
	}

	// Token: 0x06008ACF RID: 35535 RVA: 0x0034E4E4 File Offset: 0x0034C6E4
	public void SetInputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Input[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008AD0 RID: 35536 RVA: 0x0034E4F9 File Offset: 0x0034C6F9
	public ItemStack[] GetOutputStacks()
	{
		return this.tileEntity.Output;
	}

	// Token: 0x06008AD1 RID: 35537 RVA: 0x0034E506 File Offset: 0x0034C706
	public void SetOutputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Output = _itemStacks;
	}

	// Token: 0x06008AD2 RID: 35538 RVA: 0x0034E514 File Offset: 0x0034C714
	public void SetOutputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Output[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008AD3 RID: 35539 RVA: 0x0034E529 File Offset: 0x0034C729
	public ItemStack[] GetToolStacks()
	{
		return this.tileEntity.Tools;
	}

	// Token: 0x06008AD4 RID: 35540 RVA: 0x0034E536 File Offset: 0x0034C736
	public void SetToolStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Tools = _itemStacks;
	}

	// Token: 0x06008AD5 RID: 35541 RVA: 0x0034E544 File Offset: 0x0034C744
	public void SetToolInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Tools[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008AD6 RID: 35542 RVA: 0x0034E559 File Offset: 0x0034C759
	public ItemStack[] GetFuelStacks()
	{
		return this.tileEntity.Fuel;
	}

	// Token: 0x06008AD7 RID: 35543 RVA: 0x0034E566 File Offset: 0x0034C766
	public void SetFuelStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Fuel = _itemStacks;
	}

	// Token: 0x06008AD8 RID: 35544 RVA: 0x0034E574 File Offset: 0x0034C774
	public void SetFuelInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Fuel[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008AD9 RID: 35545 RVA: 0x0034E589 File Offset: 0x0034C789
	public float GetBurnTimeLeft()
	{
		if (this.tileEntity.BurnTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTimeLeft + 0.5f;
	}

	// Token: 0x06008ADA RID: 35546 RVA: 0x0034E5B4 File Offset: 0x0034C7B4
	public float GetTotalBurnTimeLeft()
	{
		if (this.tileEntity.BurnTotalTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTotalTimeLeft + 0.5f;
	}

	// Token: 0x06008ADB RID: 35547 RVA: 0x0034E5DF File Offset: 0x0034C7DF
	public RecipeQueueItem[] GetRecipeQueueItems()
	{
		return this.tileEntity.Queue;
	}

	// Token: 0x06008ADC RID: 35548 RVA: 0x0034E5EC File Offset: 0x0034C7EC
	public void SetRecipeQueueItems(RecipeQueueItem[] _queueStacks)
	{
		this.tileEntity.Queue = _queueStacks;
	}

	// Token: 0x06008ADD RID: 35549 RVA: 0x0034E5FA File Offset: 0x0034C7FA
	public void SetQueueInSlot(int _idx, RecipeQueueItem _queueStack)
	{
		this.tileEntity.Queue[_idx] = _queueStack;
	}

	// Token: 0x06008ADE RID: 35550 RVA: 0x0034E60A File Offset: 0x0034C80A
	public void SetUserAccessing(bool _isUserAccessing)
	{
		this.tileEntity.SetUserAccessing(_isUserAccessing);
	}

	// Token: 0x06008ADF RID: 35551 RVA: 0x0034E618 File Offset: 0x0034C818
	public string[] GetMaterialNames()
	{
		return this.tileEntity.MaterialNames;
	}

	// Token: 0x04006686 RID: 26246
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityWorkstation tileEntity;
}
