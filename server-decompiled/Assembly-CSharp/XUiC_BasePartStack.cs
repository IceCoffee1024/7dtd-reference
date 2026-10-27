using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EE4 RID: 3812
[global::UnityEngine.Scripting.Preserve]
public class XUiC_BasePartStack : XUiC_SelectableEntry
{
	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x0600768C RID: 30348 RVA: 0x002CB46D File Offset: 0x002C966D
	// (set) Token: 0x0600768D RID: 30349 RVA: 0x002CB475 File Offset: 0x002C9675
	public int SlotNumber { get; set; }

	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x0600768E RID: 30350 RVA: 0x002CB47E File Offset: 0x002C967E
	// (set) Token: 0x0600768F RID: 30351 RVA: 0x002CB486 File Offset: 0x002C9686
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x06007690 RID: 30352 RVA: 0x002CB490 File Offset: 0x002C9690
	// (remove) Token: 0x06007691 RID: 30353 RVA: 0x002CB4C8 File Offset: 0x002C96C8
	public event XUiEvent_SlotChangedEventHandler SlotChangingEvent;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06007692 RID: 30354 RVA: 0x002CB500 File Offset: 0x002C9700
	// (remove) Token: 0x06007693 RID: 30355 RVA: 0x002CB538 File Offset: 0x002C9738
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06007694 RID: 30356 RVA: 0x002CB56D File Offset: 0x002C976D
	// (set) Token: 0x06007695 RID: 30357 RVA: 0x002CB575 File Offset: 0x002C9775
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x06007696 RID: 30358 RVA: 0x002CB57E File Offset: 0x002C977E
	// (set) Token: 0x06007697 RID: 30359 RVA: 0x002CB586 File Offset: 0x002C9786
	public string SlotType
	{
		get
		{
			return this.slotType;
		}
		set
		{
			this.slotType = value;
			this.SetEmptySpriteName();
		}
	}

	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x06007698 RID: 30360 RVA: 0x002CB595 File Offset: 0x002C9795
	// (set) Token: 0x06007699 RID: 30361 RVA: 0x002CB5A0 File Offset: 0x002C97A0
	public ItemStack ItemStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			if (this.itemStack != value)
			{
				this.itemStack = value;
				this.IsDirty = true;
				if (this.itemStack.IsEmpty())
				{
					base.IsSelected = false;
				}
				if (base.IsSelected)
				{
					this.InfoWindow.SetItemStack(this, true);
				}
				if (this.SlotChangedEvent != null)
				{
					this.SlotChangedEvent(this.SlotNumber, this.itemStack);
				}
				this.itemClass = this.itemStack.itemValue.ItemClass;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x0600769A RID: 30362 RVA: 0x002CB628 File Offset: 0x002C9828
	public ItemClass ItemClass
	{
		get
		{
			return this.itemClass;
		}
	}

	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x0600769B RID: 30363 RVA: 0x002CB630 File Offset: 0x002C9830
	// (set) Token: 0x0600769C RID: 30364 RVA: 0x002CB638 File Offset: 0x002C9838
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x0600769D RID: 30365 RVA: 0x002CB641 File Offset: 0x002C9841
	public virtual string GetAtlas()
	{
		if (this.itemClass == null)
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x0600769E RID: 30366 RVA: 0x002CB656 File Offset: 0x002C9856
	public virtual string GetPartName()
	{
		if (this.itemClass == null)
		{
			return string.Format("[MISSING {0}]", this.SlotType);
		}
		return this.itemClass.GetLocalizedItemName();
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x002CB67C File Offset: 0x002C987C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2285454806U)
		{
			if (num <= 776984821U)
			{
				if (num != 289581667U)
				{
					if (num == 776984821U)
					{
						if (bindingName == "partquality")
						{
							value = ((this.itemClass != null && this.itemStack != null) ? this.qualityFormatter.Format((int)this.itemStack.itemValue.Quality) : "");
							return true;
						}
					}
				}
				else if (bindingName == "partatlas")
				{
					value = this.GetAtlas();
					return true;
				}
			}
			else if (num != 1419324424U)
			{
				if (num == 2285454806U)
				{
					if (bindingName == "partvisible")
					{
						value = ((this.itemClass != null) ? "true" : "false");
						return true;
					}
				}
			}
			else if (bindingName == "particoncolor")
			{
				if (this.itemClass == null)
				{
					value = "255, 255, 255, 178";
				}
				else
				{
					Color32 color = this.itemStack.itemValue.ItemClass.GetIconTint(this.itemStack.itemValue);
					value = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
				}
				return true;
			}
		}
		else if (num <= 2552573645U)
		{
			if (num != 2498838587U)
			{
				if (num == 2552573645U)
				{
					if (bindingName == "partfill")
					{
						value = ((this.itemStack.itemValue.MaxUseTimes == 0) ? "1" : this.partfillFormatter.Format(((float)this.itemStack.itemValue.MaxUseTimes - this.itemStack.itemValue.UseTimes) / (float)this.itemStack.itemValue.MaxUseTimes));
						return true;
					}
				}
			}
			else if (bindingName == "partcolor")
			{
				if (this.itemClass != null)
				{
					Color32 color2 = QualityInfo.GetQualityColor((int)this.itemStack.itemValue.Quality);
					value = this.partcolorFormatter.Format(color2);
				}
				else
				{
					value = "255, 255, 255, 0";
				}
				return true;
			}
		}
		else if (num != 2733906447U)
		{
			if (num != 3045999413U)
			{
				if (num == 3130438080U)
				{
					if (bindingName == "emptyvisible")
					{
						value = ((this.itemClass == null) ? "true" : "false");
						return true;
					}
				}
			}
			else if (bindingName == "particon")
			{
				if (this.itemClass == null)
				{
					value = this.emptySpriteName;
				}
				else
				{
					value = this.itemStack.itemValue.GetPropertyOverride("CustomIcon", (this.itemClass.CustomIcon != null) ? this.itemClass.CustomIcon.Value : this.itemClass.GetIconName());
				}
				return true;
			}
		}
		else if (bindingName == "partname")
		{
			value = this.GetPartName();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060076A0 RID: 30368 RVA: 0x002CB9CC File Offset: 0x002C9BCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.SetColor(isSelected ? this.selectColor : XUiC_BasePartStack.backgroundColor);
		((XUiV_Sprite)this.background.ViewComponent).SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
	}

	// Token: 0x060076A1 RID: 30369 RVA: 0x002CBA08 File Offset: 0x002C9C08
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetColor(Color32 color)
	{
		((XUiV_Sprite)this.background.ViewComponent).Color = color;
	}

	// Token: 0x060076A2 RID: 30370 RVA: 0x002CBA25 File Offset: 0x002C9C25
	public override void Init()
	{
		base.Init();
		this.itemIcon = base.GetChildById("itemIcon");
		this.background = base.GetChildById("background");
		base.RefreshBindings();
	}

	// Token: 0x060076A3 RID: 30371 RVA: 0x002CBA58 File Offset: 0x002C9C58
	public XUiC_BasePartStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x060076A4 RID: 30372 RVA: 0x002CBB1C File Offset: 0x002C9D1C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (base.WindowGroup.isShowing)
		{
			PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			Vector3 vector = cursorController.GetScreenPosition();
			bool mouseButtonUp = cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton);
			bool mouseButtonDown = cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
			bool mouseButton = cursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
			if (this.isOver && base.ViewComponent.UiTransformIsHovered && base.ViewComponent.EventOnPress)
			{
				if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
				{
					if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty())
					{
						if (!this.ItemStack.IsEmpty())
						{
							if (guiactions.Submit.WasReleased && this.CanRemove())
							{
								this.SwapItem();
								this.currentColor = XUiC_BasePartStack.backgroundColor;
								if (this.itemStack.IsEmpty())
								{
									((XUiV_Sprite)this.background.ViewComponent).Color = XUiC_BasePartStack.backgroundColor;
									return;
								}
							}
							else
							{
								if (guiactions.RightStick.WasReleased)
								{
									this.HandleMoveToPreferredLocation();
									return;
								}
								if (guiactions.Inspect.WasReleased)
								{
									this.HandleItemInspect();
									return;
								}
							}
						}
					}
					else if (guiactions.Submit.WasReleased)
					{
						this.HandleStackSwap();
						return;
					}
				}
				else if (InputUtils.ShiftKeyPressed)
				{
					if (mouseButtonUp)
					{
						this.HandleMoveToPreferredLocation();
						return;
					}
				}
				else if (mouseButton)
				{
					if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && !this.ItemStack.IsEmpty())
					{
						if (!this.lastClicked)
						{
							this.startMousePos = vector;
						}
						else if (this.CanRemove() && Mathf.Abs((vector - this.startMousePos).magnitude) > (float)this.PickupSnapDistance)
						{
							this.SwapItem();
							this.currentColor = XUiC_BasePartStack.backgroundColor;
							if (this.itemStack.IsEmpty())
							{
								((XUiV_Sprite)this.background.ViewComponent).Color = XUiC_BasePartStack.backgroundColor;
							}
						}
					}
					if (mouseButtonDown)
					{
						this.lastClicked = true;
						return;
					}
				}
				else
				{
					if (!mouseButtonUp)
					{
						this.lastClicked = false;
						return;
					}
					if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty())
					{
						this.HandleItemInspect();
						return;
					}
					if (this.lastClicked)
					{
						this.HandleStackSwap();
						return;
					}
				}
			}
			else
			{
				this.currentColor = XUiC_BasePartStack.backgroundColor;
				if (!base.IsSelected)
				{
					((XUiV_Sprite)this.background.ViewComponent).Color = this.currentColor;
				}
				this.lastClicked = false;
			}
		}
	}

	// Token: 0x060076A5 RID: 30373 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetEmptySpriteName()
	{
	}

	// Token: 0x060076A6 RID: 30374 RVA: 0x002CBDBB File Offset: 0x002C9FBB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemInspect()
	{
		if (!this.ItemStack.IsEmpty() && this.InfoWindow != null)
		{
			base.IsSelected = true;
			this.InfoWindow.SetItemStack(this, true);
		}
		this.HandleClickComplete();
	}

	// Token: 0x060076A7 RID: 30375 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack stack)
	{
		return false;
	}

	// Token: 0x060076A8 RID: 30376 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanRemove()
	{
		return true;
	}

	// Token: 0x060076A9 RID: 30377 RVA: 0x002CBDEC File Offset: 0x002C9FEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleStackSwap()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.IsEmpty())
		{
			if (this.CanSwap(currentStack))
			{
				this.SwapItem();
			}
			else
			{
				Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
			}
		}
		else if (this.CanRemove())
		{
			this.SwapItem();
		}
		else
		{
			Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
		}
		base.IsSelected = false;
		this.HandleClickComplete();
	}

	// Token: 0x060076AA RID: 30378 RVA: 0x002CBE66 File Offset: 0x002CA066
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleClickComplete()
	{
		this.lastClicked = false;
		this.currentColor = XUiC_BasePartStack.backgroundColor;
		if (this.itemStack.IsEmpty())
		{
			((XUiV_Sprite)this.background.ViewComponent).Color = XUiC_BasePartStack.backgroundColor;
		}
	}

	// Token: 0x060076AB RID: 30379 RVA: 0x002CBEA8 File Offset: 0x002CA0A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				((XUiV_Sprite)this.background.ViewComponent).Color = XUiC_BasePartStack.highlightColor;
			}
			else
			{
				((XUiV_Sprite)this.background.ViewComponent).Color = XUiC_BasePartStack.backgroundColor;
			}
		}
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		bool flag = !currentStack.IsEmpty() && this.CanSwap(currentStack);
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, flag, this.CanRemove(), true);
		base.OnHovered(_isOver);
	}

	// Token: 0x060076AC RID: 30380 RVA: 0x002CBF68 File Offset: 0x002CA168
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SwapItem()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (this.itemStack.IsEmpty())
		{
			if (this.placeSound != null)
			{
				Manager.PlayXUiSound(this.placeSound, 0.75f);
			}
		}
		else if (this.pickupSound != null)
		{
			Manager.PlayXUiSound(this.pickupSound, 0.75f);
		}
		if (this.SlotChangingEvent != null)
		{
			this.SlotChangingEvent(this.SlotNumber, this.itemStack);
		}
		this.xui.DragAndDropWindow.SetCurrentStack(this.itemStack.Clone(), true);
		this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
		this.ItemStack = currentStack.Clone();
	}

	// Token: 0x060076AD RID: 30381 RVA: 0x002CC030 File Offset: 0x002CA230
	public void HandleMoveToPreferredLocation()
	{
		if (this.ItemStack.IsEmpty() || !this.CanRemove())
		{
			return;
		}
		if (this.SlotChangingEvent != null)
		{
			this.SlotChangingEvent(this.SlotNumber, this.ItemStack);
		}
		if (this.xui.PlayerInventory.AddItemToBackpack(this.ItemStack))
		{
			if (this.xui.CurrentSelectedEntry == this)
			{
				this.xui.CurrentSelectedEntry.IsSelected = false;
				this.InfoWindow.SetItemStack(null, false);
			}
			this.ItemStack = ItemStack.Empty;
			if (this.placeSound != null)
			{
				Manager.PlayXUiSound(this.placeSound, 0.75f);
			}
			if (this.SlotChangedEvent != null)
			{
				this.SlotChangedEvent(this.SlotNumber, this.itemStack);
				return;
			}
		}
		else if (this.xui.PlayerInventory.AddItemToToolbelt(this.ItemStack))
		{
			if (this.xui.CurrentSelectedEntry == this)
			{
				this.xui.CurrentSelectedEntry.IsSelected = false;
				this.InfoWindow.SetItemStack(null, false);
			}
			this.ItemStack = ItemStack.Empty;
			if (this.placeSound != null)
			{
				Manager.PlayXUiSound(this.placeSound, 0.75f);
			}
			if (this.SlotChangedEvent != null)
			{
				this.SlotChangedEvent(this.SlotNumber, this.itemStack);
			}
		}
	}

	// Token: 0x060076AE RID: 30382 RVA: 0x002CC18E File Offset: 0x002CA38E
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(null, true);
		}
	}

	// Token: 0x060076AF RID: 30383 RVA: 0x002CC1A8 File Offset: 0x002CA3A8
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "background_color"))
			{
				if (!(name == "highlight_color"))
				{
					if (!(name == "pickup_snap_distance"))
					{
						if (!(name == "hover_icon_grow"))
						{
							if (!(name == "pickup_sound"))
							{
								if (!(name == "place_sound"))
								{
									return false;
								}
								this.xui.LoadData<AudioClip>(value, delegate(AudioClip o)
								{
									this.placeSound = o;
								});
							}
							else
							{
								this.xui.LoadData<AudioClip>(value, delegate(AudioClip o)
								{
									this.pickupSound = o;
								});
							}
						}
						else
						{
							this.HoverIconGrow = StringParsers.ParseFloat(value, 0, -1, NumberStyles.Any);
						}
					}
					else
					{
						this.PickupSnapDistance = int.Parse(value);
					}
				}
				else
				{
					XUiC_BasePartStack.highlightColor = StringParsers.ParseColor32(value);
				}
			}
			else
			{
				XUiC_BasePartStack.backgroundColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x040057D4 RID: 22484
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x040057D5 RID: 22485
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass itemClass;

	// Token: 0x040057D6 RID: 22486
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bHighlightEnabled;

	// Token: 0x040057D7 RID: 22487
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDropEnabled = true;

	// Token: 0x040057D8 RID: 22488
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x040057D9 RID: 22489
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x040057DA RID: 22490
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x040057DB RID: 22491
	[PublicizedFrom(EAccessModifier.Protected)]
	public string slotType;

	// Token: 0x040057DC RID: 22492
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 currentColor;

	// Token: 0x040057DD RID: 22493
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057DE RID: 22494
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057DF RID: 22495
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x040057E2 RID: 22498
	public int PickupSnapDistance = 4;

	// Token: 0x040057E3 RID: 22499
	public static Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057E4 RID: 22500
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040057E5 RID: 22501
	public static Color32 highlightColor = new Color32(128, 128, 128, byte.MaxValue);

	// Token: 0x040057E6 RID: 22502
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x040057E7 RID: 22503
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController stackValue;

	// Token: 0x040057E8 RID: 22504
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController itemIcon;

	// Token: 0x040057E9 RID: 22505
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durability;

	// Token: 0x040057EA RID: 22506
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durabilityBackground;

	// Token: 0x040057EB RID: 22507
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController tintedOverlay;

	// Token: 0x040057EC RID: 22508
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController highlightOverlay;

	// Token: 0x040057ED RID: 22509
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController background;

	// Token: 0x040057EE RID: 22510
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController lblItemName;

	// Token: 0x040057F2 RID: 22514
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt qualityFormatter = new CachedStringFormatterInt();

	// Token: 0x040057F3 RID: 22515
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor partcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040057F4 RID: 22516
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat partfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040057F5 RID: 22517
	[PublicizedFrom(EAccessModifier.Protected)]
	public string emptySpriteName = "";

	// Token: 0x040057F7 RID: 22519
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
