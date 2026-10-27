using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001044 RID: 4164
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ShapeStack : XUiC_SelectableEntry
{
	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06008302 RID: 33538 RVA: 0x0031DD61 File Offset: 0x0031BF61
	// (set) Token: 0x06008303 RID: 33539 RVA: 0x0031DD69 File Offset: 0x0031BF69
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06008304 RID: 33540 RVA: 0x0031DD72 File Offset: 0x0031BF72
	// (set) Token: 0x06008305 RID: 33541 RVA: 0x0031DD7C File Offset: 0x0031BF7C
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

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06008306 RID: 33542 RVA: 0x0031DE06 File Offset: 0x0031C006
	// (set) Token: 0x06008307 RID: 33543 RVA: 0x0031DE0E File Offset: 0x0031C00E
	public XUiC_ShapeInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x06008308 RID: 33544 RVA: 0x0031DE17 File Offset: 0x0031C017
	// (set) Token: 0x06008309 RID: 33545 RVA: 0x0031DE1F File Offset: 0x0031C01F
	public XUiC_ShapeMaterialInfoWindow MaterialInfoWindow { get; set; }

	// Token: 0x0600830A RID: 33546 RVA: 0x0031DE28 File Offset: 0x0031C028
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

	// Token: 0x0600830B RID: 33547 RVA: 0x0031DE8B File Offset: 0x0031C08B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x0600830C RID: 33548 RVA: 0x0031DEA0 File Offset: 0x0031C0A0
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x0600830D RID: 33549 RVA: 0x0031DEFC File Offset: 0x0031C0FC
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

	// Token: 0x0600830E RID: 33550 RVA: 0x0031E0CC File Offset: 0x0031C2CC
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

	// Token: 0x0600830F RID: 33551 RVA: 0x0031E0E8 File Offset: 0x0031C2E8
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

	// Token: 0x06008310 RID: 33552 RVA: 0x0031E1CD File Offset: 0x0031C3CD
	public void SetSelectedShapeForItem()
	{
		if (!this.IsLocked)
		{
			this.Owner.Owner.ItemValue.Meta = this.ShapeIndex;
			this.Owner.Owner.RefreshItemStack();
		}
		base.IsSelected = true;
	}

	// Token: 0x06008311 RID: 33553 RVA: 0x0031E20C File Offset: 0x0031C40C
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

	// Token: 0x06008312 RID: 33554 RVA: 0x0031E260 File Offset: 0x0031C460
	public XUiC_ShapeStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008313 RID: 33555 RVA: 0x0031E2DC File Offset: 0x0031C4DC
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

	// Token: 0x06008314 RID: 33556 RVA: 0x0031E3C8 File Offset: 0x0031C5C8
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

	// Token: 0x04006124 RID: 24868
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04006125 RID: 24869
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04006126 RID: 24870
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04006127 RID: 24871
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04006128 RID: 24872
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04006129 RID: 24873
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x0400612A RID: 24874
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400612B RID: 24875
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400612C RID: 24876
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x0400612D RID: 24877
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x0400612E RID: 24878
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400612F RID: 24879
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04006130 RID: 24880
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04006131 RID: 24881
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04006132 RID: 24882
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006133 RID: 24883
	public int ShapeIndex = -1;

	// Token: 0x04006135 RID: 24885
	public XUiC_ShapeStackGrid Owner;

	// Token: 0x04006136 RID: 24886
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x04006139 RID: 24889
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x0400613A RID: 24890
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
