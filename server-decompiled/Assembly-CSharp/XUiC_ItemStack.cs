using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F5B RID: 3931
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemStack : XUiC_SelectableEntry
{
	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x06007B7D RID: 31613 RVA: 0x002E9F6E File Offset: 0x002E816E
	// (set) Token: 0x06007B7E RID: 31614 RVA: 0x002E9F76 File Offset: 0x002E8176
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

	// Token: 0x17000E69 RID: 3689
	// (get) Token: 0x06007B7F RID: 31615 RVA: 0x002E9F90 File Offset: 0x002E8190
	// (set) Token: 0x06007B80 RID: 31616 RVA: 0x002E9F98 File Offset: 0x002E8198
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

	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x06007B81 RID: 31617 RVA: 0x002E9FB6 File Offset: 0x002E81B6
	// (set) Token: 0x06007B82 RID: 31618 RVA: 0x002E9FBE File Offset: 0x002E81BE
	public int SlotNumber { get; set; }

	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x06007B83 RID: 31619 RVA: 0x002E9FC7 File Offset: 0x002E81C7
	// (set) Token: 0x06007B84 RID: 31620 RVA: 0x002E9FCF File Offset: 0x002E81CF
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x06007B85 RID: 31621 RVA: 0x002E9FD8 File Offset: 0x002E81D8
	// (remove) Token: 0x06007B86 RID: 31622 RVA: 0x002EA010 File Offset: 0x002E8210
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x06007B87 RID: 31623 RVA: 0x002EA048 File Offset: 0x002E8248
	// (remove) Token: 0x06007B88 RID: 31624 RVA: 0x002EA080 File Offset: 0x002E8280
	public event XUiEvent_ToolLockChangeEventHandler ToolLockChangedEvent;

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x06007B89 RID: 31625 RVA: 0x002EA0B8 File Offset: 0x002E82B8
	// (remove) Token: 0x06007B8A RID: 31626 RVA: 0x002EA0F0 File Offset: 0x002E82F0
	public event XUiEvent_LockChangeEventHandler LockChangedEvent;

	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06007B8B RID: 31627 RVA: 0x002EA128 File Offset: 0x002E8328
	// (remove) Token: 0x06007B8C RID: 31628 RVA: 0x002EA160 File Offset: 0x002E8360
	public event XUiEvent_TimeIntervalElapsedEventHandler TimeIntervalElapsedEvent;

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x06007B8D RID: 31629 RVA: 0x002EA195 File Offset: 0x002E8395
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

	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x06007B8E RID: 31630 RVA: 0x002EA1B3 File Offset: 0x002E83B3
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

	// Token: 0x17000E6E RID: 3694
	// (get) Token: 0x06007B8F RID: 31631 RVA: 0x002EA1D1 File Offset: 0x002E83D1
	// (set) Token: 0x06007B90 RID: 31632 RVA: 0x002EA1D9 File Offset: 0x002E83D9
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E6F RID: 3695
	// (get) Token: 0x06007B91 RID: 31633 RVA: 0x002EA1E2 File Offset: 0x002E83E2
	// (set) Token: 0x06007B92 RID: 31634 RVA: 0x002EA1ED File Offset: 0x002E83ED
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

	// Token: 0x17000E70 RID: 3696
	// (get) Token: 0x06007B93 RID: 31635 RVA: 0x002EA203 File Offset: 0x002E8403
	// (set) Token: 0x06007B94 RID: 31636 RVA: 0x002EA20E File Offset: 0x002E840E
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

	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x06007B95 RID: 31637 RVA: 0x002EA235 File Offset: 0x002E8435
	// (set) Token: 0x06007B96 RID: 31638 RVA: 0x002EA240 File Offset: 0x002E8440
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

	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06007B97 RID: 31639 RVA: 0x002EA274 File Offset: 0x002E8474
	// (set) Token: 0x06007B98 RID: 31640 RVA: 0x002EA27F File Offset: 0x002E847F
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

	// Token: 0x17000E73 RID: 3699
	// (get) Token: 0x06007B99 RID: 31641 RVA: 0x002EA295 File Offset: 0x002E8495
	// (set) Token: 0x06007B9A RID: 31642 RVA: 0x002EA29D File Offset: 0x002E849D
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

	// Token: 0x17000E74 RID: 3700
	// (get) Token: 0x06007B9B RID: 31643 RVA: 0x002EA2AD File Offset: 0x002E84AD
	public bool StackLock
	{
		get
		{
			return this.stackLockType > XUiC_ItemStack.StackLockTypes.None;
		}
	}

	// Token: 0x17000E75 RID: 3701
	// (get) Token: 0x06007B9C RID: 31644 RVA: 0x002EA2B8 File Offset: 0x002E84B8
	// (set) Token: 0x06007B9D RID: 31645 RVA: 0x002EA2C0 File Offset: 0x002E84C0
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

	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x06007B9E RID: 31646 RVA: 0x002EA2E5 File Offset: 0x002E84E5
	// (set) Token: 0x06007B9F RID: 31647 RVA: 0x002EA2ED File Offset: 0x002E84ED
	public bool IsHolding { get; set; }

	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x06007BA0 RID: 31648 RVA: 0x002EA2F6 File Offset: 0x002E84F6
	// (set) Token: 0x06007BA1 RID: 31649 RVA: 0x002EA2FE File Offset: 0x002E84FE
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

	// Token: 0x17000E78 RID: 3704
	// (get) Token: 0x06007BA2 RID: 31650 RVA: 0x002EA318 File Offset: 0x002E8518
	// (set) Token: 0x06007BA3 RID: 31651 RVA: 0x002EA320 File Offset: 0x002E8520
	public int RepairAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E79 RID: 3705
	// (get) Token: 0x06007BA4 RID: 31652 RVA: 0x002EA329 File Offset: 0x002E8529
	public bool AllowIconGrow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.itemClass != null;
		}
	}

	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x06007BA5 RID: 31653 RVA: 0x002EA334 File Offset: 0x002E8534
	// (set) Token: 0x06007BA6 RID: 31654 RVA: 0x002EA33C File Offset: 0x002E853C
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x06007BA7 RID: 31655 RVA: 0x002EA345 File Offset: 0x002E8545
	// (set) Token: 0x06007BA8 RID: 31656 RVA: 0x002EA34D File Offset: 0x002E854D
	public bool SimpleClick
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x06007BA9 RID: 31657 RVA: 0x002EA356 File Offset: 0x002E8556
	// (set) Token: 0x06007BAA RID: 31658 RVA: 0x002EA35E File Offset: 0x002E855E
	public bool AllowDropping
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = true;

	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x06007BAB RID: 31659 RVA: 0x002EA367 File Offset: 0x002E8567
	// (set) Token: 0x06007BAC RID: 31660 RVA: 0x002EA36F File Offset: 0x002E856F
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x06007BAD RID: 31661 RVA: 0x002EA378 File Offset: 0x002E8578
	// (set) Token: 0x06007BAE RID: 31662 RVA: 0x002EA380 File Offset: 0x002E8580
	public bool ShowFavorites
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06007BAF RID: 31663 RVA: 0x002EA389 File Offset: 0x002E8589
	// (set) Token: 0x06007BB0 RID: 31664 RVA: 0x002EA394 File Offset: 0x002E8594
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

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06007BB1 RID: 31665 RVA: 0x002EA41A File Offset: 0x002E861A
	// (set) Token: 0x06007BB2 RID: 31666 RVA: 0x002EA422 File Offset: 0x002E8622
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

	// Token: 0x06007BB3 RID: 31667 RVA: 0x002EA43B File Offset: 0x002E863B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack.Clone();
	}

	// Token: 0x06007BB4 RID: 31668 RVA: 0x002EA449 File Offset: 0x002E8649
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateItemInfoWindow(XUiC_ItemStack _itemStack)
	{
		this.InfoWindow.SetItemStack(_itemStack, true);
	}

	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x06007BB5 RID: 31669 RVA: 0x002EA458 File Offset: 0x002E8658
	// (set) Token: 0x06007BB6 RID: 31670 RVA: 0x002EA460 File Offset: 0x002E8660
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

	// Token: 0x06007BB7 RID: 31671 RVA: 0x002EA53C File Offset: 0x002E873C
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
			this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|131_0(uiBackgroundTextureId);
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
				this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|131_0(num3);
				return;
			}
			this.backgroundTexture.Texture = null;
			this.backgroundTexture.SetTextureDirty();
			return;
		}
	}

	// Token: 0x06007BB8 RID: 31672 RVA: 0x002EA644 File Offset: 0x002E8844
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

	// Token: 0x06007BB9 RID: 31673 RVA: 0x002EA68C File Offset: 0x002E888C
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

	// Token: 0x06007BBA RID: 31674 RVA: 0x002EA6BC File Offset: 0x002E88BC
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

	// Token: 0x06007BBB RID: 31675 RVA: 0x002EA744 File Offset: 0x002E8944
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

	// Token: 0x06007BBC RID: 31676 RVA: 0x002EA7B2 File Offset: 0x002E89B2
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

	// Token: 0x06007BBD RID: 31677 RVA: 0x002EA7EC File Offset: 0x002E89EC
	public override void Init()
	{
		base.Init();
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

	// Token: 0x06007BBE RID: 31678 RVA: 0x002EA908 File Offset: 0x002E8B08
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

	// Token: 0x06007BBF RID: 31679 RVA: 0x002EAA78 File Offset: 0x002E8C78
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

	// Token: 0x06007BC0 RID: 31680 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007BC1 RID: 31681 RVA: 0x002EAFA0 File Offset: 0x002E91A0
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

	// Token: 0x06007BC2 RID: 31682 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack _stack)
	{
		return true;
	}

	// Token: 0x06007BC3 RID: 31683 RVA: 0x002EB011 File Offset: 0x002E9211
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

	// Token: 0x06007BC4 RID: 31684 RVA: 0x002EB048 File Offset: 0x002E9248
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleStackSwap()
	{
		this.xui.PopupMenuWindow.Close();
		if (!this.AllowDropping)
		{
			this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
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
				this.xui.DragAndDropWindow.CurrentStack = currentStack;
				this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
				this.ItemStack = itemStack;
			}
			else
			{
				ItemStack itemStack2 = this.itemStack.Clone();
				itemStack2.count += currentStack.count;
				this.ItemStack = itemStack2;
				this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
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
			this.xui.DragAndDropWindow.CurrentStack = currentStack;
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

	// Token: 0x06007BC5 RID: 31685 RVA: 0x002EB30C File Offset: 0x002E950C
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
				this.xui.DragAndDropWindow.CurrentStack = itemStack;
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

	// Token: 0x06007BC6 RID: 31686 RVA: 0x002EB3FC File Offset: 0x002E95FC
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
				this.xui.DragAndDropWindow.CurrentStack = currentStack;
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
					itemStack2.count++;
					currentStack.count--;
					this.ItemStack = itemStack2.Clone();
					this.xui.DragAndDropWindow.CurrentStack = currentStack;
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
				this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
			}
		}
		base.IsSelected = false;
		this.HandleClickComplete();
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007BC7 RID: 31687 RVA: 0x002EB5B4 File Offset: 0x002E97B4
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

	// Token: 0x06007BC8 RID: 31688 RVA: 0x002EB63C File Offset: 0x002E983C
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

	// Token: 0x06007BC9 RID: 31689 RVA: 0x002EB78C File Offset: 0x002E998C
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
		this.xui.DragAndDropWindow.CurrentStack = this.itemStack.Clone();
		this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.ToolBelt)
		{
			this.xui.DragAndDropWindow.CurrentStack.Deactivate();
		}
		this.ForceSetItemStack(currentStack.Clone());
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, true, true, true);
	}

	// Token: 0x06007BCA RID: 31690 RVA: 0x002EB918 File Offset: 0x002E9B18
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
							childByType2.SetSlots(this.xui.LootContainer, this.xui.LootContainer.items);
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
							childByType2.SetSlots(this.xui.LootContainer, this.xui.LootContainer.items);
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
					if (this.xui.PlayerInventory.AddItemToToolbelt(this.ItemStack))
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

	// Token: 0x06007BCB RID: 31691 RVA: 0x002EC22C File Offset: 0x002EA42C
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

	// Token: 0x06007BCC RID: 31692 RVA: 0x002EC2D8 File Offset: 0x002EA4D8
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

	// Token: 0x06007BCD RID: 31693 RVA: 0x002EC35C File Offset: 0x002EA55C
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

	// Token: 0x06007BCE RID: 31694 RVA: 0x002EC3B4 File Offset: 0x002EA5B4
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

	// Token: 0x06007BCF RID: 31695 RVA: 0x002EC42C File Offset: 0x002EA62C
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
							for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.Modifications.Length; i++)
							{
								ItemValue itemValue = this.xui.AssembleItem.CurrentItem.itemValue.Modifications[i];
								if (!itemValue.IsEmpty())
								{
									ItemClassModifier itemClassModifier2 = itemValue.ItemClass as ItemClassModifier;
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

	// Token: 0x06007BD0 RID: 31696 RVA: 0x002EC657 File Offset: 0x002EA857
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLockTypeIconColor(Color _color)
	{
		if (this.lockTypeIcon == null)
		{
			return;
		}
		this.lockTypeIcon.Color = _color;
	}

	// Token: 0x06007BD1 RID: 31697 RVA: 0x002EC66E File Offset: 0x002EA86E
	public void ForceRefreshItemStack()
	{
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x06007BD2 RID: 31698 RVA: 0x002EC68C File Offset: 0x002EA88C
	public bool IsFavorite
	{
		get
		{
			return !this.itemStack.IsEmpty() && this.xui.playerUI.entityPlayer.favoriteCreativeStacks.Contains((ushort)this.itemStack.itemValue.type);
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x06007BD3 RID: 31699 RVA: 0x002EC6C8 File Offset: 0x002EA8C8
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

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06007BD4 RID: 31700 RVA: 0x002EC768 File Offset: 0x002EA968
	public virtual string ItemIconColor
	{
		get
		{
			ItemClass itemClassOrMissing = this.itemClassOrMissing;
			if (itemClassOrMissing == null)
			{
				return "255,255,255,0";
			}
			Color32 color = itemClassOrMissing.GetIconTint(this.itemStack.itemValue);
			return this.itemiconcolorFormatter.Format(color);
		}
	}

	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06007BD5 RID: 31701 RVA: 0x002EC7A8 File Offset: 0x002EA9A8
	// (set) Token: 0x06007BD6 RID: 31702 RVA: 0x002EC7BF File Offset: 0x002EA9BF
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

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06007BD7 RID: 31703 RVA: 0x002EC7E4 File Offset: 0x002EA9E4
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

	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x06007BD8 RID: 31704 RVA: 0x002EC873 File Offset: 0x002EAA73
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

	// Token: 0x06007BD9 RID: 31705 RVA: 0x002EC898 File Offset: 0x002EAA98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1768227542U)
		{
			if (num <= 727013168U)
			{
				if (num <= 468119243U)
				{
					if (num != 143608997U)
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
					else if (_bindingName == "removeddurabilityfill")
					{
						ItemStack itemStack2 = this.itemStack;
						if (((itemStack2 != null) ? itemStack2.itemValue : null) == null)
						{
							_value = "1";
							return true;
						}
						_value = this.removeddurabilityFillFormatter.Format(this.itemStack.itemValue.MaxDurabilityModifier);
						return true;
					}
				}
				else if (num != 684314633U)
				{
					if (num != 696352375U)
					{
						if (num == 727013168U)
						{
							if (_bindingName == "backgroundcolor")
							{
								_value = this.backgroundcolorFormatter.Format(this.AttributeLock ? this.attributeLockColor : this.backgroundColor);
								return true;
							}
						}
					}
					else if (_bindingName == "isQuickSwap")
					{
						_value = this.isQuickSwap.ToString();
						return true;
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
			else if (num <= 1062608009U)
			{
				if (num != 770575247U)
				{
					if (num != 847165955U)
					{
						if (num == 1062608009U)
						{
							if (_bindingName == "durabilitycolor")
							{
								CachedStringFormatter<Color32> cachedStringFormatter = this.durabilitycolorFormatter;
								ItemStack itemStack3 = this.itemStack;
								_value = cachedStringFormatter.Format(QualityInfo.GetQualityColor((int)((itemStack3 != null) ? itemStack3.itemValue.Quality : 0)));
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
				else if (_bindingName == "stacklockcolor")
				{
					if (this.attributeLock && this.itemStack.IsEmpty())
					{
						_value = "200,200,200,64";
					}
					else
					{
						_value = "255,255,255,255";
					}
					return true;
				}
			}
			else if (num != 1129104269U)
			{
				if (num != 1388578781U)
				{
					if (num == 1768227542U)
					{
						if (_bindingName == "selectionbordercolor")
						{
							_value = this.selectionbordercolorFormatter.Format(this.SelectionBorderColor);
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
			else if (_bindingName == "showicon")
			{
				_value = (this.ItemIcon != "").ToString();
				return true;
			}
		}
		else if (num <= 2944858628U)
		{
			if (num <= 2648037987U)
			{
				if (num != 1820482109U)
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
				else if (_bindingName == "isfavorite")
				{
					_value = (this.ShowFavorites && this.IsFavorite).ToString();
					return true;
				}
			}
			else if (num != 2705680661U)
			{
				if (num != 2733860383U)
				{
					if (num == 2944858628U)
					{
						if (_bindingName == "hasdurability")
						{
							_value = this.ShowDurability.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "locktypeicon")
				{
					_value = this.lockSprite ?? "";
					return true;
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
		else if (num <= 3741212336U)
		{
			if (num != 3106195591U)
			{
				if (num != 3708628627U)
				{
					if (num == 3741212336U)
					{
						if (_bindingName == "tooltip")
						{
							_value = this.ItemNameText;
							return true;
						}
					}
				}
				else if (_bindingName == "itemicon")
				{
					_value = this.ItemIcon;
					return true;
				}
			}
			else if (_bindingName == "iconcolor")
			{
				_value = this.ItemIconColor;
				return true;
			}
		}
		else if (num != 4046184996U)
		{
			if (num != 4049247086U)
			{
				if (num == 4172540779U)
				{
					if (_bindingName == "durabilityfill")
					{
						ItemStack itemStack4 = this.itemStack;
						_value = ((((itemStack4 != null) ? itemStack4.itemValue : null) == null) ? "0.0" : ((this.itemStack.itemValue.MaxUseTimes == 0) ? "1" : this.durabilityFillFormatter.Format(((float)this.itemStack.itemValue.MaxUseTimes - this.itemStack.itemValue.UseTimes) / (float)this.itemStack.itemValue.MaxUseTimesUI)));
						return true;
					}
				}
			}
			else if (_bindingName == "itemtypeicontint")
			{
				_value = XUiBindingHelper.GetItemTypeIconTintValue(this.xui, this.itemStack, this.iconTypeIconTintFormatter);
				return true;
			}
		}
		else if (_bindingName == "ishovered")
		{
			_value = this.isOver.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007BDA RID: 31706 RVA: 0x002ECFC4 File Offset: 0x002EB1C4
	[XuiXmlBinding("ishovered")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIshovered()
	{
		return this.isOver;
	}

	// Token: 0x06007BDB RID: 31707 RVA: 0x002ECFCC File Offset: 0x002EB1CC
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

	// Token: 0x06007BDC RID: 31708 RVA: 0x002ED3B0 File Offset: 0x002EB5B0
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

	// Token: 0x06007BDE RID: 31710 RVA: 0x002ED5C4 File Offset: 0x002EB7C4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|131_0(int _textureId)
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
		this.backgroundTexture.SetTextureDirty();
	}

	// Token: 0x04005B63 RID: 23395
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005B64 RID: 23396
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005B65 RID: 23397
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x04005B66 RID: 23398
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x04005B67 RID: 23399
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005B68 RID: 23400
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005B69 RID: 23401
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x04005B6A RID: 23402
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.LockTypes lockType;

	// Token: 0x04005B6B RID: 23403
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockSprite;

	// Token: 0x04005B6C RID: 23404
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lockTime;

	// Token: 0x04005B6D RID: 23405
	public int TimeInterval = 5;

	// Token: 0x04005B6E RID: 23406
	public int OverrideStackCount = -1;

	// Token: 0x04005B6F RID: 23407
	public bool _isQuickSwap;

	// Token: 0x04005B70 RID: 23408
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005B73 RID: 23411
	[PublicizedFrom(EAccessModifier.Protected)]
	public int PickupSnapDistance = 4;

	// Token: 0x04005B74 RID: 23412
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005B75 RID: 23413
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005B76 RID: 23414
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005B77 RID: 23415
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x04005B78 RID: 23416
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 attributeLockColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x04005B79 RID: 23417
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAllowedColor = Color.green;

	// Token: 0x04005B7A RID: 23418
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAlreadyEquippedColor = Color.yellow;

	// Token: 0x04005B7B RID: 23419
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x04005B7C RID: 23420
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005B7D RID: 23421
	public XUiV_Label timer;

	// Token: 0x04005B7E RID: 23422
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005B84 RID: 23428
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.flashLockTypes flashLockTypeIcon;

	// Token: 0x04005B85 RID: 23429
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.StackLockTypes stackLockType;

	// Token: 0x04005B86 RID: 23430
	[PublicizedFrom(EAccessModifier.Private)]
	public bool attributeLock;

	// Token: 0x04005B87 RID: 23431
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x04005B89 RID: 23433
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04005B90 RID: 23440
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockedSlot;

	// Token: 0x04005B91 RID: 23441
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentInterval = -1;

	// Token: 0x04005B92 RID: 23442
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x04005B93 RID: 23443
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x04005B94 RID: 23444
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdate = new ProfilerMarker("XCItemStack");

	// Token: 0x04005B95 RID: 23445
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdateBase = new ProfilerMarker("XCItemStack.Base");

	// Token: 0x04005B96 RID: 23446
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly CachedStringFormatterXuiRgbaColor itemiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B97 RID: 23447
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005B98 RID: 23448
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005B99 RID: 23449
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removeddurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005B9A RID: 23450
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor iconTypeIconTintFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B9B RID: 23451
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B9C RID: 23452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B9D RID: 23453
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005B9E RID: 23454
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000F5C RID: 3932
	[PublicizedFrom(EAccessModifier.Private)]
	public enum flashLockTypes
	{
		// Token: 0x04005BA0 RID: 23456
		None,
		// Token: 0x04005BA1 RID: 23457
		Allowed,
		// Token: 0x04005BA2 RID: 23458
		AlreadyEquipped
	}

	// Token: 0x02000F5D RID: 3933
	public enum LockTypes
	{
		// Token: 0x04005BA4 RID: 23460
		None,
		// Token: 0x04005BA5 RID: 23461
		Shell,
		// Token: 0x04005BA6 RID: 23462
		Crafting,
		// Token: 0x04005BA7 RID: 23463
		Repairing,
		// Token: 0x04005BA8 RID: 23464
		Scrapping,
		// Token: 0x04005BA9 RID: 23465
		Burning
	}

	// Token: 0x02000F5E RID: 3934
	public enum StackLockTypes
	{
		// Token: 0x04005BAB RID: 23467
		None,
		// Token: 0x04005BAC RID: 23468
		Assemble,
		// Token: 0x04005BAD RID: 23469
		Quest,
		// Token: 0x04005BAE RID: 23470
		Tool,
		// Token: 0x04005BAF RID: 23471
		Hidden
	}

	// Token: 0x02000F5F RID: 3935
	public enum StackLocationTypes
	{
		// Token: 0x04005BB1 RID: 23473
		Backpack,
		// Token: 0x04005BB2 RID: 23474
		ToolBelt,
		// Token: 0x04005BB3 RID: 23475
		LootContainer,
		// Token: 0x04005BB4 RID: 23476
		Equipment,
		// Token: 0x04005BB5 RID: 23477
		Creative,
		// Token: 0x04005BB6 RID: 23478
		Vehicle,
		// Token: 0x04005BB7 RID: 23479
		Workstation,
		// Token: 0x04005BB8 RID: 23480
		Merge,
		// Token: 0x04005BB9 RID: 23481
		Cosmetics,
		// Token: 0x04005BBA RID: 23482
		Part,
		// Token: 0x04005BBB RID: 23483
		CollectorCatalysts,
		// Token: 0x04005BBC RID: 23484
		CollectorFuel,
		// Token: 0x04005BBD RID: 23485
		Collector
	}
}
