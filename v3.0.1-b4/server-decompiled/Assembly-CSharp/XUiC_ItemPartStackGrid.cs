using System;
using UnityEngine.Scripting;

// Token: 0x02000F3E RID: 3902
[Preserve]
public class XUiC_ItemPartStackGrid : XUiController
{
	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x06007A9A RID: 31386 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x06007A9B RID: 31387 RVA: 0x002E5BB9 File Offset: 0x002E3DB9
	// (set) Token: 0x06007A9C RID: 31388 RVA: 0x002E5BC1 File Offset: 0x002E3DC1
	public ItemStack CurrentItem { get; set; }

	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06007A9D RID: 31389 RVA: 0x002E5BCA File Offset: 0x002E3DCA
	// (set) Token: 0x06007A9E RID: 31390 RVA: 0x002E5BD2 File Offset: 0x002E3DD2
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007A9F RID: 31391 RVA: 0x002E5BDC File Offset: 0x002E3DDC
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemPartStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007AA0 RID: 31392 RVA: 0x002E5C05 File Offset: 0x002E3E05
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007AA1 RID: 31393 RVA: 0x002E5C28 File Offset: 0x002E3E28
	public void SetParts(ItemValue[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.currentItemClass = this.CurrentItem.itemValue.ItemClass;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[i];
			if (i < this.CurrentItem.itemValue.Modifications.Length)
			{
				ItemValue itemValue = this.CurrentItem.itemValue.Modifications[i];
				if (itemValue != null && itemValue.ItemClass is ItemClassModifier)
				{
					xuiC_ItemPartStack.SlotType = (itemValue.ItemClass as ItemClassModifier).Type.ToStringCached<ItemClassModifier.ModifierTypes>().ToLower();
				}
				xuiC_ItemPartStack.SlotChangedEvent -= this.HandleSlotChangedEvent;
				xuiC_ItemPartStack.ItemValue = ((itemValue != null) ? itemValue : ItemValue.None);
				xuiC_ItemPartStack.SlotChangedEvent += this.HandleSlotChangedEvent;
				xuiC_ItemPartStack.SlotNumber = i;
				xuiC_ItemPartStack.InfoWindow = childByType;
				xuiC_ItemPartStack.StackLocation = this.StackLocation;
				xuiC_ItemPartStack.ViewComponent.IsVisible = true;
			}
			else
			{
				xuiC_ItemPartStack.ViewComponent.IsVisible = false;
			}
		}
	}

	// Token: 0x06007AA2 RID: 31394 RVA: 0x002E5D48 File Offset: 0x002E3F48
	public void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[slotNumber];
		ItemValue itemValue = (xuiC_ItemPartStack.ItemStack.IsEmpty() ? ItemValue.None : xuiC_ItemPartStack.ItemStack.itemValue);
		if (itemValue.ItemClass != null)
		{
			ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.CurrentItem.itemValue.CosmeticMods.Length != 0)
				{
					this.CurrentItem.itemValue.CosmeticMods[0] = itemValue;
					goto IL_00A7;
				}
				this.CurrentItem.itemValue.Modifications[slotNumber] = itemValue;
				goto IL_00A7;
			}
		}
		this.CurrentItem.itemValue.Modifications[slotNumber] = itemValue;
		IL_00A7:
		this.AssembleWindow.ItemStack = this.CurrentItem;
		this.AssembleWindow.OnChanged();
		this.xui.AssembleItem.RefreshAssembleItem();
	}

	// Token: 0x06007AA3 RID: 31395 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007AA4 RID: 31396 RVA: 0x002DD4B9 File Offset: 0x002DB6B9
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007AA5 RID: 31397 RVA: 0x002E5E28 File Offset: 0x002E4028
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005AA2 RID: 23202
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005AA3 RID: 23203
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005AA4 RID: 23204
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005AA5 RID: 23205
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005AA8 RID: 23208
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass currentItemClass;
}
