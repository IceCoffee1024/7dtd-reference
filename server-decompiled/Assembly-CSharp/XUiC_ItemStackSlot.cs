using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F71 RID: 3953
[Preserve]
public class XUiC_ItemStackSlot : XUiC_SelectableEntry, IItemStackController
{
	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x06007BFA RID: 31738 RVA: 0x002F0A9A File Offset: 0x002EEC9A
	public XUiC_ItemStackSlotGrid Grid
	{
		get
		{
			return this.grid;
		}
	}

	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x06007BFB RID: 31739 RVA: 0x002F0AA2 File Offset: 0x002EECA2
	public ItemStackGrid Source
	{
		get
		{
			return this.grid.Source;
		}
	}

	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06007BFC RID: 31740 RVA: 0x002F0AAF File Offset: 0x002EECAF
	// (set) Token: 0x06007BFD RID: 31741 RVA: 0x002F0AB7 File Offset: 0x002EECB7
	public int Index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x06007BFE RID: 31742 RVA: 0x002F0AC0 File Offset: 0x002EECC0
	// (set) Token: 0x06007BFF RID: 31743 RVA: 0x002F0AC8 File Offset: 0x002EECC8
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x06007C00 RID: 31744 RVA: 0x002F0AD1 File Offset: 0x002EECD1
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

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x06007C01 RID: 31745 RVA: 0x002F0AF2 File Offset: 0x002EECF2
	// (set) Token: 0x06007C02 RID: 31746 RVA: 0x002F0AFA File Offset: 0x002EECFA
	public ItemStack IItemStackController.ItemStack
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.ItemStack;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			ItemStackGrid source = this.Source;
			if (source == null)
			{
				return;
			}
			source.SetItem(this.Index, value);
		}
	}

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x06007C03 RID: 31747 RVA: 0x002F0B13 File Offset: 0x002EED13
	public int SlotNumber
	{
		get
		{
			return this.Index;
		}
	}

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x06007C04 RID: 31748 RVA: 0x002F0B1B File Offset: 0x002EED1B
	public XUiC_ItemStack.StackLocationTypes StackLocation
	{
		get
		{
			if (this.Source == null)
			{
				return XUiC_ItemStack.StackLocationTypes.None;
			}
			return this.Source.StackLocation;
		}
	}

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06007C05 RID: 31749 RVA: 0x002F0B32 File Offset: 0x002EED32
	// (set) Token: 0x06007C06 RID: 31750 RVA: 0x002F0B3A File Offset: 0x002EED3A
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06007C07 RID: 31751 RVA: 0x002ED585 File Offset: 0x002EB785
	public XUi IItemStackController.xui
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui;
		}
	}

	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x06007C08 RID: 31752 RVA: 0x0012AC85 File Offset: 0x00128E85
	public XUiController IItemStackController.Controller
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this;
		}
	}

	// Token: 0x06007C09 RID: 31753 RVA: 0x0008A995 File Offset: 0x00088B95
	public void ForceRefreshItemStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007C0A RID: 31754 RVA: 0x002F0B43 File Offset: 0x002EED43
	public void HandleMoveToPreferredLocation()
	{
		XUiC_ItemStackSlotGrid xuiC_ItemStackSlotGrid = this.grid;
		if (xuiC_ItemStackSlotGrid == null)
		{
			return;
		}
		xuiC_ItemStackSlotGrid.MoveToPreferred(this.Index);
	}

	// Token: 0x06007C0B RID: 31755 RVA: 0x002F0B5B File Offset: 0x002EED5B
	public void Bind(XUiC_ItemStackSlotGrid _grid, int _index)
	{
		this.grid = _grid;
		this.Index = _index;
		this.IsDirty = true;
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x06007C0C RID: 31756 RVA: 0x002F0B72 File Offset: 0x002EED72
	[XuiXmlBinding("itemicon")]
	public string ItemIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack);
		}
	}

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x06007C0D RID: 31757 RVA: 0x002F0B80 File Offset: 0x002EED80
	[XuiXmlBinding("iconcolor")]
	public Color IconColor
	{
		get
		{
			return this.iconColor(this.ItemStack);
		}
	}

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x06007C0E RID: 31758 RVA: 0x002F0B8E File Offset: 0x002EED8E
	[XuiXmlBinding("showicon")]
	public bool ShowIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack) != "";
		}
	}

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06007C0F RID: 31759 RVA: 0x002F0BA6 File Offset: 0x002EEDA6
	[XuiXmlBinding("tooltip")]
	public string Tooltip
	{
		get
		{
			return this.itemName(this.ItemStack, this.PrefixId);
		}
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06007C10 RID: 31760 RVA: 0x002F0BBA File Offset: 0x002EEDBA
	[XuiXmlBinding("itemcount")]
	public string ItemCount
	{
		get
		{
			return this.itemCount(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06007C11 RID: 31761 RVA: 0x002F0BD4 File Offset: 0x002EEDD4
	[XuiXmlBinding("hasdurability")]
	public bool HasDurability
	{
		get
		{
			return this.showDurability(this.ItemStack);
		}
	}

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06007C12 RID: 31762 RVA: 0x002F0BE2 File Offset: 0x002EEDE2
	[XuiXmlBinding("haspermadurability")]
	public bool HasPermaDurability
	{
		get
		{
			return this.hasPermaDurability(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x06007C13 RID: 31763 RVA: 0x002F0BFC File Offset: 0x002EEDFC
	[XuiXmlBinding("durabilitycolor")]
	public Color DurabilityColor
	{
		get
		{
			return this.durabilityColor(this.ItemStack);
		}
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06007C14 RID: 31764 RVA: 0x002F0C0A File Offset: 0x002EEE0A
	[XuiXmlBinding("durabilityfill")]
	public float DurabilityFill
	{
		get
		{
			return this.durabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06007C15 RID: 31765 RVA: 0x002F0C18 File Offset: 0x002EEE18
	[XuiXmlBinding("removeddurabilityfill")]
	public float RemovedDurabilityFill
	{
		get
		{
			return this.removedDurabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06007C16 RID: 31766 RVA: 0x002F0C26 File Offset: 0x002EEE26
	[XuiXmlBinding("itemtypeicon")]
	public string ItemTypeIcon
	{
		get
		{
			return this.itemTypeIcon(this.ItemStack, this.xui.playerUI.entityPlayer);
		}
	}

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x06007C17 RID: 31767 RVA: 0x002F0C44 File Offset: 0x002EEE44
	[XuiXmlBinding("hasitemtypeicon")]
	public bool HasItemTypeIcon
	{
		get
		{
			return this.hasItemTypeIcon(this.ItemStack);
		}
	}

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x06007C18 RID: 31768 RVA: 0x002F0C52 File Offset: 0x002EEE52
	[XuiXmlBinding("itemtypeicontint")]
	public Color ItemTypeIconTint
	{
		get
		{
			return XUiBindingHelper.GetItemTypeIconTintColor(this.xui, this.ItemStack);
		}
	}

	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x06007C19 RID: 31769 RVA: 0x002F0C6A File Offset: 0x002EEE6A
	[XuiXmlBinding("userlockedslot")]
	public bool UserLockedSlot
	{
		get
		{
			return this.Source != null && this.Source.IsLocked(this.Index);
		}
	}

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x06007C1A RID: 31770 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isfavorite")]
	public bool IsFavorite
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x06007C1B RID: 31771 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isQuickSwap")]
	public bool IsQuickSwap
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x06007C1C RID: 31772 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("islocked")]
	public bool IsLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x06007C1D RID: 31773 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("ishovered")]
	public bool IsHovered
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000EC6 RID: 3782
	// (get) Token: 0x06007C1E RID: 31774 RVA: 0x002F0C87 File Offset: 0x002EEE87
	[XuiXmlBinding("selectionbordercolor")]
	public Color SelectionBorderColor
	{
		get
		{
			return this.borderColor();
		}
	}

	// Token: 0x17000EC7 RID: 3783
	// (get) Token: 0x06007C1F RID: 31775 RVA: 0x002F0C94 File Offset: 0x002EEE94
	[XuiXmlBinding("backgroundcolor")]
	public Color BackgroundColor
	{
		get
		{
			return this.backgroundColor;
		}
	}

	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x06007C20 RID: 31776 RVA: 0x0003223F File Offset: 0x0003043F
	[XuiXmlBinding("locktypeicon")]
	public string LockTypeIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x06007C21 RID: 31777 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isassemblelocked")]
	public bool IsAssembleLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06007C22 RID: 31778 RVA: 0x0003223F File Offset: 0x0003043F
	[XuiXmlBinding("stacklockicon")]
	public string StackLockIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06007C23 RID: 31779 RVA: 0x000BD7EF File Offset: 0x000BB9EF
	[XuiXmlBinding("stacklockcolor")]
	public Color StackLockColor
	{
		get
		{
			return Color.white;
		}
	}

	// Token: 0x06007C24 RID: 31780 RVA: 0x002F0CA1 File Offset: 0x002EEEA1
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

	// Token: 0x06007C25 RID: 31781 RVA: 0x002F0CC8 File Offset: 0x002EEEC8
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

	// Token: 0x06007C26 RID: 31782 RVA: 0x002F0D5C File Offset: 0x002EEF5C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color iconColor(ItemStack _stack)
	{
		ItemValue itemValue = _stack.itemValue;
		ItemClass itemClass = ((itemValue != null) ? itemValue.ItemClassOrMissing : null);
		if (itemClass == null)
		{
			return new Color(1f, 1f, 1f, 0f);
		}
		return itemClass.GetIconTint(_stack.itemValue);
	}

	// Token: 0x06007C27 RID: 31783 RVA: 0x002F0DA8 File Offset: 0x002EEFA8
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

	// Token: 0x06007C28 RID: 31784 RVA: 0x002F0E24 File Offset: 0x002EF024
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

	// Token: 0x06007C29 RID: 31785 RVA: 0x002F0E7C File Offset: 0x002EF07C
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

	// Token: 0x06007C2A RID: 31786 RVA: 0x002F0F02 File Offset: 0x002EF102
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPermaDurability(ItemStack _stack, bool _showDurability)
	{
		return _showDurability && ((_stack != null) ? _stack.itemValue : null) != null && EntityPlayerLocal.PermaDegrationOn && _stack.itemValue.MaxDurabilityModifier < 1f;
	}

	// Token: 0x06007C2B RID: 31787 RVA: 0x002F0F30 File Offset: 0x002EF130
	[PublicizedFrom(EAccessModifier.Private)]
	public Color durabilityColor(ItemStack _stack)
	{
		return QualityInfo.GetQualityColor((int)((_stack != null) ? _stack.itemValue.Quality : 0));
	}

	// Token: 0x06007C2C RID: 31788 RVA: 0x002F0F48 File Offset: 0x002EF148
	[PublicizedFrom(EAccessModifier.Private)]
	public float durabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) != null)
		{
			return ((_stack.itemValue.MaxUseTimes == 0) ? 1f : ((float)_stack.itemValue.MaxUseTimes - _stack.itemValue.UseTimes)) / (float)_stack.itemValue.MaxUseTimesUI;
		}
		return 0f;
	}

	// Token: 0x06007C2D RID: 31789 RVA: 0x002F0FA2 File Offset: 0x002EF1A2
	[PublicizedFrom(EAccessModifier.Private)]
	public float removedDurabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) == null)
		{
			return 1f;
		}
		return _stack.itemValue.MaxDurabilityModifier;
	}

	// Token: 0x06007C2E RID: 31790 RVA: 0x002F0FC4 File Offset: 0x002EF1C4
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

	// Token: 0x06007C2F RID: 31791 RVA: 0x002F1060 File Offset: 0x002EF260
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

	// Token: 0x06007C30 RID: 31792 RVA: 0x002F10C1 File Offset: 0x002EF2C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		this.IsDirty = true;
		base.OnHovered(_isOver);
	}

	// Token: 0x06007C31 RID: 31793 RVA: 0x002F10D8 File Offset: 0x002EF2D8
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

	// Token: 0x04005BF3 RID: 23539
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStackSlotGrid grid;

	// Token: 0x04005BF7 RID: 23543
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005BF8 RID: 23544
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005BF9 RID: 23545
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005BFA RID: 23546
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005BFB RID: 23547
	[PublicizedFrom(EAccessModifier.Private)]
	public const int PickupSnapDistance = 4;

	// Token: 0x04005BFC RID: 23548
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005BFD RID: 23549
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005BFE RID: 23550
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos;
}
