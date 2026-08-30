using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB5 RID: 3765
[global::UnityEngine.Scripting.Preserve]
public class XUiC_BasePartStack : XUiC_SelectableEntry
{
	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x060075CB RID: 30155 RVA: 0x002C36B5 File Offset: 0x002C18B5
	// (set) Token: 0x060075CC RID: 30156 RVA: 0x002C36BD File Offset: 0x002C18BD
	public int SlotNumber { get; set; }

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x060075CD RID: 30157 RVA: 0x002C36C6 File Offset: 0x002C18C6
	// (set) Token: 0x060075CE RID: 30158 RVA: 0x002C36CE File Offset: 0x002C18CE
	public XUiC_ItemStack.StackLocationTypes StackLocation { get; set; }

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x060075CF RID: 30159 RVA: 0x002C36D8 File Offset: 0x002C18D8
	// (remove) Token: 0x060075D0 RID: 30160 RVA: 0x002C3710 File Offset: 0x002C1910
	public event XUiEvent_SlotChangedEventHandler SlotChangingEvent;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x060075D1 RID: 30161 RVA: 0x002C3748 File Offset: 0x002C1948
	// (remove) Token: 0x060075D2 RID: 30162 RVA: 0x002C3780 File Offset: 0x002C1980
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x060075D3 RID: 30163 RVA: 0x002C37B5 File Offset: 0x002C19B5
	// (set) Token: 0x060075D4 RID: 30164 RVA: 0x002C37BD File Offset: 0x002C19BD
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x060075D5 RID: 30165 RVA: 0x002C37C6 File Offset: 0x002C19C6
	// (set) Token: 0x060075D6 RID: 30166 RVA: 0x002C37CE File Offset: 0x002C19CE
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

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x060075D7 RID: 30167 RVA: 0x002C37DD File Offset: 0x002C19DD
	// (set) Token: 0x060075D8 RID: 30168 RVA: 0x002C37E8 File Offset: 0x002C19E8
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

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x060075D9 RID: 30169 RVA: 0x002C3870 File Offset: 0x002C1A70
	public ItemClass ItemClass
	{
		get
		{
			return this.itemClass;
		}
	}

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x060075DA RID: 30170 RVA: 0x002C3878 File Offset: 0x002C1A78
	// (set) Token: 0x060075DB RID: 30171 RVA: 0x002C3880 File Offset: 0x002C1A80
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x060075DC RID: 30172 RVA: 0x002C3889 File Offset: 0x002C1A89
	public virtual string GetAtlas()
	{
		if (this.itemClass == null)
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x060075DD RID: 30173 RVA: 0x002C389E File Offset: 0x002C1A9E
	public virtual string GetPartName()
	{
		if (this.itemClass == null)
		{
			return string.Format("[MISSING {0}]", this.SlotType);
		}
		return this.itemClass.GetLocalizedItemName();
	}

	// Token: 0x060075DE RID: 30174 RVA: 0x002C38C4 File Offset: 0x002C1AC4
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

	// Token: 0x060075DF RID: 30175 RVA: 0x002C3C14 File Offset: 0x002C1E14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.SetColor(isSelected ? this.selectColor : XUiC_BasePartStack.backgroundColor);
		((XUiV_Sprite)this.background.ViewComponent).SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
	}

	// Token: 0x060075E0 RID: 30176 RVA: 0x002C3C50 File Offset: 0x002C1E50
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetColor(Color32 color)
	{
		((XUiV_Sprite)this.background.ViewComponent).Color = color;
	}

	// Token: 0x060075E1 RID: 30177 RVA: 0x002C3C6D File Offset: 0x002C1E6D
	public override void Init()
	{
		base.Init();
		this.itemIcon = base.GetChildById("itemIcon");
		this.background = base.GetChildById("background");
		base.RefreshBindings();
	}

	// Token: 0x060075E2 RID: 30178 RVA: 0x002C3CA0 File Offset: 0x002C1EA0
	public XUiC_BasePartStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x060075E3 RID: 30179 RVA: 0x002C3D64 File Offset: 0x002C1F64
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

	// Token: 0x060075E4 RID: 30180 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetEmptySpriteName()
	{
	}

	// Token: 0x060075E5 RID: 30181 RVA: 0x002C4003 File Offset: 0x002C2203
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

	// Token: 0x060075E6 RID: 30182 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanSwap(ItemStack stack)
	{
		return false;
	}

	// Token: 0x060075E7 RID: 30183 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanRemove()
	{
		return true;
	}

	// Token: 0x060075E8 RID: 30184 RVA: 0x002C4034 File Offset: 0x002C2234
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

	// Token: 0x060075E9 RID: 30185 RVA: 0x002C40AE File Offset: 0x002C22AE
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

	// Token: 0x060075EA RID: 30186 RVA: 0x002C40F0 File Offset: 0x002C22F0
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

	// Token: 0x060075EB RID: 30187 RVA: 0x002C41B0 File Offset: 0x002C23B0
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

	// Token: 0x060075EC RID: 30188 RVA: 0x002C4278 File Offset: 0x002C2478
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

	// Token: 0x060075ED RID: 30189 RVA: 0x002C43D6 File Offset: 0x002C25D6
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(null, true);
		}
	}

	// Token: 0x060075EE RID: 30190 RVA: 0x002C43F0 File Offset: 0x002C25F0
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

	// Token: 0x040056D2 RID: 22226
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x040056D3 RID: 22227
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemClass itemClass;

	// Token: 0x040056D4 RID: 22228
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bHighlightEnabled;

	// Token: 0x040056D5 RID: 22229
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDropEnabled = true;

	// Token: 0x040056D6 RID: 22230
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip pickupSound;

	// Token: 0x040056D7 RID: 22231
	[PublicizedFrom(EAccessModifier.Protected)]
	public AudioClip placeSound;

	// Token: 0x040056D8 RID: 22232
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;

	// Token: 0x040056D9 RID: 22233
	[PublicizedFrom(EAccessModifier.Protected)]
	public string slotType;

	// Token: 0x040056DA RID: 22234
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 currentColor;

	// Token: 0x040056DB RID: 22235
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040056DC RID: 22236
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040056DD RID: 22237
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastClicked;

	// Token: 0x040056E0 RID: 22240
	public int PickupSnapDistance = 4;

	// Token: 0x040056E1 RID: 22241
	public static Color32 finalPressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040056E2 RID: 22242
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040056E3 RID: 22243
	public static Color32 highlightColor = new Color32(128, 128, 128, byte.MaxValue);

	// Token: 0x040056E4 RID: 22244
	public Color32 holdingColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

	// Token: 0x040056E5 RID: 22245
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController stackValue;

	// Token: 0x040056E6 RID: 22246
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController itemIcon;

	// Token: 0x040056E7 RID: 22247
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durability;

	// Token: 0x040056E8 RID: 22248
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController durabilityBackground;

	// Token: 0x040056E9 RID: 22249
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController tintedOverlay;

	// Token: 0x040056EA RID: 22250
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController highlightOverlay;

	// Token: 0x040056EB RID: 22251
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController background;

	// Token: 0x040056EC RID: 22252
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController lblItemName;

	// Token: 0x040056F0 RID: 22256
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt qualityFormatter = new CachedStringFormatterInt();

	// Token: 0x040056F1 RID: 22257
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor partcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040056F2 RID: 22258
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat partfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040056F3 RID: 22259
	[PublicizedFrom(EAccessModifier.Protected)]
	public string emptySpriteName = "";

	// Token: 0x040056F5 RID: 22261
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
