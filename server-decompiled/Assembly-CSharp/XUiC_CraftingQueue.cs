using System;
using UnityEngine.Scripting;

// Token: 0x0200100E RID: 4110
[Preserve]
public class XUiC_CraftingQueue : XUiController
{
	// Token: 0x0600814E RID: 33102 RVA: 0x003133C4 File Offset: 0x003115C4
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_RecipeStack>(null);
		this.queueItems = childrenByType;
		for (int i = 0; i < this.queueItems.Length; i++)
		{
			((XUiC_RecipeStack)this.queueItems[i]).Owner = this;
		}
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x0031340C File Offset: 0x0031160C
	public void ClearQueue()
	{
		for (int i = this.queueItems.Length - 1; i >= 0; i--)
		{
			XUiC_RecipeStack xuiC_RecipeStack = (XUiC_RecipeStack)this.queueItems[i];
			xuiC_RecipeStack.SetRecipe(null, 0, 0f, true, -1, -1, -1f);
			xuiC_RecipeStack.IsCrafting = false;
			xuiC_RecipeStack.IsDirty = true;
		}
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x0031345E File Offset: 0x0031165E
	public void HaltCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = false;
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x0031347C File Offset: 0x0031167C
	public void ResumeCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = true;
	}

	// Token: 0x06008152 RID: 33106 RVA: 0x0031349A File Offset: 0x0031169A
	public bool IsCrafting()
	{
		return ((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting;
	}

	// Token: 0x06008153 RID: 33107 RVA: 0x003134B8 File Offset: 0x003116B8
	public bool AddItemToRepair(float _repairTimeLeft, ItemValue _itemToRepair, int _amountToRepair)
	{
		for (int i = this.queueItems.Length - 1; i >= 0; i--)
		{
			XUiC_RecipeStack xuiC_RecipeStack = (XUiC_RecipeStack)this.queueItems[i];
			if (!xuiC_RecipeStack.HasRecipe() && xuiC_RecipeStack.SetRepairRecipe(_repairTimeLeft, _itemToRepair, _amountToRepair))
			{
				xuiC_RecipeStack.IsCrafting = i == this.queueItems.Length - 1;
				xuiC_RecipeStack.IsDirty = true;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008154 RID: 33108 RVA: 0x00313518 File Offset: 0x00311718
	public bool AddRecipeToCraft(Recipe _recipe, int _count = 1, float craftTime = -1f, bool isCrafting = true, float _oneItemCraftingTime = -1f)
	{
		for (int i = this.queueItems.Length - 1; i >= 0; i--)
		{
			if (this.AddRecipeToCraftAtIndex(i, _recipe, _count, craftTime, isCrafting, false, -1, -1, _oneItemCraftingTime))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008155 RID: 33109 RVA: 0x00313550 File Offset: 0x00311750
	public bool AddRecipeToCraftAtIndex(int _index, Recipe _recipe, int _count = 1, float craftTime = -1f, bool isCrafting = true, bool recipeModification = false, int lastQuality = -1, int startingEntityId = -1, float _oneItemCraftingTime = -1f)
	{
		XUiC_RecipeStack xuiC_RecipeStack = (XUiC_RecipeStack)this.queueItems[_index];
		if (xuiC_RecipeStack.SetRecipe(_recipe, _count, craftTime, recipeModification, -1, -1, _oneItemCraftingTime))
		{
			xuiC_RecipeStack.IsCrafting = _index == this.queueItems.Length - 1 && isCrafting;
			if (lastQuality != -1)
			{
				xuiC_RecipeStack.OutputQuality = lastQuality;
			}
			if (startingEntityId != -1)
			{
				xuiC_RecipeStack.StartingEntityId = startingEntityId;
			}
			xuiC_RecipeStack.IsDirty = true;
			return true;
		}
		return false;
	}

	// Token: 0x06008156 RID: 33110 RVA: 0x003135B8 File Offset: 0x003117B8
	public bool AddItemToRepairAtIndex(int _index, float _repairTimeLeft, ItemValue _itemToRepair, int _amountToRepair, bool isCrafting = true, int startingEntityId = -1)
	{
		XUiC_RecipeStack xuiC_RecipeStack = (XUiC_RecipeStack)this.queueItems[_index];
		if (xuiC_RecipeStack.SetRepairRecipe(_repairTimeLeft, _itemToRepair, _amountToRepair))
		{
			xuiC_RecipeStack.IsCrafting = _index == this.queueItems.Length - 1;
			xuiC_RecipeStack.StartingEntityId = ((startingEntityId != -1) ? startingEntityId : xuiC_RecipeStack.StartingEntityId);
			xuiC_RecipeStack.IsDirty = true;
			return true;
		}
		return false;
	}

	// Token: 0x06008157 RID: 33111 RVA: 0x00313614 File Offset: 0x00311814
	public XUiC_RecipeStack[] GetRecipesToCraft()
	{
		XUiC_RecipeStack[] array = new XUiC_RecipeStack[this.queueItems.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (XUiC_RecipeStack)this.queueItems[i];
		}
		return array;
	}

	// Token: 0x06008158 RID: 33112 RVA: 0x00313650 File Offset: 0x00311850
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = false;
		for (int i = this.queueItems.Length - 1; i >= 0; i--)
		{
			if (((XUiC_RecipeStack)this.queueItems[i]).HasRecipe())
			{
				flag = true;
				break;
			}
		}
		if (this.toolGrid != null)
		{
			this.toolGrid.SetToolLocks(flag);
		}
		if (!flag)
		{
			return;
		}
		XUiC_RecipeStack xuiC_RecipeStack = (XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1];
		if (!xuiC_RecipeStack.HasRecipe())
		{
			for (int j = this.queueItems.Length - 1; j >= 0; j--)
			{
				XUiC_RecipeStack xuiC_RecipeStack2 = (XUiC_RecipeStack)this.queueItems[j];
				if (j != 0)
				{
					((XUiC_RecipeStack)this.queueItems[j - 1]).CopyTo(xuiC_RecipeStack2);
				}
				else
				{
					((XUiC_RecipeStack)this.queueItems[0]).SetRecipe(null, 0, 0f, true, -1, -1, -1f);
				}
			}
		}
		if (xuiC_RecipeStack.HasRecipe() && !xuiC_RecipeStack.IsCrafting)
		{
			xuiC_RecipeStack.IsCrafting = true;
		}
	}

	// Token: 0x06008159 RID: 33113 RVA: 0x00313742 File Offset: 0x00311942
	public override void OnOpen()
	{
		base.OnOpen();
		this.toolGrid = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
	}

	// Token: 0x0600815A RID: 33114 RVA: 0x00313760 File Offset: 0x00311960
	public void RefreshQueue()
	{
		XUiC_RecipeStack xuiC_RecipeStack = null;
		for (int i = this.queueItems.Length - 1; i >= 0; i--)
		{
			XUiC_RecipeStack xuiC_RecipeStack2 = (XUiC_RecipeStack)this.queueItems[i];
			if (xuiC_RecipeStack2.GetRecipe() != null && xuiC_RecipeStack != null && xuiC_RecipeStack.GetRecipe() == null)
			{
				xuiC_RecipeStack2.CopyTo(xuiC_RecipeStack);
				xuiC_RecipeStack2.SetRecipe(null, 0, 0f, true, -1, -1, -1f);
			}
			xuiC_RecipeStack = xuiC_RecipeStack2;
		}
	}

	// Token: 0x04006000 RID: 24576
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolGrid;

	// Token: 0x04006001 RID: 24577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] queueItems;
}
