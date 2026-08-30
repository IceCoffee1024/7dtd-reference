using System;
using UnityEngine.Scripting;

// Token: 0x02000F1D RID: 3869
[Preserve]
public class XUiC_DewCollectorModGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06007962 RID: 31074 RVA: 0x000817FC File Offset: 0x0007F9FC
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x06007963 RID: 31075 RVA: 0x002DC3A5 File Offset: 0x002DA5A5
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x06007964 RID: 31076 RVA: 0x002DC3AD File Offset: 0x002DA5AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.SetModSlots(stackList, false);
		this.tileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007965 RID: 31077 RVA: 0x002DC3E0 File Offset: 0x002DA5E0
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

	// Token: 0x06007966 RID: 31078 RVA: 0x002DC48C File Offset: 0x002DA68C
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

	// Token: 0x06007967 RID: 31079 RVA: 0x002DC4E0 File Offset: 0x002DA6E0
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentCollectorModGrid = this;
	}

	// Token: 0x06007968 RID: 31080 RVA: 0x002DC4F4 File Offset: 0x002DA6F4
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentCollectorModGrid = null;
	}

	// Token: 0x040059BB RID: 22971
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x040059BC RID: 22972
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockCollector blockCollector;
}
