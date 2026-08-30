using System;
using UnityEngine.Scripting;

// Token: 0x02000EFB RID: 3835
[Preserve]
public class XUiC_Creative2StackGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x06007853 RID: 30803 RVA: 0x002D6BBC File Offset: 0x002D4DBC
	// (set) Token: 0x06007854 RID: 30804 RVA: 0x002D6BC4 File Offset: 0x002D4DC4
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06007855 RID: 30805 RVA: 0x002D6BCD File Offset: 0x002D4DCD
	// (set) Token: 0x06007856 RID: 30806 RVA: 0x002D6BD5 File Offset: 0x002D4DD5
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			this.page = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007857 RID: 30807 RVA: 0x002D6BE5 File Offset: 0x002D4DE5
	public override void Init()
	{
		base.Init();
		this.Length = this.itemControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x06007858 RID: 30808 RVA: 0x002C27A6 File Offset: 0x002C09A6
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x06007859 RID: 30809 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x0600785A RID: 30810 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x0600785B RID: 30811 RVA: 0x002D6C04 File Offset: 0x002D4E04
	public void SetSlots(ItemStack[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.items = stackList;
		this.IsDirty = true;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.Creative;
			this.itemControllers[i].ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x0600785C RID: 30812 RVA: 0x002D6C68 File Offset: 0x002D4E68
	public override void Update(float _dt)
	{
		if (!base.ViewComponent.IsVisible)
		{
			return;
		}
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.IsDirty && this.xui.PlayerInventory != null)
		{
			for (int i = 0; i < this.Length; i++)
			{
				int num = i + this.Length * this.page;
				XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
				if (xuiC_ItemStack != null)
				{
					if (num < this.items.Length)
					{
						xuiC_ItemStack.ItemStack = this.items[num];
					}
					else
					{
						xuiC_ItemStack.ItemStack = ItemStack.Empty;
						if (xuiC_ItemStack.IsSelected)
						{
							xuiC_ItemStack.IsSelected = false;
						}
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x040058D6 RID: 22742
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
