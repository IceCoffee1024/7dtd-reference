using System;
using UnityEngine.Scripting;

// Token: 0x02000F06 RID: 3846
[Preserve]
public class XUiC_DewCollectorModGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x060078AA RID: 30890 RVA: 0x00081148 File Offset: 0x0007F348
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x060078AB RID: 30891 RVA: 0x002D8419 File Offset: 0x002D6619
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060078AC RID: 30892 RVA: 0x002D8421 File Offset: 0x002D6621
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.SetModSlots(stackList, false);
		this.tileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x060078AD RID: 30893 RVA: 0x002D8454 File Offset: 0x002D6654
	public void SetTileEntity(TileEntityCollector te)
	{
		this.tileEntity = te;
		this.blockCollector = te.blockValue.Block as BlockCollector;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_RequiredItemStack xuiC_RequiredItemStack = this.itemControllers[i] as XUiC_RequiredItemStack;
			if (xuiC_RequiredItemStack != null)
			{
				if (i < this.blockCollector.RequiredMods.Length)
				{
					xuiC_RequiredItemStack.SetAllowedItemClassSingle(this.blockCollector.RequiredMods[i]);
					xuiC_RequiredItemStack.RequiredItemOnly = this.blockCollector.RequiredModsOnly;
				}
				else
				{
					xuiC_RequiredItemStack.ClearAllowedItemClasses();
					xuiC_RequiredItemStack.RequiredItemOnly = false;
				}
				xuiC_RequiredItemStack.StackLocation = this.StackLocation;
			}
		}
		this.SetStacks(te.ModSlots);
	}

	// Token: 0x060078AE RID: 30894 RVA: 0x002D8500 File Offset: 0x002D6700
	public bool TryAddMod(ItemClass newItemClass, ItemStack newItemStack)
	{
		if (!this.blockCollector.RequiredModsOnly)
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

	// Token: 0x060078AF RID: 30895 RVA: 0x002D8554 File Offset: 0x002D6754
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentCollectorModGrid = this;
	}

	// Token: 0x060078B0 RID: 30896 RVA: 0x002D8568 File Offset: 0x002D6768
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentCollectorModGrid = null;
	}

	// Token: 0x04005909 RID: 22793
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x0400590A RID: 22794
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockCollector blockCollector;
}
