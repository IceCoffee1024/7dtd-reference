using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100C RID: 4108
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ShapeStack : XUiC_SelectableEntry
{
	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x060081AF RID: 33199 RVA: 0x003147F1 File Offset: 0x003129F1
	// (set) Token: 0x060081B0 RID: 33200 RVA: 0x003147F9 File Offset: 0x003129F9
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x060081B1 RID: 33201 RVA: 0x00314802 File Offset: 0x00312A02
	// (set) Token: 0x060081B2 RID: 33202 RVA: 0x0031480C File Offset: 0x00312A0C
	public Block BlockData
	{
		get
		{
			return this.blockData;
		}
		set
		{
			if (this.blockData != value)
			{
				this.blockData = value;
				this.IsDirty = true;
				if (this.blockData == null)
				{
					this.viewComponent.ToolTip = string.Empty;
					this.IsLocked = false;
				}
				else
				{
					this.viewComponent.ToolTip = ((this.blockData.GetAutoShapeType() != EAutoShapeType.None) ? this.blockData.GetLocalizedAutoShapeShapeName() : this.blockData.GetLocalizedBlockName());
				}
			}
			base.ViewComponent.Enabled = value != null;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x060081B3 RID: 33203 RVA: 0x00314896 File Offset: 0x00312A96
	// (set) Token: 0x060081B4 RID: 33204 RVA: 0x0031489E File Offset: 0x00312A9E
	public XUiC_ShapeInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x060081B5 RID: 33205 RVA: 0x003148A7 File Offset: 0x00312AA7
	// (set) Token: 0x060081B6 RID: 33206 RVA: 0x003148AF File Offset: 0x00312AAF
	public XUiC_ShapeMaterialInfoWindow MaterialInfoWindow { get; set; }

	// Token: 0x060081B7 RID: 33207 RVA: 0x003148B8 File Offset: 0x00312AB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (isSelected)
		{
			this.SetColor(this.selectColor);
			if (this.xui.CurrentSelectedEntry == this)
			{
				XUiC_ShapeInfoWindow infoWindow = this.InfoWindow;
				if (infoWindow != null)
				{
					infoWindow.SetShape(this.blockData);
				}
				XUiC_ShapeMaterialInfoWindow materialInfoWindow = this.MaterialInfoWindow;
				if (materialInfoWindow == null)
				{
					return;
				}
				materialInfoWindow.SetShape(this.blockData);
				return;
			}
		}
		else
		{
			this.SetColor(XUiC_ShapeStack.backgroundColor);
		}
	}

	// Token: 0x060081B8 RID: 33208 RVA: 0x0031491B File Offset: 0x00312B1B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x060081B9 RID: 33209 RVA: 0x00314930 File Offset: 0x00312B30
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060081BA RID: 33210 RVA: 0x0031498C File Offset: 0x00312B8C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if (base.WindowGroup.isShowing)
		{
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			cursorController.GetScreenPosition();
			bool mouseButtonUp = cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButton(UICamera.MouseButton.LeftButton);
			cursorController.GetMouseButtonUp(UICamera.MouseButton.RightButton);
			cursorController.GetMouseButtonDown(UICamera.MouseButton.RightButton);
			cursorController.GetMouseButton(UICamera.MouseButton.RightButton);
			if (this.isOver && base.ViewComponent.UiTransformIsHovered && base.ViewComponent.EventOnPress)
			{
				if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
				{
					bool wasReleased = guiactions.Submit.WasReleased;
					bool wasReleased2 = guiactions.HalfStack.WasReleased;
					bool wasReleased3 = guiactions.Inspect.WasReleased;
					bool wasReleased4 = guiactions.RightStick.WasReleased;
					if (wasReleased && this.blockData != null)
					{
						this.SetSelectedShapeForItem();
						if (wasReleased4)
						{
							this.xui.playerUI.windowManager.Close("shapes");
						}
					}
				}
				else if (mouseButtonUp && this.blockData != null)
				{
					this.SetSelectedShapeForItem();
					if (InputUtils.ShiftKeyPressed)
					{
						this.xui.playerUI.windowManager.Close("shapes");
					}
				}
			}
			else
			{
				this.currentColor = XUiC_ShapeStack.backgroundColor;
				if (this.highlightOverlay != null)
				{
					this.highlightOverlay.Color = XUiC_ShapeStack.backgroundColor;
				}
				if (!base.IsSelected)
				{
					this.background.Color = this.currentColor;
				}
				this.lastClicked = false;
				if (this.isOver)
				{
					this.isOver = false;
				}
			}
		}
		if (this.IsDirty)
		{
			this.IsDirty = false;
		}
		this.UpdateInput();
	}

	// Token: 0x060081BB RID: 33211 RVA: 0x00314B5C File Offset: 0x00312D5C
	public static string GetFavoritesEntryName(Block _block)
	{
		if (_block == null)
		{
			return null;
		}
		if (_block.GetAutoShapeType() == EAutoShapeType.None)
		{
			return _block.GetBlockName();
		}
		return _block.GetAutoShapeShapeName();
	}

	// Token: 0x060081BC RID: 33212 RVA: 0x00314B78 File Offset: 0x00312D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if (!base.IsSelected || this.BlockData == null)
		{
			return;
		}
		if (this.xui.playerUI.playerInput != null)
		{
			PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
			if ((PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && guiactions.DPad_Right.WasReleased && !this.xui.playerUI.windowManager.IsInputActive()) || (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && guiactions.Inspect.WasReleased))
			{
				EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
				string favoritesEntryName = XUiC_ShapeStack.GetFavoritesEntryName(this.BlockData);
				if (!entityPlayer.favoriteShapes.Remove(favoritesEntryName))
				{
					entityPlayer.favoriteShapes.Add(favoritesEntryName);
				}
				this.Owner.Owner.UpdateAll();
			}
		}
	}

	// Token: 0x060081BD RID: 33213 RVA: 0x00314C5D File Offset: 0x00312E5D
	public void SetSelectedShapeForItem()
	{
		if (!this.IsLocked)
		{
			this.Owner.Owner.ItemValue.Meta = this.ShapeIndex;
			this.Owner.Owner.RefreshItemStack();
		}
		base.IsSelected = true;
	}

	// Token: 0x060081BE RID: 33214 RVA: 0x00314C9C File Offset: 0x00312E9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = XUiC_ShapeStack.highlightColor;
			}
			else
			{
				this.background.Color = XUiC_ShapeStack.backgroundColor;
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x060081BF RID: 33215 RVA: 0x00314CF0 File Offset: 0x00312EF0
	public XUiC_ShapeStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x060081C0 RID: 33216 RVA: 0x00314D6C File Offset: 0x00312F6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "islocked")
		{
			value = this.IsLocked.ToString();
			return true;
		}
		if (bindingName == "itemicon")
		{
			value = ((this.BlockData == null) ? "" : this.BlockData.GetIconName());
			return true;
		}
		if (bindingName == "itemicontint")
		{
			Color32 color = Color.white;
			if (this.BlockData != null)
			{
				color = this.BlockData.CustomIconTint;
			}
			value = this.itemicontintcolorFormatter.Format(color);
			return true;
		}
		if (!(bindingName == "isfavorite"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		string favoritesEntryName = XUiC_ShapeStack.GetFavoritesEntryName(this.BlockData);
		value = (favoritesEntryName != null && this.xui.playerUI.entityPlayer.favoriteShapes.Contains(favoritesEntryName)).ToString();
		return true;
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x00314E58 File Offset: 0x00313058
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "select_color"))
			{
				if (!(name == "press_color"))
				{
					if (!(name == "background_color"))
					{
						if (!(name == "highlight_color"))
						{
							if (!(name == "select_sound"))
							{
								return false;
							}
							this.xui.LoadData<AudioClip>(value, delegate(AudioClip o)
							{
								this.selectSound = o;
							});
						}
						else
						{
							XUiC_ShapeStack.highlightColor = StringParsers.ParseColor32(value);
						}
					}
					else
					{
						XUiC_ShapeStack.backgroundColor = StringParsers.ParseColor32(value);
					}
				}
				else
				{
					this.pressColor = StringParsers.ParseColor32(value);
				}
			}
			else
			{
				this.selectColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x04005FF6 RID: 24566
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005FF7 RID: 24567
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005FF8 RID: 24568
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04005FF9 RID: 24569
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005FFA RID: 24570
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005FFB RID: 24571
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04005FFC RID: 24572
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005FFD RID: 24573
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005FFE RID: 24574
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005FFF RID: 24575
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04006000 RID: 24576
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006001 RID: 24577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04006002 RID: 24578
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04006003 RID: 24579
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04006004 RID: 24580
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006005 RID: 24581
	public int ShapeIndex = -1;

	// Token: 0x04006007 RID: 24583
	public XUiC_ShapeStackGrid Owner;

	// Token: 0x04006008 RID: 24584
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x0400600B RID: 24587
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x0400600C RID: 24588
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
