using System;
using System.Collections.Generic;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000ECB RID: 3787
[Preserve]
public class XUiC_CharacterCosmeticList : XUiController
{
	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x060076E0 RID: 30432 RVA: 0x002CAA0C File Offset: 0x002C8C0C
	// (set) Token: 0x060076E1 RID: 30433 RVA: 0x002CAA14 File Offset: 0x002C8C14
	public XUiC_CharacterCosmeticEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = true;
				this.SetTempCosmeticSlot(this.selectedEntry);
			}
			this.Owner.RefreshBindings();
		}
	}

	// Token: 0x060076E2 RID: 30434 RVA: 0x002CAA67 File Offset: 0x002C8C67
	public override void Init()
	{
		base.Init();
		this.entryList = base.GetChildrenByType<XUiC_CharacterCosmeticEntry>(null);
	}

	// Token: 0x060076E3 RID: 30435 RVA: 0x002CAA7C File Offset: 0x002C8C7C
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.IsDirty = true;
	}

	// Token: 0x060076E4 RID: 30436 RVA: 0x002CAAA1 File Offset: 0x002C8CA1
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerEquipment.Equipment.ClearTempCosmeticSlot();
	}

	// Token: 0x060076E5 RID: 30437 RVA: 0x002CAABE File Offset: 0x002C8CBE
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.xui.playerUI.entityPlayer.AimingGun = false;
		bool isVisible = base.ViewComponent.IsVisible;
	}

	// Token: 0x060076E6 RID: 30438 RVA: 0x002CAAEC File Offset: 0x002C8CEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCallouts(bool _holdingItem)
	{
		string text = string.Format(Localization.Get("igcoItemActions", false, null), InControlExtensions.GetBlankDPadSourceString());
		XUiC_GamepadCalloutWindow calloutWindow = this.xui.CalloutWindow;
		if (calloutWindow != null)
		{
			calloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
			if (!_holdingItem)
			{
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoInspect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, text, XUiC_GamepadCalloutWindow.CalloutType.Menu);
				calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
			}
			calloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		}
	}

	// Token: 0x060076E7 RID: 30439 RVA: 0x002CAB5C File Offset: 0x002C8D5C
	public void SetCosmeticList(List<ItemClass> _currentItems, EquipmentSlots slot)
	{
		this.currentItems = _currentItems;
		if (this.currentItems == null)
		{
			return;
		}
		ItemClass cosmeticSlot = this.xui.PlayerEquipment.Equipment.GetCosmeticSlot((int)slot, false);
		this.SelectedEntry = null;
		int i = 0;
		for (int j = 0; j < this.entryList.Length; j++)
		{
			XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry = this.entryList[j];
			xuiC_CharacterCosmeticEntry.OnPress -= this.OnPressCosmetic;
			xuiC_CharacterCosmeticEntry.Selected = false;
			xuiC_CharacterCosmeticEntry.Owner = this;
		}
		XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry2 = this.entryList[0];
		xuiC_CharacterCosmeticEntry2.EntryType = XUiC_CharacterCosmeticEntry.EntryTypes.Empty;
		xuiC_CharacterCosmeticEntry2.ItemClass = null;
		xuiC_CharacterCosmeticEntry2.Index = (int)slot;
		xuiC_CharacterCosmeticEntry2.OnPress += this.OnPressCosmetic;
		if (xuiC_CharacterCosmeticEntry2.ItemClass == cosmeticSlot)
		{
			this.SelectedEntry = xuiC_CharacterCosmeticEntry2;
		}
		i++;
		XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry3 = this.entryList[1];
		xuiC_CharacterCosmeticEntry3.EntryType = XUiC_CharacterCosmeticEntry.EntryTypes.Hide;
		xuiC_CharacterCosmeticEntry3.ItemClass = ItemClass.MissingItem;
		xuiC_CharacterCosmeticEntry3.Index = (int)slot;
		xuiC_CharacterCosmeticEntry3.OnPress += this.OnPressCosmetic;
		if (xuiC_CharacterCosmeticEntry3.ItemClass == cosmeticSlot)
		{
			this.SelectedEntry = xuiC_CharacterCosmeticEntry3;
		}
		i++;
		int num = 0;
		while (i < this.entryList.Length)
		{
			if (num >= this.currentItems.Count)
			{
				break;
			}
			XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry4 = this.entryList[i];
			xuiC_CharacterCosmeticEntry4.EntryType = XUiC_CharacterCosmeticEntry.EntryTypes.Item;
			xuiC_CharacterCosmeticEntry4.ItemClass = this.currentItems[num];
			xuiC_CharacterCosmeticEntry4.Index = (int)slot;
			xuiC_CharacterCosmeticEntry4.OnPress += this.OnPressCosmetic;
			if (xuiC_CharacterCosmeticEntry4.ItemClass == cosmeticSlot)
			{
				this.SelectedEntry = xuiC_CharacterCosmeticEntry4;
			}
			i++;
			num++;
		}
		while (i < this.entryList.Length)
		{
			XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry5 = this.entryList[i];
			xuiC_CharacterCosmeticEntry5.EntryType = XUiC_CharacterCosmeticEntry.EntryTypes.Item;
			xuiC_CharacterCosmeticEntry5.ItemClass = null;
			xuiC_CharacterCosmeticEntry5.Index = (int)slot;
			i++;
		}
		this.entryList[0].SelectCursorElement(true, false);
	}

	// Token: 0x060076E8 RID: 30440 RVA: 0x002CAD20 File Offset: 0x002C8F20
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressCosmetic(XUiController _sender, int _mouseButton)
	{
		XUiC_CharacterCosmeticEntry xuiC_CharacterCosmeticEntry = _sender as XUiC_CharacterCosmeticEntry;
		if (xuiC_CharacterCosmeticEntry != null)
		{
			if (this.SelectedEntry == xuiC_CharacterCosmeticEntry)
			{
				return;
			}
			this.SelectedEntry = xuiC_CharacterCosmeticEntry;
		}
	}

	// Token: 0x060076E9 RID: 30441 RVA: 0x002CAD48 File Offset: 0x002C8F48
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetTempCosmeticSlot(XUiC_CharacterCosmeticEntry _entry)
	{
		if (_entry == null)
		{
			return;
		}
		switch (_entry.EntryType)
		{
		case XUiC_CharacterCosmeticEntry.EntryTypes.Item:
			this.xui.PlayerEquipment.Equipment.SetTempCosmeticSlot(_entry.Index, _entry.ItemClass);
			break;
		case XUiC_CharacterCosmeticEntry.EntryTypes.Empty:
			this.xui.PlayerEquipment.Equipment.SetTempCosmeticSlot(_entry.Index, null);
			break;
		case XUiC_CharacterCosmeticEntry.EntryTypes.Hide:
			this.xui.PlayerEquipment.Equipment.SetTempCosmeticSlot(_entry.Index, ItemClass.MissingItem);
			break;
		}
		((XUiC_CharacterCosmeticWindowGroup)base.WindowGroup.Controller).ResetPreview();
	}

	// Token: 0x060076EA RID: 30442 RVA: 0x002CADEA File Offset: 0x002C8FEA
	public void SetEmptyCosmetic()
	{
		this.SelectedEntry = this.entryList[0];
	}

	// Token: 0x04005791 RID: 22417
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005792 RID: 22418
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry[] entryList;

	// Token: 0x04005793 RID: 22419
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry selectedEntry;

	// Token: 0x04005794 RID: 22420
	public XUiC_CharacterCosmeticsListWindow Owner;

	// Token: 0x04005795 RID: 22421
	public List<ItemClass> currentItems;
}
