using System;
using System.Collections.Generic;
using System.Linq;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine.Scripting;

// Token: 0x02000EF0 RID: 3824
[Preserve]
public class XUiC_CharacterCosmeticsListWindow : XUiController
{
	// Token: 0x06007788 RID: 30600 RVA: 0x002D16F8 File Offset: 0x002CF8F8
	public override void Init()
	{
		base.Init();
		this.categoryList = base.GetChildByType<XUiC_CategoryList>();
		this.cosmeticGrid = base.GetChildByType<XUiC_CharacterCosmeticList>();
		this.cosmeticGrid.Owner = this;
		this.btnApply = base.GetChildById("btnApply").GetChildByType<XUiC_SimpleButton>();
		this.btnApply.OnPressed += this.BtnApply_OnPressed;
		base.GetChildById("btnApplySet").GetChildByType<XUiC_SimpleButton>().OnPressed += this.BtnApplySet_OnPressed;
		this.txtInput = (XUiC_TextInput)base.GetChildById("searchInput");
		if (this.txtInput != null)
		{
			this.txtInput.OnChangeHandler += this.HandleOnChangedHandler;
			this.txtInput.Text = "";
		}
		this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
	}

	// Token: 0x06007789 RID: 30601 RVA: 0x002D17DC File Offset: 0x002CF9DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplySelected()
	{
		ValueTuple<bool, EntitlementSetEnum> valueTuple = this.cosmeticGrid.SelectedEntry.IsUnlocked();
		bool item = valueTuple.Item1;
		EntitlementSetEnum item2 = valueTuple.Item2;
		if (!item && item2 != EntitlementSetEnum.None)
		{
			EntitlementManager.Instance.OpenStore(item2, delegate(EntitlementSetEnum _)
			{
				this.IsDirty = true;
			});
		}
		if (item)
		{
			this.xui.PlayerEquipment.Equipment.ApplyTempCosmeticSlot();
			EModelSDCS emodelSDCS = this.xui.playerUI.entityPlayer.emodel as EModelSDCS;
			if (emodelSDCS != null)
			{
				emodelSDCS.GenerateMeshes();
			}
			return;
		}
	}

