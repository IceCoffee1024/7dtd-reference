using System;
using UnityEngine.Scripting;

// Token: 0x02000F64 RID: 3940
[Preserve]
public class XUiC_ItemPartStackGrid : XUiController
{
	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06007B50 RID: 31568 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06007B51 RID: 31569 RVA: 0x002ECD73 File Offset: 0x002EAF73
	// (set) Token: 0x06007B52 RID: 31570 RVA: 0x002ECD7B File Offset: 0x002EAF7B
	public ItemStack CurrentItem
	{
		get
		{
			return this.currentItem;
		}
		set
		{
			this.currentItem = value;
			this.SetParts();
		}
	}

	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x06007B53 RID: 31571 RVA: 0x002ECD8A File Offset: 0x002EAF8A
	// (set) Token: 0x06007B54 RID: 31572 RVA: 0x002ECD92 File Offset: 0x002EAF92
	public XUiC_AssembleWindow AssembleWindow { get; set; }

	// Token: 0x06007B55 RID: 31573 RVA: 0x002ECD9C File Offset: 0x002EAF9C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_ItemPartStack>(null);
		this.itemControllers = childrenByType;
		this.IsDirty = false;
	}

	// Token: 0x06007B56 RID: 31574 RVA: 0x002ECDC5 File Offset: 0x002EAFC5
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007B57 RID: 31575 RVA: 0x002ECDE8 File Offset: 0x002EAFE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParts()
	{
		ItemValue itemValue = this.currentItem.itemValue;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[i];
			if (i < itemValue.ModificationCount)
			{
				ItemValue modification = itemValue.GetModification(i);
				if (modification != null && modification.ItemClass is ItemClassModifier)
				{
					xuiC_ItemPartStack.SlotType = (modification.ItemClass as ItemClassModifier).Type.ToStringCached<ItemClassModifier.ModifierTypes>().ToLower();
				}
				xuiC_ItemPartStack.SlotChangedEvent -= this.HandleSlotChangedEvent;
				xuiC_ItemPartStack.ItemValue = ((modification != null) ? modification : ItemValue.None);
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

	// Token: 0x06007B58 RID: 31576 RVA: 0x002ECEE8 File Offset: 0x002EB0E8
	public void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		XUiC_ItemPartStack xuiC_ItemPartStack = (XUiC_ItemPartStack)this.itemControllers[slotNumber];
		ItemValue itemValue = (xuiC_ItemPartStack.ItemStack.IsEmpty() ? ItemValue.None : xuiC_ItemPartStack.ItemStack.itemValue);
		if (itemValue.ItemClass != null)
		{
			ItemClassModifier itemClassModifier = itemValue.ItemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.CurrentItem.itemValue.CosmeticModCount > 0)
				{
					this.CurrentItem.itemValue.SetCosmeticMod(0, itemValue);
					goto IL_00A4;
				}
				this.CurrentItem.itemValue.SetModification(slotNumber, itemValue);
				goto IL_00A4;
			}
		}
		this.CurrentItem.itemValue.SetModification(slotNumber, itemValue);
		IL_00A4:
		this.AssembleWindow.ItemStack = this.CurrentItem;
		this.AssembleWindow.OnChanged();
		this.xui.AssembleItem.RefreshAssembleItem();
	}

	// Token: 0x06007B59 RID: 31577 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B5A RID: 31578 RVA: 0x002E47DD File Offset: 0x002E29DD
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007B5B RID: 31579 RVA: 0x002ECFC5 File Offset: 0x002EB1C5
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x04005B8D RID: 23437
	[PublicizedFrom(EAccessModifier.Protected)]
	public int curPageIdx;

	// Token: 0x04005B8E RID: 23438
	[PublicizedFrom(EAccessModifier.Protected)]
	public int numPages;

	// Token: 0x04005B8F RID: 23439
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController[] itemControllers;

	// Token: 0x04005B90 RID: 23440
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005B91 RID: 23441
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;
}
