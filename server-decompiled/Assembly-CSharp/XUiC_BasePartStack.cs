using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ECA RID: 3786
[global::UnityEngine.Scripting.Preserve]
public class XUiC_BasePartStack : XUiC_SelectableEntry
{
	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x0600766C RID: 30316 RVA: 0x002C6BD9 File Offset: 0x002C4DD9
	// (set) Token: 0x0600766D RID: 30317 RVA: 0x002C6BE1 File Offset: 0x002C4DE1
	public int SlotNumber { get; set; }

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x0600766E RID: 30318 RVA: 0x002C6BEA File Offset: 0x002C4DEA
	// (set) Token: 0x0600766F RID: 30319 RVA: 0x002C6BF2 File Offset: 0x002C4DF2
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06007670 RID: 30320 RVA: 0x002C6BFC File Offset: 0x002C4DFC
	// (remove) Token: 0x06007671 RID: 30321 RVA: 0x002C6C34 File Offset: 0x002C4E34
	public event XUiEvent_SlotChangedEventHandler SlotChangingEvent;

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06007672 RID: 30322 RVA: 0x002C6C6C File Offset: 0x002C4E6C
	// (remove) Token: 0x06007673 RID: 30323 RVA: 0x002C6CA4 File Offset: 0x002C4EA4
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x06007674 RID: 30324 RVA: 0x002C6CD9 File Offset: 0x002C4ED9
	// (set) Token: 0x06007675 RID: 30325 RVA: 0x002C6CE1 File Offset: 0x002C4EE1
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x06007676 RID: 30326 RVA: 0x002C6CEA File Offset: 0x002C4EEA
	// (set) Token: 0x06007677 RID: 30327 RVA: 0x002C6CF2 File Offset: 0x002C4EF2
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

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x06007678 RID: 30328 RVA: 0x002C6D01 File Offset: 0x002C4F01
	// (set) Token: 0x06007679 RID: 30329 RVA: 0x002C6D0C File Offset: 0x002C4F0C
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

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x0600767A RID: 30330 RVA: 0x002C6D94 File Offset: 0x002C4F94
	public ItemClass ItemClass
	{
		get
		{
			return this.itemClass;
		}
	}

	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x0600767B RID: 30331 RVA: 0x002C6D9C File Offset: 0x002C4F9C
	// (set) Token: 0x0600767C RID: 30332 RVA: 0x002C6DA4 File Offset: 0x002C4FA4
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x0600767D RID: 30333 RVA: 0x002C6DAD File Offset: 0x002C4FAD
	public virtual string GetAtlas()
	{
		if (this.itemClass == null)
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x0600767E RID: 30334 RVA: 0x002C6DC2 File Offset: 0x002C4FC2
	public virtual string GetPartName()
	{
		if (this.itemClass == null)
		{
			return string.Format("[MISSING {0}]", this.SlotType);
		}
		return this.itemClass.GetLocalizedItemName();
	}

	// Token: 0x0600767F RID: 30335 RVA: 0x002C6DE8 File Offset: 0x002C4FE8
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

	// Token: 0x06007680 RID: 30336 RVA: 0x002C7138 File Offset: 0x002C5338
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.SetColor(isSelected ? this.selectColor : XUiC_BasePartStack.backgroundColor);
		((XUiV_Sprite)this.background.ViewComponent).SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
	}

	// Token: 0x06007681 RID: 30337 RVA: 0x002C7174 File Offset: 0x002C5374
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetColor(Color32 color)
	{
		((XUiV_Sprite)this.background.ViewComponent).Color = color;
	}

	// Token: 0x06007682 RID: 30338 RVA: 0x002C7191 File Offset: 0x002C5391
	public override void Init()
	{
		base.Init();
		this.itemIcon = base.GetChildById("itemIcon");
		this.background = base.GetChildById("background");
		base.RefreshBindings();
	}

	// Token: 0x06007683 RID: 30339 RVA: 0x002C71C4 File Offset: 0x002C53C4
	public XUiC_BasePartStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007684 RID: 30340 RVA: 0x002C7288 File Offset: 0x002C5488
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

	// Token: 0x06007685 RID: 30341 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetEmptySpriteName()
	{
	}

	// Token: 0x06007686 RID: 30342 RVA: 0x002C7527 File Offset: 0x002C5727
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

	// Token: 0x06007687 RID: 30343 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack stack)
	{
		return false;
	}

	// Token: 0x06007688 RID: 30344 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanRemove()
	{
		return true;
	}

	// Token: 0x06007689 RID: 30345 RVA: 0x002C7558 File Offset: 0x002C5758
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

	// Token: 0x0600768A RID: 30346 RVA: 0x002C75D2 File Offset: 0x002C57D2
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

	// Token: 0x0600768B RID: 30347 RVA: 0x002C7614 File Offset: 0x002C5814
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

	// Token: 0x0600768C RID: 30348 RVA: 0x002C76D4 File Offset: 0x002C58D4
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
		this.xui.DragAndDropWindow.CurrentStack = this.itemStack.Clone();
		this.xui.DragAndDropWindow.PickUpType = this.StackLocation;
		this.ItemStack = currentStack.Clone();
	}

	// Token: 0x0600768D RID: 30349 RVA: 0x002C779C File Offset: 0x002C599C
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

	// Token: 0x0600768E RID: 30350 RVA: 0x002C78FA File Offset: 0x002C5AFA
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(null, true);
		}
	}

	// Token: 0x0600768F RID: 30351 RVA: 0x002C7914 File Offset: 0x002C5B14
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

	// Token: 0x04005777 RID: 22391
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005778 RID: 22392
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass itemClass;

	// Token: 0x04005779 RID: 22393
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bHighlightEnabled;

	// Token: 0x0400577A RID: 22394
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDropEnabled = true;

	// Token: 0x0400577B RID: 22395
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x0400577C RID: 22396
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x0400577D RID: 22397
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x0400577E RID: 22398
	[PublicizedFrom(EAccessModifier.Protected)]
	public string slotType;

	// Token: 0x0400577F RID: 22399
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 currentColor;

	// Token: 0x04005780 RID: 22400
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005781 RID: 22401
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005782 RID: 22402
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x04005785 RID: 22405
	public int PickupSnapDistance = 4;

	// Token: 0x04005786 RID: 22406
	public static Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005787 RID: 22407
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005788 RID: 22408
	public static Color32 highlightColor = new Color32(128, 128, 128, byte.MaxValue);

	// Token: 0x04005789 RID: 22409
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x0400578A RID: 22410
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController stackValue;

	// Token: 0x0400578B RID: 22411
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController itemIcon;

	// Token: 0x0400578C RID: 22412
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durability;

	// Token: 0x0400578D RID: 22413
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durabilityBackground;

	// Token: 0x0400578E RID: 22414
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController tintedOverlay;

	// Token: 0x0400578F RID: 22415
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController highlightOverlay;

	// Token: 0x04005790 RID: 22416
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController background;

	// Token: 0x04005791 RID: 22417
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController lblItemName;

	// Token: 0x04005795 RID: 22421
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt qualityFormatter = new CachedStringFormatterInt();

	// Token: 0x04005796 RID: 22422
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor partcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005797 RID: 22423
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat partfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005798 RID: 22424
	[PublicizedFrom(EAccessModifier.Protected)]
	public string emptySpriteName = "";

	// Token: 0x0400579A RID: 22426
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
