using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F50 RID: 3920
[global::UnityEngine.Scripting.Preserve]
public class XUiC_EquipmentStack : XUiC_SelectableEntry
{
	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x06007A53 RID: 31315 RVA: 0x002E4D46 File Offset: 0x002E2F46
	// (set) Token: 0x06007A54 RID: 31316 RVA: 0x002E4D4E File Offset: 0x002E2F4E
	public EquipmentSlots EquipSlot
	{
		get
		{
			return this.equipSlot;
		}
		set
		{
			this.equipSlot = value;
		}
	}

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x06007A55 RID: 31317 RVA: 0x002E4D57 File Offset: 0x002E2F57
	public int SlotNumber
	{
		get
		{
			return (int)this.EquipSlot;
		}
	}

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06007A56 RID: 31318 RVA: 0x002E4D60 File Offset: 0x002E2F60
	// (remove) Token: 0x06007A57 RID: 31319 RVA: 0x002E4D98 File Offset: 0x002E2F98
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x06007A58 RID: 31320 RVA: 0x002E4DCD File Offset: 0x002E2FCD
	// (set) Token: 0x06007A59 RID: 31321 RVA: 0x002E4DD5 File Offset: 0x002E2FD5
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E6E RID: 3694
	// (get) Token: 0x06007A5A RID: 31322 RVA: 0x002E4DDE File Offset: 0x002E2FDE
	// (set) Token: 0x06007A5B RID: 31323 RVA: 0x002E4DE8 File Offset: 0x002E2FE8
	public ItemValue ItemValue
	{
		get
		{
			return this.itemValue;
		}
		set
		{
			if (this.itemValue != value)
			{
				this.itemValue = value;
				this.itemStack.itemValue = this.itemValue;
				if (!this.itemStack.itemValue.IsEmpty())
				{
					this.itemStack.count = 1;
				}
				if (value.IsEmpty() && base.IsSelected)
				{
					base.IsSelected = false;
					XUiC_ItemInfoWindow infoWindow = this.InfoWindow;
					if (infoWindow != null)
					{
						infoWindow.SetItemStack(null, true);
					}
				}
				XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
				if (slotChangedEvent != null)
				{
					slotChangedEvent(this.SlotNumber, this.itemStack);
				}
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E6F RID: 3695
	// (get) Token: 0x06007A5C RID: 31324 RVA: 0x002E4E82 File Offset: 0x002E3082
	// (set) Token: 0x06007A5D RID: 31325 RVA: 0x002E4E8A File Offset: 0x002E308A
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
				this.itemStack = value.Clone();
				this.ItemValue = this.itemStack.itemValue.Clone();
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000E70 RID: 3696
	// (get) Token: 0x06007A5E RID: 31326 RVA: 0x002E4EBE File Offset: 0x002E30BE
	// (set) Token: 0x06007A5F RID: 31327 RVA: 0x002E4EC6 File Offset: 0x002E30C6
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x06007A60 RID: 31328 RVA: 0x002E4ECF File Offset: 0x002E30CF
	// (set) Token: 0x06007A61 RID: 31329 RVA: 0x002E4ED7 File Offset: 0x002E30D7
	public XUiC_CharacterFrameWindow FrameWindow { get; set; }

	// Token: 0x06007A62 RID: 31330 RVA: 0x002E4EE0 File Offset: 0x002E30E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool _isSelected)
	{
		this.SetBorderColor(_isSelected ? this.selectedBorderColor : this.normalBorderColor);
	}

	// Token: 0x06007A63 RID: 31331 RVA: 0x002E4EF9 File Offset: 0x002E30F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBorderColor(Color32 _color)
	{
		this.background.Color = _color;
	}

	// Token: 0x06007A64 RID: 31332 RVA: 0x002E4F0C File Offset: 0x002E310C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("stackValue");
		this.stackValue = ((childById != null) ? childById.ViewComponent : null) as XUiV_Label;
		XUiController childById2 = base.GetChildById("background");
		this.background = ((childById2 != null) ? childById2.ViewComponent : null) as XUiV_Sprite;
		this.SetBorderColor(this.normalBorderColor);
		XUiController childById3 = base.GetChildById("itemIcon");
		this.itemIcon = ((childById3 != null) ? childById3.ViewComponent : null) as XUiV_Sprite;
		XUiController childById4 = base.GetChildById("durabilityBackground");
		this.durabilityBackground = ((childById4 != null) ? childById4.ViewComponent : null) as XUiV_Sprite;
		XUiController childById5 = base.GetChildById("durabilityRemoveBackground");
		this.durabilityRemoveBackground = ((childById5 != null) ? childById5.ViewComponent : null) as XUiV_Sprite;
		XUiController childById6 = base.GetChildById("durability");
		this.durability = ((childById6 != null) ? childById6.ViewComponent : null) as XUiV_Sprite;
		XUiController childById7 = base.GetChildById("lockTypeIcon");
		this.lockTypeIcon = ((childById7 != null) ? childById7.ViewComponent : null) as XUiV_Sprite;
		this.itemStack.count = 1;
		XUiV_Sprite xuiV_Sprite = this.itemIcon;
		this.tweenScale = ((xuiV_Sprite != null) ? xuiV_Sprite.UiTransform.gameObject.AddComponent<TweenScale>() : null);
		base.ViewComponent.UseSelectionBox = false;
	}

	// Token: 0x06007A65 RID: 31333 RVA: 0x002E5054 File Offset: 0x002E3254
	public XUiC_EquipmentStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007A66 RID: 31334 RVA: 0x002E5114 File Offset: 0x002E3314
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
					bool wasReleased = guiactions.Submit.WasReleased;
					bool wasPressed = guiactions.Inspect.WasPressed;
					bool wasReleased2 = guiactions.RightStick.WasReleased;
					if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && !this.ItemStack.IsEmpty())
					{
						if (wasReleased)
						{
							this.SwapItem();
						}
						else if (wasReleased2)
						{
							this.HandleMoveToPreferredLocation();
							this.xui.PlayerEquipment.RefreshEquipment();
						}
						else if (wasPressed)
						{
							this.HandleItemInspect();
						}
						if (this.itemStack.IsEmpty())
						{
							this.SetBorderColor(this.normalBorderColor);
						}
					}
					else if (wasReleased)
					{
						this.HandleStackSwap();
					}
				}
				else if (InputUtils.ShiftKeyPressed)
				{
					if (mouseButtonUp)
					{
						this.HandleMoveToPreferredLocation();
						this.xui.PlayerEquipment.RefreshEquipment();
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
						else if (Mathf.Abs((vector - this.startMousePos).magnitude) > (float)this.PickupSnapDistance)
						{
							this.SwapItem();
							this.xui.PlayerEquipment.RefreshEquipment();
						}
						this.SetBorderColor(this.normalBorderColor);
					}
					if (mouseButtonDown)
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
					else if (this.lastClicked)
					{
						this.HandleStackSwap();
						this.xui.PlayerEquipment.RefreshEquipment();
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
				if (this.isOver || this.itemIcon.UiTransform.localScale != Vector3.one)
				{
					if (this.tweenScale.value != Vector3.one && !this.itemStack.IsEmpty())
					{
						this.tweenScale.from = Vector3.one * 1.5f;
						this.tweenScale.to = Vector3.one;
						this.tweenScale.enabled = true;
						this.tweenScale.duration = 0.5f;
					}
					this.isOver = false;
				}
			}
		}
		if (this.IsDirty)
		{
			bool flag = !this.itemValue.IsEmpty();
			ItemClass itemClass = null;
			if (flag)
			{
				itemClass = ItemClass.GetForId(this.itemValue.type);
			}
			if (this.itemIcon != null)
			{
				this.itemIcon.SpriteName = (flag ? this.itemStack.itemValue.GetPropertyOverride("CustomIcon", itemClass.GetIconName()) : this.emptySpriteName);
				this.itemIcon.UIAtlas = (flag ? "ItemIconAtlas" : "ItemIconAtlasGreyscale");
				this.itemIcon.Color = (flag ? Color.white : new Color(1f, 1f, 1f, 0.7f));
				string text = string.Empty;
				if (flag)
				{
					text = itemClass.GetLocalizedItemName();
				}
				base.ViewComponent.ToolTip = (flag ? text : this.emptyTooltipName);
			}
			if (itemClass != null)
			{
				this.itemIcon.Color = this.itemStack.itemValue.ItemClass.GetIconTint(this.itemStack.itemValue);
				if (itemClass.ShowQualityBar)
				{
					if (this.durability != null)
					{
						this.durability.IsVisible = true;
						this.durabilityBackground.IsVisible = EntityPlayerLocal.PermaDegrationOn && this.itemStack.itemValue.MaxDurabilityModifier < 1f;
						if (this.durabilityRemoveBackground != null)
						{
							this.durabilityRemoveBackground.IsVisible = true;
							this.durabilityRemoveBackground.Fill = this.itemValue.MaxDurabilityModifier;
						}
						this.durability.Color = QualityInfo.GetQualityColor((int)this.itemValue.Quality);
						this.durability.Fill = ((this.itemValue.MaxUseTimes == 0) ? 1f : (((float)this.itemValue.MaxUseTimes - this.itemValue.UseTimes) / (float)this.itemValue.MaxUseTimesUI));
					}
					if (this.stackValue != null)
					{
						this.stackValue.IsVisible = true;
						this.stackValue.Alignment = NGUIText.Alignment.Center;
						this.stackValue.Text = ((this.itemStack.itemValue.Quality > 0) ? this.itemStack.itemValue.Quality.ToString() : (this.itemStack.itemValue.IsMod ? "*" : ""));
					}
				}
				else
				{
					if (this.durability != null)
					{
						this.durability.IsVisible = false;
						this.durabilityBackground.IsVisible = false;
					}
					if (this.stackValue != null)
					{
						this.stackValue.IsVisible = false;
					}
				}
				if (this.lockTypeIcon != null)
				{
					this.lockTypeIcon.SpriteName = (this.itemStack.itemValue.HasMods() ? "ui_game_symbol_modded" : "");
				}
			}
			else
			{
				if (this.durability != null)
				{
					this.durability.IsVisible = false;
				}
				if (this.durabilityBackground != null)
				{
					this.durabilityBackground.IsVisible = false;
				}
				if (this.durabilityRemoveBackground != null)
				{
					this.durabilityRemoveBackground.IsVisible = false;
				}
				if (this.stackValue != null)
				{
					this.stackValue.Text = "";
				}
				if (this.lockTypeIcon != null)
				{
					this.lockTypeIcon.SpriteName = "";
				}
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06007A67 RID: 31335 RVA: 0x002E5763 File Offset: 0x002E3963
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleItemInspect()
	{
		if (!this.ItemStack.IsEmpty() && this.InfoWindow != null)
		{
			base.IsSelected = true;
			this.InfoWindow.SetItemStack(this, true);
		}
		this.HandleClickComplete();
	}

	// Token: 0x06007A68 RID: 31336 RVA: 0x002E5794 File Offset: 0x002E3994
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleStackSwap()
	{
		ItemClass itemClass = this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass;
		if (itemClass == null)
		{
			return;
		}
		ItemClassArmor itemClassArmor = itemClass as ItemClassArmor;
		if (itemClassArmor != null && itemClassArmor.EquipSlot == this.EquipSlot)
		{
			this.SwapItem();
		}
		base.IsSelected = false;
		this.HandleClickComplete();
	}

	// Token: 0x06007A69 RID: 31337 RVA: 0x002E57EB File Offset: 0x002E39EB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleClickComplete()
	{
		this.lastClicked = false;
		if (this.itemValue.IsEmpty())
		{
			this.SetBorderColor(this.normalBorderColor);
		}
	}

	// Token: 0x06007A6A RID: 31338 RVA: 0x002E5810 File Offset: 0x002E3A10
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (_isOver)
		{
			if (!base.IsSelected)
			{
				this.SetBorderColor(this.hoverBorderColor);
			}
			if (!this.itemStack.IsEmpty())
			{
				this.tweenScale.from = Vector3.one;
				this.tweenScale.to = Vector3.one * 1.5f;
				this.tweenScale.enabled = true;
				this.tweenScale.duration = 0.5f;
			}
		}
		else
		{
			if (!base.IsSelected)
			{
				this.SetBorderColor(this.normalBorderColor);
			}
			this.tweenScale.from = Vector3.one * 1.5f;
			this.tweenScale.to = Vector3.one;
			this.tweenScale.enabled = true;
			this.tweenScale.duration = 0.5f;
		}
		bool flag = false;
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		ItemClassArmor itemClassArmor = (currentStack.IsEmpty() ? null : currentStack.itemValue.ItemClass) as ItemClassArmor;
		if (itemClassArmor != null)
		{
			flag = this.equipSlot == itemClassArmor.EquipSlot;
		}
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, this.ItemStack, this.isOver, flag, true, true);
		if (!_isOver && this.tweenScale.value != Vector3.one && !this.itemStack.IsEmpty())
		{
			this.tweenScale.from = Vector3.one * 1.5f;
			this.tweenScale.to = Vector3.one;
			this.tweenScale.enabled = true;
			this.tweenScale.duration = 0.5f;
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x06007A6B RID: 31339 RVA: 0x002E59D4 File Offset: 0x002E3BD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SwapItem()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (this.AllowRemoveItem)
		{
			this.xui.DragAndDropWindow.SetCurrentStack(this.itemStack.Clone(), true);
			this.xui.DragAndDropWindow.PickUpType = XUiC_ItemStack.StackLocationTypes.Equipment;
		}
		else
		{
			if (currentStack.IsEmpty())
			{
				return;
			}
			if (!this.itemStack.IsEmpty())
			{
				ItemClassArmor itemClassArmor = this.itemStack.itemValue.ItemClass as ItemClassArmor;
				if (itemClassArmor != null && itemClassArmor.ReplaceByTag != null)
				{
					FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(itemClassArmor.ReplaceByTag);
					ItemClassArmor itemClassArmor2 = currentStack.itemValue.ItemClass as ItemClassArmor;
					if (itemClassArmor2 != null && !fastTags.Test_AnySet(itemClassArmor2.ItemTags))
					{
						return;
					}
				}
			}
			this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
		}
		this.ItemStack = currentStack.Clone();
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x06007A6C RID: 31340 RVA: 0x002E5AD0 File Offset: 0x002E3CD0
	public void HandleMoveToPreferredLocation()
	{
		if (!this.AllowRemoveItem)
		{
			return;
		}
		ItemStack itemStack = this.ItemStack.Clone();
		if (this.xui.PlayerInventory.AddItemToBackpack(itemStack))
		{
			this.ItemValue = ItemStack.Empty.itemValue.Clone();
			if (this.itemStack.IsEmpty() && base.IsSelected)
			{
				base.IsSelected = false;
			}
		}
		else if (this.xui.PlayerInventory.AddItemToToolbelt(itemStack))
		{
			this.ItemValue = ItemStack.Empty.itemValue.Clone();
			XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
			if (slotChangedEvent != null)
			{
				slotChangedEvent(this.SlotNumber, this.itemStack);
			}
			if (this.itemStack.IsEmpty() && base.IsSelected)
			{
				base.IsSelected = false;
			}
		}
		if (!itemStack.IsEmpty())
		{
			string soundPlace = itemStack.itemValue.ItemClass.SoundPlace;
			if (!string.IsNullOrEmpty(soundPlace))
			{
				Manager.PlayInsidePlayerHead(soundPlace, -1, 0f, false, false);
			}
		}
	}

	// Token: 0x06007A6D RID: 31341 RVA: 0x002E5BC8 File Offset: 0x002E3DC8
	public override bool ParseAttribute(string _name, string _value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_name);
		if (num <= 1619874765U)
		{
			if (num <= 1352047028U)
			{
				if (num != 783618599U)
				{
					if (num == 1352047028U)
					{
						if (_name == "hover_border_color")
						{
							this.hoverBorderColor = StringParsers.ParseColor32(_value);
							return true;
						}
					}
				}
				else if (_name == "hover_icon_grow")
				{
					this.HoverIconGrow = StringParsers.ParseFloat(_value, 0, -1, NumberStyles.Any);
					return true;
				}
			}
			else if (num != 1563791203U)
			{
				if (num == 1619874765U)
				{
					if (_name == "normal_border_color")
					{
						this.normalBorderColor = StringParsers.ParseColor32(_value);
						return true;
					}
				}
			}
			else if (_name == "allow_remove_item")
			{
				this.AllowRemoveItem = StringParsers.ParseBool(_value, 0, -1, true);
				return true;
			}
		}
		else if (num <= 2848822010U)
		{
			if (num != 1888831345U)
			{
				if (num == 2848822010U)
				{
					if (_name == "empty_sprite")
					{
						this.emptySpriteName = _value;
						return true;
					}
				}
			}
			else if (_name == "slot")
			{
				if (!EnumUtils.TryParse<EquipmentSlots>(_value, out this.equipSlot, true))
				{
					Log.Error("[XUi] EquipmentStack slot '" + _value + "' unknown");
					this.equipSlot = EquipmentSlots.Count;
				}
				return true;
			}
		}
		else if (num != 3765930259U)
		{
			if (num == 4254382130U)
			{
				if (_name == "empty_tooltip")
				{
					this.emptyTooltipName = Localization.Get(_value, false, null);
					return true;
				}
			}
		}
		else if (_name == "selected_border_color")
		{
			this.selectedBorderColor = StringParsers.ParseColor32(_value);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007A6E RID: 31342 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x04005ABA RID: 23226
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005ABB RID: 23227
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue = new ItemValue();

	// Token: 0x04005ABC RID: 23228
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005ABD RID: 23229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005ABE RID: 23230
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDisabled;

	// Token: 0x04005ABF RID: 23231
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005AC0 RID: 23232
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedBorderColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005AC1 RID: 23233
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 hoverBorderColor = new Color32(182, 166, 123, byte.MaxValue);

	// Token: 0x04005AC2 RID: 23234
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005AC3 RID: 23235
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005AC4 RID: 23236
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptySpriteName = "";

	// Token: 0x04005AC5 RID: 23237
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptyTooltipName = "";

	// Token: 0x04005AC6 RID: 23238
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots equipSlot = EquipmentSlots.Count;

	// Token: 0x04005AC7 RID: 23239
	public int PickupSnapDistance = 4;

	// Token: 0x04005AC8 RID: 23240
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005AC9 RID: 23241
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite itemIcon;

	// Token: 0x04005ACA RID: 23242
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durability;

	// Token: 0x04005ACB RID: 23243
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityRemoveBackground;

	// Token: 0x04005ACC RID: 23244
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityBackground;

	// Token: 0x04005ACD RID: 23245
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x04005ACE RID: 23246
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005AD1 RID: 23249
	public bool AllowRemoveItem = true;

	// Token: 0x04005AD4 RID: 23252
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;

	// Token: 0x04005AD5 RID: 23253
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
