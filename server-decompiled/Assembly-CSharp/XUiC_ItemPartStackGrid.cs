using System;
using UnityEngine.Scripting;

// Token: 0x02000F55 RID: 3925
[Preserve]
public class XUiC_ItemPartStackGrid : XUiController
{
	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x06007B53 RID: 31571 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06007B54 RID: 31572 RVA: 0x002E9A45 File Offset: 0x002E7C45
	// (set) Token: 0x06007B55 RID: 31573 RVA: 0x002E9A4D File Offset: 0x002E7C4D
	public ItemStack CurrentItem { get; set; }

	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x06007B56 RID: 31574 RVA: 0x002E9A56 File Offset: 0x002E7C56
	// (set) Token: 0x06007B57 RID: 31575 RVA: 0x002E9A5E File Offset: 0x002E7C5E
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007B58 RID: 31576 RVA: 0x002E9A68 File Offset: 0x002E7C68
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemPartStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007B59 RID: 31577 RVA: 0x002E9A91 File Offset: 0x002E7C91
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007B5A RID: 31578 RVA: 0x002E9AB4 File Offset: 0x002E7CB4
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

	// Token: 0x06007B5B RID: 31579 RVA: 0x002E9BD4 File Offset: 0x002E7DD4
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

	// Token: 0x06007B5C RID: 31580 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B5D RID: 31581 RVA: 0x002E146D File Offset: 0x002DF66D
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007B5E RID: 31582 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005B55 RID: 23381
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005B56 RID: 23382
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005B57 RID: 23383
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005B58 RID: 23384
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005B5B RID: 23387
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass currentItemClass;
}
