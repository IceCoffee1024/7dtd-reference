using System;
using UnityEngine.Scripting;

// Token: 0x02000F2C RID: 3884
[Preserve]
public class XUiC_DewCollectorModGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x0600795D RID: 31069 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Workstation;
		}
	}

	// Token: 0x0600795E RID: 31070 RVA: 0x002DF741 File Offset: 0x002DD941
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600795F RID: 31071 RVA: 0x002DF749 File Offset: 0x002DD949
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		this.tileEntity.SetModSlots(stackList, false);
		this.tileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007960 RID: 31072 RVA: 0x002DF77C File Offset: 0x002DD97C
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

	// Token: 0x06007961 RID: 31073 RVA: 0x002DF828 File Offset: 0x002DDA28
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

	// Token: 0x06007962 RID: 31074 RVA: 0x002DF87C File Offset: 0x002DDA7C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CurrentCollectorModGrid = this;
	}

	// Token: 0x06007963 RID: 31075 RVA: 0x002DF890 File Offset: 0x002DDA90
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CurrentCollectorModGrid = null;
	}

	// Token: 0x040059F3 RID: 23027
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector tileEntity;

	// Token: 0x040059F4 RID: 23028
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockCollector blockCollector;
}
