using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F62 RID: 3938
[Preserve]
public class XUiC_ItemStackSlot : XUiC_SelectableEntry
{
	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x06007BF3 RID: 31731 RVA: 0x002ED90A File Offset: 0x002EBB0A
	public XUiC_ItemStackSlotGrid Grid
	{
		get
		{
			return this.grid;
		}
	}

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06007BF4 RID: 31732 RVA: 0x002ED912 File Offset: 0x002EBB12
	public ItemStackGrid Source
	{
		get
		{
			return this.grid.Source;
		}
	}

	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06007BF5 RID: 31733 RVA: 0x002ED91F File Offset: 0x002EBB1F
	// (set) Token: 0x06007BF6 RID: 31734 RVA: 0x002ED927 File Offset: 0x002EBB27
	public int Index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x06007BF7 RID: 31735 RVA: 0x002ED930 File Offset: 0x002EBB30
	// (set) Token: 0x06007BF8 RID: 31736 RVA: 0x002ED938 File Offset: 0x002EBB38
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x06007BF9 RID: 31737 RVA: 0x002ED941 File Offset: 0x002EBB41
	public ItemStack ItemStack
	{
		get
		{
			if (this.Source == null)
			{
				return ItemStack.Empty;
			}
			return this.Source[this.Index];
		}
	}

