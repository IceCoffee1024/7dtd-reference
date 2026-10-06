using System;

// Token: 0x0200115B RID: 4443
public class XUiM_Workstation : XUiModel
{
	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06008C25 RID: 35877 RVA: 0x003570D3 File Offset: 0x003552D3
	public TileEntityWorkstation TileEntity
	{
		get
		{
			return this.tileEntity;
		}
	}

	// Token: 0x06008C26 RID: 35878 RVA: 0x003570DB File Offset: 0x003552DB
	public XUiM_Workstation(TileEntityWorkstation _te)
	{
		this.tileEntity = _te;
	}

	// Token: 0x06008C27 RID: 35879 RVA: 0x003570EA File Offset: 0x003552EA
	public bool GetIsBurning()
	{
		return this.tileEntity.IsBurning;
	}

	// Token: 0x06008C28 RID: 35880 RVA: 0x003570F7 File Offset: 0x003552F7
	public bool GetIsBesideWater()
	{
		return this.tileEntity.IsBesideWater;
	}

	// Token: 0x06008C29 RID: 35881 RVA: 0x00357104 File Offset: 0x00355304
	public void SetIsBurning(bool _isBurning)
	{
		this.tileEntity.IsBurning = _isBurning;
		this.tileEntity.ResetTickTime();
	}

	// Token: 0x06008C2A RID: 35882 RVA: 0x0035711D File Offset: 0x0035531D
	public ItemStack[] GetInputStacks()
	{
		return this.tileEntity.Input;
	}

	// Token: 0x06008C2B RID: 35883 RVA: 0x0035712A File Offset: 0x0035532A
	public void SetInputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Input = _itemStacks;
	}

	// Token: 0x06008C2C RID: 35884 RVA: 0x00357138 File Offset: 0x00355338
	public void SetInputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Input[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C2D RID: 35885 RVA: 0x0035714D File Offset: 0x0035534D
	public ItemStack[] GetOutputStacks()
	{
		return this.tileEntity.Output;
	}

	// Token: 0x06008C2E RID: 35886 RVA: 0x0035715A File Offset: 0x0035535A
	public void SetOutputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Output = _itemStacks;
	}

	// Token: 0x06008C2F RID: 35887 RVA: 0x00357168 File Offset: 0x00355368
	public void SetOutputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Output[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C30 RID: 35888 RVA: 0x0035717D File Offset: 0x0035537D
	public ItemStack[] GetToolStacks()
	{
		return this.tileEntity.Tools;
	}

	// Token: 0x06008C31 RID: 35889 RVA: 0x0035718A File Offset: 0x0035538A
	public void SetToolStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Tools = _itemStacks;
	}

	// Token: 0x06008C32 RID: 35890 RVA: 0x00357198 File Offset: 0x00355398
	public void SetToolInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Tools[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C33 RID: 35891 RVA: 0x003571AD File Offset: 0x003553AD
	public ItemStack[] GetFuelStacks()
	{
		return this.tileEntity.Fuel;
	}

	// Token: 0x06008C34 RID: 35892 RVA: 0x003571BA File Offset: 0x003553BA
	public void SetFuelStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Fuel = _itemStacks;
	}

	// Token: 0x06008C35 RID: 35893 RVA: 0x003571C8 File Offset: 0x003553C8
	public void SetFuelInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Fuel[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C36 RID: 35894 RVA: 0x003571DD File Offset: 0x003553DD
	public float GetBurnTimeLeft()
	{
		if (this.tileEntity.BurnTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTimeLeft + 0.5f;
	}

	// Token: 0x06008C37 RID: 35895 RVA: 0x00357208 File Offset: 0x00355408
	public float GetTotalBurnTimeLeft()
	{
		if (this.tileEntity.BurnTotalTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTotalTimeLeft + 0.5f;
	}

	// Token: 0x06008C38 RID: 35896 RVA: 0x00357233 File Offset: 0x00355433
	public RecipeQueueItem[] GetRecipeQueueItems()
	{
		return this.tileEntity.Queue;
	}

	// Token: 0x06008C39 RID: 35897 RVA: 0x00357240 File Offset: 0x00355440
	public void SetRecipeQueueItems(RecipeQueueItem[] _queueStacks)
	{
		this.tileEntity.Queue = _queueStacks;
	}

	// Token: 0x06008C3A RID: 35898 RVA: 0x0035724E File Offset: 0x0035544E
	public void SetQueueInSlot(int _idx, RecipeQueueItem _queueStack)
	{
		this.tileEntity.Queue[_idx] = _queueStack;
	}

	// Token: 0x06008C3B RID: 35899 RVA: 0x0035725E File Offset: 0x0035545E
	public void SetUserAccessing(bool _isUserAccessing)
	{
		this.tileEntity.SetUserAccessing(_isUserAccessing);
	}

	// Token: 0x06008C3C RID: 35900 RVA: 0x0035726C File Offset: 0x0035546C
	public string[] GetMaterialNames()
	{
		return this.tileEntity.MaterialNames;
	}

	// Token: 0x040067A5 RID: 26533
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityWorkstation tileEntity;
}
