using System;
using System.Globalization;
using Audio;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F1B RID: 3867
[global::UnityEngine.Scripting.Preserve]
public class XUiC_EquipmentStack : XUiC_SelectableEntry
{
	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x0600795A RID: 31066 RVA: 0x002DC136 File Offset: 0x002DA336
	// (set) Token: 0x0600795B RID: 31067 RVA: 0x002DC13E File Offset: 0x002DA33E
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

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x0600795C RID: 31068 RVA: 0x002DC147 File Offset: 0x002DA347
	public int SlotNumber
	{
		get
		{
			return (int)this.EquipSlot;
		}
	}

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x0600795D RID: 31069 RVA: 0x002DC150 File Offset: 0x002DA350
	// (remove) Token: 0x0600795E RID: 31070 RVA: 0x002DC188 File Offset: 0x002DA388
	public event XUiEvent_SlotChangedEventHandler SlotChangedEvent;

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x0600795F RID: 31071 RVA: 0x002DC1BD File Offset: 0x002DA3BD
	// (set) Token: 0x06007960 RID: 31072 RVA: 0x002DC1C5 File Offset: 0x002DA3C5
	public float HoverIconGrow
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x06007961 RID: 31073 RVA: 0x002DC1CE File Offset: 0x002DA3CE
	// (set) Token: 0x06007962 RID: 31074 RVA: 0x002DC1D8 File Offset: 0x002DA3D8
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

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x06007963 RID: 31075 RVA: 0x002DC272 File Offset: 0x002DA472
	// (set) Token: 0x06007964 RID: 31076 RVA: 0x002DC27A File Offset: 0x002DA47A
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

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x06007965 RID: 31077 RVA: 0x002DC2AE File Offset: 0x002DA4AE
	// (set) Token: 0x06007966 RID: 31078 RVA: 0x002DC2B6 File Offset: 0x002DA4B6
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06007967 RID: 31079 RVA: 0x002DC2BF File Offset: 0x002DA4BF
	// (set) Token: 0x06007968 RID: 31080 RVA: 0x002DC2C7 File Offset: 0x002DA4C7
	public XUiC_CharacterFrameWindow FrameWindow { get; set; }

	// Token: 0x06007969 RID: 31081 RVA: 0x002DC2D0 File Offset: 0x002DA4D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool _isSelected)
	{
		this.SetBorderColor(_isSelected ? this.selectedBorderColor : this.normalBorderColor);
	}

	// Token: 0x0600796A RID: 31082 RVA: 0x002DC2E9 File Offset: 0x002DA4E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBorderColor(Color32 _color)
	{
		this.background.Color = _color;
	}

	// Token: 0x0600796B RID: 31083 RVA: 0x002DC2FC File Offset: 0x002DA4FC
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

	// Token: 0x0600796C RID: 31084 RVA: 0x002DC444 File Offset: 0x002DA644
	public XUiC_EquipmentStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600796D RID: 31085 RVA: 0x002DC504 File Offset: 0x002DA704
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

	// Token: 0x0600796E RID: 31086 RVA: 0x002DCB4F File Offset: 0x002DAD4F
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

	// Token: 0x0600796F RID: 31087 RVA: 0x002DCB80 File Offset: 0x002DAD80
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

	// Token: 0x06007970 RID: 31088 RVA: 0x002DCBD7 File Offset: 0x002DADD7
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleClickComplete()
	{
		this.lastClicked = false;
		if (this.itemValue.IsEmpty())
		{
			this.SetBorderColor(this.normalBorderColor);
		}
	}

	// Token: 0x06007971 RID: 31089 RVA: 0x002DCBFC File Offset: 0x002DADFC
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

	// Token: 0x06007972 RID: 31090 RVA: 0x002DCDC0 File Offset: 0x002DAFC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SwapItem()
	{
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (this.AllowRemoveItem)
		{
			this.xui.DragAndDropWindow.CurrentStack = this.itemStack.Clone();
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
			this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
		}
		this.ItemStack = currentStack.Clone();
		XUiEvent_SlotChangedEventHandler slotChangedEvent = this.SlotChangedEvent;
		if (slotChangedEvent == null)
		{
			return;
		}
		slotChangedEvent(this.SlotNumber, this.itemStack);
	}

	// Token: 0x06007973 RID: 31091 RVA: 0x002DCEB8 File Offset: 0x002DB0B8
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

	// Token: 0x06007974 RID: 31092 RVA: 0x002DCFB0 File Offset: 0x002DB1B0
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

	// Token: 0x06007975 RID: 31093 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0400599C RID: 22940
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x0400599D RID: 22941
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue = new ItemValue();

	// Token: 0x0400599E RID: 22942
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x0400599F RID: 22943
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x040059A0 RID: 22944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDisabled;

	// Token: 0x040059A1 RID: 22945
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040059A2 RID: 22946
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedBorderColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040059A3 RID: 22947
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 hoverBorderColor = new Color32(182, 166, 123, byte.MaxValue);

	// Token: 0x040059A4 RID: 22948
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 normalBorderColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040059A5 RID: 22949
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x040059A6 RID: 22950
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptySpriteName = "";

	// Token: 0x040059A7 RID: 22951
	[PublicizedFrom(EAccessModifier.Private)]
	public string emptyTooltipName = "";

	// Token: 0x040059A8 RID: 22952
	[PublicizedFrom(EAccessModifier.Private)]
	public EquipmentSlots equipSlot = EquipmentSlots.Count;

	// Token: 0x040059A9 RID: 22953
	public int PickupSnapDistance = 4;

	// Token: 0x040059AA RID: 22954
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x040059AB RID: 22955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite itemIcon;

	// Token: 0x040059AC RID: 22956
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durability;

	// Token: 0x040059AD RID: 22957
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityRemoveBackground;

	// Token: 0x040059AE RID: 22958
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite durabilityBackground;

	// Token: 0x040059AF RID: 22959
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite lockTypeIcon;

	// Token: 0x040059B0 RID: 22960
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040059B3 RID: 22963
	public bool AllowRemoveItem = true;

	// Token: 0x040059B6 RID: 22966
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;

	// Token: 0x040059B7 RID: 22967
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
