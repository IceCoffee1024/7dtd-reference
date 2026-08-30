using System;
using UnityEngine.Scripting;

// Token: 0x02000FE6 RID: 4070
[Preserve]
public class XUiC_CraftingQueue : XUiController
{
	// Token: 0x0600804D RID: 32845 RVA: 0x0030BBA8 File Offset: 0x00309DA8
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

	// Token: 0x0600804E RID: 32846 RVA: 0x0030BBF0 File Offset: 0x00309DF0
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

	// Token: 0x0600804F RID: 32847 RVA: 0x0030BC42 File Offset: 0x00309E42
	public void HaltCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = false;
	}

	// Token: 0x06008050 RID: 32848 RVA: 0x0030BC60 File Offset: 0x00309E60
	public void ResumeCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = true;
	}

	// Token: 0x06008051 RID: 32849 RVA: 0x0030BC7E File Offset: 0x00309E7E
	public bool IsCrafting()
	{
		return ((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting;
	}

	// Token: 0x06008052 RID: 32850 RVA: 0x0030BC9C File Offset: 0x00309E9C
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

	// Token: 0x06008053 RID: 32851 RVA: 0x0030BCFC File Offset: 0x00309EFC
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

	// Token: 0x06008054 RID: 32852 RVA: 0x0030BD34 File Offset: 0x00309F34
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

	// Token: 0x06008055 RID: 32853 RVA: 0x0030BD9C File Offset: 0x00309F9C
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

	// Token: 0x06008056 RID: 32854 RVA: 0x0030BDF8 File Offset: 0x00309FF8
	public XUiC_RecipeStack[] GetRecipesToCraft()
	{
		XUiC_RecipeStack[] array = new XUiC_RecipeStack[this.queueItems.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (XUiC_RecipeStack)this.queueItems[i];
		}
		return array;
	}

	// Token: 0x06008057 RID: 32855 RVA: 0x0030BE34 File Offset: 0x0030A034
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

	// Token: 0x06008058 RID: 32856 RVA: 0x0030BF26 File Offset: 0x0030A126
	public override void OnOpen()
	{
		base.OnOpen();
		this.toolGrid = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
	}

	// Token: 0x06008059 RID: 32857 RVA: 0x0030BF44 File Offset: 0x0030A144
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

	// Token: 0x04005F10 RID: 24336
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolGrid;

	// Token: 0x04005F11 RID: 24337
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] queueItems;
}
