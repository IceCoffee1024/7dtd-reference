using System;
using UnityEngine.Scripting;

// Token: 0x02000FFF RID: 4095
[Preserve]
public class XUiC_CraftingQueue : XUiController
{
	// Token: 0x0600813F RID: 33087 RVA: 0x003103DC File Offset: 0x0030E5DC
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

	// Token: 0x06008140 RID: 33088 RVA: 0x00310424 File Offset: 0x0030E624
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

	// Token: 0x06008141 RID: 33089 RVA: 0x00310476 File Offset: 0x0030E676
	public void HaltCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = false;
	}

	// Token: 0x06008142 RID: 33090 RVA: 0x00310494 File Offset: 0x0030E694
	public void ResumeCrafting()
	{
		((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting = true;
	}

	// Token: 0x06008143 RID: 33091 RVA: 0x003104B2 File Offset: 0x0030E6B2
	public bool IsCrafting()
	{
		return ((XUiC_RecipeStack)this.queueItems[this.queueItems.Length - 1]).IsCrafting;
	}

	// Token: 0x06008144 RID: 33092 RVA: 0x003104D0 File Offset: 0x0030E6D0
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

	// Token: 0x06008145 RID: 33093 RVA: 0x00310530 File Offset: 0x0030E730
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

	// Token: 0x06008146 RID: 33094 RVA: 0x00310568 File Offset: 0x0030E768
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

	// Token: 0x06008147 RID: 33095 RVA: 0x003105D0 File Offset: 0x0030E7D0
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

	// Token: 0x06008148 RID: 33096 RVA: 0x0031062C File Offset: 0x0030E82C
	public XUiC_RecipeStack[] GetRecipesToCraft()
	{
		XUiC_RecipeStack[] array = new XUiC_RecipeStack[this.queueItems.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (XUiC_RecipeStack)this.queueItems[i];
		}
		return array;
	}

	// Token: 0x06008149 RID: 33097 RVA: 0x00310668 File Offset: 0x0030E868
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

	// Token: 0x0600814A RID: 33098 RVA: 0x0031075A File Offset: 0x0030E95A
	public override void OnOpen()
	{
		base.OnOpen();
		this.toolGrid = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
	}

	// Token: 0x0600814B RID: 33099 RVA: 0x00310778 File Offset: 0x0030E978
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

	// Token: 0x04005FD7 RID: 24535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolGrid;

	// Token: 0x04005FD8 RID: 24536
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] queueItems;
}
