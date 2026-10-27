using System;
using UnityEngine.Scripting;

// Token: 0x020010F7 RID: 4343
[Preserve]
public class XUiC_WorkstationToolGrid : XUiC_WorkstationGrid
{
	// Token: 0x140000E1 RID: 225
	// (add) Token: 0x060089E5 RID: 35301 RVA: 0x0034B31C File Offset: 0x0034951C
	// (remove) Token: 0x060089E6 RID: 35302 RVA: 0x0034B354 File Offset: 0x00349554
	public event XuiEvent_WorkstationItemsChanged OnWorkstationToolsChanged;

	// Token: 0x060089E7 RID: 35303 RVA: 0x0034B38C File Offset: 0x0034958C
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

	// Token: 0x060089E8 RID: 35304 RVA: 0x0034B41C File Offset: 0x0034961C
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

	// Token: 0x060089E9 RID: 35305 RVA: 0x0034B468 File Offset: 0x00349668
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

	// Token: 0x060089EA RID: 35306 RVA: 0x0034B4A6 File Offset: 0x003496A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.workstationData.SetToolStacks(stackList);
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x060089EB RID: 35307 RVA: 0x0034B4CC File Offset: 0x003496CC
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

	// Token: 0x060089EC RID: 35308 RVA: 0x0034B520 File Offset: 0x00349720
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

	// Token: 0x060089ED RID: 35309 RVA: 0x0034B577 File Offset: 0x00349777
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		base.HandleSlotChangedEvent(slotNumber, stack);
		if (this.OnWorkstationToolsChanged != null)
		{
			this.OnWorkstationToolsChanged();
		}
	}

	// Token: 0x060089EE RID: 35310 RVA: 0x0034B594 File Offset: 0x00349794
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentWorkstationToolGrid = this;
	}

	// Token: 0x060089EF RID: 35311 RVA: 0x0034B5A8 File Offset: 0x003497A8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentWorkstationToolGrid = null;
	}

	// Token: 0x0400668F RID: 26255
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04006690 RID: 26256
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredTools = "";

	// Token: 0x04006691 RID: 26257
	[PublicizedFrom(EAccessModifier.Private)]
	public bool requiredToolsOnly;
}
