using System;
using UnityEngine.Scripting;

// Token: 0x02000F21 RID: 3873
[Preserve]
public class XUiC_Creative2StackGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x06007906 RID: 30982 RVA: 0x002DDEDD File Offset: 0x002DC0DD
	// (set) Token: 0x06007907 RID: 30983 RVA: 0x002DDEE5 File Offset: 0x002DC0E5
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x06007908 RID: 30984 RVA: 0x002DDEEE File Offset: 0x002DC0EE
	// (set) Token: 0x06007909 RID: 30985 RVA: 0x002DDEF6 File Offset: 0x002DC0F6
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

	// Token: 0x0600790A RID: 30986 RVA: 0x002DDF06 File Offset: 0x002DC106
	public override void Init()
	{
		base.Init();
		this.Length = this.itemControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x0600790B RID: 30987 RVA: 0x002DDF23 File Offset: 0x002DC123
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x0600790C RID: 30988 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x0600790D RID: 30989 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x0600790E RID: 30990 RVA: 0x002DDF2C File Offset: 0x002DC12C
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

	// Token: 0x0600790F RID: 30991 RVA: 0x002DDF90 File Offset: 0x002DC190
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

	// Token: 0x040059C0 RID: 22976
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
