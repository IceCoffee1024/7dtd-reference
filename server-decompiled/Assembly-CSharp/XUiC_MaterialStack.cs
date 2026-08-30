using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F97 RID: 3991
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MaterialStack : XUiC_SelectableEntry
{
	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06007DFC RID: 32252 RVA: 0x002FBD54 File Offset: 0x002F9F54
	// (set) Token: 0x06007DFD RID: 32253 RVA: 0x002FBD5C File Offset: 0x002F9F5C
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06007DFE RID: 32254 RVA: 0x002FBD65 File Offset: 0x002F9F65
	// (set) Token: 0x06007DFF RID: 32255 RVA: 0x002FBD70 File Offset: 0x002F9F70
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

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06007E00 RID: 32256 RVA: 0x002FBF1A File Offset: 0x002FA11A
	// (set) Token: 0x06007E01 RID: 32257 RVA: 0x002FBF22 File Offset: 0x002FA122
	public XUiC_MaterialInfoWindow InfoWindow { get; set; }

	// Token: 0x06007E02 RID: 32258 RVA: 0x002FBF2B File Offset: 0x002FA12B
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

	// Token: 0x06007E03 RID: 32259 RVA: 0x002FBF67 File Offset: 0x002FA167
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06007E04 RID: 32260 RVA: 0x002FBF7C File Offset: 0x002FA17C
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.textMaterial = base.GetChildById("textMaterial").ViewComponent as XUiV_Texture;
		this.textMaterial.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
	}

	// Token: 0x06007E05 RID: 32261 RVA: 0x002FC004 File Offset: 0x002FA204
	public XUiC_MaterialStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007E06 RID: 32262 RVA: 0x002FC070 File Offset: 0x002FA270
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

	// Token: 0x06007E07 RID: 32263 RVA: 0x002FC1E4 File Offset: 0x002FA3E4
	public void SetSelectedTextureForItem()
	{
		if (!this.IsLocked)
		{
			if (this.xui.playerUI.entityPlayer.inventory.holdingItem is ItemClassBlock)
			{
				this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.TextureFullArray[0] = Chunk.TextureIdxToTextureFullValue64(this.textureData.ID);
			}
			else
			{
				((ItemActionTextureBlock.ItemActionTextureBlockData)this.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[1]).idx = this.textureData.ID;
				this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue.Meta = (int)((byte)this.textureData.ID);
				this.xui.playerUI.entityPlayer.inventory.holdingItemData.actionData[1].invData.itemValue = this.xui.playerUI.entityPlayer.inventory.holdingItemItemValue;
			}
		}
		base.IsSelected = true;
	}

	// Token: 0x06007E08 RID: 32264 RVA: 0x002FC30C File Offset: 0x002FA50C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleItemInspect()
	{
		if (this.textureData != null && this.InfoWindow != null)
		{
			base.IsSelected = true;
		}
	}

	// Token: 0x06007E09 RID: 32265 RVA: 0x002FC325 File Offset: 0x002FA525
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = ((this.textureData != null) ? name : string.Empty);
	}

	// Token: 0x06007E0A RID: 32266 RVA: 0x002FC344 File Offset: 0x002FA544
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

	// Token: 0x06007E0B RID: 32267 RVA: 0x002FC396 File Offset: 0x002FA596
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetMaterial(null);
		}
	}

	// Token: 0x06007E0C RID: 32268 RVA: 0x002FC3AC File Offset: 0x002FA5AC
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

	// Token: 0x06007E0D RID: 32269 RVA: 0x002FC3E0 File Offset: 0x002FA5E0
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

	// Token: 0x04005D57 RID: 23895
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005D58 RID: 23896
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005D59 RID: 23897
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04005D5A RID: 23898
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005D5B RID: 23899
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005D5C RID: 23900
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04005D5D RID: 23901
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005D5E RID: 23902
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005D5F RID: 23903
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005D60 RID: 23904
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005D61 RID: 23905
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005D62 RID: 23906
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04005D63 RID: 23907
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005D64 RID: 23908
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04005D65 RID: 23909
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005D66 RID: 23910
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textMaterial;

	// Token: 0x04005D68 RID: 23912
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData textureData;

	// Token: 0x04005D6A RID: 23914
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
