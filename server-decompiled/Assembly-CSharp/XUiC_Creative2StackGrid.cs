using System;
using UnityEngine.Scripting;

// Token: 0x02000F12 RID: 3858
[Preserve]
public class XUiC_Creative2StackGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x0600790B RID: 30987 RVA: 0x002DAB48 File Offset: 0x002D8D48
	// (set) Token: 0x0600790C RID: 30988 RVA: 0x002DAB50 File Offset: 0x002D8D50
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x0600790D RID: 30989 RVA: 0x002DAB59 File Offset: 0x002D8D59
	// (set) Token: 0x0600790E RID: 30990 RVA: 0x002DAB61 File Offset: 0x002D8D61
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

	// Token: 0x0600790F RID: 30991 RVA: 0x002DAB71 File Offset: 0x002D8D71
	public override void Init()
	{
		base.Init();
		this.Length = this.itemControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x06007910 RID: 30992 RVA: 0x002C5CCA File Offset: 0x002C3ECA
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x06007911 RID: 30993 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007912 RID: 30994 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x06007913 RID: 30995 RVA: 0x002DAB90 File Offset: 0x002D8D90
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

	// Token: 0x06007914 RID: 30996 RVA: 0x002DABF4 File Offset: 0x002D8DF4
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

	// Token: 0x04005988 RID: 22920
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
