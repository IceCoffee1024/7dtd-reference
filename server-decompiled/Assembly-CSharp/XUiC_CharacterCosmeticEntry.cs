using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EED RID: 3821
[Preserve]
public class XUiC_CharacterCosmeticEntry : XUiController
{
	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x06007764 RID: 30564 RVA: 0x002D0C38 File Offset: 0x002CEE38
	// (set) Token: 0x06007765 RID: 30565 RVA: 0x002D0C40 File Offset: 0x002CEE40
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

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x06007766 RID: 30566 RVA: 0x002D0C50 File Offset: 0x002CEE50
	// (set) Token: 0x06007767 RID: 30567 RVA: 0x002D0C58 File Offset: 0x002CEE58
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

	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x06007768 RID: 30568 RVA: 0x002D0C68 File Offset: 0x002CEE68
	// (set) Token: 0x06007769 RID: 30569 RVA: 0x002D0C70 File Offset: 0x002CEE70
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

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x0600776A RID: 30570 RVA: 0x002D0C80 File Offset: 0x002CEE80
	// (set) Token: 0x0600776B RID: 30571 RVA: 0x002D0C88 File Offset: 0x002CEE88
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

	// Token: 0x0600776C RID: 30572 RVA: 0x002D0C94 File Offset: 0x002CEE94
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

	// Token: 0x0600776D RID: 30573 RVA: 0x002D0D06 File Offset: 0x002CEF06
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked += this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x0600776E RID: 30574 RVA: 0x002D0D2F File Offset: 0x002CEF2F
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked -= this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x0600776F RID: 30575 RVA: 0x002D0D58 File Offset: 0x002CEF58
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

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x06007770 RID: 30576 RVA: 0x002D0DAE File Offset: 0x002CEFAE
	// (set) Token: 0x06007771 RID: 30577 RVA: 0x002D0DB6 File Offset: 0x002CEFB6
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

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x06007772 RID: 30578 RVA: 0x002D0DD4 File Offset: 0x002CEFD4
	// (set) Token: 0x06007773 RID: 30579 RVA: 0x002D0DDC File Offset: 0x002CEFDC
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

	// Token: 0x06007774 RID: 30580 RVA: 0x002D0E04 File Offset: 0x002CF004
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

	// Token: 0x06007775 RID: 30581 RVA: 0x002D0E60 File Offset: 0x002CF060
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

	// Token: 0x06007776 RID: 30582 RVA: 0x002D0E98 File Offset: 0x002CF098
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

	// Token: 0x06007777 RID: 30583 RVA: 0x002D0EFF File Offset: 0x002CF0FF
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

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x06007778 RID: 30584 RVA: 0x002D0F3C File Offset: 0x002CF13C
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

	// Token: 0x06007779 RID: 30585 RVA: 0x002D0FA0 File Offset: 0x002CF1A0
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

	// Token: 0x0600777A RID: 30586 RVA: 0x002D115C File Offset: 0x002CF35C
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

	// Token: 0x04005858 RID: 22616
	public XUiC_CharacterCosmeticList Owner;

	// Token: 0x04005859 RID: 22617
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x0400585A RID: 22618
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry.EntryTypes entryType;

	// Token: 0x0400585B RID: 22619
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x0400585C RID: 22620
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x0400585D RID: 22621
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x0400585E RID: 22622
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x0400585F RID: 22623
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005860 RID: 22624
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005861 RID: 22625
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005862 RID: 22626
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005863 RID: 22627
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005864 RID: 22628
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 lockedBackgroundColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x04005865 RID: 22629
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005866 RID: 22630
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x04005867 RID: 22631
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x04005868 RID: 22632
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005869 RID: 22633
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000EEE RID: 3822
	public enum EntryTypes
	{
		// Token: 0x0400586B RID: 22635
		Item,
		// Token: 0x0400586C RID: 22636
		Empty,
		// Token: 0x0400586D RID: 22637
		Hide
	}
}
