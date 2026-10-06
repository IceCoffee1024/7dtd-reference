using System;
using UnityEngine.Scripting;

// Token: 0x020010E7 RID: 4327
[Preserve]
public class XUiC_WorkstationToolGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000E1 RID: 225
	// (add) Token: 0x06008992 RID: 35218 RVA: 0x00349298 File Offset: 0x00347498
	// (remove) Token: 0x06008993 RID: 35219 RVA: 0x003492D0 File Offset: 0x003474D0
	public event XuiEvent_WorkstationItemsChanged OnWorkstationToolsChanged;

	// Token: 0x06008994 RID: 35220 RVA: 0x00349308 File Offset: 0x00347508
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

	// Token: 0x06008995 RID: 35221 RVA: 0x00349398 File Offset: 0x00347598
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

	// Token: 0x06008996 RID: 35222 RVA: 0x003493E4 File Offset: 0x003475E4
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

	// Token: 0x06008997 RID: 35223 RVA: 0x00349422 File Offset: 0x00347622
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetToolStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06008998 RID: 35224 RVA: 0x00349448 File Offset: 0x00347648
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

	// Token: 0x06008999 RID: 35225 RVA: 0x0034949C File Offset: 0x0034769C
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

	// Token: 0x0600899A RID: 35226 RVA: 0x003494F3 File Offset: 0x003476F3
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		if (this.OnWorkstationToolsChanged != null)
		{
			this.OnWorkstationToolsChanged();
		}
	}

	// Token: 0x0600899B RID: 35227 RVA: 0x00349510 File Offset: 0x00347710
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationToolGrid = this;
	}

	// Token: 0x0600899C RID: 35228 RVA: 0x00349524 File Offset: 0x00347724
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationToolGrid = null;
	}

	// Token: 0x04006650 RID: 26192
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04006651 RID: 26193
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredTools = "";

	// Token: 0x04006652 RID: 26194
	[PublicizedFrom(EAccessModifier.Private)]
	public bool requiredToolsOnly;
}
