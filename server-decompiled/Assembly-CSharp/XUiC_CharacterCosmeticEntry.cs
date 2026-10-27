using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF8 RID: 3832
[Preserve]
public class XUiC_CharacterCosmeticEntry : XUiController
{
	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x0600778A RID: 30602 RVA: 0x002D2190 File Offset: 0x002D0390
	// (set) Token: 0x0600778B RID: 30603 RVA: 0x002D2198 File Offset: 0x002D0398
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

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x0600778C RID: 30604 RVA: 0x002D21A8 File Offset: 0x002D03A8
	// (set) Token: 0x0600778D RID: 30605 RVA: 0x002D21B0 File Offset: 0x002D03B0
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

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x0600778E RID: 30606 RVA: 0x002D21C0 File Offset: 0x002D03C0
	// (set) Token: 0x0600778F RID: 30607 RVA: 0x002D21C8 File Offset: 0x002D03C8
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

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x06007790 RID: 30608 RVA: 0x002D21D8 File Offset: 0x002D03D8
	// (set) Token: 0x06007791 RID: 30609 RVA: 0x002D21E0 File Offset: 0x002D03E0
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

	// Token: 0x06007792 RID: 30610 RVA: 0x002D21EC File Offset: 0x002D03EC
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

	// Token: 0x06007793 RID: 30611 RVA: 0x002D225E File Offset: 0x002D045E
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked += this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x06007794 RID: 30612 RVA: 0x002D2287 File Offset: 0x002D0487
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked -= this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x06007795 RID: 30613 RVA: 0x002D22B0 File Offset: 0x002D04B0
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

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06007796 RID: 30614 RVA: 0x002D2306 File Offset: 0x002D0506
	// (set) Token: 0x06007797 RID: 30615 RVA: 0x002D230E File Offset: 0x002D050E
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

	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x06007798 RID: 30616 RVA: 0x002D232C File Offset: 0x002D052C
	// (set) Token: 0x06007799 RID: 30617 RVA: 0x002D2334 File Offset: 0x002D0534
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

	// Token: 0x0600779A RID: 30618 RVA: 0x002D235C File Offset: 0x002D055C
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

	// Token: 0x0600779B RID: 30619 RVA: 0x002D23B8 File Offset: 0x002D05B8
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

	// Token: 0x0600779C RID: 30620 RVA: 0x002D23F0 File Offset: 0x002D05F0
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

	// Token: 0x0600779D RID: 30621 RVA: 0x002D2457 File Offset: 0x002D0657
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

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x0600779E RID: 30622 RVA: 0x002D2494 File Offset: 0x002D0694
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

	// Token: 0x0600779F RID: 30623 RVA: 0x002D24F8 File Offset: 0x002D06F8
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

	// Token: 0x060077A0 RID: 30624 RVA: 0x002D26B4 File Offset: 0x002D08B4
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

	// Token: 0x0400587D RID: 22653
	public XUiC_CharacterCosmeticList Owner;

	// Token: 0x0400587E RID: 22654
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x0400587F RID: 22655
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry.EntryTypes entryType;

	// Token: 0x04005880 RID: 22656
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x04005881 RID: 22657
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005882 RID: 22658
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005883 RID: 22659
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005884 RID: 22660
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005885 RID: 22661
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005886 RID: 22662
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005887 RID: 22663
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005888 RID: 22664
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005889 RID: 22665
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 lockedBackgroundColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x0400588A RID: 22666
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400588B RID: 22667
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x0400588C RID: 22668
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x0400588D RID: 22669
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400588E RID: 22670
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000EF9 RID: 3833
	public enum EntryTypes
	{
		// Token: 0x04005890 RID: 22672
		Item,
		// Token: 0x04005891 RID: 22673
		Empty,
		// Token: 0x04005892 RID: 22674
		Hide
	}
}
