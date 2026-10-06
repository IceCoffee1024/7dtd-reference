using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED9 RID: 3801
[global::UnityEngine.Scripting.Preserve]
public class XUiC_BasePartStack : XUiC_SelectableEntry
{
	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06007667 RID: 30311 RVA: 0x002C9FB1 File Offset: 0x002C81B1
	// (set) Token: 0x06007668 RID: 30312 RVA: 0x002C9FB9 File Offset: 0x002C81B9
	public int SlotNumber { get; set; }

	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06007669 RID: 30313 RVA: 0x002C9FC2 File Offset: 0x002C81C2
	// (set) Token: 0x0600766A RID: 30314 RVA: 0x002C9FCA File Offset: 0x002C81CA
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x0600766B RID: 30315 RVA: 0x002C9FD4 File Offset: 0x002C81D4
	// (remove) Token: 0x0600766C RID: 30316 RVA: 0x002CA00C File Offset: 0x002C820C
	public event XUiEvent_SlotChangedEventHandler SlotChangingEvent;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x0600766D RID: 30317 RVA: 0x002CA044 File Offset: 0x002C8244
	// (remove) Token: 0x0600766E RID: 30318 RVA: 0x002CA07C File Offset: 0x002C827C
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x0600766F RID: 30319 RVA: 0x002CA0B1 File Offset: 0x002C82B1
	// (set) Token: 0x06007670 RID: 30320 RVA: 0x002CA0B9 File Offset: 0x002C82B9
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06007671 RID: 30321 RVA: 0x002CA0C2 File Offset: 0x002C82C2
	// (set) Token: 0x06007672 RID: 30322 RVA: 0x002CA0CA File Offset: 0x002C82CA
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

	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x06007673 RID: 30323 RVA: 0x002CA0D9 File Offset: 0x002C82D9
	// (set) Token: 0x06007674 RID: 30324 RVA: 0x002CA0E4 File Offset: 0x002C82E4
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

	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x06007675 RID: 30325 RVA: 0x002CA16C File Offset: 0x002C836C
	public ItemClass ItemClass
	{
		get
		{
			return this.itemClass;
		}
	}

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06007676 RID: 30326 RVA: 0x002CA174 File Offset: 0x002C8374
	// (set) Token: 0x06007677 RID: 30327 RVA: 0x002CA17C File Offset: 0x002C837C
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x06007678 RID: 30328 RVA: 0x002CA185 File Offset: 0x002C8385
	public virtual string GetAtlas()
	{
		if (this.itemClass == null)
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007679 RID: 30329 RVA: 0x002CA19A File Offset: 0x002C839A
	public virtual string GetPartName()
	{
		if (this.itemClass == null)
		{
			return string.Format("[MISSING {0}]", this.SlotType);
		}
		return this.itemClass.GetLocalizedItemName();
	}

	// Token: 0x0600767A RID: 30330 RVA: 0x002CA1C0 File Offset: 0x002C83C0
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

	// Token: 0x0600767B RID: 30331 RVA: 0x002CA510 File Offset: 0x002C8710
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.SetColor(isSelected ? this.selectColor : XUiC_BasePartStack.backgroundColor);
		((XUiV_Sprite)this.background.ViewComponent).SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
	}

	// Token: 0x0600767C RID: 30332 RVA: 0x002CA54C File Offset: 0x002C874C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetColor(Color32 color)
	{
		((XUiV_Sprite)this.background.ViewComponent).Color = color;
	}

	// Token: 0x0600767D RID: 30333 RVA: 0x002CA569 File Offset: 0x002C8769
	public override void Init()
	{
		base.Init();
		this.itemIcon = base.GetChildById("itemIcon");
		this.background = base.GetChildById("background");
		base.RefreshBindings();
	}

	// Token: 0x0600767E RID: 30334 RVA: 0x002CA59C File Offset: 0x002C879C
	public XUiC_BasePartStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600767F RID: 30335 RVA: 0x002CA660 File Offset: 0x002C8860
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

	// Token: 0x06007680 RID: 30336 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetEmptySpriteName()
	{
	}

	// Token: 0x06007681 RID: 30337 RVA: 0x002CA8FF File Offset: 0x002C8AFF
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

	// Token: 0x06007682 RID: 30338 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack stack)
	{
		return false;
	}

	// Token: 0x06007683 RID: 30339 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanRemove()
	{
		return true;
	}

	// Token: 0x06007684 RID: 30340 RVA: 0x002CA930 File Offset: 0x002C8B30
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

	// Token: 0x06007685 RID: 30341 RVA: 0x002CA9AA File Offset: 0x002C8BAA
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

	// Token: 0x06007686 RID: 30342 RVA: 0x002CA9EC File Offset: 0x002C8BEC
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

	// Token: 0x06007687 RID: 30343 RVA: 0x002CAAAC File Offset: 0x002C8CAC
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

	// Token: 0x06007688 RID: 30344 RVA: 0x002CAB74 File Offset: 0x002C8D74
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

	// Token: 0x06007689 RID: 30345 RVA: 0x002CACD2 File Offset: 0x002C8ED2
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(null, true);
		}
	}

	// Token: 0x0600768A RID: 30346 RVA: 0x002CACEC File Offset: 0x002C8EEC
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

	// Token: 0x040057AF RID: 22447
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x040057B0 RID: 22448
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass itemClass;

	// Token: 0x040057B1 RID: 22449
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bHighlightEnabled;

	// Token: 0x040057B2 RID: 22450
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDropEnabled = true;

	// Token: 0x040057B3 RID: 22451
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x040057B4 RID: 22452
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x040057B5 RID: 22453
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x040057B6 RID: 22454
	[PublicizedFrom(EAccessModifier.Protected)]
	public string slotType;

	// Token: 0x040057B7 RID: 22455
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 currentColor;

	// Token: 0x040057B8 RID: 22456
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057B9 RID: 22457
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057BA RID: 22458
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x040057BD RID: 22461
	public int PickupSnapDistance = 4;

	// Token: 0x040057BE RID: 22462
	public static Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040057BF RID: 22463
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040057C0 RID: 22464
	public static Color32 highlightColor = new Color32(128, 128, 128, byte.MaxValue);

	// Token: 0x040057C1 RID: 22465
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x040057C2 RID: 22466
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController stackValue;

	// Token: 0x040057C3 RID: 22467
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController itemIcon;

	// Token: 0x040057C4 RID: 22468
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durability;

	// Token: 0x040057C5 RID: 22469
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durabilityBackground;

	// Token: 0x040057C6 RID: 22470
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController tintedOverlay;

	// Token: 0x040057C7 RID: 22471
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController highlightOverlay;

	// Token: 0x040057C8 RID: 22472
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController background;

	// Token: 0x040057C9 RID: 22473
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController lblItemName;

	// Token: 0x040057CD RID: 22477
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt qualityFormatter = new CachedStringFormatterInt();

	// Token: 0x040057CE RID: 22478
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor partcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040057CF RID: 22479
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat partfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040057D0 RID: 22480
	[PublicizedFrom(EAccessModifier.Protected)]
	public string emptySpriteName = "";

	// Token: 0x040057D2 RID: 22482
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
