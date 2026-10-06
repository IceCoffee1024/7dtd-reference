using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001034 RID: 4148
[global::UnityEngine.Scripting.Preserve]
public class XUiC_ShapeStack : XUiC_SelectableEntry
{
	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x060082B0 RID: 33456 RVA: 0x0031C05D File Offset: 0x0031A25D
	// (set) Token: 0x060082B1 RID: 33457 RVA: 0x0031C065 File Offset: 0x0031A265
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x060082B2 RID: 33458 RVA: 0x0031C06E File Offset: 0x0031A26E
	// (set) Token: 0x060082B3 RID: 33459 RVA: 0x0031C078 File Offset: 0x0031A278
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

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x060082B4 RID: 33460 RVA: 0x0031C102 File Offset: 0x0031A302
	// (set) Token: 0x060082B5 RID: 33461 RVA: 0x0031C10A File Offset: 0x0031A30A
	public XUiC_ShapeInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x060082B6 RID: 33462 RVA: 0x0031C113 File Offset: 0x0031A313
	// (set) Token: 0x060082B7 RID: 33463 RVA: 0x0031C11B File Offset: 0x0031A31B
	public XUiC_ShapeMaterialInfoWindow MaterialInfoWindow { get; set; }

	// Token: 0x060082B8 RID: 33464 RVA: 0x0031C124 File Offset: 0x0031A324
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

	// Token: 0x060082B9 RID: 33465 RVA: 0x0031C187 File Offset: 0x0031A387
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x060082BA RID: 33466 RVA: 0x0031C19C File Offset: 0x0031A39C
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
	}

	// Token: 0x060082BB RID: 33467 RVA: 0x0031C1F8 File Offset: 0x0031A3F8
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

	// Token: 0x060082BC RID: 33468 RVA: 0x0031C3C8 File Offset: 0x0031A5C8
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

	// Token: 0x060082BD RID: 33469 RVA: 0x0031C3E4 File Offset: 0x0031A5E4
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

	// Token: 0x060082BE RID: 33470 RVA: 0x0031C4C9 File Offset: 0x0031A6C9
	public void SetSelectedShapeForItem()
	{
		if (!this.IsLocked)
		{
			this.Owner.Owner.ItemValue.Meta = this.ShapeIndex;
			this.Owner.Owner.RefreshItemStack();
		}
		base.IsSelected = true;
	}

	// Token: 0x060082BF RID: 33471 RVA: 0x0031C508 File Offset: 0x0031A708
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

	// Token: 0x060082C0 RID: 33472 RVA: 0x0031C55C File Offset: 0x0031A75C
	public XUiC_ShapeStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x060082C1 RID: 33473 RVA: 0x0031C5D8 File Offset: 0x0031A7D8
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

	// Token: 0x060082C2 RID: 33474 RVA: 0x0031C6C4 File Offset: 0x0031A8C4
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

	// Token: 0x040060E6 RID: 24806
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x040060E7 RID: 24807
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x040060E8 RID: 24808
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x040060E9 RID: 24809
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x040060EA RID: 24810
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040060EB RID: 24811
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x040060EC RID: 24812
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040060ED RID: 24813
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040060EE RID: 24814
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x040060EF RID: 24815
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x040060F0 RID: 24816
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x040060F1 RID: 24817
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x040060F2 RID: 24818
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x040060F3 RID: 24819
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040060F4 RID: 24820
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040060F5 RID: 24821
	public int ShapeIndex = -1;

	// Token: 0x040060F7 RID: 24823
	public XUiC_ShapeStackGrid Owner;

	// Token: 0x040060F8 RID: 24824
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x040060FB RID: 24827
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;

	// Token: 0x040060FC RID: 24828
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
