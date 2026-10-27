using System;
using UnityEngine.Scripting;

// Token: 0x02000F30 RID: 3888
[Preserve]
public class XUiC_Creative2StackGrid : XUiC_ItemStackGrid
{
	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x06007949 RID: 31049 RVA: 0x002DF7CD File Offset: 0x002DD9CD
	// (set) Token: 0x0600794A RID: 31050 RVA: 0x002DF7D5 File Offset: 0x002DD9D5
	public int Length
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x0600794B RID: 31051 RVA: 0x002DF7DE File Offset: 0x002DD9DE
	// (set) Token: 0x0600794C RID: 31052 RVA: 0x002DF7E6 File Offset: 0x002DD9E6
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

	// Token: 0x0600794D RID: 31053 RVA: 0x002DF7F6 File Offset: 0x002DD9F6
	public override void Init()
	{
		base.Init();
		this.Length = this.itemControllers.Length;
		this.IsDirty = false;
	}

	// Token: 0x0600794E RID: 31054 RVA: 0x002DF813 File Offset: 0x002DDA13
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x0600794F RID: 31055 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007950 RID: 31056 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x06007951 RID: 31057 RVA: 0x002DF81C File Offset: 0x002DDA1C
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

	// Token: 0x06007952 RID: 31058 RVA: 0x002DF880 File Offset: 0x002DDA80
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

	// Token: 0x040059F3 RID: 23027
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;
}
