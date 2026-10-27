using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB6 RID: 4022
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MaterialStack : XUiC_SelectableEntry
{
	// Token: 0x17000EE9 RID: 3817
	// (get) Token: 0x06007E5B RID: 32347 RVA: 0x00300914 File Offset: 0x002FEB14
	// (set) Token: 0x06007E5C RID: 32348 RVA: 0x0030091C File Offset: 0x002FEB1C
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06007E5D RID: 32349 RVA: 0x00300925 File Offset: 0x002FEB25
	// (set) Token: 0x06007E5E RID: 32350 RVA: 0x00300930 File Offset: 0x002FEB30
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

	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06007E5F RID: 32351 RVA: 0x00300ADA File Offset: 0x002FECDA
	// (set) Token: 0x06007E60 RID: 32352 RVA: 0x00300AE2 File Offset: 0x002FECE2
	public XUiC_MaterialInfoWindow InfoWindow { get; set; }

	// Token: 0x06007E61 RID: 32353 RVA: 0x00300AEB File Offset: 0x002FECEB
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

	// Token: 0x06007E62 RID: 32354 RVA: 0x00300B27 File Offset: 0x002FED27
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06007E63 RID: 32355 RVA: 0x00300B3C File Offset: 0x002FED3C
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.textMaterial = base.GetChildById("textMaterial").ViewComponent as XUiV_Texture;
		this.textMaterial.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
	}

	// Token: 0x06007E64 RID: 32356 RVA: 0x00300BC4 File Offset: 0x002FEDC4
	public XUiC_MaterialStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007E65 RID: 32357 RVA: 0x00300C30 File Offset: 0x002FEE30
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

	// Token: 0x06007E66 RID: 32358 RVA: 0x00300DA4 File Offset: 0x002FEFA4
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

	// Token: 0x06007E67 RID: 32359 RVA: 0x00300E7B File Offset: 0x002FF07B
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleItemInspect()
	{
		if (this.textureData != null && this.InfoWindow != null)
		{
			base.IsSelected = true;
		}
	}

	// Token: 0x06007E68 RID: 32360 RVA: 0x00300E94 File Offset: 0x002FF094
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = ((this.textureData != null) ? name : string.Empty);
	}

	// Token: 0x06007E69 RID: 32361 RVA: 0x00300EB4 File Offset: 0x002FF0B4
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

	// Token: 0x06007E6A RID: 32362 RVA: 0x00300F06 File Offset: 0x002FF106
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetMaterial(null);
		}
	}

	// Token: 0x06007E6B RID: 32363 RVA: 0x00300F1C File Offset: 0x002FF11C
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

	// Token: 0x06007E6C RID: 32364 RVA: 0x00300F50 File Offset: 0x002FF150
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

	// Token: 0x04005DBB RID: 23995
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005DBC RID: 23996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005DBD RID: 23997
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04005DBE RID: 23998
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005DBF RID: 23999
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005DC0 RID: 24000
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04005DC1 RID: 24001
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005DC2 RID: 24002
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005DC3 RID: 24003
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005DC4 RID: 24004
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005DC5 RID: 24005
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005DC6 RID: 24006
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04005DC7 RID: 24007
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005DC8 RID: 24008
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04005DC9 RID: 24009
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005DCA RID: 24010
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textMaterial;

	// Token: 0x04005DCC RID: 24012
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData textureData;

	// Token: 0x04005DCE RID: 24014
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
