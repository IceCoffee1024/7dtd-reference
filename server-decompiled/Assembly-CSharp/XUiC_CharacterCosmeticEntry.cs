using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC9 RID: 3785
[Preserve]
public class XUiC_CharacterCosmeticEntry : XUiController
{
	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x060076C8 RID: 30408 RVA: 0x002CA33C File Offset: 0x002C853C
	// (set) Token: 0x060076C9 RID: 30409 RVA: 0x002CA344 File Offset: 0x002C8544
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

	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x060076CA RID: 30410 RVA: 0x002CA354 File Offset: 0x002C8554
	// (set) Token: 0x060076CB RID: 30411 RVA: 0x002CA35C File Offset: 0x002C855C
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

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x060076CC RID: 30412 RVA: 0x002CA36C File Offset: 0x002C856C
	// (set) Token: 0x060076CD RID: 30413 RVA: 0x002CA374 File Offset: 0x002C8574
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

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x060076CE RID: 30414 RVA: 0x002CA384 File Offset: 0x002C8584
	// (set) Token: 0x060076CF RID: 30415 RVA: 0x002CA38C File Offset: 0x002C858C
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

	// Token: 0x060076D0 RID: 30416 RVA: 0x002CA398 File Offset: 0x002C8598
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

	// Token: 0x060076D1 RID: 30417 RVA: 0x002CA40A File Offset: 0x002C860A
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked += this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x060076D2 RID: 30418 RVA: 0x002CA433 File Offset: 0x002C8633
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerEquipment.Equipment.CosmeticUnlocked -= this.Equipment_CosmeticUnlocked;
	}

	// Token: 0x060076D3 RID: 30419 RVA: 0x002CA45C File Offset: 0x002C865C
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

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x060076D4 RID: 30420 RVA: 0x002CA4B2 File Offset: 0x002C86B2
	// (set) Token: 0x060076D5 RID: 30421 RVA: 0x002CA4BA File Offset: 0x002C86BA
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

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x060076D6 RID: 30422 RVA: 0x002CA4D8 File Offset: 0x002C86D8
	// (set) Token: 0x060076D7 RID: 30423 RVA: 0x002CA4E0 File Offset: 0x002C86E0
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

	// Token: 0x060076D8 RID: 30424 RVA: 0x002CA508 File Offset: 0x002C8708
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

	// Token: 0x060076D9 RID: 30425 RVA: 0x002CA564 File Offset: 0x002C8764
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

	// Token: 0x060076DA RID: 30426 RVA: 0x002CA59C File Offset: 0x002C879C
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

	// Token: 0x060076DB RID: 30427 RVA: 0x002CA603 File Offset: 0x002C8803
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

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x060076DC RID: 30428 RVA: 0x002CA640 File Offset: 0x002C8840
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

	// Token: 0x060076DD RID: 30429 RVA: 0x002CA6A4 File Offset: 0x002C88A4
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

	// Token: 0x060076DE RID: 30430 RVA: 0x002CA860 File Offset: 0x002C8A60
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

	// Token: 0x0400577B RID: 22395
	public XUiC_CharacterCosmeticList Owner;

	// Token: 0x0400577C RID: 22396
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x0400577D RID: 22397
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CharacterCosmeticEntry.EntryTypes entryType;

	// Token: 0x0400577E RID: 22398
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x0400577F RID: 22399
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005780 RID: 22400
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005781 RID: 22401
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005782 RID: 22402
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005783 RID: 22403
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005784 RID: 22404
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005785 RID: 22405
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005786 RID: 22406
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005787 RID: 22407
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 lockedBackgroundColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x04005788 RID: 22408
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005789 RID: 22409
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x0400578A RID: 22410
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x0400578B RID: 22411
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400578C RID: 22412
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000ECA RID: 3786
	public enum EntryTypes
	{
		// Token: 0x0400578E RID: 22414
		Item,
		// Token: 0x0400578F RID: 22415
		Empty,
		// Token: 0x04005790 RID: 22416
		Hide
	}
}
