using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F7E RID: 3966
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MaterialStack : XUiC_SelectableEntry
{
	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x06007D0A RID: 32010 RVA: 0x002F7580 File Offset: 0x002F5780
	// (set) Token: 0x06007D0B RID: 32011 RVA: 0x002F7588 File Offset: 0x002F5788
	public bool IsLocked
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06007D0C RID: 32012 RVA: 0x002F7591 File Offset: 0x002F5791
	// (set) Token: 0x06007D0D RID: 32013 RVA: 0x002F759C File Offset: 0x002F579C
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

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06007D0E RID: 32014 RVA: 0x002F7746 File Offset: 0x002F5946
	// (set) Token: 0x06007D0F RID: 32015 RVA: 0x002F774E File Offset: 0x002F594E
	public XUiC_MaterialInfoWindow InfoWindow { get; set; }

	// Token: 0x06007D10 RID: 32016 RVA: 0x002F7757 File Offset: 0x002F5957
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

	// Token: 0x06007D11 RID: 32017 RVA: 0x002F7793 File Offset: 0x002F5993
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06007D12 RID: 32018 RVA: 0x002F77A8 File Offset: 0x002F59A8
	public override void Init()
	{
		base.Init();
		this.tintedOverlay = base.GetChildById("tintedOverlay");
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.textMaterial = base.GetChildById("textMaterial").ViewComponent as XUiV_Texture;
		this.textMaterial.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
	}

	// Token: 0x06007D13 RID: 32019 RVA: 0x002F7830 File Offset: 0x002F5A30
	public XUiC_MaterialStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007D14 RID: 32020 RVA: 0x002F789C File Offset: 0x002F5A9C
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

	// Token: 0x06007D15 RID: 32021 RVA: 0x002F7A10 File Offset: 0x002F5C10
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

	// Token: 0x06007D16 RID: 32022 RVA: 0x002F7B38 File Offset: 0x002F5D38
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleItemInspect()
	{
		if (this.textureData != null && this.InfoWindow != null)
		{
			base.IsSelected = true;
		}
	}

	// Token: 0x06007D17 RID: 32023 RVA: 0x002F7B51 File Offset: 0x002F5D51
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = ((this.textureData != null) ? name : string.Empty);
	}

	// Token: 0x06007D18 RID: 32024 RVA: 0x002F7B70 File Offset: 0x002F5D70
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

	// Token: 0x06007D19 RID: 32025 RVA: 0x002F7BC2 File Offset: 0x002F5DC2
	public void ClearSelectedInfoWindow()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetMaterial(null);
		}
	}

	// Token: 0x06007D1A RID: 32026 RVA: 0x002F7BD8 File Offset: 0x002F5DD8
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

	// Token: 0x06007D1B RID: 32027 RVA: 0x002F7C0C File Offset: 0x002F5E0C
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

	// Token: 0x04005C90 RID: 23696
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bHighlightEnabled;

	// Token: 0x04005C91 RID: 23697
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropEnabled = true;

	// Token: 0x04005C92 RID: 23698
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip selectSound;

	// Token: 0x04005C93 RID: 23699
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005C94 RID: 23700
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005C95 RID: 23701
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04005C96 RID: 23702
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005C97 RID: 23703
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 pressColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005C98 RID: 23704
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;

	// Token: 0x04005C99 RID: 23705
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005C9A RID: 23706
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005C9B RID: 23707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController tintedOverlay;

	// Token: 0x04005C9C RID: 23708
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label stackValue;

	// Token: 0x04005C9D RID: 23709
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04005C9E RID: 23710
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005C9F RID: 23711
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textMaterial;

	// Token: 0x04005CA1 RID: 23713
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData textureData;

	// Token: 0x04005CA3 RID: 23715
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startMousePos = Vector3.zero;
}
