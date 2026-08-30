using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDE RID: 3806
[Preserve]
public class XUiC_CharacterCosmeticEntry : XUiController
{
	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x06007769 RID: 30569 RVA: 0x002CD860 File Offset: 0x002CBA60
	// (set) Token: 0x0600776A RID: 30570 RVA: 0x002CD868 File Offset: 0x002CBA68
	public ItemClass ItemClass
	{
		get
		{
			return this.itemClass;
		}
		set
		{
			this.itemClass = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x0600776B RID: 30571 RVA: 0x002CD878 File Offset: 0x002CBA78
	// (set) Token: 0x0600776C RID: 30572 RVA: 0x002CD880 File Offset: 0x002CBA80
	public XUiC_CharacterCosmeticEntry.EntryTypes EntryType
	{
		get
		{
			return this.entryType;
		}
		set
		{
			this.entryType = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x0600776D RID: 30573 RVA: 0x002CD890 File Offset: 0x002CBA90
	// (set) Token: 0x0600776E RID: 30574 RVA: 0x002CD898 File Offset: 0x002CBA98
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x0600776F RID: 30575 RVA: 0x002CD8A8 File Offset: 0x002CBAA8
	// (set) Token: 0x06007770 RID: 30576 RVA: 0x002CD8B0 File Offset: 0x002CBAB0
	public new bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			this.selected = value;
		}
	}

	// Token: 0x06007771 RID: 30577 RVA: 0x002CD8BC File Offset: 0x002CBABC
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
		XUiController childById = base.GetChildById("itemIcon");
		if (childById != null)
		{
			this.itemIconSprite = childById.ViewComponent as XUiV_Sprite;
		}
		XUiController childById2 = base.GetChildById("backgroundTexture");
		if (childById2 != null)
		{
			this.backgroundTexture = childById2.ViewComponent as XUiV_Texture;
			if (this.backgroundTexture != null)
			{
				this.backgroundTexture.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
			}
		}
	}

	// Token: 0x06007772 RID: 30578 RVA: 0x002CD92E File Offset: 0x002CBB2E
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked += this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x06007773 RID: 30579 RVA: 0x002CD957 File Offset: 0x002CBB57
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked -= this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x06007774 RID: 30580 RVA: 0x002CD980 File Offset: 0x002CBB80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Equipment_CosmeticUnlocked(string itemName)
	{
		if (this.entryType == XUiC_CharacterCosmeticEntry.EntryTypes.Item)
		{
			if (this.ItemClass == null)
			{
				return;
			}
			if (this.ItemClass.GetItemName() == itemName)
			{
				if (this.Owner.SelectedEntry == this)
				{
					this.Owner.Owner.RefreshBindings();
				}
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06007775 RID: 30581 RVA: 0x002CD9D6 File Offset: 0x002CBBD6
	// (set) Token: 0x06007776 RID: 30582 RVA: 0x002CD9DE File Offset: 0x002CBBDE
	public Color32 SelectionBorderColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.selectionBorderColor;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (!this.selectionBorderColor.ColorEquals(value))
			{
				this.selectionBorderColor = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06007777 RID: 30583 RVA: 0x002CD9FC File Offset: 0x002CBBFC
	// (set) Token: 0x06007778 RID: 30584 RVA: 0x002CDA04 File Offset: 0x002CBC04
	public bool IsDragAndDrop
	{
		get
		{
			return this.isDragAndDrop;
		}
		set
		{
			this.isDragAndDrop = value;
			if (!value)
			{
				return;
			}
			base.ViewComponent.EventOnPress = false;
			base.ViewComponent.EventOnHover = false;
		}
	}

	// Token: 0x06007779 RID: 30585 RVA: 0x002CDA2C File Offset: 0x002CBC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBorderColor()
	{
		if (this.IsDragAndDrop)
		{
			this.SelectionBorderColor = Color.clear;
			return;
		}
		if (this.Selected)
		{
			this.SelectionBorderColor = this.selectColor;
			return;
		}
		if (this.isHovered)
		{
			this.SelectionBorderColor = this.highlightColor;
			return;
		}
		this.SelectionBorderColor = this.backgroundColor;
	}

	// Token: 0x0600777A RID: 30586 RVA: 0x002CDA88 File Offset: 0x002CBC88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.EntryType == XUiC_CharacterCosmeticEntry.EntryTypes.Item && this.itemClass == null)
		{
			this.isHovered = false;
			return;
		}
		if (this.isHovered != _isOver)
		{
			this.isHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x0600777B RID: 30587 RVA: 0x002CDAC0 File Offset: 0x002CBCC0
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			base.ViewComponent.IsNavigatable = (base.ViewComponent.IsSnappable = this.entryType != XUiC_CharacterCosmeticEntry.EntryTypes.Item || (this.entryType == XUiC_CharacterCosmeticEntry.EntryTypes.Item && this.itemClass != null));
			base.RefreshBindings();
			this.IsDirty = false;
		}
		this.updateBorderColor();
		base.Update(_dt);
	}

	// Token: 0x0600777C RID: 30588 RVA: 0x002CDB27 File Offset: 0x002CBD27
	[return: TupleElementNames(new string[] { "isUnlocked", "set" })]
	public ValueTuple<bool, EntitlementSetEnum> IsUnlocked()
	{
		if (this.entryType != XUiC_CharacterCosmeticEntry.EntryTypes.Item)
		{
			return new ValueTuple<bool, EntitlementSetEnum>(true, EntitlementSetEnum.None);
		}
		if (this.itemClass == null)
		{
			return new ValueTuple<bool, EntitlementSetEnum>(false, EntitlementSetEnum.None);
		}
		return this.xui.PlayerEquipment.Equipment.HasCosmeticUnlocked(this.itemClass);
	}

	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x0600777D RID: 30589 RVA: 0x002CDB64 File Offset: 0x002CBD64
	public string Name
	{
		get
		{
			switch (this.EntryType)
			{
			case XUiC_CharacterCosmeticEntry.EntryTypes.Item:
				if (this.itemClass != null)
				{
					return this.itemClass.GetLocalizedItemName();
				}
				break;
			case XUiC_CharacterCosmeticEntry.EntryTypes.Empty:
				return Localization.Get("xuiEmpty", false, null);
			case XUiC_CharacterCosmeticEntry.EntryTypes.Hide:
				return Localization.Get("xuiHide", false, null);
			}
			return Localization.Get("xuiCosmetics", false, null);
		}
	}

	// Token: 0x0600777E RID: 30590 RVA: 0x002CDBC8 File Offset: 0x002CBDC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		ValueTuple<bool, EntitlementSetEnum> valueTuple = this.IsUnlocked();
		bool item = valueTuple.Item1;
		EntitlementSetEnum item2 = valueTuple.Item2;
		if (bindingName == "itemicon")
		{
			value = "";
			switch (this.EntryType)
			{
			case XUiC_CharacterCosmeticEntry.EntryTypes.Item:
				if (this.itemClass != null)
				{
					value = this.itemClass.GetIconName();
				}
				break;
			case XUiC_CharacterCosmeticEntry.EntryTypes.Empty:
				value = "server_refresh";
				break;
			case XUiC_CharacterCosmeticEntry.EntryTypes.Hide:
				value = "ui_game_symbol_x";
				break;
			}
			return true;
		}
		if (bindingName == "iconatlas")
		{
			value = "ItemIconAtlas";
			if (this.EntryType != XUiC_CharacterCosmeticEntry.EntryTypes.Item)
			{
				value = "UIAtlas";
			}
			else
			{
				if (this.itemClass == null)
				{
					return true;
				}
				if (!item)
				{
					value = "ItemIconAtlasGreyscale";
				}
			}
			return true;
		}
		if (bindingName == "islocked")
		{
			value = "false";
			if (!item && item2 != EntitlementSetEnum.None)
			{
				value = "true";
			}
			return true;
		}
		if (bindingName == "selectionbordercolor")
		{
			value = this.selectionbordercolorFormatter.Format(this.SelectionBorderColor);
			return true;
		}
		if (bindingName == "backgroundcolor")
		{
			if (!item && this.itemClass != null)
			{
				value = this.backgroundcolorFormatter.Format(this.lockedBackgroundColor);
			}
			else
			{
				value = this.backgroundcolorFormatter.Format(this.backgroundColor);
			}
			return true;
		}
		if (!(bindingName == "tooltip"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = "";
		switch (this.EntryType)
		{
		case XUiC_CharacterCosmeticEntry.EntryTypes.Item:
			if (this.itemClass != null)
			{
				value = this.itemClass.GetLocalizedItemName();
			}
			break;
		case XUiC_CharacterCosmeticEntry.EntryTypes.Empty:
			value = Localization.Get("xuiEmpty", false, null);
			break;
		case XUiC_CharacterCosmeticEntry.EntryTypes.Hide:
			value = Localization.Get("xuiHide", false, null);
			break;
		}
		return true;
	}

	// Token: 0x0600777F RID: 30591 RVA: 0x002CDD84 File Offset: 0x002CBF84
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "select_color")
		{
			this.selectColor = StringParsers.ParseColor32(_value);
			return true;
		}
		if (_name == "press_color")
		{
			this.pressColor = StringParsers.ParseColor32(_value);
			return true;
		}
		if (_name == "background_color")
		{
			this.backgroundColor = StringParsers.ParseColor32(_value);
			return true;
		}
		if (_name == "locked_background_color")
		{
			this.backgroundColor = StringParsers.ParseColor32(_value);
			return true;
		}
		if (_name == "highlight_color")
		{
			this.highlightColor = StringParsers.ParseColor32(_value);
			return true;
		}
		if (!(_name == "holding_color"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.holdingColor = StringParsers.ParseColor32(_value);
		return true;
	}

	// Token: 0x04005820 RID: 22560
	public XUiC_CharacterCosmeticList Owner;

	// Token: 0x04005821 RID: 22561
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x04005822 RID: 22562
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry.EntryTypes entryType;

	// Token: 0x04005823 RID: 22563
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x04005824 RID: 22564
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005825 RID: 22565
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005826 RID: 22566
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005827 RID: 22567
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005828 RID: 22568
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005829 RID: 22569
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400582A RID: 22570
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400582B RID: 22571
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x0400582C RID: 22572
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 lockedBackgroundColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x0400582D RID: 22573
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400582E RID: 22574
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x0400582F RID: 22575
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x04005830 RID: 22576
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005831 RID: 22577
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000EDF RID: 3807
	public enum EntryTypes
	{
		// Token: 0x04005833 RID: 22579
		Item,
		// Token: 0x04005834 RID: 22580
		Empty,
		// Token: 0x04005835 RID: 22581
		Hide
	}
}
