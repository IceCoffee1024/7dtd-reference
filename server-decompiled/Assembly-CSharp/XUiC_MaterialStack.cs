using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FA6 RID: 4006
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MaterialStack : XUiC_SelectableEntry
{
	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x06007E0C RID: 32268 RVA: 0x002FEDF8 File Offset: 0x002FCFF8
	// (set) Token: 0x06007E0D RID: 32269 RVA: 0x002FEE00 File Offset: 0x002FD000
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x06007E0E RID: 32270 RVA: 0x002FEE09 File Offset: 0x002FD009
	// (set) Token: 0x06007E0F RID: 32271 RVA: 0x002FEE14 File Offset: 0x002FD014
	public BlockTextureData TextureData
	{
		get
		{
			return this.textureData;
		}
		set
		{
			this.textMaterial.IsVisible = false;
			base.ViewComponent.Enabled = value != null;
			if (this.textureData != value)
			{
				this.textureData = value;
				this.IsDirty = true;
				if (this.textureData == null)
				{
					this.SetItemNameText("");
					this.IsLocked = false;
				}
				else
				{
					this.textMaterial.IsVisible = true;
					MeshDescription meshDescription = MeshDescription.meshes[0];
					int textureID = (int)this.textureData.TextureID;
					Rect rect;
					if (textureID == 0)
					{
						rect = WorldConstants.uvRectZero;
					}
					else
					{
						rect = meshDescription.textureAtlas.uvMapping[textureID].uv;
					}
					this.textMaterial.Texture = meshDescription.textureAtlas.diffuseTexture;
					if (meshDescription.bTextureArray)
					{
						this.textMaterial.Material.SetTexture("_BumpMap", meshDescription.textureAtlas.normalTexture);
						this.textMaterial.Material.SetFloat("_Index", (float)meshDescription.textureAtlas.uvMapping[textureID].index);
						this.textMaterial.Material.SetFloat("_Size", (float)meshDescription.textureAtlas.uvMapping[textureID].blockW);
					}
					else
					{
						this.textMaterial.UVRect = rect;
					}
					this.SetItemNameText(string.Format("({0}) {1}", this.textureData.ID, this.textureData.LocalizedName));
				}
			}
			if (this.textureData != null)
			{
				if (!(this.textureData.LockedByPerk != ""))
				{
					this.IsLocked = false;
				}
				this.textMaterial.IsVisible = true;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x17000EE0 RID: 3808
	// (get) Token: 0x06007E10 RID: 32272 RVA: 0x002FEFBE File Offset: 0x002FD1BE
	// (set) Token: 0x06007E11 RID: 32273 RVA: 0x002FEFC6 File Offset: 0x002FD1C6
	public XUiC_MaterialInfoWindow InfoWindow { get; set; }

	// Token: 0x06007E12 RID: 32274 RVA: 0x002FEFCF File Offset: 0x002FD1CF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (isSelected)
		{
			this.SetColor(this.selectColor);
			if (this.xui.CurrentSelectedEntry == this)
			{
				this.InfoWindow.SetMaterial(this.textureData);
				return;
			}
		}
		else
		{
			this.SetColor(XUiC_MaterialStack.backgroundColor);
		}
	}

	// Token: 0x06007E13 RID: 32275 RVA: 0x002FF00B File Offset: 0x002FD20B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06007E14 RID: 32276 RVA: 0x002FF020 File Offset: 0x002FD220
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.textMaterial = base.GetChildById("textMaterial").ViewComponent as XUiV_Texture;
		this.textMaterial.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
	}

	// Token: 0x06007E15 RID: 32277 RVA: 0x002FF0A8 File Offset: 0x002FD2A8
	public XUiC_MaterialStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007E16 RID: 32278 RVA: 0x002FF114 File Offset: 0x002FD314
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
					if (wasReleased && this.textureData != null)
					{
						this.SetSelectedTextureForItem();
					}
				}
				else if (mouseButtonUp && this.textureData != null)
				{
					this.SetSelectedTextureForItem();
				}
			}
			else
			{
				this.currentColor = XUiC_MaterialStack.backgroundColor;
				if (this.highlightOverlay != null)
				{
					this.highlightOverlay.Color = XUiC_MaterialStack.backgroundColor;
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
	}

	// Token: 0x06007E17 RID: 32279 RVA: 0x002FF288 File Offset: 0x002FD488
	public void SetSelectedTextureForItem()
	{
		if (!this.IsLocked)
		{
			if (this.xui.playerUI.entityPlayer.inventory.holdingItem is ItemClassBlock)
			{
				this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.SetTextureFull(0, Chunk.TextureIdxToTextureFullValue64(this.textureData.ID));
			}
			else
			{
				((ItemActionTextureBlock.ItemActionTextureBlockData)this.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[1]).idx = this.textureData.ID;
				this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.Meta = (int)((byte)this.textureData.ID);
			}
		}
		base.IsSelected = true;
	}

	// Token: 0x06007E18 RID: 32280 RVA: 0x002FF35F File Offset: 0x002FD55F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleItemInspect()
	{
		if (this.textureData != null && this.InfoWindow != null)
		{
			base.IsSelected = true;
		}
	}

	// Token: 0x06007E19 RID: 32281 RVA: 0x002FF378 File Offset: 0x002FD578
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = ((this.textureData != null) ? name : string.Empty);
	}

	// Token: 0x06007E1A RID: 32282 RVA: 0x002FF398 File Offset: 0x002FD598
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = XUiC_MaterialStack.highlightColor;
			}
			else
			{
				this.background.Color = XUiC_MaterialStack.backgroundColor;
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x06007E1B RID: 32283 RVA: 0x002FF3EA File Offset: 0x002FD5EA
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetMaterial(null);
		}
	}

	// Token: 0x06007E1C RID: 32284 RVA: 0x002FF400 File Offset: 0x002FD600
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "islocked")
		{
			value = this.IsLocked.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007E1D RID: 32285 RVA: 0x002FF434 File Offset: 0x002FD634
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
							XUiC_MaterialStack.highlightColor = StringParsers.ParseColor32(value);
						}
					}
					else
					{
						XUiC_MaterialStack.backgroundColor = StringParsers.ParseColor32(value);
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

	// Token: 0x04005D80 RID: 23936
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005D81 RID: 23937
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005D82 RID: 23938
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04005D83 RID: 23939
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005D84 RID: 23940
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005D85 RID: 23941
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04005D86 RID: 23942
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005D87 RID: 23943
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005D88 RID: 23944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005D89 RID: 23945
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005D8A RID: 23946
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005D8B RID: 23947
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04005D8C RID: 23948
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005D8D RID: 23949
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04005D8E RID: 23950
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005D8F RID: 23951
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textMaterial;

	// Token: 0x04005D91 RID: 23953
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData textureData;

	// Token: 0x04005D93 RID: 23955
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
