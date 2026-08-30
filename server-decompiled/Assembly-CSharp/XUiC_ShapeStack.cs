using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001025 RID: 4133
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ShapeStack : XUiC_SelectableEntry
{
	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x060082A1 RID: 33441 RVA: 0x00319019 File Offset: 0x00317219
	// (set) Token: 0x060082A2 RID: 33442 RVA: 0x00319021 File Offset: 0x00317221
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x060082A3 RID: 33443 RVA: 0x0031902A File Offset: 0x0031722A
	// (set) Token: 0x060082A4 RID: 33444 RVA: 0x00319034 File Offset: 0x00317234
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

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x060082A5 RID: 33445 RVA: 0x003190BE File Offset: 0x003172BE
	// (set) Token: 0x060082A6 RID: 33446 RVA: 0x003190C6 File Offset: 0x003172C6
	public XUiC_ShapeInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060082A7 RID: 33447 RVA: 0x003190CF File Offset: 0x003172CF
	// (set) Token: 0x060082A8 RID: 33448 RVA: 0x003190D7 File Offset: 0x003172D7
	public XUiC_ShapeMaterialInfoWindow MaterialInfoWindow { get; set; }

	// Token: 0x060082A9 RID: 33449 RVA: 0x003190E0 File Offset: 0x003172E0
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

	// Token: 0x060082AA RID: 33450 RVA: 0x00319143 File Offset: 0x00317343
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x060082AB RID: 33451 RVA: 0x00319158 File Offset: 0x00317358
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060082AC RID: 33452 RVA: 0x003191B4 File Offset: 0x003173B4
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

	// Token: 0x060082AD RID: 33453 RVA: 0x00319384 File Offset: 0x00317584
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

	// Token: 0x060082AE RID: 33454 RVA: 0x003193A0 File Offset: 0x003175A0
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

	// Token: 0x060082AF RID: 33455 RVA: 0x00319485 File Offset: 0x00317685
	public void SetSelectedShapeForItem()
	{
		if (!this.IsLocked)
		{
			this.Owner.Owner.ItemValue.Meta = this.ShapeIndex;
			this.Owner.Owner.RefreshItemStack();
		}
		base.IsSelected = true;
	}

	// Token: 0x060082B0 RID: 33456 RVA: 0x003194C4 File Offset: 0x003176C4
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

	// Token: 0x060082B1 RID: 33457 RVA: 0x00319518 File Offset: 0x00317718
	public XUiC_ShapeStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x060082B2 RID: 33458 RVA: 0x00319594 File Offset: 0x00317794
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

	// Token: 0x060082B3 RID: 33459 RVA: 0x00319680 File Offset: 0x00317880
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

	// Token: 0x040060BD RID: 24765
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x040060BE RID: 24766
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x040060BF RID: 24767
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x040060C0 RID: 24768
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x040060C1 RID: 24769
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040060C2 RID: 24770
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x040060C3 RID: 24771
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040060C4 RID: 24772
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040060C5 RID: 24773
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x040060C6 RID: 24774
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040060C7 RID: 24775
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040060C8 RID: 24776
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x040060C9 RID: 24777
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x040060CA RID: 24778
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040060CB RID: 24779
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040060CC RID: 24780
	public int ShapeIndex = -1;

	// Token: 0x040060CE RID: 24782
	public XUiC_ShapeStackGrid Owner;

	// Token: 0x040060CF RID: 24783
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x040060D2 RID: 24786
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x040060D3 RID: 24787
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
