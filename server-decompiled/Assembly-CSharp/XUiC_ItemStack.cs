using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F79 RID: 3961
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemStack : XUiC_SelectableEntry, IItemStackController
{
	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06007BBD RID: 31677 RVA: 0x002EEB58 File Offset: 0x002ECD58
	// (set) Token: 0x06007BBE RID: 31678 RVA: 0x002EEB60 File Offset: 0x002ECD60
	public bool isQuickSwap
	{
		get
		{
			return this._isQuickSwap;
		}
		set
		{
			if (value == this._isQuickSwap)
			{
				return;
			}
			this._isQuickSwap = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06007BBF RID: 31679 RVA: 0x002EEB7A File Offset: 0x002ECD7A
	// (set) Token: 0x06007BC0 RID: 31680 RVA: 0x002EEB82 File Offset: 0x002ECD82
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

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06007BC1 RID: 31681 RVA: 0x002EEBA0 File Offset: 0x002ECDA0
	// (set) Token: 0x06007BC2 RID: 31682 RVA: 0x002EEBA8 File Offset: 0x002ECDA8
	[XuiXmlBinding("matches_search")]
	public bool MatchesSearch
	{
		get
		{
			return this.matchesSearch;
		}
		set
		{
			if (this.matchesSearch == value)
			{
				return;
			}
			this.matchesSearch = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x06007BC3 RID: 31683 RVA: 0x002EEBC2 File Offset: 0x002ECDC2
	// (set) Token: 0x06007BC4 RID: 31684 RVA: 0x002EEBCA File Offset: 0x002ECDCA
	public int SlotNumber { get; set; }

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06007BC5 RID: 31685 RVA: 0x002EEBD3 File Offset: 0x002ECDD3
	// (set) Token: 0x06007BC6 RID: 31686 RVA: 0x002EEBDB File Offset: 0x002ECDDB
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x06007BC7 RID: 31687 RVA: 0x002EEBE4 File Offset: 0x002ECDE4
	// (remove) Token: 0x06007BC8 RID: 31688 RVA: 0x002EEC1C File Offset: 0x002ECE1C
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x06007BC9 RID: 31689 RVA: 0x002EEC54 File Offset: 0x002ECE54
	// (remove) Token: 0x06007BCA RID: 31690 RVA: 0x002EEC8C File Offset: 0x002ECE8C
	public event XUiEvent_ToolLockChangeEventHandler ToolLockChangedEvent;

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x06007BCB RID: 31691 RVA: 0x002EECC4 File Offset: 0x002ECEC4
	// (remove) Token: 0x06007BCC RID: 31692 RVA: 0x002EECFC File Offset: 0x002ECEFC
	public event XUiEvent_LockChangeEventHandler LockChangedEvent;

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x06007BCD RID: 31693 RVA: 0x002EED34 File Offset: 0x002ECF34
	// (remove) Token: 0x06007BCE RID: 31694 RVA: 0x002EED6C File Offset: 0x002ECF6C
	public event XUiEvent_TimeIntervalElapsedEventHandler TimeIntervalElapsedEvent;

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x06007BCF RID: 31695 RVA: 0x002EEDA1 File Offset: 0x002ECFA1
	public ItemClass itemClass
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			ItemStack itemStack = this.itemStack;
			if (itemStack == null)
			{
				return null;
			}
			ItemValue itemValue = itemStack.itemValue;
			if (itemValue == null)
			{
				return null;
			}
			return itemValue.ItemClass;
		}
	}

	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06007BD0 RID: 31696 RVA: 0x002EEDBF File Offset: 0x002ECFBF
	public ItemClass itemClassOrMissing
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			ItemStack itemStack = this.itemStack;
			if (itemStack == null)
			{
				return null;
			}
			ItemValue itemValue = itemStack.itemValue;
			if (itemValue == null)
			{
				return null;
			}
			return itemValue.ItemClassOrMissing;
		}
	}

	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06007BD1 RID: 31697 RVA: 0x002EEDDD File Offset: 0x002ECFDD
	// (set) Token: 0x06007BD2 RID: 31698 RVA: 0x002EEDE5 File Offset: 0x002ECFE5
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E97 RID: 3735
	// (get) Token: 0x06007BD3 RID: 31699 RVA: 0x002EEDEE File Offset: 0x002ECFEE
	// (set) Token: 0x06007BD4 RID: 31700 RVA: 0x002EEDF9 File Offset: 0x002ECFF9
	public bool AssembleLock
	{
		get
		{
			return this.stackLockType == XUiC_ItemStack.StackLockTypes.Assemble;
		}
		set
		{
			this.stackLockType = (value ? XUiC_ItemStack.StackLockTypes.Assemble : XUiC_ItemStack.StackLockTypes.None);
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06007BD5 RID: 31701 RVA: 0x002EEE0F File Offset: 0x002ED00F
	// (set) Token: 0x06007BD6 RID: 31702 RVA: 0x002EEE1A File Offset: 0x002ED01A
	public bool QuestLock
	{
		get
		{
			return this.stackLockType == XUiC_ItemStack.StackLockTypes.Quest;
		}
		set
		{
			this.stackLockType = (value ? XUiC_ItemStack.StackLockTypes.Quest : ((this.stackLockType == XUiC_ItemStack.StackLockTypes.Quest) ? XUiC_ItemStack.StackLockTypes.None : this.stackLockType));
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06007BD7 RID: 31703 RVA: 0x002EEE41 File Offset: 0x002ED041
	// (set) Token: 0x06007BD8 RID: 31704 RVA: 0x002EEE4C File Offset: 0x002ED04C
	public bool ToolLock
	{
		get
		{
			return this.stackLockType == XUiC_ItemStack.StackLockTypes.Tool;
		}
		set
		{
			this.stackLockType = (value ? XUiC_ItemStack.StackLockTypes.Tool : XUiC_ItemStack.StackLockTypes.None);
			XUiEvent_ToolLockChangeEventHandler toolLockChangedEvent = this.ToolLockChangedEvent;
			if (toolLockChangedEvent != null)
			{
				toolLockChangedEvent(this.SlotNumber, this.itemStack, value);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x06007BD9 RID: 31705 RVA: 0x002EEE80 File Offset: 0x002ED080
	// (set) Token: 0x06007BDA RID: 31706 RVA: 0x002EEE8B File Offset: 0x002ED08B
	public bool HiddenLock
	{
		get
		{
			return this.stackLockType == XUiC_ItemStack.StackLockTypes.Hidden;
		}
		set
		{
			this.stackLockType = (value ? XUiC_ItemStack.StackLockTypes.Hidden : XUiC_ItemStack.StackLockTypes.None);
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x06007BDB RID: 31707 RVA: 0x002EEEA1 File Offset: 0x002ED0A1
	public XUi IItemStackController.xui
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.xui;
		}
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x06007BDC RID: 31708 RVA: 0x0012B879 File Offset: 0x00129A79
	public XUiController IItemStackController.Controller
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this;
		}
	}

	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06007BDD RID: 31709 RVA: 0x002EEEA9 File Offset: 0x002ED0A9
	// (set) Token: 0x06007BDE RID: 31710 RVA: 0x002EEEB1 File Offset: 0x002ED0B1
	public bool AttributeLock
	{
		get
		{
			return this.attributeLock;
		}
		set
		{
			this.attributeLock = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06007BDF RID: 31711 RVA: 0x002EEEC1 File Offset: 0x002ED0C1
	public bool StackLock
	{
		get
		{
			return this.stackLockType > XUiC_ItemStack.StackLockTypes.None;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06007BE0 RID: 31712 RVA: 0x002EEECC File Offset: 0x002ED0CC
	// (set) Token: 0x06007BE1 RID: 31713 RVA: 0x002EEED4 File Offset: 0x002ED0D4
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

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06007BE2 RID: 31714 RVA: 0x002EEEF9 File Offset: 0x002ED0F9
	// (set) Token: 0x06007BE3 RID: 31715 RVA: 0x002EEF01 File Offset: 0x002ED101
	public bool IsHolding { get; set; }

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06007BE4 RID: 31716 RVA: 0x002EEF0A File Offset: 0x002ED10A
	// (set) Token: 0x06007BE5 RID: 31717 RVA: 0x002EEF12 File Offset: 0x002ED112
	[XuiXmlBinding("islocked")]
	public bool IsLocked
	{
		get
		{
			return this.isLocked;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == this.isLocked)
			{
				return;
			}
			this.isLocked = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06007BE6 RID: 31718 RVA: 0x002EEF2C File Offset: 0x002ED12C
	// (set) Token: 0x06007BE7 RID: 31719 RVA: 0x002EEF34 File Offset: 0x002ED134
	public int RepairAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x06007BE8 RID: 31720 RVA: 0x002EEF3D File Offset: 0x002ED13D
	public bool AllowIconGrow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.itemClass != null;
		}
	}

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x06007BE9 RID: 31721 RVA: 0x002EEF48 File Offset: 0x002ED148
	// (set) Token: 0x06007BEA RID: 31722 RVA: 0x002EEF50 File Offset: 0x002ED150
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x06007BEB RID: 31723 RVA: 0x002EEF59 File Offset: 0x002ED159
	// (set) Token: 0x06007BEC RID: 31724 RVA: 0x002EEF61 File Offset: 0x002ED161
	public bool SimpleClick
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x06007BED RID: 31725 RVA: 0x002EEF6A File Offset: 0x002ED16A
	// (set) Token: 0x06007BEE RID: 31726 RVA: 0x002EEF72 File Offset: 0x002ED172
	public bool AllowDropping
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = true;

	// Token: 0x17000EA7 RID: 3751
	// (get) Token: 0x06007BEF RID: 31727 RVA: 0x002EEF7B File Offset: 0x002ED17B
	// (set) Token: 0x06007BF0 RID: 31728 RVA: 0x002EEF83 File Offset: 0x002ED183
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000EA8 RID: 3752
	// (get) Token: 0x06007BF1 RID: 31729 RVA: 0x002EEF8C File Offset: 0x002ED18C
	// (set) Token: 0x06007BF2 RID: 31730 RVA: 0x002EEF94 File Offset: 0x002ED194
	public bool ShowFavorites
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x06007BF3 RID: 31731 RVA: 0x002EEF9D File Offset: 0x002ED19D
	// (set) Token: 0x06007BF4 RID: 31732 RVA: 0x002EEFA8 File Offset: 0x002ED1A8
	public float LockTime
	{
		get
		{
			return this.lockTime;
		}
		set
		{
			this.lockTime = value;
			if (value == 0f)
			{
				this.timer.Text = "";
				this.timer.IsVisible = false;
				return;
			}
			this.timer.Text = string.Format("{0:00}:{1:00}", Mathf.Floor(this.lockTime / 60f), Mathf.Floor(this.lockTime % 60f));
			this.timer.IsVisible = true;
		}
	}

	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x06007BF5 RID: 31733 RVA: 0x002EF02E File Offset: 0x002ED22E
	// (set) Token: 0x06007BF6 RID: 31734 RVA: 0x002EF036 File Offset: 0x002ED236
	[XuiXmlBinding("userlockedslot")]
	public bool UserLockedSlot
	{
		get
		{
			return this.userLockedSlot;
		}
		set
		{
			if (value == this.userLockedSlot)
			{
				return;
			}
			this.userLockedSlot = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007BF7 RID: 31735 RVA: 0x002EF04F File Offset: 0x002ED24F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack.Clone();
	}

	// Token: 0x06007BF8 RID: 31736 RVA: 0x002EF05D File Offset: 0x002ED25D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateItemInfoWindow(XUiC_ItemStack _itemStack)
	{
		this.InfoWindow.SetItemStack(_itemStack, true);
	}

	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06007BF9 RID: 31737 RVA: 0x002EF06C File Offset: 0x002ED26C
	// (set) Token: 0x06007BFA RID: 31738 RVA: 0x002EF074 File Offset: 0x002ED274
	public ItemStack ItemStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			if (!this.itemStack.Equals(value))
			{
				this.setItemStack(value);
				if (this.itemStack.IsEmpty())
				{
					this.itemStack.Clear();
				}
				if (base.IsSelected)
				{
					this.updateItemInfoWindow(this);
				}
				this.HandleSlotChangeEvent();
				if (value.IsEmpty())
				{
					this.stackLockType = XUiC_ItemStack.StackLockTypes.None;
				}
				this.updateBackgroundTexture();
				this.IsDirty = true;
			}
			else
			{
				if (this.ItemStack.IsEmpty() && this.backgroundTexture != null)
				{
					this.backgroundTexture.Texture = null;
				}
				if (base.IsSelected)
				{
					this.updateItemInfoWindow(this);
				}
			}
			if (this.ItemStack.IsEmpty() && base.IsSelected)
			{
				base.IsSelected = false;
			}
			this.viewComponent.IsSnappable = !this.itemStack.IsEmpty();
			this.IsDirty = true;
		}
	}

	// Token: 0x06007BFB RID: 31739 RVA: 0x002EF150 File Offset: 0x002ED350
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBackgroundTexture()
	{
		if (this.backgroundTexture == null)
		{
			return;
		}
		ItemClassBlock itemClassBlock = this.itemClass as ItemClassBlock;
		if (itemClassBlock != null)
		{
			Block block = itemClassBlock.GetBlock();
			if (block.GetAutoShapeType() == EAutoShapeType.None)
			{
				this.backgroundTexture.Texture = null;
				return;
			}
			int uiBackgroundTextureId = block.GetUiBackgroundTextureId(this.itemStack.itemValue.ToBlockValue(false), BlockFace.Top, 0);
			this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|139_0(uiBackgroundTextureId);
			return;
		}
		else
		{
			ItemClass itemClass = this.itemClass;
			ItemActionTextureBlock itemActionTextureBlock = ((itemClass != null) ? itemClass.Actions[0] : null) as ItemActionTextureBlock;
			if (itemActionTextureBlock == null)
			{
				this.backgroundTexture.Texture = null;
				return;
			}
			int num = this.itemStack.itemValue.Meta;
			if (num == 0)
			{
				num = itemActionTextureBlock.DefaultTextureID;
			}
			int num2;
			int num3;
			if (XUiC_ItemStack.TryResolvePaintTextureId(num, itemActionTextureBlock.DefaultTextureID, out num2, out num3))
			{
				if (num2 != this.itemStack.itemValue.Meta)
				{
					this.itemStack.itemValue.Meta = (int)((byte)num2);
				}
				this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|139_0(num3);
				return;
			}
			this.backgroundTexture.Texture = null;
			this.backgroundTexture.ForceTextureUpdate();
			return;
		}
	}

	// Token: 0x06007BFC RID: 31740 RVA: 0x002EF258 File Offset: 0x002ED458
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryResolvePaintTextureId(int requestedPaintId, int fallbackPaintId, out int resolvedPaintId, out int textureId)
	{
		resolvedPaintId = 0;
		textureId = 0;
		BlockTextureData blockTextureData;
		if (XUiC_ItemStack.TryGetBlockTextureData(requestedPaintId, out blockTextureData))
		{
			resolvedPaintId = requestedPaintId;
			textureId = (int)blockTextureData.TextureID;
			return true;
		}
		BlockTextureData blockTextureData2;
		if (fallbackPaintId != requestedPaintId && XUiC_ItemStack.TryGetBlockTextureData(fallbackPaintId, out blockTextureData2))
		{
			resolvedPaintId = fallbackPaintId;
			textureId = (int)blockTextureData2.TextureID;
			return true;
		}
		return false;
	}

	// Token: 0x06007BFD RID: 31741 RVA: 0x002EF2A0 File Offset: 0x002ED4A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryGetBlockTextureData(int paintId, out BlockTextureData data)
	{
		data = null;
		BlockTextureData[] list = BlockTextureData.list;
		if (list == null || paintId < 0 || paintId >= list.Length)
		{
			return false;
		}
		data = list[paintId];
		return data != null;
	}

	// Token: 0x06007BFE RID: 31742 RVA: 0x002EF2D0 File Offset: 0x002ED4D0
	public void ResetTweenScale()
	{
		if (this.tweenScale != null && this.tweenScale.value != Vector3.one && (!this.isOver || this.IsLocked))
		{
			this.tweenScale.from = Vector3.one * 1.5f;
			this.tweenScale.to = Vector3.one;
			this.tweenScale.enabled = true;
			this.tweenScale.duration = 0.1f;
		}
	}

	// Token: 0x06007BFF RID: 31743 RVA: 0x002EF358 File Offset: 0x002ED558
	public void ForceSetItemStack(ItemStack _stack)
	{
		bool isSelected = base.IsSelected;
		this.itemStack = ItemStack.Empty;
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent != null)
		{
			slotChangedEvent(this.SlotNumber, this.itemStack);
		}
		if (!_stack.IsEmpty())
		{
			base.IsSelected = isSelected;
		}
		this.ItemStack = _stack;
		XUiEvent_SlotChangedEventHandler slotChangedEvent2 = this.SlotChangedEvent;
		if (slotChangedEvent2 == null)
		{
			return;
		}
		slotChangedEvent2(this.SlotNumber, this.itemStack);
	}

	// Token: 0x06007C00 RID: 31744 RVA: 0x002EF3C6 File Offset: 0x002ED5C6
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleSlotChangeEvent()
	{
		if (this.itemStack.IsEmpty() && base.IsSelected)
		{
			base.IsSelected = false;
		}
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x06007C01 RID: 31745 RVA: 0x002EF400 File Offset: 0x002ED600
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += this.SlotVisibilityChanged;
		XUiController childById = base.GetChildById("timer");
		if (childById != null)
		{
			this.timer = childById.ViewComponent as XUiV_Label;
		}
		XUiController childById2 = base.GetChildById("itemIcon");
		if (childById2 != null)
		{
			this.itemIconSprite = childById2.ViewComponent as XUiV_Sprite;
		}
		XUiController childById3 = base.GetChildById("lockTypeIcon");
		if (childById3 != null)
		{
			this.lockTypeIcon = childById3.ViewComponent as XUiV_Sprite;
		}
		XUiController childById4 = base.GetChildById("backgroundTexture");
		if (childById4 != null)
		{
			this.backgroundTexture = childById4.ViewComponent as XUiV_Texture;
			if (this.backgroundTexture != null)
			{
				this.backgroundTexture.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
			}
		}
		XUiController childById5 = base.GetChildById("rectSlotLock");
		if (childById5 != null)
		{
			childById5.OnHover += delegate(XUiController _, bool _over)
			{
			};
			childById5.OnPress += delegate(XUiController _, int _)
			{
				this.UserLockedSlot = !this.UserLockedSlot;
				base.RefreshBindings();
			};
		}
		this.tweenScale = this.itemIconSprite.UiTransform.gameObject.AddComponent<TweenScale>();
		base.ViewComponent.UseSelectionBox = false;
	}

	// Token: 0x06007C02 RID: 31746 RVA: 0x002EF52E File Offset: 0x002ED72E
	[PublicizedFrom(EAccessModifier.Private)]
	public void SlotVisibilityChanged(XUiController sender, bool visibleSelf, bool visibleInScene)
	{
		if (!visibleSelf && base.IsSelected)
		{
			base.IsSelected = false;
			this.updateItemInfoWindow(null);
		}
	}

	// Token: 0x06007C03 RID: 31747 RVA: 0x002EF54C File Offset: 0x002ED74C
	public void UpdateTimer(float _dt)
	{
		if (!this.IsLocked)
		{
			return;
		}
		if (this.lockType == XUiC_ItemStack.LockTypes.Shell || this.lockType == XUiC_ItemStack.LockTypes.Burning)
		{
			return;
		}
		float num = this.lockTime;
		if (this.lockTime > 0f)
		{
			this.lockTime -= _dt;
			if (this.currentInterval == -1)
			{
				this.currentInterval = (int)this.lockTime / this.TimeInterval;
			}
			if (this.TimeIntervalElapsedEvent != null && this.TimeInterval != 0)
			{
				int num2 = (int)this.lockTime / this.TimeInterval;
				if (num2 != this.currentInterval)
				{
					this.TimeIntervalElapsedEvent(this.lockTime, this);
					this.currentInterval = num2;
				}
			}
		}
		if (this.lockTime <= 0f && num != 0f)
		{
			XUiEvent_TimeIntervalElapsedEventHandler timeIntervalElapsedEvent = this.TimeIntervalElapsedEvent;
			if (timeIntervalElapsedEvent != null)
			{
				timeIntervalElapsedEvent(this.lockTime, this);
			}
			if (this.LockChangedEvent != null)
			{
				this.LockChangedEvent(this.lockType, this);
			}
			else
			{
				this.IsLocked = false;
			}
		}
		if (this.lockTime <= 0f)
		{
			this.timer.IsVisible = false;
			this.timer.Text = "";
			return;
		}
		this.timer.IsVisible = true;
		this.timer.Text = string.Format("{0:00}:{1:00}", Mathf.Floor(this.lockTime / 60f), Mathf.Floor(this.lockTime % 60f));
	}

	// Token: 0x06007C04 RID: 31748 RVA: 0x002EF6BC File Offset: 0x002ED8BC
	public override void Update(float _dt)
	{
		using (this.pmIsUpdate.Auto())
		{
			using (this.pmIsUpdateBase.Auto())
			{
				base.Update(_dt);
			}
			if (base.WindowGroup.isShowing)
			{
				PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
				CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
				Vector3 vector = cursorController.GetScreenPosition();
				bool mouseButtonUp = cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton);
				bool mouseButtonDown = cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
				bool mouseButton = cursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
				bool mouseButtonUp2 = cursorController.GetMouseButtonUp(UICamera.MouseButton.RightButton);
				bool mouseButtonDown2 = cursorController.GetMouseButtonDown(UICamera.MouseButton.RightButton);
				bool mouseButton2 = cursorController.GetMouseButton(UICamera.MouseButton.RightButton);
				if (!this.IsLocked && !this.isDragAndDrop)
				{
					if (this.isOver && base.ViewComponent.UiTransformIsHovered && base.ViewComponent.EventOnPress)
					{
						if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
						{
							bool wasReleased = guiactions.Submit.WasReleased;
							bool wasReleased2 = guiactions.HalfStack.WasReleased;
							bool wasPressed = guiactions.Inspect.WasPressed;
							bool wasReleased3 = guiactions.RightStick.WasReleased;
							if (this.SimpleClick && !this.StackLock)
							{
								if (wasReleased)
								{
									this.HandleMoveToPreferredLocation();
								}
								else if (wasPressed)
								{
									this.HandleItemInspect();
								}
							}
							else if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && !this.ItemStack.IsEmpty())
							{
								if (!this.StackLock)
								{
									if (wasReleased)
									{
										this.SwapItem();
									}
									else if (wasReleased2)
									{
										this.HandlePartialStackPickup();
									}
									else if (wasReleased3)
									{
										this.HandleMoveToPreferredLocation();
									}
									else if (wasPressed)
									{
										this.HandleItemInspect();
									}
								}
							}
							else if (!this.StackLock)
							{
								if (wasReleased)
								{
									this.HandleStackSwap();
								}
								else if (wasReleased2 && this.AllowDropping)
								{
									this.HandleDropOne();
								}
							}
						}
						else if (this.SimpleClick && !this.StackLock)
						{
							if (mouseButtonUp)
							{
								this.HandleMoveToPreferredLocation();
							}
						}
						else if (InputUtils.ShiftKeyPressed)
						{
							if (!this.StackLock && mouseButtonUp)
							{
								this.HandleMoveToPreferredLocation();
							}
						}
						else if (mouseButton || mouseButton2)
						{
							if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && !this.ItemStack.IsEmpty())
							{
								if (!this.lastClicked)
								{
									this.startMousePos = vector;
								}
								else if (Mathf.Abs((vector - this.startMousePos).magnitude) > (float)this.PickupSnapDistance && !this.StackLock)
								{
									if (mouseButton)
									{
										this.SwapItem();
									}
									else
									{
										this.HandlePartialStackPickup();
									}
								}
							}
							if (mouseButtonDown || mouseButtonDown2)
							{
								this.lastClicked = true;
							}
						}
						else if (mouseButtonUp)
						{
							if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty())
							{
								this.HandleItemInspect();
							}
							else if (this.lastClicked && !this.StackLock)
							{
								this.HandleStackSwap();
							}
						}
						else if (mouseButtonUp2)
						{
							if (this.lastClicked && !this.StackLock && this.AllowDropping)
							{
								this.HandleDropOne();
							}
						}
						else
						{
							this.lastClicked = false;
						}
					}
					else
					{
						this.lastClicked = false;
						if ((this.isOver || this.itemIconSprite.UiTransform.localScale != Vector3.one) && this.tweenScale.value != Vector3.one && !this.itemStack.IsEmpty())
						{
							this.tweenScale.from = Vector3.one * 1.5f;
							this.tweenScale.to = Vector3.one;
							this.tweenScale.enabled = true;
							this.tweenScale.duration = 0.5f;
						}
					}
				}
				else if (this.IsLocked && ((guiactions.LastInputType == BindingSourceType.DeviceBindingSource && guiactions.Submit.WasReleased) || (guiactions.LastInputType != BindingSourceType.DeviceBindingSource && guiactions.LeftClick.WasPressed)) && this.isOver)
				{
					XUiEvent_LockChangeEventHandler lockChangedEvent = this.LockChangedEvent;
					if (lockChangedEvent != null)
					{
						lockChangedEvent(XUiC_ItemStack.LockTypes.None, this);
					}
				}
			}
			this.updateBorderColor();
			if (this.flashLockTypeIcon != XUiC_ItemStack.flashLockTypes.None)
			{
				Color color = ((this.flashLockTypeIcon == XUiC_ItemStack.flashLockTypes.Allowed) ? this.modAllowedColor : this.modAlreadyEquippedColor);
				float num = Mathf.PingPong(Time.time, 0.5f);
				this.setLockTypeIconColor(Color.Lerp(Color.grey, color, num * 4f));
			}
			if (this.IsDirty)
			{
				this.IsDirty = false;
				this.updateLockTypeIcon();
				base.RefreshBindings();
				this.ResetTweenScale();
			}
			if (this.IsLocked && this.lockType != XUiC_ItemStack.LockTypes.None)
			{
				this.UpdateTimer(_dt);
			}
		}
	}

	// Token: 0x06007C05 RID: 31749 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007C06 RID: 31750 RVA: 0x002EFBE4 File Offset: 0x002EDDE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBorderColor()
	{
		if (this.IsDragAndDrop)
		{
			this.SelectionBorderColor = Color.clear;
			return;
		}
		if (base.IsSelected)
		{
			this.SelectionBorderColor = this.selectColor;
			return;
		}
		if (this.isOver)
		{
			this.SelectionBorderColor = this.highlightColor;
			return;
		}
		if (this.IsHolding)
		{
			this.SelectionBorderColor = this.holdingColor;
			return;
		}
		this.SelectionBorderColor = this.backgroundColor;
	}

	// Token: 0x06007C07 RID: 31751 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack _stack)
	{
		return true;
	}

	// Token: 0x06007C08 RID: 31752 RVA: 0x002EFC55 File Offset: 0x002EDE55
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemInspect()
	{
		if (!this.ItemStack.IsEmpty() && this.InfoWindow != null)
		{
			base.IsSelected = true;
			this.InfoWindow.SetMaxCountOnDirty = true;
			this.updateItemInfoWindow(this);
		}
		this.HandleClickComplete();
	}

	// Token: 0x06007C09 RID: 31753 RVA: 0x002EFC8C File Offset: 0x002EDE8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleStackSwap()
	{
		this.xui.PopupMenuWindow.Close();
		if (!this.AllowDropping)
		{
			this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
			this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
		}
		bool flag = false;
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		ItemClass itemClassOrMissing = currentStack.itemValue.ItemClassOrMissing;
		int num = 0;
		if (itemClassOrMissing != null)
		{
			num = ((this.OverrideStackCount == -1) ? itemClassOrMissing.MaxCount : Mathf.Min(itemClassOrMissing.MaxCount, this.OverrideStackCount));
			if (!currentStack.IsEmpty() && this.itemStack.IsEmpty() && num < currentStack.count)
			{
				flag = true;
			}
		}
		if (!flag && (this.itemStack.IsEmpty() || currentStack.IsEmpty()))
		{
			this.SwapItem();
			base.IsSelected = false;
		}
		else if (!flag && (!this.itemStack.itemValue.ItemClassOrMissing.CanStack() || !itemClassOrMissing.CanStack()))
		{
			this.SwapItem();
			base.IsSelected = false;
		}
		else if (currentStack.itemValue.type == this.itemStack.itemValue.type && !currentStack.itemValue.HasQuality && !this.itemStack.itemValue.HasQuality)
		{
			if (currentStack.count + this.itemStack.count > num)
			{
				int num2 = currentStack.count + this.itemStack.count - num;
				ItemStack itemStack = this.itemStack.Clone();
				itemStack.count = num;
				currentStack.count = num2;
				this.xui.DragAndDropWindow.SetCurrentStack(currentStack, true);
				this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
				this.ItemStack = itemStack;
			}
			else
			{
				ItemStack itemStack2 = this.itemStack.Clone();
				itemStack2.count += currentStack.count;
				this.ItemStack = itemStack2;
				this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
				this.PlayPlaceSound(null);
			}
			if (base.IsSelected)
			{
				this.updateItemInfoWindow(this);
			}
		}
		else if (flag)
		{
			int num3 = currentStack.count - num;
			ItemStack itemStack3 = currentStack.Clone();
			itemStack3.count = num;
			currentStack.count = num3;
			this.xui.DragAndDropWindow.SetCurrentStack(currentStack, true);
			this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
			this.ItemStack = itemStack3;
		}
		else
		{
			this.SwapItem();
			base.IsSelected = false;
		}
		this.HandleClickComplete();
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007C0A RID: 31754 RVA: 0x002EFF54 File Offset: 0x002EE154
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandlePartialStackPickup()
	{
		ItemStack itemStack = this.xui.DragAndDropWindow.CurrentStack;
		if (itemStack.IsEmpty() && !this.itemStack.IsEmpty())
		{
			int num = this.itemStack.count / 2;
			if (num > 0)
			{
				itemStack = this.itemStack.Clone();
				itemStack.count = num;
				if (this.AllowDropping)
				{
					ItemStack itemStack2 = this.itemStack.Clone();
					itemStack2.count -= num;
					this.ItemStack = itemStack2;
				}
				this.xui.DragAndDropWindow.SetCurrentStack(itemStack, true);
				this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
			}
		}
		if (base.IsSelected)
		{
			base.IsSelected = false;
		}
		this.HandleClickComplete();
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007C0B RID: 31755 RVA: 0x002F0044 File Offset: 0x002EE244
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleDropOne()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.CanMoveTo(this.StackLocation, -1))
		{
			return;
		}
		if (!currentStack.IsEmpty())
		{
			int num = 1;
			if (this.itemStack.IsEmpty())
			{
				ItemStack itemStack = currentStack.Clone();
				itemStack.count = num;
				currentStack.count -= num;
				this.xui.DragAndDropWindow.SetCurrentStack(currentStack, false);
				this.ItemStack = itemStack;
				this.PlayPlaceSound(null);
			}
			else if (currentStack.itemValue.type == this.itemStack.itemValue.type)
			{
				ItemClass itemClassOrMissing = currentStack.itemValue.ItemClassOrMissing;
				int num2 = ((this.OverrideStackCount == -1) ? itemClassOrMissing.MaxCount : Mathf.Min(itemClassOrMissing.MaxCount, this.OverrideStackCount));
				if (this.itemStack.count + 1 <= num2)
				{
					ItemStack itemStack2 = this.itemStack.Clone();
					ItemStack itemStack3 = itemStack2;
					int num3 = itemStack3.count;
					itemStack3.count = num3 + 1;
					ItemStack itemStack4 = currentStack;
					num3 = itemStack4.count;
					itemStack4.count = num3 - 1;
					this.ItemStack = itemStack2.Clone();
					this.xui.DragAndDropWindow.SetCurrentStack(currentStack, true);
					XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
					if (slotChangedEvent != null)
					{
						slotChangedEvent(this.SlotNumber, this.itemStack);
					}
					this.IsDirty = true;
				}
				this.PlayPlaceSound(null);
			}
			if (currentStack.count == 0)
			{
				this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
			}
		}
		base.IsSelected = false;
		this.HandleClickComplete();
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007C0C RID: 31756 RVA: 0x002F0204 File Offset: 0x002EE404
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClickComplete()
	{
		this.lastClicked = false;
		if (this.itemIconSprite.UiTransform.localScale.x <= 1f)
		{
			return;
		}
		if (this.itemStack.IsEmpty())
		{
			return;
		}
		this.tweenScale.from = Vector3.one * 1.5f;
		this.tweenScale.to = Vector3.one;
		this.tweenScale.enabled = true;
		this.tweenScale.duration = 0.5f;
	}

	// Token: 0x06007C0D RID: 31757 RVA: 0x002F028C File Offset: 0x002EE48C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!this.IsLocked)
		{
			if (_isOver)
			{
				if (this.InfoWindow != null && this.InfoWindow.ViewComponent.IsVisible)
				{
					this.InfoWindow.HoverEntry = this;
				}
				if (this.AllowIconGrow)
				{
					this.tweenScale.from = Vector3.one;
					this.tweenScale.to = Vector3.one * 1.5f;
					this.tweenScale.enabled = true;
					this.tweenScale.duration = 0.5f;
				}
			}
			else
			{
				if (this.InfoWindow != null && this.InfoWindow.ViewComponent.IsVisible)
				{
					this.InfoWindow.HoverEntry = null;
				}
				if (this.AllowIconGrow)
				{
					this.tweenScale.from = Vector3.one * 1.5f;
					this.tweenScale.to = Vector3.one;
					this.tweenScale.enabled = true;
					this.tweenScale.duration = 0.5f;
				}
			}
		}
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, !this.StackLock, true);
		base.OnHovered(_isOver);
	}

	// Token: 0x06007C0E RID: 31758 RVA: 0x002F03DC File Offset: 0x002EE5DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SwapItem()
	{
		this.xui.PopupMenuWindow.Close();
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.IsEmpty() && !this.CanSwap(currentStack))
		{
			return;
		}
		if (!currentStack.CanMoveTo(this.StackLocation, this.SlotNumber))
		{
			return;
		}
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.LootContainer && this.xui.DragAndDropWindow.PickUpType != XUiC_ItemStack.StackLocationTypes.LootContainer && !currentStack.IsEmpty() && !currentStack.itemValue.ItemClassOrMissing.CanPlaceInContainer())
		{
			Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "Quest Items cannot be placed in containers.", false, false, 0f);
			return;
		}
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.CollectorCatalysts)
		{
			currentStack.itemValue.ItemClassOrMissing.OnPlacedAsCatalyst(this.xui.PlayerInventory.Toolbelt.holdingItemData);
		}
		if (this.itemStack.IsEmpty())
		{
			this.PlayPlaceSound(currentStack);
		}
		this.xui.DragAndDropWindow.SetCurrentStack(this.itemStack.Clone(), true);
		this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt)
		{
			this.xui.DragAndDropWindow.CurrentStack.Deactivate();
		}
		this.ForceSetItemStack(currentStack.Clone());
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007C0F RID: 31759 RVA: 0x002F056C File Offset: 0x002EE76C
	public void HandleMoveToPreferredLocation()
	{
		this.xui.PopupMenuWindow.Close();
		if (this.ItemStack.IsEmpty())
		{
			return;
		}
		if (this.StackLock)
		{
			return;
		}
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt)
		{
			this.ItemStack.Deactivate();
		}
		int count = this.ItemStack.count;
		switch (this.StackLocation)
		{
		case XUiC_ItemStack.StackLocationTypes.Backpack:
		case XUiC_ItemStack.StackLocationTypes.ToolBelt:
		{
			XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
			bool flag = ((assembleItem != null) ? assembleItem.CurrentItem : null) != null;
			if (!flag)
			{
				XUiC_BagStorageWindowGroup xuiC_BagStorageWindowGroup = this.xui.FindWindowGroupByName(XUiC_BagStorageWindowGroup.ID) as XUiC_BagStorageWindowGroup;
				if (xuiC_BagStorageWindowGroup != null && xuiC_BagStorageWindowGroup.Bag != null)
				{
					string vehicleSlotType = this.ItemStack.itemValue.ItemClass.VehicleSlotType;
					ItemStack itemStack;
					if (vehicleSlotType != "" && this.xui.Vehicle.SetPart(this.xui, vehicleSlotType, this.ItemStack, out itemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = itemStack;
						this.HandleSlotChangeEvent();
						return;
					}
					XUiC_BagContainer childByType = xuiC_BagStorageWindowGroup.GetChildByType<XUiC_BagContainer>();
					if (childByType != null && childByType.Bag != null && childByType.Bag.CanStack(this.itemStack) && childByType.AddItem(this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
			}
			if (flag && this.ItemStack.itemValue.ItemClass is ItemClassModifier)
			{
				ItemStack itemStack2;
				if (this.xui.AssembleItem.AddPartToItem(this.ItemStack, out itemStack2))
				{
					this.PlayPlaceSound(null);
					this.ItemStack = itemStack2;
					this.HandleSlotChangeEvent();
					return;
				}
				Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
				return;
			}
			else
			{
				if (this.xui.PlayerEquipment != null && this.xui.PlayerEquipment.IsOpen && this.itemStack.itemValue.ItemClass.CanEquip())
				{
					this.PlayPlaceSound(null);
					this.ItemStack = this.xui.PlayerEquipment.EquipItem(this.ItemStack);
					this.HandleSlotChangeEvent();
					return;
				}
				if (this.xui.LootContainer != null)
				{
					XUiC_LootContainer childByType2 = this.xui.FindWindowGroupByName(XUiC_LootWindowGroup.ID).GetChildByType<XUiC_LootContainer>();
					if (XUiM_LootContainer.AddItem(this.ItemStack, this.xui))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						if (childByType2 != null)
						{
							childByType2.SetSlots(this.xui.LootContainer);
						}
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						if (childByType2 != null)
						{
							childByType2.SetSlots(this.xui.LootContainer);
						}
						return;
					}
				}
				if (this.xui.CurrentWorkstationToolGrid != null && this.xui.CurrentWorkstationToolGrid.TryAddTool(this.itemClass, this.ItemStack))
				{
					this.PlayPlaceSound(null);
					this.ItemStack = ItemStack.Empty;
					this.HandleSlotChangeEvent();
					return;
				}
				if (this.xui.CurrentWorkstationFuelGrid != null && this.itemClass.FuelValue != null && this.itemClass.FuelValue.Value > 0)
				{
					if (this.xui.CurrentWorkstationFuelGrid.AddItem(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
				if (this.xui.CurrentWorkstationInputGrid != null && this.xui.CurrentWorkstationInputGrid.AcceptsMaterial(this.itemClass.MadeOfMaterial))
				{
					if (this.xui.CurrentWorkstationInputGrid.AddItem(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
				if (this.xui.CurrentWorkstationOutputGrid != null)
				{
					if (this.xui.CurrentWorkstationOutputGrid.AddItem(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
				if (this.xui.CurrentCollectorModGrid != null)
				{
					if (this.xui.CurrentCollectorModGrid.TryAddMod(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (this.xui.CurrentCollectorFuelGrid != null && this.xui.CurrentCollectorFuelGrid.TryAddFuel(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (this.xui.currentCollectorCatalystGrid != null && this.xui.currentCollectorCatalystGrid.TryAddFuel(this.itemClass, this.ItemStack))
					{
						this.itemClass.OnPlacedAsCatalyst(this.xui.PlayerInventory.Toolbelt.holdingItemData);
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
				}
				if (this.xui.CurrentCombineGrid != null && this.xui.CurrentCombineGrid.TryAddItemToSlot(this.itemClass, this.ItemStack))
				{
					this.PlayPlaceSound(null);
					this.ItemStack = ItemStack.Empty;
					this.HandleSlotChangeEvent();
					return;
				}
				if (this.xui.CurrentPowerSourceSlots != null && this.xui.CurrentPowerSourceSlots.TryAddItemToSlot(this.itemClass, this.ItemStack))
				{
					this.PlayPlaceSound(null);
					this.ItemStack = ItemStack.Empty;
					this.HandleSlotChangeEvent();
					return;
				}
				if (this.xui.CurrentPowerAmmoSlots != null)
				{
					if (this.xui.CurrentPowerAmmoSlots.TryAddItemToSlot(this.itemClass, this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
				if (this.xui.Trader.TraderData != null && (this.StackLocation == XUiC_ItemStack.StackLocationTypes.Backpack || this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt))
				{
					this.HandleItemInspect();
					this.InfoWindow.SetMaxCountOnDirty = true;
					return;
				}
				if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.Backpack)
				{
					XUiController xuiController = this.xui.FindWindowGroupByName("toolbelt");
					XUiC_Toolbelt xuiC_Toolbelt = ((xuiController != null) ? xuiController.GetChildByType<XUiC_Toolbelt>() : null);
					if (xuiC_Toolbelt != null && xuiC_Toolbelt.AddItemToCurrentPage(this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
				else if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt)
				{
					if (this.xui.PlayerInventory.AddItemToBackpack(this.ItemStack))
					{
						this.PlayPlaceSound(null);
						this.ItemStack = ItemStack.Empty;
						this.HandleSlotChangeEvent();
						return;
					}
					if (count != this.ItemStack.count)
					{
						this.PlayPlaceSound(null);
						if (this.ItemStack.count == 0)
						{
							this.ItemStack = ItemStack.Empty;
						}
						this.HandleSlotChangeEvent();
						return;
					}
				}
			}
			break;
		}
		case XUiC_ItemStack.StackLocationTypes.LootContainer:
		case XUiC_ItemStack.StackLocationTypes.Workstation:
		case XUiC_ItemStack.StackLocationTypes.Merge:
		case XUiC_ItemStack.StackLocationTypes.CollectorCatalysts:
		case XUiC_ItemStack.StackLocationTypes.CollectorFuel:
		case XUiC_ItemStack.StackLocationTypes.Collector:
			if (this.xui.PlayerInventory.AddItem(this.ItemStack))
			{
				this.PlayPlaceSound(null);
				this.ItemStack = ItemStack.Empty;
				this.HandleSlotChangeEvent();
			}
			else if (count != this.ItemStack.count)
			{
				this.PlayPlaceSound(null);
				if (this.ItemStack.count == 0)
				{
					this.ItemStack = ItemStack.Empty;
				}
				this.HandleSlotChangeEvent();
				return;
			}
			break;
		case XUiC_ItemStack.StackLocationTypes.Creative:
		{
			ItemStack itemStack3 = this.itemStack.Clone();
			if (!this.xui.PlayerInventory.AddItem(itemStack3))
			{
				return;
			}
			this.PlayPlaceSound(null);
			break;
		}
		}
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007C10 RID: 31760 RVA: 0x002F0E80 File Offset: 0x002EF080
	public void PlayPlaceSound(ItemStack newStack = null)
	{
		string text;
		if (newStack != null)
		{
			text = ((newStack.itemValue.ItemClass == null) ? "" : newStack.itemValue.ItemClass.SoundPlace);
		}
		else
		{
			text = ((this.itemStack.itemValue.ItemClass == null) ? "" : this.itemStack.itemValue.ItemClass.SoundPlace);
		}
		if (text != "")
		{
			if (text != null)
			{
				Manager.PlayInsidePlayerHead(text, -1, 0f, false, false);
				return;
			}
		}
		else if (this.placeSound != null)
		{
			Manager.PlayXUiSound(this.placeSound, 0.75f);
		}
	}

	// Token: 0x06007C11 RID: 31761 RVA: 0x002F0F2C File Offset: 0x002EF12C
	public void PlayPickupSound(ItemStack newStack = null)
	{
		ItemStack itemStack = ((newStack != null) ? newStack : this.itemStack);
		string text = ((itemStack.IsEmpty() || itemStack.itemValue.ItemClass == null) ? "" : itemStack.itemValue.ItemClass.SoundPickup);
		if (text != "")
		{
			if (text != null)
			{
				Manager.PlayInsidePlayerHead(text, -1, 0f, false, false);
				return;
			}
		}
		else if (this.pickupSound != null)
		{
			Manager.PlayXUiSound(this.pickupSound, 0.75f);
		}
	}

	// Token: 0x06007C12 RID: 31762 RVA: 0x002F0FB0 File Offset: 0x002EF1B0
	public void UnlockStack()
	{
		this.lockType = XUiC_ItemStack.LockTypes.None;
		this.IsLocked = false;
		this.lockTime = 0f;
		this.lockSprite = "";
		this.setLockTypeIconColor(Color.white);
		this.RepairAmount = 0;
		this.timer.IsVisible = false;
		this.IsDirty = true;
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x002F1008 File Offset: 0x002EF208
	public void LockStack(XUiC_ItemStack.LockTypes _lockType, float _time, int _count, BaseItemActionEntry _itemActionEntry)
	{
		if (_lockType == XUiC_ItemStack.LockTypes.Crafting)
		{
			this.lockSprite = _itemActionEntry.IconName;
		}
		else if (_lockType == XUiC_ItemStack.LockTypes.Scrapping)
		{
			this.lockSprite = "ui_game_symbol_scrap";
		}
		else if (_lockType == XUiC_ItemStack.LockTypes.Burning)
		{
			this.lockSprite = "ui_game_symbol_campfire";
		}
		else if (_lockType == XUiC_ItemStack.LockTypes.Repairing)
		{
			this.lockSprite = "ui_game_symbol_hammer";
		}
		this.IsLocked = true;
		this.lockType = _lockType;
		if (_lockType == XUiC_ItemStack.LockTypes.Repairing)
		{
			this.RepairAmount = _count;
		}
		this.LockTime = _time;
		this.IsDirty = true;
	}

	// Token: 0x06007C14 RID: 31764 RVA: 0x002F1080 File Offset: 0x002EF280
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateLockTypeIcon()
	{
		if (this.IsLocked && this.lockType != XUiC_ItemStack.LockTypes.None)
		{
			return;
		}
		this.lockSprite = "";
		if (this.itemClass != null)
		{
			Color color = Color.clear;
			ItemClassBlock itemClassBlock = this.itemClass as ItemClassBlock;
			if (itemClassBlock != null)
			{
				itemClassBlock.GetBlock();
				if (!this.itemStack.itemValue.TextureFullArray.IsDefault)
				{
					this.lockSprite = "ui_game_symbol_paint_brush";
				}
			}
			this.QuestLock = this.itemClass.IsQuestItem && (this.StackLocation == XUiC_ItemStack.StackLocationTypes.Backpack || this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt);
			ItemClassModifier itemClassModifier = this.itemClass as ItemClassModifier;
			if (itemClassModifier != null)
			{
				this.lockSprite = "ui_game_symbol_assemble";
				if (itemClassModifier.HasAnyTags(ItemClassModifier.CosmeticModTypes))
				{
					this.lockSprite = "ui_game_symbol_paint_bucket";
				}
				if (this.xui.AssembleItem.CurrentItem != null)
				{
					if ((itemClassModifier.InstallableTags.IsEmpty || this.xui.AssembleItem.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.InstallableTags)) && !this.xui.AssembleItem.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.DisallowedTags))
					{
						if (this.StackLocation != XUiC_ItemStack.StackLocationTypes.Part)
						{
							int num = 0;
							for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.ModificationCount; i++)
							{
								ItemValue modification = this.xui.AssembleItem.CurrentItem.itemValue.GetModification(i);
								if (!modification.IsEmpty())
								{
									ItemClassModifier itemClassModifier2 = modification.ItemClass as ItemClassModifier;
									if (itemClassModifier2 != null && itemClassModifier2.ModifierTags.Test_AnySet(itemClassModifier.ModifierTags))
									{
										num++;
									}
								}
							}
							if (num >= itemClassModifier.MaxModsAllowed)
							{
								this.flashLockTypeIcon = XUiC_ItemStack.flashLockTypes.AlreadyEquipped;
								return;
							}
						}
						this.flashLockTypeIcon = XUiC_ItemStack.flashLockTypes.Allowed;
					}
					else
					{
						color = Color.grey;
					}
				}
				else
				{
					color = Color.white;
				}
			}
			if (color.a > 0f)
			{
				this.setLockTypeIconColor(color);
				this.flashLockTypeIcon = XUiC_ItemStack.flashLockTypes.None;
			}
		}
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.Part)
		{
			this.lockSprite = "";
		}
	}

	// Token: 0x06007C15 RID: 31765 RVA: 0x002F12B0 File Offset: 0x002EF4B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLockTypeIconColor(Color _color)
	{
		if (this.lockTypeIcon == null)
		{
			return;
		}
		this.lockTypeIcon.Color = _color;
	}

	// Token: 0x06007C16 RID: 31766 RVA: 0x002F12C7 File Offset: 0x002EF4C7
	public void ForceRefreshItemStack()
	{
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x06007C17 RID: 31767 RVA: 0x002F12E5 File Offset: 0x002EF4E5
	public bool IsFavorite
	{
		get
		{
			return !this.itemStack.IsEmpty() && this.xui.playerUI.entityPlayer.favoriteCreativeStacks.Contains((ushort)this.itemStack.itemValue.type);
		}
	}

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x06007C18 RID: 31768 RVA: 0x002F1324 File Offset: 0x002EF524
	public virtual string ItemIcon
	{
		get
		{
			if (this.itemStack.IsEmpty())
			{
				return "";
			}
			ItemClass itemClassOrMissing = this.itemClassOrMissing;
			ItemClassBlock itemClassBlock = itemClassOrMissing as ItemClassBlock;
			Block block = ((itemClassBlock != null) ? itemClassBlock.GetBlock() : null);
			if (block == null)
			{
				return this.itemStack.itemValue.GetPropertyOverride(ItemClass.PropCustomIcon, itemClassOrMissing.GetIconName());
			}
			if (!block.SelectAlternates)
			{
				return this.itemStack.itemValue.GetPropertyOverride(ItemClass.PropCustomIcon, itemClassOrMissing.GetIconName());
			}
			return block.GetAltBlockValue(this.itemStack.itemValue.Meta).Block.GetIconName();
		}
	}

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x06007C19 RID: 31769 RVA: 0x002F13C4 File Offset: 0x002EF5C4
	[XuiXmlBinding("iconcolor")]
	public virtual Color ItemIconColor
	{
		get
		{
			ItemClass itemClassOrMissing = this.itemClassOrMissing;
			if (itemClassOrMissing == null)
			{
				return new Color(1f, 1f, 1f, 0f);
			}
			return itemClassOrMissing.GetIconTint(this.itemStack.itemValue);
		}
	}

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x06007C1A RID: 31770 RVA: 0x002F1406 File Offset: 0x002EF606
	// (set) Token: 0x06007C1B RID: 31771 RVA: 0x002F141D File Offset: 0x002EF61D
	public bool GreyedOut
	{
		get
		{
			return this.itemIconSprite.UIAtlas == "ItemIconAtlasGreyscale";
		}
		set
		{
			if (this.itemIconSprite != null)
			{
				this.itemIconSprite.UIAtlas = (value ? "ItemIconAtlasGreyscale" : "ItemIconAtlas");
			}
		}
	}

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x06007C1C RID: 31772 RVA: 0x002F1444 File Offset: 0x002EF644
	public string ItemNameText
	{
		get
		{
			if (this.itemStack.IsEmpty())
			{
				return "";
			}
			ItemClass itemClassOrMissing = this.itemClassOrMissing;
			string text = itemClassOrMissing.GetLocalizedItemName();
			if (itemClassOrMissing.IsBlock())
			{
				text = Block.list[this.itemStack.itemValue.type].GetLocalizedBlockName(this.itemStack.itemValue);
			}
			if (!this.PrefixId)
			{
				return text;
			}
			int itemOrBlockId = this.itemStack.itemValue.GetItemOrBlockId();
			return string.Format("{0}\n({1}) {2}", text, itemOrBlockId, itemClassOrMissing.Name);
		}
	}

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06007C1D RID: 31773 RVA: 0x002F14D3 File Offset: 0x002EF6D3
	[XuiXmlBinding("hasdurability")]
	public bool ShowDurability
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (!this.IsLocked || this.lockType == XUiC_ItemStack.LockTypes.None)
			{
				ItemClass itemClass = this.itemClass;
				return itemClass != null && itemClass.ShowQualityBar;
			}
			return false;
		}
	}

	// Token: 0x06007C1E RID: 31774 RVA: 0x002F14F8 File Offset: 0x002EF6F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1820482109U)
		{
			if (num <= 696352375U)
			{
				if (num <= 468119243U)
				{
					if (num != 402247192U)
					{
						if (num == 468119243U)
						{
							if (_bindingName == "haspermadurability")
							{
								if (this.ShowDurability)
								{
									ItemStack itemStack = this.itemStack;
									if (((itemStack != null) ? itemStack.itemValue : null) != null && EntityPlayerLocal.PermaDegrationOn)
									{
										_value = (this.itemStack.itemValue.MaxDurabilityModifier < 1f).ToString();
										return true;
									}
								}
								_value = "false";
								return true;
							}
						}
					}
					else if (_bindingName == "userlockedslot")
					{
						_value = this.UserLockedSlot.ToString();
						return true;
					}
				}
				else if (num != 684314633U)
				{
					if (num == 696352375U)
					{
						if (_bindingName == "isQuickSwap")
						{
							_value = this.isQuickSwap.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "stacklockicon")
				{
					if (this.stackLockType == XUiC_ItemStack.StackLockTypes.Quest)
					{
						_value = "ui_game_symbol_quest";
					}
					else if (this.attributeLock && this.itemStack.IsEmpty())
					{
						_value = "ui_game_symbol_pack_mule";
					}
					else
					{
						_value = "ui_game_symbol_lock";
					}
					return true;
				}
			}
			else if (num <= 1129104269U)
			{
				if (num != 847165955U)
				{
					if (num == 1129104269U)
					{
						if (_bindingName == "showicon")
						{
							_value = (this.ItemIcon != "").ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "itemtypeicon")
				{
					_value = XUiBindingHelper.GetItemTypeIconValue(this.xui, this.itemStack);
					return true;
				}
			}
			else if (num != 1388578781U)
			{
				if (num == 1820482109U)
				{
					if (_bindingName == "isfavorite")
					{
						_value = (this.ShowFavorites && this.IsFavorite).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "hasitemtypeicon")
			{
				_value = "false";
				if (string.IsNullOrEmpty(this.lockSprite))
				{
					_value = XUiBindingHelper.GetHasItemTypeIconValue(this.itemStack);
				}
				return true;
			}
		}
		else if (num <= 2733860383U)
		{
			if (num <= 2648037987U)
			{
				if (num != 2412344255U)
				{
					if (num == 2648037987U)
					{
						if (_bindingName == "islocked")
						{
							_value = this.isLocked.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "isassemblelocked")
				{
					_value = ((this.stackLockType != XUiC_ItemStack.StackLockTypes.None && this.stackLockType != XUiC_ItemStack.StackLockTypes.Hidden) || (this.attributeLock && this.itemStack.IsEmpty())).ToString();
					return true;
				}
			}
			else if (num != 2705680661U)
			{
				if (num == 2733860383U)
				{
					if (_bindingName == "locktypeicon")
					{
						_value = this.lockSprite ?? "";
						return true;
					}
				}
			}
			else if (_bindingName == "itemcount")
			{
				_value = "";
				if (!this.itemStack.IsEmpty())
				{
					if (this.ShowDurability)
					{
						_value = ((this.itemStack.itemValue.Quality > 0) ? this.itemcountFormatter.Format((int)this.itemStack.itemValue.Quality) : (this.itemStack.itemValue.IsMod ? "*" : ""));
					}
					else
					{
						_value = ((this.itemClassOrMissing.MaxCount == 1) ? "" : this.itemcountFormatter.Format(this.itemStack.count));
					}
				}
				return true;
			}
		}
		else if (num <= 3708628627U)
		{
			if (num != 2944858628U)
			{
				if (num == 3708628627U)
				{
					if (_bindingName == "itemicon")
					{
						_value = this.ItemIcon;
						return true;
					}
				}
			}
			else if (_bindingName == "hasdurability")
			{
				_value = this.ShowDurability.ToString();
				return true;
			}
		}
		else if (num != 3741212336U)
		{
			if (num == 4046184996U)
			{
				if (_bindingName == "ishovered")
				{
					_value = this.isOver.ToString();
					return true;
				}
			}
		}
		else if (_bindingName == "tooltip")
		{
			_value = this.ItemNameText;
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007C1F RID: 31775 RVA: 0x002F19BB File Offset: 0x002EFBBB
	[XuiXmlBinding("ishovered")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIshovered()
	{
		return this.isOver;
	}

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06007C20 RID: 31776 RVA: 0x002F19C3 File Offset: 0x002EFBC3
	[XuiXmlBinding("stacklockcolor")]
	public Color StackLockColor
	{
		get
		{
			if (this.attributeLock && this.itemStack.IsEmpty())
			{
				return new Color32(200, 200, 200, 64);
			}
			return Color.white;
		}
	}

	// Token: 0x06007C21 RID: 31777 RVA: 0x002F19FB File Offset: 0x002EFBFB
	[XuiXmlBinding("durabilitycolor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color bindingDurabilityColor()
	{
		ItemStack itemStack = this.itemStack;
		return QualityInfo.GetQualityColor((int)((itemStack != null) ? itemStack.itemValue.Quality : 0));
	}

	// Token: 0x06007C22 RID: 31778 RVA: 0x002F1A1C File Offset: 0x002EFC1C
	[XuiXmlBinding("durabilityfill")]
	[PublicizedFrom(EAccessModifier.Private)]
	public float bindingDurabilityFill()
	{
		ItemStack itemStack = this.itemStack;
		if (((itemStack != null) ? itemStack.itemValue : null) == null)
		{
			return 0f;
		}
		if (this.itemStack.itemValue.MaxUseTimes != 0)
		{
			return ((float)this.itemStack.itemValue.MaxUseTimes - this.itemStack.itemValue.UseTimes) / (float)this.itemStack.itemValue.MaxUseTimesUI;
		}
		return 1f;
	}

	// Token: 0x06007C23 RID: 31779 RVA: 0x002F1A8F File Offset: 0x002EFC8F
	[XuiXmlBinding("removeddurabilityfill")]
	[PublicizedFrom(EAccessModifier.Private)]
	public float bindingRemovedDurabilityFill()
	{
		ItemStack itemStack = this.itemStack;
		if (((itemStack != null) ? itemStack.itemValue : null) != null)
		{
			return this.itemStack.itemValue.MaxDurabilityModifier;
		}
		return 1f;
	}

	// Token: 0x06007C24 RID: 31780 RVA: 0x002F1ABB File Offset: 0x002EFCBB
	[XuiXmlBinding("itemtypeicontint")]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color bindingItemTypeIconTint()
	{
		return XUiBindingHelper.GetItemTypeIconTintColor(this.xui, this.itemStack);
	}

	// Token: 0x06007C25 RID: 31781 RVA: 0x002F1AD3 File Offset: 0x002EFCD3
	[XuiXmlBinding("selectionbordercolor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color bindingSelectionBorderColor()
	{
		return this.SelectionBorderColor;
	}

	// Token: 0x06007C26 RID: 31782 RVA: 0x002F1AE0 File Offset: 0x002EFCE0
	[XuiXmlBinding("backgroundcolor")]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color bindingBackgroundColor()
	{
		return this.AttributeLock ? this.attributeLockColor : this.backgroundColor;
	}

	// Token: 0x06007C27 RID: 31783 RVA: 0x002F1B00 File Offset: 0x002EFD00
	public override bool ParseAttribute(string _name, string _value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
		if (num <= 783618599U)
		{
			if (num <= 551907657U)
			{
				if (num <= 448973864U)
				{
					if (num != 310027284U)
					{
						if (num == 448973864U)
						{
							if (_name == "mod_already_equipped_color")
							{
								this.modAlreadyEquippedColor = StringParsers.ParseColor32(_value);
								return true;
							}
						}
					}
					else if (_name == "press_color")
					{
						this.pressColor = StringParsers.ParseColor32(_value);
						return true;
					}
				}
				else if (num != 505492686U)
				{
					if (num == 551907657U)
					{
						if (_name == "background_color")
						{
							this.backgroundColor = StringParsers.ParseColor32(_value);
							return true;
						}
					}
				}
				else if (_name == "show_favorites")
				{
					this.ShowFavorites = StringParsers.ParseBool(_value, 0, -1, true);
					return true;
				}
			}
			else if (num <= 589168786U)
			{
				if (num != 587922239U)
				{
					if (num == 589168786U)
					{
						if (_name == "final_pressed_color")
						{
							this.finalPressedColor = StringParsers.ParseColor32(_value);
							return true;
						}
					}
				}
				else if (_name == "highlight_color")
				{
					this.highlightColor = StringParsers.ParseColor32(_value);
					return true;
				}
			}
			else if (num != 605893325U)
			{
				if (num == 783618599U)
				{
					if (_name == "hover_icon_grow")
					{
						this.HoverIconGrow = StringParsers.ParseFloat(_value, 0, -1, NumberStyles.Any);
						return true;
					}
				}
			}
			else if (_name == "pickup_sound")
			{
				this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
				{
					this.pickupSound = _o;
				});
				return true;
			}
		}
		else if (num <= 1808090607U)
		{
			if (num <= 933221862U)
			{
				if (num != 785383727U)
				{
					if (num == 933221862U)
					{
						if (_name == "allow_dropping")
						{
							this.AllowDropping = StringParsers.ParseBool(_value, 0, -1, true);
							return true;
						}
					}
				}
				else if (_name == "prefix_id")
				{
					this.PrefixId = StringParsers.ParseBool(_value, 0, -1, true);
					return true;
				}
			}
			else if (num != 1079211868U)
			{
				if (num == 1808090607U)
				{
					if (_name == "attribute_lock_color")
					{
						this.attributeLockColor = StringParsers.ParseColor32(_value);
						return true;
					}
				}
			}
			else if (_name == "mod_allowed_color")
			{
				this.modAllowedColor = StringParsers.ParseColor32(_value);
				return true;
			}
		}
		else if (num <= 3096841498U)
		{
			if (num != 2540527193U)
			{
				if (num == 3096841498U)
				{
					if (_name == "override_stack_count")
					{
						this.OverrideStackCount = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
						return true;
					}
				}
			}
			else if (_name == "select_color")
			{
				this.selectColor = StringParsers.ParseColor32(_value);
				return true;
			}
		}
		else if (num != 3396886192U)
		{
			if (num != 3919060864U)
			{
				if (num == 3936377800U)
				{
					if (_name == "pickup_snap_distance")
					{
						this.PickupSnapDistance = int.Parse(_value);
						return true;
					}
				}
			}
			else if (_name == "place_sound")
			{
				this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
				{
					this.placeSound = _o;
				});
				return true;
			}
		}
		else if (_name == "holding_color")
		{
			this.holdingColor = StringParsers.ParseColor32(_value);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007C28 RID: 31784 RVA: 0x002F1EE4 File Offset: 0x002F00E4
	public static bool IsStackLocationFromPlayer(XUiC_ItemStack.StackLocationTypes? location)
	{
		if (location != null)
		{
			XUiC_ItemStack.StackLocationTypes? stackLocationTypes = location;
			XUiC_ItemStack.StackLocationTypes stackLocationTypes2 = XUiC_ItemStack.StackLocationTypes.Backpack;
			if (!((stackLocationTypes.GetValueOrDefault() == stackLocationTypes2) & (stackLocationTypes != null)))
			{
				stackLocationTypes = location;
				stackLocationTypes2 = XUiC_ItemStack.StackLocationTypes.ToolBelt;
				if (!((stackLocationTypes.GetValueOrDefault() == stackLocationTypes2) & (stackLocationTypes != null)))
				{
					stackLocationTypes = location;
					stackLocationTypes2 = XUiC_ItemStack.StackLocationTypes.Equipment;
					return (stackLocationTypes.GetValueOrDefault() == stackLocationTypes2) & (stackLocationTypes != null);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06007C2A RID: 31786 RVA: 0x002F20A8 File Offset: 0x002F02A8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|139_0(int _textureId)
	{
		if (_textureId == 0)
		{
			this.backgroundTexture.Texture = null;
			return;
		}
		MeshDescription meshDescription = MeshDescription.meshes[0];
		UVRectTiling uvrectTiling = meshDescription.textureAtlas.uvMapping[_textureId];
		this.backgroundTexture.Texture = meshDescription.textureAtlas.diffuseTexture;
		if (meshDescription.bTextureArray)
		{
			this.backgroundTexture.Material.SetTexture("_BumpMap", meshDescription.textureAtlas.normalTexture);
			this.backgroundTexture.Material.SetFloat("_Index", (float)uvrectTiling.index);
			this.backgroundTexture.Material.SetFloat("_Size", (float)uvrectTiling.blockW);
		}
		else
		{
			this.backgroundTexture.UVRect = uvrectTiling.uv;
		}
		this.backgroundTexture.ForceTextureUpdate();
	}

	// Token: 0x04005BCC RID: 23500
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005BCD RID: 23501
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005BCE RID: 23502
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x04005BCF RID: 23503
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x04005BD0 RID: 23504
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005BD1 RID: 23505
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005BD2 RID: 23506
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x04005BD3 RID: 23507
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.LockTypes lockType;

	// Token: 0x04005BD4 RID: 23508
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockSprite;

	// Token: 0x04005BD5 RID: 23509
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lockTime;

	// Token: 0x04005BD6 RID: 23510
	public int TimeInterval = 5;

	// Token: 0x04005BD7 RID: 23511
	public int OverrideStackCount = -1;

	// Token: 0x04005BD8 RID: 23512
	public bool _isQuickSwap;

	// Token: 0x04005BD9 RID: 23513
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005BDA RID: 23514
	[PublicizedFrom(EAccessModifier.Private)]
	public bool matchesSearch = true;

	// Token: 0x04005BDD RID: 23517
	[PublicizedFrom(EAccessModifier.Protected)]
	public int PickupSnapDistance = 4;

	// Token: 0x04005BDE RID: 23518
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005BDF RID: 23519
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005BE0 RID: 23520
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005BE1 RID: 23521
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x04005BE2 RID: 23522
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 attributeLockColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x04005BE3 RID: 23523
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAllowedColor = Color.green;

	// Token: 0x04005BE4 RID: 23524
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAlreadyEquippedColor = Color.yellow;

	// Token: 0x04005BE5 RID: 23525
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x04005BE6 RID: 23526
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005BE7 RID: 23527
	public XUiV_Label timer;

	// Token: 0x04005BE8 RID: 23528
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005BEE RID: 23534
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.flashLockTypes flashLockTypeIcon;

	// Token: 0x04005BEF RID: 23535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.StackLockTypes stackLockType;

	// Token: 0x04005BF0 RID: 23536
	[PublicizedFrom(EAccessModifier.Private)]
	public bool attributeLock;

	// Token: 0x04005BF1 RID: 23537
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x04005BF3 RID: 23539
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04005BFA RID: 23546
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockedSlot;

	// Token: 0x04005BFB RID: 23547
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentInterval = -1;

	// Token: 0x04005BFC RID: 23548
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x04005BFD RID: 23549
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x04005BFE RID: 23550
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdate = new ProfilerMarker("XCItemStack");

	// Token: 0x04005BFF RID: 23551
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdateBase = new ProfilerMarker("XCItemStack.Base");

	// Token: 0x04005C00 RID: 23552
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x02000F7A RID: 3962
	[PublicizedFrom(EAccessModifier.Private)]
	public enum flashLockTypes
	{
		// Token: 0x04005C02 RID: 23554
		None,
		// Token: 0x04005C03 RID: 23555
		Allowed,
		// Token: 0x04005C04 RID: 23556
		AlreadyEquipped
	}

	// Token: 0x02000F7B RID: 3963
	public enum LockTypes
	{
		// Token: 0x04005C06 RID: 23558
		None,
		// Token: 0x04005C07 RID: 23559
		Shell,
		// Token: 0x04005C08 RID: 23560
		Crafting,
		// Token: 0x04005C09 RID: 23561
		Repairing,
		// Token: 0x04005C0A RID: 23562
		Scrapping,
		// Token: 0x04005C0B RID: 23563
		Burning
	}

	// Token: 0x02000F7C RID: 3964
	public enum StackLockTypes
	{
		// Token: 0x04005C0D RID: 23565
		None,
		// Token: 0x04005C0E RID: 23566
		Assemble,
		// Token: 0x04005C0F RID: 23567
		Quest,
		// Token: 0x04005C10 RID: 23568
		Tool,
		// Token: 0x04005C11 RID: 23569
		Hidden
	}

	// Token: 0x02000F7D RID: 3965
	public enum StackLocationTypes
	{
		// Token: 0x04005C13 RID: 23571
		None,
		// Token: 0x04005C14 RID: 23572
		Backpack,
		// Token: 0x04005C15 RID: 23573
		ToolBelt,
		// Token: 0x04005C16 RID: 23574
		LootContainer,
		// Token: 0x04005C17 RID: 23575
		Equipment,
		// Token: 0x04005C18 RID: 23576
		Creative,
		// Token: 0x04005C19 RID: 23577
		Vehicle,
		// Token: 0x04005C1A RID: 23578
		Workstation,
		// Token: 0x04005C1B RID: 23579
		Merge,
		// Token: 0x04005C1C RID: 23580
		Cosmetics,
		// Token: 0x04005C1D RID: 23581
		Part,
		// Token: 0x04005C1E RID: 23582
		CollectorCatalysts,
		// Token: 0x04005C1F RID: 23583
		CollectorFuel,
		// Token: 0x04005C20 RID: 23584
		Collector
	}
}