	// Token: 0x06007BFA RID: 31738 RVA: 0x002ED962 File Offset: 0x002EBB62
	public void Bind(XUiC_ItemStackSlotGrid _grid, int _index)
	{
		this.grid = _grid;
		this.Index = _index;
		this.IsDirty = true;
	}

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06007BFB RID: 31739 RVA: 0x002ED979 File Offset: 0x002EBB79
	[XuiXmlBinding("itemicon")]
	public string ItemIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack);
		}
	}

	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06007BFC RID: 31740 RVA: 0x002ED987 File Offset: 0x002EBB87
	[XuiXmlBinding("iconcolor")]
	public string IconColor
	{
		get
		{
			return this.iconColor(this.ItemStack);
		}
	}

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06007BFD RID: 31741 RVA: 0x002ED995 File Offset: 0x002EBB95
	[XuiXmlBinding("showicon")]
	public bool ShowIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack) != "";
		}
	}

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06007BFE RID: 31742 RVA: 0x002ED9AD File Offset: 0x002EBBAD
	[XuiXmlBinding("tooltip")]
	public string Tooltip
	{
		get
		{
			return this.itemName(this.ItemStack, this.PrefixId);
		}
	}

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06007BFF RID: 31743 RVA: 0x002ED9C1 File Offset: 0x002EBBC1
	[XuiXmlBinding("itemcount")]
	public string ItemCount
	{
		get
		{
			return this.itemCount(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06007C00 RID: 31744 RVA: 0x002ED9DB File Offset: 0x002EBBDB
	[XuiXmlBinding("hasdurability")]
	public bool HasDurability
	{
		get
		{
			return this.showDurability(this.ItemStack);
		}
	}

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06007C01 RID: 31745 RVA: 0x002ED9E9 File Offset: 0x002EBBE9
	[XuiXmlBinding("haspermadurability")]
	public bool HasPermaDurability
	{
		get
		{
			return this.hasPermaDurability(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06007C02 RID: 31746 RVA: 0x002EDA03 File Offset: 0x002EBC03
	[XuiXmlBinding("durabilitycolor")]
	public string DurabilityColor
	{
		get
		{
			return this.durabilityColor(this.ItemStack);
		}
	}

	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06007C03 RID: 31747 RVA: 0x002EDA11 File Offset: 0x002EBC11
	[XuiXmlBinding("durabilityfill")]
	public string DurabilityFill
	{
		get
		{
			return this.durabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000E97 RID: 3735
	// (get) Token: 0x06007C04 RID: 31748 RVA: 0x002EDA1F File Offset: 0x002EBC1F
	[XuiXmlBinding("removeddurabilityfill")]
	public string RemovedDurabilityFill
	{
		get
		{
			return this.removedDurabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06007C05 RID: 31749 RVA: 0x002EDA2D File Offset: 0x002EBC2D
	[XuiXmlBinding("itemtypeicon")]
	public string ItemTypeIcon
	{
		get
		{
			return this.itemTypeIcon(this.ItemStack, this.xui.playerUI.entityPlayer);
		}
	}

	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06007C06 RID: 31750 RVA: 0x002EDA4B File Offset: 0x002EBC4B
	[XuiXmlBinding("hasitemtypeicon")]
	public bool HasItemTypeIcon
	{
		get
		{
			return this.hasItemTypeIcon(this.ItemStack);
		}
	}

	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x06007C07 RID: 31751 RVA: 0x002EDA59 File Offset: 0x002EBC59
	[XuiXmlBinding("itemtypeicontint")]
	public string ItemTypeIconTint
	{
		get
		{
			return this.itemTypeIconTint(this.ItemStack, this.xui.playerUI.entityPlayer);
		}
	}

	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x06007C08 RID: 31752 RVA: 0x002EDA77 File Offset: 0x002EBC77
	[XuiXmlBinding("userlockedslot")]
	public bool UserLockedSlot
	{
		get
		{
			return this.Source != null && this.Source.IsLocked(this.Index);
		}
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x06007C09 RID: 31753 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isfavorite")]
	public bool IsFavorite
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06007C0A RID: 31754 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isQuickSwap")]
	public bool IsQuickSwap
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06007C0B RID: 31755 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("islocked")]
	public bool IsLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06007C0C RID: 31756 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("ishovered")]
	public bool IsHovered
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06007C0D RID: 31757 RVA: 0x002EDA94 File Offset: 0x002EBC94
	[XuiXmlBinding("selectionbordercolor")]
	public string SelectionBorderColor
	{
		get
		{
			return this.selectionbordercolorFormatter.Format(this.borderColor());
		}
	}

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06007C0E RID: 31758 RVA: 0x002EDAA7 File Offset: 0x002EBCA7
	[XuiXmlBinding("backgroundcolor")]
	public string BackgroundColor
	{
		get
		{
			return this.backgroundcolorFormatter.Format(this.backgroundColor);
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06007C0F RID: 31759 RVA: 0x00032147 File Offset: 0x00030347
	[XuiXmlBinding("locktypeicon")]
	public string LockTypeIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x06007C10 RID: 31760 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isassemblelocked")]
	public bool IsAssembleLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x06007C11 RID: 31761 RVA: 0x00032147 File Offset: 0x00030347
	[XuiXmlBinding("stacklockicon")]
	public string StackLockIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x06007C12 RID: 31762 RVA: 0x002EDABA File Offset: 0x002EBCBA
	[XuiXmlBinding("stacklockcolor")]
	public string StackLockColor
	{
		get
		{
			return "255,255,255,255";
		}
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x002EDAC1 File Offset: 0x002EBCC1
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 borderColor()
	{
		if (base.IsSelected)
		{
			return this.selectColor;
		}
		if (this.isOver)
		{
			return this.highlightColor;
		}
		return this.backgroundColor;
	}

	// Token: 0x06007C14 RID: 31764 RVA: 0x002EDAE8 File Offset: 0x002EBCE8
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemIcon(ItemStack _stack)
	{
		if (_stack.IsEmpty())
		{
			return "";
		}
		ItemClass itemClassOrMissing = _stack.itemValue.ItemClassOrMissing;
		ItemClassBlock itemClassBlock = itemClassOrMissing as ItemClassBlock;
		Block block = ((itemClassBlock != null) ? itemClassBlock.GetBlock() : null);
		if (block == null)
		{
			return _stack.itemValue.GetPropertyOverride(ItemClass.PropCustomIcon, itemClassOrMissing.GetIconName());
		}
		if (!block.SelectAlternates)
		{
			return _stack.itemValue.GetPropertyOverride(ItemClass.PropCustomIcon, itemClassOrMissing.GetIconName());
		}
		return block.GetAltBlockValue(_stack.itemValue.Meta).Block.GetIconName();
	}

	// Token: 0x06007C15 RID: 31765 RVA: 0x002EDB7C File Offset: 0x002EBD7C
	[PublicizedFrom(EAccessModifier.Private)]
	public string iconColor(ItemStack _stack)
	{
		ItemValue itemValue = _stack.itemValue;
		ItemClass itemClass = ((itemValue != null) ? itemValue.ItemClassOrMissing : null);
		if (itemClass == null)
		{
			return "255,255,255,0";
		}
		Color32 color = itemClass.GetIconTint(_stack.itemValue);
		return this.itemiconcolorFormatter.Format(color);
	}

	// Token: 0x06007C16 RID: 31766 RVA: 0x002EDBC4 File Offset: 0x002EBDC4
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemName(ItemStack _stack, bool _prefixId)
	{
		if (_stack.IsEmpty())
		{
			return "";
		}
		ItemClass itemClassOrMissing = _stack.itemValue.ItemClassOrMissing;
		string text = itemClassOrMissing.GetLocalizedItemName();
		if (itemClassOrMissing.IsBlock())
		{
			text = Block.list[_stack.itemValue.type].GetLocalizedBlockName(_stack.itemValue);
		}
		if (!_prefixId)
		{
			return text;
		}
		int itemOrBlockId = _stack.itemValue.GetItemOrBlockId();
		return string.Format("{0}\n({1}) {2}", text, itemOrBlockId, itemClassOrMissing.Name);
	}

	// Token: 0x06007C17 RID: 31767 RVA: 0x002EDC40 File Offset: 0x002EBE40
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showDurability(ItemStack _stack)
	{
		bool? flag;
		if (_stack == null)
		{
			flag = null;
		}
		else
		{
			ItemValue itemValue = _stack.itemValue;
			if (itemValue == null)
			{
				flag = null;
			}
			else
			{
				ItemClass itemClass = itemValue.ItemClass;
				flag = ((itemClass != null) ? new bool?(itemClass.ShowQualityBar) : null);
			}
		}
		bool? flag2 = flag;
		return flag2.GetValueOrDefault();
	}

	// Token: 0x06007C18 RID: 31768 RVA: 0x002EDC98 File Offset: 0x002EBE98
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemCount(ItemStack _stack, bool _showDurability)
	{
		if (_stack.IsEmpty())
		{
			return "";
		}
		if (_showDurability)
		{
			if (_stack.itemValue.Quality > 0)
			{
				return this.itemcountFormatter.Format((int)_stack.itemValue.Quality);
			}
			if (!_stack.itemValue.IsMod)
			{
				return "";
			}
			return "*";
		}
		else
		{
			if (_stack.itemValue.ItemClassOrMissing.MaxCount != 1)
			{
				return this.itemcountFormatter.Format(_stack.count);
			}
			return "";
		}
	}

	// Token: 0x06007C19 RID: 31769 RVA: 0x002EDD1E File Offset: 0x002EBF1E
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPermaDurability(ItemStack _stack, bool _showDurability)
	{
		return _showDurability && ((_stack != null) ? _stack.itemValue : null) != null && EntityPlayerLocal.PermaDegrationOn && _stack.itemValue.MaxDurabilityModifier < 1f;
	}

	// Token: 0x06007C1A RID: 31770 RVA: 0x002EDD4C File Offset: 0x002EBF4C
	[PublicizedFrom(EAccessModifier.Private)]
	public string durabilityColor(ItemStack _stack)
	{
		return this.durabilitycolorFormatter.Format(QualityInfo.GetQualityColor((int)((_stack != null) ? _stack.itemValue.Quality : 0)));
	}

	// Token: 0x06007C1B RID: 31771 RVA: 0x002EDD74 File Offset: 0x002EBF74
	[PublicizedFrom(EAccessModifier.Private)]
	public string durabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) == null)
		{
			return "0.0";
		}
		if (_stack.itemValue.MaxUseTimes != 0)
		{
			return this.durabilityFillFormatter.Format(((float)_stack.itemValue.MaxUseTimes - _stack.itemValue.UseTimes) / (float)_stack.itemValue.MaxUseTimesUI);
		}
		return "1";
	}

	// Token: 0x06007C1C RID: 31772 RVA: 0x002EDDD8 File Offset: 0x002EBFD8
	[PublicizedFrom(EAccessModifier.Private)]
	public string removedDurabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) == null)
		{
			return "1";
		}
		return this.removeddurabilityFillFormatter.Format(_stack.itemValue.MaxDurabilityModifier);
	}

	// Token: 0x06007C1D RID: 31773 RVA: 0x002EDE04 File Offset: 0x002EC004
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemTypeIcon(ItemStack _stack, EntityPlayerLocal _player)
	{
		if (_stack.IsEmpty())
		{
			return "";
		}
		ItemClass itemClassOrMissing = _stack.itemValue.ItemClassOrMissing;
		if (itemClassOrMissing == null)
		{
			return "";
		}
		if (itemClassOrMissing.IsBlock() && _stack.itemValue.TextureFullArray.IsDefault)
		{
			return Block.list[_stack.itemValue.type].ItemTypeIcon;
		}
		if (itemClassOrMissing.AltItemTypeIcon != null && itemClassOrMissing.Unlocks != "" && XUiM_ItemStack.CheckKnown(_player, itemClassOrMissing, _stack.itemValue))
		{
			return itemClassOrMissing.AltItemTypeIcon;
		}
		return itemClassOrMissing.ItemTypeIcon;
	}

	// Token: 0x06007C1E RID: 31774 RVA: 0x002EDE9C File Offset: 0x002EC09C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasItemTypeIcon(ItemStack _stack)
	{
		if (_stack.IsEmpty())
		{
			return false;
		}
		ItemClass itemClassOrMissing = _stack.itemValue.ItemClassOrMissing;
		if (itemClassOrMissing == null)
		{
			return false;
		}
		if (!itemClassOrMissing.IsBlock())
		{
			return itemClassOrMissing.ItemTypeIcon != "";
		}
		return Block.list[_stack.itemValue.type].ItemTypeIcon != "";
	}

	// Token: 0x06007C1F RID: 31775 RVA: 0x002EDF00 File Offset: 0x002EC100
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemTypeIconTint(ItemStack _stack, EntityPlayerLocal _player)
	{
		if (!_stack.IsEmpty())
		{
			ItemClass itemClassOrMissing = _stack.itemValue.ItemClassOrMissing;
			if (itemClassOrMissing != null && itemClassOrMissing.Unlocks != "" && XUiM_ItemStack.CheckKnown(_player, itemClassOrMissing, _stack.itemValue))
			{
				return this.altitemtypeiconcolorFormatter.Format(itemClassOrMissing.AltItemTypeIconColor);
			}
		}
		return "255,255,255,255";
	}

	// Token: 0x06007C20 RID: 31776 RVA: 0x002EDF61 File Offset: 0x002EC161
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		this.IsDirty = true;
		base.OnHovered(_isOver);
	}

	// Token: 0x06007C21 RID: 31777 RVA: 0x002EDF78 File Offset: 0x002EC178
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.WindowGroup.isShowing && this.isOver && base.ViewComponent.UiTransformIsHovered && base.ViewComponent.EventOnPress)
		{
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			if (InputUtils.ShiftKeyPressed)
			{
				if (cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton))
				{
					this.grid.MoveToPreferred(this.Index);
				}
				this.lastClicked = false;
				base.handleDirtyUpdateDefault();
				return;
			}
			ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
			if (cursorController.GetMouseButton(UICamera.MouseButton.LeftButton))
			{
				Vector3 vector = cursorController.GetScreenPosition();
				if (currentStack.IsEmpty() && !this.ItemStack.IsEmpty())
				{
					if (!this.lastClicked)
					{
						this.startMousePos = vector;
					}
					else if ((vector - this.startMousePos).magnitude > 4f)
					{
						this.grid.OnCellSelected(this.Index);
					}
				}
				if (cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton))
				{
					this.lastClicked = true;
				}
			}
			else if (cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton))
			{
				if (!currentStack.IsEmpty() && this.lastClicked)
				{
					this.grid.OnCellSelected(this.Index);
				}
				this.lastClicked = false;
				base.IsSelected = false;
			}
			else
			{
				this.lastClicked = false;
			}
		}
		else
		{
			this.lastClicked = false;
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005BC4 RID: 23492
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStackSlotGrid grid;

	// Token: 0x04005BC7 RID: 23495
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BC8 RID: 23496
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005BC9 RID: 23497
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005BCA RID: 23498
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removeddurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005BCB RID: 23499
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BCC RID: 23500
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BCD RID: 23501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BCE RID: 23502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BCF RID: 23503
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005BD0 RID: 23504
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005BD1 RID: 23505
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005BD2 RID: 23506
	[PublicizedFrom(EAccessModifier.Private)]
	public const int PickupSnapDistance = 4;

	// Token: 0x04005BD3 RID: 23507
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005BD4 RID: 23508
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005BD5 RID: 23509
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos;
}
