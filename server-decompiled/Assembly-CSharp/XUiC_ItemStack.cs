using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F44 RID: 3908
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ItemStack : XUiC_SelectableEntry
{
	// Token: 0x17000E4D RID: 3661
	// (get) Token: 0x06007AC4 RID: 31428 RVA: 0x002E60E2 File Offset: 0x002E42E2
	// (set) Token: 0x06007AC5 RID: 31429 RVA: 0x002E60EA File Offset: 0x002E42EA
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

	// Token: 0x17000E4E RID: 3662
	// (get) Token: 0x06007AC6 RID: 31430 RVA: 0x002E6104 File Offset: 0x002E4304
	// (set) Token: 0x06007AC7 RID: 31431 RVA: 0x002E610C File Offset: 0x002E430C
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

	// Token: 0x17000E4F RID: 3663
	// (get) Token: 0x06007AC8 RID: 31432 RVA: 0x002E612A File Offset: 0x002E432A
	// (set) Token: 0x06007AC9 RID: 31433 RVA: 0x002E6132 File Offset: 0x002E4332
	public int SlotNumber { get; set; }

	// Token: 0x17000E50 RID: 3664
	// (get) Token: 0x06007ACA RID: 31434 RVA: 0x002E613B File Offset: 0x002E433B
	// (set) Token: 0x06007ACB RID: 31435 RVA: 0x002E6143 File Offset: 0x002E4343
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x06007ACC RID: 31436 RVA: 0x002E614C File Offset: 0x002E434C
	// (remove) Token: 0x06007ACD RID: 31437 RVA: 0x002E6184 File Offset: 0x002E4384
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x06007ACE RID: 31438 RVA: 0x002E61BC File Offset: 0x002E43BC
	// (remove) Token: 0x06007ACF RID: 31439 RVA: 0x002E61F4 File Offset: 0x002E43F4
	public event XUiEvent_ToolLockChangeEventHandler ToolLockChangedEvent;

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x06007AD0 RID: 31440 RVA: 0x002E622C File Offset: 0x002E442C
	// (remove) Token: 0x06007AD1 RID: 31441 RVA: 0x002E6264 File Offset: 0x002E4464
	public event XUiEvent_LockChangeEventHandler LockChangedEvent;

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x06007AD2 RID: 31442 RVA: 0x002E629C File Offset: 0x002E449C
	// (remove) Token: 0x06007AD3 RID: 31443 RVA: 0x002E62D4 File Offset: 0x002E44D4
	public event XUiEvent_TimeIntervalElapsedEventHandler TimeIntervalElapsedEvent;

	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x06007AD4 RID: 31444 RVA: 0x002E6309 File Offset: 0x002E4509
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

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06007AD5 RID: 31445 RVA: 0x002E6327 File Offset: 0x002E4527
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

	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06007AD6 RID: 31446 RVA: 0x002E6345 File Offset: 0x002E4545
	// (set) Token: 0x06007AD7 RID: 31447 RVA: 0x002E634D File Offset: 0x002E454D
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x06007AD8 RID: 31448 RVA: 0x002E6356 File Offset: 0x002E4556
	// (set) Token: 0x06007AD9 RID: 31449 RVA: 0x002E6361 File Offset: 0x002E4561
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

	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x06007ADA RID: 31450 RVA: 0x002E6377 File Offset: 0x002E4577
	// (set) Token: 0x06007ADB RID: 31451 RVA: 0x002E6382 File Offset: 0x002E4582
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

	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x06007ADC RID: 31452 RVA: 0x002E63A9 File Offset: 0x002E45A9
	// (set) Token: 0x06007ADD RID: 31453 RVA: 0x002E63B4 File Offset: 0x002E45B4
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

	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x06007ADE RID: 31454 RVA: 0x002E63E8 File Offset: 0x002E45E8
	// (set) Token: 0x06007ADF RID: 31455 RVA: 0x002E63F3 File Offset: 0x002E45F3
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

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x06007AE0 RID: 31456 RVA: 0x002E6409 File Offset: 0x002E4609
	// (set) Token: 0x06007AE1 RID: 31457 RVA: 0x002E6411 File Offset: 0x002E4611
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

	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x06007AE2 RID: 31458 RVA: 0x002E6421 File Offset: 0x002E4621
	public bool StackLock
	{
		get
		{
			return this.stackLockType > XUiC_ItemStack.StackLockTypes.None;
		}
	}

	// Token: 0x17000E5A RID: 3674
	// (get) Token: 0x06007AE3 RID: 31459 RVA: 0x002E642C File Offset: 0x002E462C
	// (set) Token: 0x06007AE4 RID: 31460 RVA: 0x002E6434 File Offset: 0x002E4634
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

	// Token: 0x17000E5B RID: 3675
	// (get) Token: 0x06007AE5 RID: 31461 RVA: 0x002E6459 File Offset: 0x002E4659
	// (set) Token: 0x06007AE6 RID: 31462 RVA: 0x002E6461 File Offset: 0x002E4661
	public bool IsHolding { get; set; }

	// Token: 0x17000E5C RID: 3676
	// (get) Token: 0x06007AE7 RID: 31463 RVA: 0x002E646A File Offset: 0x002E466A
	// (set) Token: 0x06007AE8 RID: 31464 RVA: 0x002E6472 File Offset: 0x002E4672
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

	// Token: 0x17000E5D RID: 3677
	// (get) Token: 0x06007AE9 RID: 31465 RVA: 0x002E648C File Offset: 0x002E468C
	// (set) Token: 0x06007AEA RID: 31466 RVA: 0x002E6494 File Offset: 0x002E4694
	public int RepairAmount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x06007AEB RID: 31467 RVA: 0x002E649D File Offset: 0x002E469D
	public bool AllowIconGrow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.itemClass != null;
		}
	}

	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x06007AEC RID: 31468 RVA: 0x002E64A8 File Offset: 0x002E46A8
	// (set) Token: 0x06007AED RID: 31469 RVA: 0x002E64B0 File Offset: 0x002E46B0
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x06007AEE RID: 31470 RVA: 0x002E64B9 File Offset: 0x002E46B9
	// (set) Token: 0x06007AEF RID: 31471 RVA: 0x002E64C1 File Offset: 0x002E46C1
	public bool SimpleClick
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x06007AF0 RID: 31472 RVA: 0x002E64CA File Offset: 0x002E46CA
	// (set) Token: 0x06007AF1 RID: 31473 RVA: 0x002E64D2 File Offset: 0x002E46D2
	public bool AllowDropping
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = true;

	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x06007AF2 RID: 31474 RVA: 0x002E64DB File Offset: 0x002E46DB
	// (set) Token: 0x06007AF3 RID: 31475 RVA: 0x002E64E3 File Offset: 0x002E46E3
	public bool PrefixId
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06007AF4 RID: 31476 RVA: 0x002E64EC File Offset: 0x002E46EC
	// (set) Token: 0x06007AF5 RID: 31477 RVA: 0x002E64F4 File Offset: 0x002E46F4
	public bool ShowFavorites
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x06007AF6 RID: 31478 RVA: 0x002E64FD File Offset: 0x002E46FD
	// (set) Token: 0x06007AF7 RID: 31479 RVA: 0x002E6508 File Offset: 0x002E4708
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

	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x06007AF8 RID: 31480 RVA: 0x002E658E File Offset: 0x002E478E
	// (set) Token: 0x06007AF9 RID: 31481 RVA: 0x002E6596 File Offset: 0x002E4796
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

	// Token: 0x06007AFA RID: 31482 RVA: 0x002E65AF File Offset: 0x002E47AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack.Clone();
	}

	// Token: 0x06007AFB RID: 31483 RVA: 0x002E65BD File Offset: 0x002E47BD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateItemInfoWindow(XUiC_ItemStack _itemStack)
	{
		this.InfoWindow.SetItemStack(_itemStack, true);
	}

	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x06007AFC RID: 31484 RVA: 0x002E65CC File Offset: 0x002E47CC
	// (set) Token: 0x06007AFD RID: 31485 RVA: 0x002E65D4 File Offset: 0x002E47D4
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

	// Token: 0x06007AFE RID: 31486 RVA: 0x002E66B0 File Offset: 0x002E48B0
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
			this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|132_0(uiBackgroundTextureId);
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
				this.<updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|132_0(num3);
				return;
			}
			this.backgroundTexture.Texture = null;
			this.backgroundTexture.SetTextureDirty();
			return;
		}
	}

	// Token: 0x06007AFF RID: 31487 RVA: 0x002E67B8 File Offset: 0x002E49B8
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

	// Token: 0x06007B00 RID: 31488 RVA: 0x002E6800 File Offset: 0x002E4A00
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

	// Token: 0x06007B01 RID: 31489 RVA: 0x002E6830 File Offset: 0x002E4A30
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

	// Token: 0x06007B02 RID: 31490 RVA: 0x002E68B8 File Offset: 0x002E4AB8
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

	// Token: 0x06007B03 RID: 31491 RVA: 0x002E6926 File Offset: 0x002E4B26
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

	// Token: 0x06007B04 RID: 31492 RVA: 0x002E6960 File Offset: 0x002E4B60
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

	// Token: 0x06007B05 RID: 31493 RVA: 0x002E6A7C File Offset: 0x002E4C7C
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

	// Token: 0x06007B06 RID: 31494 RVA: 0x002E6BEC File Offset: 0x002E4DEC
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

	// Token: 0x06007B07 RID: 31495 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007B08 RID: 31496 RVA: 0x002E7114 File Offset: 0x002E5314
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

	// Token: 0x06007B09 RID: 31497 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack _stack)
	{
		return true;
	}

	// Token: 0x06007B0A RID: 31498 RVA: 0x002E7185 File Offset: 0x002E5385
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

	// Token: 0x06007B0B RID: 31499 RVA: 0x002E71BC File Offset: 0x002E53BC
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
			num = ((this.OverrideStackCount == -1) ? itemClassOrMissing.Stacknumber.Value : Mathf.Min(itemClassOrMissing.Stacknumber.Value, this.OverrideStackCount));
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

	// Token: 0x06007B0C RID: 31500 RVA: 0x002E7488 File Offset: 0x002E5688
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

	// Token: 0x06007B0D RID: 31501 RVA: 0x002E7578 File Offset: 0x002E5778
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleDropOne()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.CanMoveTo(this.StackLocation))
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
				int num2 = ((this.OverrideStackCount == -1) ? itemClassOrMissing.Stacknumber.Value : Mathf.Min(itemClassOrMissing.Stacknumber.Value, this.OverrideStackCount));
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

	// Token: 0x06007B0E RID: 31502 RVA: 0x002E7738 File Offset: 0x002E5938
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

	// Token: 0x06007B0F RID: 31503 RVA: 0x002E77C0 File Offset: 0x002E59C0
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

	// Token: 0x06007B10 RID: 31504 RVA: 0x002E7910 File Offset: 0x002E5B10
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SwapItem()
	{
		this.xui.PopupMenuWindow.Close();
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.IsEmpty() && !this.CanSwap(currentStack))
		{
			return;
		}
		if (!currentStack.CanMoveTo(this.StackLocation))
		{
			return;
		}
		if (this.StackLocation == XUiC_ItemStack.StackLocationTypes.LootContainer && this.xui.DragAndDropWindow.PickUpType != XUiC_ItemStack.StackLocationTypes.LootContainer && !currentStack.IsEmpty() && !currentStack.itemValue.ItemClassOrMissing.CanPlaceInContainer())
		{
			Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "Quest Items cannot be placed in containers.", false, false, 0f);
			return;
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

	// Token: 0x06007B11 RID: 31505 RVA: 0x002E7A68 File Offset: 0x002E5C68
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

	// Token: 0x06007B12 RID: 31506 RVA: 0x002E837C File Offset: 0x002E657C
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

	// Token: 0x06007B13 RID: 31507 RVA: 0x002E8428 File Offset: 0x002E6628
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

	// Token: 0x06007B14 RID: 31508 RVA: 0x002E84AC File Offset: 0x002E66AC
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

	// Token: 0x06007B15 RID: 31509 RVA: 0x002E8504 File Offset: 0x002E6704
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

	// Token: 0x06007B16 RID: 31510 RVA: 0x002E857C File Offset: 0x002E677C
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
			bool flag = this.itemStack.itemValue.HasAnyBoostedStats();
			if (flag)
			{
				this.lockSprite = "server_favorite";
				color = this.statBoostedColor;
			}
			if (this.itemStack.itemValue.HasMods())
			{
				this.lockSprite = "ui_game_symbol_modded";
				if (!flag)
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

	// Token: 0x06007B17 RID: 31511 RVA: 0x002E87FC File Offset: 0x002E69FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLockTypeIconColor(Color _color)
	{
		if (this.lockTypeIcon == null)
		{
			return;
		}
		this.lockTypeIcon.Color = _color;
	}

	// Token: 0x06007B18 RID: 31512 RVA: 0x002E8813 File Offset: 0x002E6A13
	public void ForceRefreshItemStack()
	{
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x06007B19 RID: 31513 RVA: 0x002E8831 File Offset: 0x002E6A31
	public bool IsFavorite
	{
		get
		{
			return !this.itemStack.IsEmpty() && this.xui.playerUI.entityPlayer.favoriteCreativeStacks.Contains((ushort)this.itemStack.itemValue.type);
		}
	}

	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x06007B1A RID: 31514 RVA: 0x002E8870 File Offset: 0x002E6A70
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

	// Token: 0x17000E69 RID: 3689
	// (get) Token: 0x06007B1B RID: 31515 RVA: 0x002E8910 File Offset: 0x002E6B10
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

	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x06007B1C RID: 31516 RVA: 0x002E8950 File Offset: 0x002E6B50
	// (set) Token: 0x06007B1D RID: 31517 RVA: 0x002E8967 File Offset: 0x002E6B67
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

	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x06007B1E RID: 31518 RVA: 0x002E898C File Offset: 0x002E6B8C
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

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x06007B1F RID: 31519 RVA: 0x002E8A1B File Offset: 0x002E6C1B
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

	// Token: 0x06007B20 RID: 31520 RVA: 0x002E8A40 File Offset: 0x002E6C40
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
						_value = "";
						if (!this.itemStack.IsEmpty())
						{
							ItemClass itemClassOrMissing = this.itemClassOrMissing;
							if (itemClassOrMissing != null)
							{
								if (itemClassOrMissing.IsBlock() && this.itemStack.itemValue.TextureFullArray.IsDefault)
								{
									_value = Block.list[this.itemStack.itemValue.type].ItemTypeIcon;
								}
								else
								{
									if (itemClassOrMissing.AltItemTypeIcon != null && itemClassOrMissing.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, itemClassOrMissing, this.itemStack.itemValue))
									{
										_value = itemClassOrMissing.AltItemTypeIcon;
										return true;
									}
									_value = itemClassOrMissing.ItemTypeIcon;
								}
							}
						}
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
					if (this.itemStack.IsEmpty() || !string.IsNullOrEmpty(this.lockSprite))
					{
						_value = "false";
					}
					else
					{
						ItemClass itemClassOrMissing2 = this.itemClassOrMissing;
						if (itemClassOrMissing2 == null)
						{
							_value = "false";
						}
						else
						{
							_value = (itemClassOrMissing2.IsBlock() ? (Block.list[this.itemStack.itemValue.type].ItemTypeIcon != "").ToString() : (itemClassOrMissing2.ItemTypeIcon != "").ToString());
						}
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
						_value = ((this.itemClassOrMissing.Stacknumber == 1) ? "" : this.itemcountFormatter.Format(this.itemStack.count));
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
				_value = "255,255,255,255";
				if (!this.itemStack.IsEmpty())
				{
					ItemClass itemClassOrMissing3 = this.itemClassOrMissing;
					if (itemClassOrMissing3 != null && itemClassOrMissing3.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, itemClassOrMissing3, this.itemStack.itemValue))
					{
						_value = this.altitemtypeiconcolorFormatter.Format(itemClassOrMissing3.AltItemTypeIconColor);
					}
				}
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

	// Token: 0x06007B21 RID: 31521 RVA: 0x002E92CC File Offset: 0x002E74CC
	[XuiXmlBinding("ishovered")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIshovered()
	{
		return this.isOver;
	}

	// Token: 0x06007B22 RID: 31522 RVA: 0x002E92D4 File Offset: 0x002E74D4
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

	// Token: 0x06007B23 RID: 31523 RVA: 0x002E96B8 File Offset: 0x002E78B8
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

	// Token: 0x06007B25 RID: 31525 RVA: 0x002E98E0 File Offset: 0x002E7AE0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <updateBackgroundTexture>g__setTextureForOpaqueMeshTextureId|132_0(int _textureId)
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

	// Token: 0x04005AB0 RID: 23216
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005AB1 RID: 23217
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005AB2 RID: 23218
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x04005AB3 RID: 23219
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x04005AB4 RID: 23220
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005AB5 RID: 23221
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005AB6 RID: 23222
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x04005AB7 RID: 23223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.LockTypes lockType;

	// Token: 0x04005AB8 RID: 23224
	[PublicizedFrom(EAccessModifier.Private)]
	public string lockSprite;

	// Token: 0x04005AB9 RID: 23225
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lockTime;

	// Token: 0x04005ABA RID: 23226
	public int TimeInterval = 5;

	// Token: 0x04005ABB RID: 23227
	public int OverrideStackCount = -1;

	// Token: 0x04005ABC RID: 23228
	public bool _isQuickSwap;

	// Token: 0x04005ABD RID: 23229
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectionBorderColor;

	// Token: 0x04005AC0 RID: 23232
	[PublicizedFrom(EAccessModifier.Protected)]
	public int PickupSnapDistance = 4;

	// Token: 0x04005AC1 RID: 23233
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005AC2 RID: 23234
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005AC3 RID: 23235
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005AC4 RID: 23236
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x04005AC5 RID: 23237
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 attributeLockColor = new Color32(48, 48, 48, byte.MaxValue);

	// Token: 0x04005AC6 RID: 23238
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAllowedColor = Color.green;

	// Token: 0x04005AC7 RID: 23239
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 modAlreadyEquippedColor = Color.yellow;

	// Token: 0x04005AC8 RID: 23240
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 statBoostedColor = new Color32(byte.MaxValue, 175, 11, byte.MaxValue);

	// Token: 0x04005AC9 RID: 23241
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x04005ACA RID: 23242
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x04005ACB RID: 23243
	public XUiV_Label timer;

	// Token: 0x04005ACC RID: 23244
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005AD2 RID: 23250
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.flashLockTypes flashLockTypeIcon;

	// Token: 0x04005AD3 RID: 23251
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack.StackLockTypes stackLockType;

	// Token: 0x04005AD4 RID: 23252
	[PublicizedFrom(EAccessModifier.Private)]
	public bool attributeLock;

	// Token: 0x04005AD5 RID: 23253
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragAndDrop;

	// Token: 0x04005AD7 RID: 23255
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04005ADE RID: 23262
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockedSlot;

	// Token: 0x04005ADF RID: 23263
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentInterval = -1;

	// Token: 0x04005AE0 RID: 23264
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x04005AE1 RID: 23265
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x04005AE2 RID: 23266
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdate = new ProfilerMarker("XCItemStack");

	// Token: 0x04005AE3 RID: 23267
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmIsUpdateBase = new ProfilerMarker("XCItemStack.Base");

	// Token: 0x04005AE4 RID: 23268
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly CachedStringFormatterXuiRgbaColor itemiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005AE5 RID: 23269
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemcountFormatter = new CachedStringFormatterInt();

	// Token: 0x04005AE6 RID: 23270
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005AE7 RID: 23271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removeddurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005AE8 RID: 23272
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005AE9 RID: 23273
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005AEA RID: 23274
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor backgroundcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005AEB RID: 23275
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor selectionbordercolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000F45 RID: 3909
	[PublicizedFrom(EAccessModifier.Private)]
	public enum flashLockTypes
	{
		// Token: 0x04005AED RID: 23277
		None,
		// Token: 0x04005AEE RID: 23278
		Allowed,
		// Token: 0x04005AEF RID: 23279
		AlreadyEquipped
	}

	// Token: 0x02000F46 RID: 3910
	public enum LockTypes
	{
		// Token: 0x04005AF1 RID: 23281
		None,
		// Token: 0x04005AF2 RID: 23282
		Shell,
		// Token: 0x04005AF3 RID: 23283
		Crafting,
		// Token: 0x04005AF4 RID: 23284
		Repairing,
		// Token: 0x04005AF5 RID: 23285
		Scrapping,
		// Token: 0x04005AF6 RID: 23286
		Burning
	}

	// Token: 0x02000F47 RID: 3911
	public enum StackLockTypes
	{
		// Token: 0x04005AF8 RID: 23288
		None,
		// Token: 0x04005AF9 RID: 23289
		Assemble,
		// Token: 0x04005AFA RID: 23290
		Quest,
		// Token: 0x04005AFB RID: 23291
		Tool,
		// Token: 0x04005AFC RID: 23292
		Hidden
	}

	// Token: 0x02000F48 RID: 3912
	public enum StackLocationTypes
	{
		// Token: 0x04005AFE RID: 23294
		Backpack,
		// Token: 0x04005AFF RID: 23295
		ToolBelt,
		// Token: 0x04005B00 RID: 23296
		LootContainer,
		// Token: 0x04005B01 RID: 23297
		Equipment,
		// Token: 0x04005B02 RID: 23298
		Creative,
		// Token: 0x04005B03 RID: 23299
		Vehicle,
		// Token: 0x04005B04 RID: 23300
		Workstation,
		// Token: 0x04005B05 RID: 23301
		Merge,
		// Token: 0x04005B06 RID: 23302
		Cosmetics,
		// Token: 0x04005B07 RID: 23303
		Part,
		// Token: 0x04005B08 RID: 23304
		CollectorCatalysts,
		// Token: 0x04005B09 RID: 23305
		CollectorFuel,
		// Token: 0x04005B0A RID: 23306
		Collector
	}
}
