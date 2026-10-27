using System;

// Token: 0x0200116B RID: 4459
public class XUiM_Workstation : XUiModel
{
	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06008C79 RID: 35961 RVA: 0x0035917F File Offset: 0x0035737F
	public TileEntityWorkstation TileEntity
	{
		get
		{
			return this.tileEntity;
		}
	}

	// Token: 0x06008C7A RID: 35962 RVA: 0x00359187 File Offset: 0x00357387
	public XUiM_Workstation(TileEntityWorkstation _te)
	{
		this.tileEntity = _te;
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x00359196 File Offset: 0x00357396
	public bool GetIsBurning()
	{
		return this.tileEntity.IsBurning;
	}

	// Token: 0x06008C7C RID: 35964 RVA: 0x003591A3 File Offset: 0x003573A3
	public bool GetIsBesideWater()
	{
		return this.tileEntity.IsBesideWater;
	}

	// Token: 0x06008C7D RID: 35965 RVA: 0x003591B0 File Offset: 0x003573B0
	public void SetIsBurning(bool _isBurning)
	{
		this.tileEntity.IsBurning = _isBurning;
		this.tileEntity.ResetTickTime();
	}

	// Token: 0x06008C7E RID: 35966 RVA: 0x003591C9 File Offset: 0x003573C9
	public ItemStack[] GetInputStacks()
	{
		return this.tileEntity.Input;
	}

	// Token: 0x06008C7F RID: 35967 RVA: 0x003591D6 File Offset: 0x003573D6
	public void SetInputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Input = _itemStacks;
	}

	// Token: 0x06008C80 RID: 35968 RVA: 0x003591E4 File Offset: 0x003573E4
	public void SetInputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Input[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C81 RID: 35969 RVA: 0x003591F9 File Offset: 0x003573F9
	public ItemStack[] GetOutputStacks()
	{
		return this.tileEntity.Output;
	}

	// Token: 0x06008C82 RID: 35970 RVA: 0x00359206 File Offset: 0x00357406
	public void SetOutputStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Output = _itemStacks;
	}

	// Token: 0x06008C83 RID: 35971 RVA: 0x00359214 File Offset: 0x00357414
	public void SetOutputInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Output[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C84 RID: 35972 RVA: 0x00359229 File Offset: 0x00357429
	public ItemStack[] GetToolStacks()
	{
		return this.tileEntity.Tools;
	}

	// Token: 0x06008C85 RID: 35973 RVA: 0x00359236 File Offset: 0x00357436
	public void SetToolStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Tools = _itemStacks;
	}

	// Token: 0x06008C86 RID: 35974 RVA: 0x00359244 File Offset: 0x00357444
	public void SetToolInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Tools[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C87 RID: 35975 RVA: 0x00359259 File Offset: 0x00357459
	public ItemStack[] GetFuelStacks()
	{
		return this.tileEntity.Fuel;
	}

	// Token: 0x06008C88 RID: 35976 RVA: 0x00359266 File Offset: 0x00357466
	public void SetFuelStacks(ItemStack[] _itemStacks)
	{
		this.tileEntity.Fuel = _itemStacks;
	}

	// Token: 0x06008C89 RID: 35977 RVA: 0x00359274 File Offset: 0x00357474
	public void SetFuelInSlot(int _idx, ItemStack _itemStack)
	{
		this.tileEntity.Fuel[_idx] = _itemStack.Clone();
	}

	// Token: 0x06008C8A RID: 35978 RVA: 0x00359289 File Offset: 0x00357489
	public float GetBurnTimeLeft()
	{
		if (this.tileEntity.BurnTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTimeLeft + 0.5f;
	}

	// Token: 0x06008C8B RID: 35979 RVA: 0x003592B4 File Offset: 0x003574B4
	public float GetTotalBurnTimeLeft()
	{
		if (this.tileEntity.BurnTotalTimeLeft == 0f)
		{
			return 0f;
		}
		return this.tileEntity.BurnTotalTimeLeft + 0.5f;
	}

	// Token: 0x06008C8C RID: 35980 RVA: 0x003592DF File Offset: 0x003574DF
	public RecipeQueueItem[] GetRecipeQueueItems()
	{
		return this.tileEntity.Queue;
	}

	// Token: 0x06008C8D RID: 35981 RVA: 0x003592EC File Offset: 0x003574EC
	public void SetRecipeQueueItems(RecipeQueueItem[] _queueStacks)
	{
		this.tileEntity.Queue = _queueStacks;
	}

	// Token: 0x06008C8E RID: 35982 RVA: 0x003592FA File Offset: 0x003574FA
	public void SetQueueInSlot(int _idx, RecipeQueueItem _queueStack)
	{
		this.tileEntity.Queue[_idx] = _queueStack;
	}

	// Token: 0x06008C8F RID: 35983 RVA: 0x0035930A File Offset: 0x0035750A
	public void SetUserAccessing(bool _isUserAccessing)
	{
		this.tileEntity.SetUserAccessing(_isUserAccessing);
	}

	// Token: 0x06008C90 RID: 35984 RVA: 0x00359318 File Offset: 0x00357518
	public string[] GetMaterialNames()
	{
		return this.tileEntity.MaterialNames;
	}

	// Token: 0x040067E5 RID: 26597
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TileEntityWorkstation tileEntity;
}
