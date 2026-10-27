using System;
using UnityEngine.Scripting;

// Token: 0x0200101E RID: 4126
[Preserve]
public class XUiC_CraftingQueue : XUiController
{
	// Token: 0x060081A0 RID: 33184 RVA: 0x003150C8 File Offset: 0x003132C8
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

	// Token: 0x060081A1 RID: 33185 RVA: 0x00315110 File Offset: 0x00313310
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

	// Token: 0x060081A2 RID: 33186 RVA: 0x00315162 File Offset: 0x00313362
	public void HaltCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = false;
	}

	// Token: 0x060081A3 RID: 33187 RVA: 0x00315180 File Offset: 0x00313380
	public void ResumeCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = true;
	}

	// Token: 0x060081A4 RID: 33188 RVA: 0x0031519E File Offset: 0x0031339E
	public bool IsCrafting()
	{
		return ((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting;
	}

	// Token: 0x060081A5 RID: 33189 RVA: 0x003151BC File Offset: 0x003133BC
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

	// Token: 0x060081A6 RID: 33190 RVA: 0x0031521C File Offset: 0x0031341C
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

	// Token: 0x060081A7 RID: 33191 RVA: 0x00315254 File Offset: 0x00313454
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

	// Token: 0x060081A8 RID: 33192 RVA: 0x003152BC File Offset: 0x003134BC
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

	// Token: 0x060081A9 RID: 33193 RVA: 0x00315318 File Offset: 0x00313518
	public XUiC_RecipeStack[] GetRecipesToCraft()
	{
		XUiC_RecipeStack[] array = new XUiC_RecipeStack[this.queueItems.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (XUiC_RecipeStack)this.queueItems[i];
		}
		return array;
	}

	// Token: 0x060081AA RID: 33194 RVA: 0x00315354 File Offset: 0x00313554
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

	// Token: 0x060081AB RID: 33195 RVA: 0x00315446 File Offset: 0x00313646
	public override void OnOpen()
	{
		base.OnOpen();
		this.toolGrid = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
	}

	// Token: 0x060081AC RID: 33196 RVA: 0x00315464 File Offset: 0x00313664
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

	// Token: 0x0400603E RID: 24638
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolGrid;

	// Token: 0x0400603F RID: 24639
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] queueItems;
}