	// Token: 0x0600778A RID: 30602 RVA: 0x002D1861 File Offset: 0x002CFA61
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnApply_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.ApplySelected();
	}

	// Token: 0x0600778B RID: 30603 RVA: 0x002D186C File Offset: 0x002CFA6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnApplySet_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.ApplySelected();
		if (!this.cosmeticGrid.SelectedEntry.IsUnlocked().Item1)
		{
			return;
		}
		ItemClassArmor itemClassArmor = this.xui.PlayerEquipment.Equipment.tempCosmeticSlot as ItemClassArmor;
		Equipment equipment = this.xui.PlayerEquipment.Equipment;
		if (itemClassArmor == null)
		{
			if (this.xui.PlayerEquipment.Equipment.tempCosmeticSlot == ItemClass.MissingItem)
			{
				this.xui.PlayerEquipment.Equipment.SetCosmeticSlot(0, -1);
				this.xui.PlayerEquipment.Equipment.SetCosmeticSlot(1, -1);
				this.xui.PlayerEquipment.Equipment.SetCosmeticSlot(2, -1);
				this.xui.PlayerEquipment.Equipment.SetCosmeticSlot(3, -1);
			}
			else
			{
				equipment.ClearCosmeticSlots();
			}
		}
		else
		{
			string armorGroup = itemClassArmor.ArmorGroup[0];
			List<ItemClass> list = ItemClass.list.Where<ItemClass>(delegate(ItemClass item)
			{
				ItemClassArmor itemClassArmor2 = item as ItemClassArmor;
				return itemClassArmor2 != null && itemClassArmor2.ArmorGroup[0] == armorGroup && itemClassArmor2.IsCosmetic;
			}).ToList<ItemClass>();
			for (int i = 0; i < list.Count; i++)
			{
				equipment.SetCosmeticSlot(list[i] as ItemClassArmor);
			}
		}
		((XUiC_CharacterCosmeticWindowGroup)base.WindowGroup.Controller).ResetPreview();
		EModelSDCS emodelSDCS = this.xui.playerUI.entityPlayer.emodel as EModelSDCS;
		if (emodelSDCS != null)
		{
			emodelSDCS.GenerateMeshes();
		}
	}

	// Token: 0x0600778C RID: 30604 RVA: 0x002D19E0 File Offset: 0x002CFBE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnChangedHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.filterText = _text;
		this.IsDirty = true;
	}

	// Token: 0x0600778D RID: 30605 RVA: 0x002D19F0 File Offset: 0x002CFBF0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.categoryList != null)
		{
			this.categoryList.SetCategoryEntry(0, "Head", "ui_game_symbol_head_shot", Localization.Get("lblHeadgear", false, null));
			this.categoryList.SetCategoryEntry(1, "Chest", "ui_game_symbol_armor_iron", Localization.Get("lblChest", false, null));
			this.categoryList.SetCategoryEntry(2, "Hands", "ui_game_symbol_hand", Localization.Get("lblHands", false, null));
			this.categoryList.SetCategoryEntry(3, "Feet", "ui_game_symbol_splint", Localization.Get("lblFeet", false, null));
			for (int i = 4; i < this.categoryList.CategoryButtons.Count; i++)
			{
				this.categoryList.SetCategoryEmpty(i);
			}
		}
		if (!string.IsNullOrEmpty(XUiC_CharacterCosmeticsListWindow.defaultSelectedElement))
		{
			base.GetChildById(XUiC_CharacterCosmeticsListWindow.defaultSelectedElement).SelectCursorElement(true, false);
			XUiC_CharacterCosmeticsListWindow.defaultSelectedElement = "";
		}
		this._cachedCosmeticSlots = this.xui.PlayerEquipment.Equipment.CosmeticSlots.Clone() as ItemClass[];
		this.IsDirty = true;
	}

	// Token: 0x0600778E RID: 30606 RVA: 0x002D1B14 File Offset: 0x002CFD14
	public override void OnClose()
	{
		base.OnClose();
		if (Helper.IsEditingOrPlaytesting())
		{
			return;
		}
		ItemClass[] cosmeticSlots = this.xui.PlayerEquipment.Equipment.CosmeticSlots;
		for (int i = 0; i < cosmeticSlots.Length; i++)
		{
			ItemClass itemClass = cosmeticSlots[i];
			ItemClass itemClass2 = this._cachedCosmeticSlots[i];
			if (itemClass != itemClass2)
			{
				ItemClassArmor itemClassArmor = itemClass as ItemClassArmor;
				string text3;
				if (itemClassArmor == null)
				{
					string text = ((itemClass != null) ? "Hide_" : "Empty_");
					ItemStack stackFromSlot = this.xui.PlayerEquipment.GetStackFromSlot((EquipmentSlots)i);
					string text2;
					if (stackFromSlot == null)
					{
						text2 = null;
					}
					else
					{
						ItemValue itemValue = stackFromSlot.itemValue;
						if (itemValue == null)
						{
							text2 = null;
						}
						else
						{
							ItemClass itemClass3 = itemValue.ItemClass;
							text2 = ((itemClass3 != null) ? itemClass3.Name : null);
						}
					}
					text3 = text + (text2 ?? "NoArmor");
				}
				else
				{
					text3 = itemClassArmor.Name;
				}
				string text4 = text3;
				CosmeticUsageEventData cosmeticUsageEventData = new CosmeticUsageEventData();
				cosmeticUsageEventData.ServerId = Helper.GetServerId();
				cosmeticUsageEventData.SaveId = Helper.GetSaveID();
				cosmeticUsageEventData.CosmeticChangeTimestamp = DateTime.UtcNow.ToString("O");
				EquipmentSlots equipmentSlots = (EquipmentSlots)i;
				cosmeticUsageEventData.Slot = equipmentSlots.ToString();
				cosmeticUsageEventData.CosmeticName = text4;
				ItemClassArmor itemClassArmor2 = itemClass as ItemClassArmor;
				cosmeticUsageEventData.SetName = ((itemClassArmor2 != null) ? itemClassArmor2.ArmorGroup[0] : null);
				CosmeticUsageEventData cosmeticUsageEventData2 = cosmeticUsageEventData;
				ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(cosmeticUsageEventData2);
			}
		}
	}

	// Token: 0x0600778F RID: 30607 RVA: 0x002D1C55 File Offset: 0x002CFE55
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetCategory(EquipmentSlots equipSlot)
	{
		this.selectedSlot = equipSlot;
		this.categoryList.SetCategory(equipSlot.ToString());
	}

	// Token: 0x06007790 RID: 30608 RVA: 0x002D1C76 File Offset: 0x002CFE76
	public void SetEmptyCosmeticForCurrentCategory()
	{
		this.cosmeticGrid.SetEmptyCosmetic();
	}

	// Token: 0x06007791 RID: 30609 RVA: 0x002D1C84 File Offset: 0x002CFE84
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.selectedSlot = (EquipmentSlots)Enum.Parse(typeof(EquipmentSlots), _categoryEntry.CategoryName, true);
		if (this.xui.playerUI.entityPlayer.equipment.GetSlotItem((int)this.selectedSlot).IsEmpty())
		{
			this.btnApply.Tooltip = Localization.Get("ttCosmeticsDisabledSlot", false, null);
		}
		else
		{
			this.btnApply.Tooltip = null;
		}
		this.IsDirty = true;
		this.xui.PlayerEquipment.Equipment.ClearTempCosmeticSlot();
		((XUiC_CharacterCosmeticWindowGroup)base.WindowGroup.Controller).ResetPreview();
	}

	// Token: 0x06007792 RID: 30610 RVA: 0x002D1D30 File Offset: 0x002CFF30
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			List<ItemClass> list = ItemClass.list.Where<ItemClass>(delegate(ItemClass item)
			{
				ItemClassArmor itemClassArmor = item as ItemClassArmor;
				return itemClassArmor != null && itemClassArmor.EquipSlot == this.selectedSlot && itemClassArmor.IsCosmetic && (this.filterText == "" || item.GetLocalizedItemName().ContainsCaseInsensitive(this.filterText)) && (item.SDCSData == null || (EntitlementManager.Instance.IsAvailableOnPlatform(item.SDCSData.PrefabName) && (EntitlementManager.Instance.IsEntitlementPurchasable(item.SDCSData.PrefabName) || EntitlementManager.Instance.HasEntitlement(item.SDCSData.PrefabName))));
			}).ToList<ItemClass>();
			this.cosmeticGrid.SetCosmeticList(list, this.selectedSlot);
			this.IsDirty = false;
		}
	}

	// Token: 0x06007793 RID: 30611 RVA: 0x002D1D84 File Offset: 0x002CFF84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = false;
		EntitlementSetEnum entitlementSetEnum = EntitlementSetEnum.None;
		XUiC_CharacterCosmeticList xuiC_CharacterCosmeticList = this.cosmeticGrid;
		if (((xuiC_CharacterCosmeticList != null) ? xuiC_CharacterCosmeticList.SelectedEntry : null) != null)
		{
			ValueTuple<bool, EntitlementSetEnum> valueTuple = this.cosmeticGrid.SelectedEntry.IsUnlocked();
			flag = valueTuple.Item1;
			entitlementSetEnum = valueTuple.Item2;
		}
		if (bindingName == "cosmeticname")
		{
			value = "xuiCosmetics";
			if (this.cosmeticGrid != null && this.cosmeticGrid.SelectedEntry != null)
			{
				value = this.cosmeticGrid.SelectedEntry.Name;
			}
			return true;
		}
		if (bindingName == "applyTextKey")
		{
			value = "xuiCosmeticsApply";
			if (!flag && entitlementSetEnum != EntitlementSetEnum.None)
			{
				value = "xuiCosmeticsPurchase";
			}
			return true;
		}
		if (bindingName == "enableApply")
		{
			value = "false";
			if (flag || entitlementSetEnum != EntitlementSetEnum.None)
			{
				value = "true";
			}
			return true;
		}
		if (!(bindingName == "enableApplySet"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = "false";
		if (flag)
		{
			value = "true";
		}
		return true;
	}

	// Token: 0x04005873 RID: 22643
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticList cosmeticGrid;

	// Token: 0x04005874 RID: 22644
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005875 RID: 22645
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnApply;

	// Token: 0x04005876 RID: 22646
	public EquipmentSlots selectedSlot;

	// Token: 0x04005877 RID: 22647
	public static string defaultSelectedElement;

	// Token: 0x04005878 RID: 22648
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005879 RID: 22649
	[PublicizedFrom(EAccessModifier.Private)]
	public string filterText = "";

	// Token: 0x0400587A RID: 22650
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] _cachedCosmeticSlots;
}
