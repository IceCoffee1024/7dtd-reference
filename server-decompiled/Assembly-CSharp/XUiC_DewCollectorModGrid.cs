using System;
using UnityEngine.Scripting;

// Token: 0x02000F3B RID: 3899
[Preserve]
public class XUiC_DewCollectorModGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x060079A0 RID: 31136 RVA: 0x00060491 File Offset: 0x0005E691
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x060079A1 RID: 31137 RVA: 0x002E1031 File Offset: 0x002DF231
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060079A2 RID: 31138 RVA: 0x002E1039 File Offset: 0x002DF239
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.SetModSlots(stackList, false);
		this.tileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x060079A3 RID: 31139 RVA: 0x002E106C File Offset: 0x002DF26C
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

	// Token: 0x060079A4 RID: 31140 RVA: 0x002E1118 File Offset: 0x002DF318
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

	// Token: 0x060079A5 RID: 31141 RVA: 0x002E116C File Offset: 0x002DF36C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentCollectorModGrid = this;
	}

	// Token: 0x060079A6 RID: 31142 RVA: 0x002E1180 File Offset: 0x002DF380
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentCollectorModGrid = null;
	}

	// Token: 0x04005A26 RID: 23078
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x04005A27 RID: 23079
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockCollector blockCollector;
}
