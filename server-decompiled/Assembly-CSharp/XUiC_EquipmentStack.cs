using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F41 RID: 3905
[global::UnityEngine.Scripting.Preserve]
public class XUiC_EquipmentStack : XUiC_SelectableEntry
{
	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x06007A10 RID: 31248 RVA: 0x002E3456 File Offset: 0x002E1656
	// (set) Token: 0x06007A11 RID: 31249 RVA: 0x002E345E File Offset: 0x002E165E
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

	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x06007A12 RID: 31250 RVA: 0x002E3467 File Offset: 0x002E1667
	public int SlotNumber
	{
		get
		{
			return (int)this.EquipSlot;
		}
	}

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06007A13 RID: 31251 RVA: 0x002E3470 File Offset: 0x002E1670
	// (remove) Token: 0x06007A14 RID: 31252 RVA: 0x002E34A8 File Offset: 0x002E16A8
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06007A15 RID: 31253 RVA: 0x002E34DD File Offset: 0x002E16DD
	// (set) Token: 0x06007A16 RID: 31254 RVA: 0x002E34E5 File Offset: 0x002E16E5
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x06007A17 RID: 31255 RVA: 0x002E34EE File Offset: 0x002E16EE
	// (set) Token: 0x06007A18 RID: 31256 RVA: 0x002E34F8 File Offset: 0x002E16F8
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

	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x06007A19 RID: 31257 RVA: 0x002E3592 File Offset: 0x002E1792
	// (set) Token: 0x06007A1A RID: 31258 RVA: 0x002E359A File Offset: 0x002E179A
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

	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x06007A1B RID: 31259 RVA: 0x002E35CE File Offset: 0x002E17CE
	// (set) Token: 0x06007A1C RID: 31260 RVA: 0x002E35D6 File Offset: 0x002E17D6
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x06007A1D RID: 31261 RVA: 0x002E35DF File Offset: 0x002E17DF
	// (set) Token: 0x06007A1E RID: 31262 RVA: 0x002E35E7 File Offset: 0x002E17E7
	public XUiC_CharacterFrameWindow FrameWindow { get; set; }

	// Token: 0x06007A1F RID: 31263 RVA: 0x002E35F0 File Offset: 0x002E17F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool _isSelected)
	{
		this.SetBorderColor(_isSelected ? this.selectedBorderColor : this.normalBorderColor);
	}

	// Token: 0x06007A20 RID: 31264 RVA: 0x002E3609 File Offset: 0x002E1809
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBorderColor(Color32 _color)
	{
		this.background.Color = _color;
	}

	// Token: 0x06007A21 RID: 31265 RVA: 0x002E361C File Offset: 0x002E181C
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

	// Token: 0x06007A22 RID: 31266 RVA: 0x002E3764 File Offset: 0x002E1964
	public XUiC_EquipmentStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007A23 RID: 31267 RVA: 0x002E3824 File Offset: 0x002E1A24
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

	// Token: 0x06007A24 RID: 31268 RVA: 0x002E3E73 File Offset: 0x002E2073
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

	// Token: 0x06007A25 RID: 31269 RVA: 0x002E3EA4 File Offset: 0x002E20A4
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

	// Token: 0x06007A26 RID: 31270 RVA: 0x002E3EFB File Offset: 0x002E20FB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleClickComplete()
	{
		this.lastClicked = false;
		if (this.itemValue.IsEmpty())
		{
			this.SetBorderColor(this.normalBorderColor);
		}
	}

	// Token: 0x06007A27 RID: 31271 RVA: 0x002E3F20 File Offset: 0x002E2120
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

	// Token: 0x06007A28 RID: 31272 RVA: 0x002E40E4 File Offset: 0x002E22E4
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

	// Token: 0x06007A29 RID: 31273 RVA: 0x002E41E0 File Offset: 0x002E23E0
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

	// Token: 0x06007A2A RID: 31274 RVA: 0x002E42D8 File Offset: 0x002E24D8
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

	// Token: 0x06007A2B RID: 31275 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x04005A87 RID: 23175
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x04005A88 RID: 23176
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue = new ItemValue();

	// Token: 0x04005A89 RID: 23177
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005A8A RID: 23178
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005A8B RID: 23179
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDisabled;

	// Token: 0x04005A8C RID: 23180
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005A8D RID: 23181
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedBorderColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005A8E RID: 23182
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 hoverBorderColor = new Color32(182, 166, 123, byte.MaxValue);

	// Token: 0x04005A8F RID: 23183
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005A90 RID: 23184
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005A91 RID: 23185
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptySpriteName = "";

	// Token: 0x04005A92 RID: 23186
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptyTooltipName = "";

	// Token: 0x04005A93 RID: 23187
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots equipSlot = EquipmentSlots.Count;

	// Token: 0x04005A94 RID: 23188
	public int PickupSnapDistance = 4;

	// Token: 0x04005A95 RID: 23189
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005A96 RID: 23190
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite itemIcon;

	// Token: 0x04005A97 RID: 23191
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durability;

	// Token: 0x04005A98 RID: 23192
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityRemoveBackground;

	// Token: 0x04005A99 RID: 23193
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityBackground;

	// Token: 0x04005A9A RID: 23194
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x04005A9B RID: 23195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005A9E RID: 23198
	public bool AllowRemoveItem = true;

	// Token: 0x04005AA1 RID: 23201
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;

	// Token: 0x04005AA2 RID: 23202
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
