using System;
using UnityEngine.Scripting;

// Token: 0x020010BD RID: 4285
[Preserve]
public class XUiC_WorkstationToolGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000E1 RID: 225
	// (add) Token: 0x06008852 RID: 34898 RVA: 0x00340CC4 File Offset: 0x0033EEC4
	// (remove) Token: 0x06008853 RID: 34899 RVA: 0x00340CFC File Offset: 0x0033EEFC
	public event XuiEvent_WorkstationItemsChanged OnWorkstationToolsChanged;

	// Token: 0x06008854 RID: 34900 RVA: 0x00340D34 File Offset: 0x0033EF34
	public override void Init()
	{
		base.Init();
		string[] array = this.requiredTools.Split(',', StringSplitOptions.None);
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			if (i < array.Length)
			{
				((XUiC_RequiredItemStack)this.itemControllers[i]).SetAllowedItemClassSingle(array[i]);
				((XUiC_RequiredItemStack)this.itemControllers[i]).RequiredItemOnly = this.requiredToolsOnly;
			}
			else
			{
				((XUiC_RequiredItemStack)this.itemControllers[i]).ClearAllowedItemClasses();
				((XUiC_RequiredItemStack)this.itemControllers[i]).RequiredItemOnly = false;
			}
		}
	}

	// Token: 0x06008855 RID: 34901 RVA: 0x00340DC4 File Offset: 0x0033EFC4
	public override bool HasRequirement(Recipe recipe)
	{
		if (recipe == null)
		{
			return false;
		}
		if (recipe.craftingToolType == 0)
		{
			return true;
		}
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].itemValue.type == recipe.craftingToolType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008856 RID: 34902 RVA: 0x00340E10 File Offset: 0x0033F010
	public void SetToolLocks(bool locked)
	{
		if (locked != this.isLocked)
		{
			this.isLocked = locked;
			for (int i = 0; i < this.itemControllers.Length; i++)
			{
				this.itemControllers[i].ToolLock = locked;
			}
		}
	}

	// Token: 0x06008857 RID: 34903 RVA: 0x00340E4E File Offset: 0x0033F04E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetToolStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008858 RID: 34904 RVA: 0x00340E74 File Offset: 0x0033F074
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "required_tools"))
			{
				if (!(name == "required_tools_only"))
				{
					return false;
				}
				this.requiredToolsOnly = StringParsers.ParseBool(value, 0, -1, true);
			}
			else
			{
				this.requiredTools = value;
			}
			return true;
		}
		return flag;
	}

	// Token: 0x06008859 RID: 34905 RVA: 0x00340EC8 File Offset: 0x0033F0C8
	public bool TryAddTool(ItemClass newItemClass, ItemStack newItemStack)
	{
		if (!this.requiredToolsOnly || this.isLocked)
		{
			return false;
		}
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		for (int i = 0; i < itemControllers.Length; i++)
		{
			((XUiC_RequiredItemStack)itemControllers[i]).TryStack(newItemStack, -1);
			if (newItemStack.count == 0)
			{
				return true;
			}
		}
		return newItemStack.count == 0;
	}

	// Token: 0x0600885A RID: 34906 RVA: 0x00340F1F File Offset: 0x0033F11F
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		if (this.OnWorkstationToolsChanged != null)
		{
			this.OnWorkstationToolsChanged();
		}
	}

	// Token: 0x0600885B RID: 34907 RVA: 0x00340F3C File Offset: 0x0033F13C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationToolGrid = this;
	}

	// Token: 0x0600885C RID: 34908 RVA: 0x00340F50 File Offset: 0x0033F150
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationToolGrid = null;
	}

	// Token: 0x04006547 RID: 25927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04006548 RID: 25928
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredTools = "";

	// Token: 0x04006549 RID: 25929
	[PublicizedFrom(EAccessModifier.Private)]
	public bool requiredToolsOnly;
}
