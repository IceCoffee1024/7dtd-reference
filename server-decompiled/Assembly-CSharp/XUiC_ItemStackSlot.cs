using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F81 RID: 3969
[Preserve]
public class XUiC_ItemStackSlot : XUiC_SelectableEntry, IItemStackController
{
	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x06007C49 RID: 31817 RVA: 0x002F2723 File Offset: 0x002F0923
	public XUiC_ItemStackSlotGrid Grid
	{
		get
		{
			return this.grid;
		}
	}

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x06007C4A RID: 31818 RVA: 0x002F272B File Offset: 0x002F092B
	public ItemStackGrid Source
	{
		get
		{
			return this.grid.Source;
		}
	}

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06007C4B RID: 31819 RVA: 0x002F2738 File Offset: 0x002F0938
	// (set) Token: 0x06007C4C RID: 31820 RVA: 0x002F2740 File Offset: 0x002F0940
	public int Index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06007C4D RID: 31821 RVA: 0x002F2749 File Offset: 0x002F0949
	// (set) Token: 0x06007C4E RID: 31822 RVA: 0x002F2751 File Offset: 0x002F0951
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06007C4F RID: 31823 RVA: 0x002F275A File Offset: 0x002F095A
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

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06007C50 RID: 31824 RVA: 0x002F277B File Offset: 0x002F097B
	// (set) Token: 0x06007C51 RID: 31825 RVA: 0x002F2783 File Offset: 0x002F0983
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

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x06007C52 RID: 31826 RVA: 0x002F279C File Offset: 0x002F099C
	public int SlotNumber
	{
		get
		{
			return this.Index;
		}
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06007C53 RID: 31827 RVA: 0x002F27A4 File Offset: 0x002F09A4
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

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06007C54 RID: 31828 RVA: 0x002F27BB File Offset: 0x002F09BB
	// (set) Token: 0x06007C55 RID: 31829 RVA: 0x002F27C3 File Offset: 0x002F09C3
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06007C56 RID: 31830 RVA: 0x002EEEA1 File Offset: 0x002ED0A1
	public XUi IItemStackController.xui
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui;
		}
	}

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x06007C57 RID: 31831 RVA: 0x0012B879 File Offset: 0x00129A79
	public XUiController IItemStackController.Controller
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this;
		}
	}

	// Token: 0x06007C58 RID: 31832 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void ForceRefreshItemStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007C59 RID: 31833 RVA: 0x002F27CC File Offset: 0x002F09CC
	public void HandleMoveToPreferredLocation()
	{
		XUiC_ItemStackSlotGrid xuiC_ItemStackSlotGrid = this.grid;
		if (xuiC_ItemStackSlotGrid == null)
		{
			return;
		}
		xuiC_ItemStackSlotGrid.MoveToPreferred(this.Index);
	}

	// Token: 0x06007C5A RID: 31834 RVA: 0x002F27E4 File Offset: 0x002F09E4
	public void Bind(XUiC_ItemStackSlotGrid _grid, int _index)
	{
		this.grid = _grid;
		this.Index = _index;
		this.IsDirty = true;
	}

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x06007C5B RID: 31835 RVA: 0x002F27FB File Offset: 0x002F09FB
	[XuiXmlBinding("itemicon")]
	public string ItemIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack);
		}
	}

	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x06007C5C RID: 31836 RVA: 0x002F2809 File Offset: 0x002F0A09
	[XuiXmlBinding("iconcolor")]
	public Color IconColor
	{
		get
		{
			return this.iconColor(this.ItemStack);
		}
	}

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x06007C5D RID: 31837 RVA: 0x002F2817 File Offset: 0x002F0A17
	[XuiXmlBinding("showicon")]
	public bool ShowIcon
	{
		get
		{
			return this.itemIcon(this.ItemStack) != "";
		}
	}

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x06007C5E RID: 31838 RVA: 0x002F282F File Offset: 0x002F0A2F
	[XuiXmlBinding("tooltip")]
	public string Tooltip
	{
		get
		{
			return this.itemName(this.ItemStack, this.PrefixId);
		}
	}

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x06007C5F RID: 31839 RVA: 0x002F2843 File Offset: 0x002F0A43
	[XuiXmlBinding("itemcount")]
	public string ItemCount
	{
		get
		{
			return this.itemCount(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x06007C60 RID: 31840 RVA: 0x002F285D File Offset: 0x002F0A5D
	[XuiXmlBinding("hasdurability")]
	public bool HasDurability
	{
		get
		{
			return this.showDurability(this.ItemStack);
		}
	}

	// Token: 0x17000EC6 RID: 3782
	// (get) Token: 0x06007C61 RID: 31841 RVA: 0x002F286B File Offset: 0x002F0A6B
	[XuiXmlBinding("haspermadurability")]
	public bool HasPermaDurability
	{
		get
		{
			return this.hasPermaDurability(this.ItemStack, this.showDurability(this.ItemStack));
		}
	}

	// Token: 0x17000EC7 RID: 3783
	// (get) Token: 0x06007C62 RID: 31842 RVA: 0x002F2885 File Offset: 0x002F0A85
	[XuiXmlBinding("durabilitycolor")]
	public Color DurabilityColor
	{
		get
		{
			return this.durabilityColor(this.ItemStack);
		}
	}

	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x06007C63 RID: 31843 RVA: 0x002F2893 File Offset: 0x002F0A93
	[XuiXmlBinding("durabilityfill")]
	public float DurabilityFill
	{
		get
		{
			return this.durabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x06007C64 RID: 31844 RVA: 0x002F28A1 File Offset: 0x002F0AA1
	[XuiXmlBinding("removeddurabilityfill")]
	public float RemovedDurabilityFill
	{
		get
		{
			return this.removedDurabilityFill(this.ItemStack);
		}
	}

	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06007C65 RID: 31845 RVA: 0x002F28AF File Offset: 0x002F0AAF
	[XuiXmlBinding("itemtypeicon")]
	public string ItemTypeIcon
	{
		get
		{
			return this.itemTypeIcon(this.ItemStack, this.xui.playerUI.entityPlayer);
		}
	}

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06007C66 RID: 31846 RVA: 0x002F28CD File Offset: 0x002F0ACD
	[XuiXmlBinding("hasitemtypeicon")]
	public bool HasItemTypeIcon
	{
		get
		{
			return this.hasItemTypeIcon(this.ItemStack);
		}
	}

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06007C67 RID: 31847 RVA: 0x002F28DB File Offset: 0x002F0ADB
	[XuiXmlBinding("itemtypeicontint")]
	public Color ItemTypeIconTint
	{
		get
		{
			return XUiBindingHelper.GetItemTypeIconTintColor(this.xui, this.ItemStack);
		}
	}

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x06007C68 RID: 31848 RVA: 0x002F28F3 File Offset: 0x002F0AF3
	[XuiXmlBinding("userlockedslot")]
	public bool UserLockedSlot
	{
		get
		{
			return this.Source != null && this.Source.IsLocked(this.Index);
		}
	}

	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x06007C69 RID: 31849 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isfavorite")]
	public bool IsFavorite
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06007C6A RID: 31850 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isQuickSwap")]
	public bool IsQuickSwap
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x06007C6B RID: 31851 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("islocked")]
	public bool IsLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06007C6C RID: 31852 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("ishovered")]
	public bool IsHovered
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06007C6D RID: 31853 RVA: 0x002F2910 File Offset: 0x002F0B10
	[XuiXmlBinding("selectionbordercolor")]
	public Color SelectionBorderColor
	{
		get
		{
			return this.borderColor();
		}
	}

	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06007C6E RID: 31854 RVA: 0x002F291D File Offset: 0x002F0B1D
	[XuiXmlBinding("backgroundcolor")]
	public Color BackgroundColor
	{
		get
		{
			return this.backgroundColor;
		}
	}

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06007C6F RID: 31855 RVA: 0x000321FF File Offset: 0x000303FF
	[XuiXmlBinding("locktypeicon")]
	public string LockTypeIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x06007C70 RID: 31856 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isassemblelocked")]
	public bool IsAssembleLocked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x06007C71 RID: 31857 RVA: 0x000321FF File Offset: 0x000303FF
	[XuiXmlBinding("stacklockicon")]
	public string StackLockIcon
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x06007C72 RID: 31858 RVA: 0x000BDB9B File Offset: 0x000BBD9B
	[XuiXmlBinding("stacklockcolor")]
	public Color StackLockColor
	{
		get
		{
			return Color.white;
		}
	}

	// Token: 0x06007C73 RID: 31859 RVA: 0x002F292A File Offset: 0x002F0B2A
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

	// Token: 0x06007C74 RID: 31860 RVA: 0x002F2950 File Offset: 0x002F0B50
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

	// Token: 0x06007C75 RID: 31861 RVA: 0x002F29E4 File Offset: 0x002F0BE4
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

	// Token: 0x06007C76 RID: 31862 RVA: 0x002F2A30 File Offset: 0x002F0C30
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

	// Token: 0x06007C77 RID: 31863 RVA: 0x002F2AAC File Offset: 0x002F0CAC
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

	// Token: 0x06007C78 RID: 31864 RVA: 0x002F2B04 File Offset: 0x002F0D04
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

	// Token: 0x06007C79 RID: 31865 RVA: 0x002F2B8A File Offset: 0x002F0D8A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPermaDurability(ItemStack _stack, bool _showDurability)
	{
		return _showDurability && ((_stack != null) ? _stack.itemValue : null) != null && EntityPlayerLocal.PermaDegrationOn && _stack.itemValue.MaxDurabilityModifier < 1f;
	}

	// Token: 0x06007C7A RID: 31866 RVA: 0x002F2BB8 File Offset: 0x002F0DB8
	[PublicizedFrom(EAccessModifier.Private)]
	public Color durabilityColor(ItemStack _stack)
	{
		return QualityInfo.GetQualityColor((int)((_stack != null) ? _stack.itemValue.Quality : 0));
	}

	// Token: 0x06007C7B RID: 31867 RVA: 0x002F2BD0 File Offset: 0x002F0DD0
	[PublicizedFrom(EAccessModifier.Private)]
	public float durabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) != null)
		{
			return ((_stack.itemValue.MaxUseTimes == 0) ? 1f : ((float)_stack.itemValue.MaxUseTimes - _stack.itemValue.UseTimes)) / (float)_stack.itemValue.MaxUseTimesUI;
		}
		return 0f;
	}

	// Token: 0x06007C7C RID: 31868 RVA: 0x002F2C2A File Offset: 0x002F0E2A
	[PublicizedFrom(EAccessModifier.Private)]
	public float removedDurabilityFill(ItemStack _stack)
	{
		if (((_stack != null) ? _stack.itemValue : null) == null)
		{
			return 1f;
		}
		return _stack.itemValue.MaxDurabilityModifier;
	}

	// Token: 0x06007C7D RID: 31869 RVA: 0x002F2C4C File Offset: 0x002F0E4C
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

	// Token: 0x06007C7E RID: 31870 RVA: 0x002F2CE8 File Offset: 0x002F0EE8
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

	// Token: 0x06007C7F RID: 31871 RVA: 0x002F2D49 File Offset: 0x002F0F49
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		this.IsDirty = true;
		base.OnHovered(_isOver);
	}

	// Token: 0x06007C80 RID: 31872 RVA: 0x002F2D60 File Offset: 0x002F0F60
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

	// Token: 0x04005C2D RID: 23597
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStackSlotGrid grid;

	// Token: 0x04005C31 RID: 23601
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005C32 RID: 23602
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005C33 RID: 23603
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005C34 RID: 23604
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005C35 RID: 23605
	[PublicizedFrom(EAccessModifier.Private)]
	public const int PickupSnapDistance = 4;

	// Token: 0x04005C36 RID: 23606
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005C37 RID: 23607
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005C38 RID: 23608
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos;
}
