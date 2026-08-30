using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200104D RID: 4173
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignStack : XUiC_SelectableEntry
{
	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x06008482 RID: 33922 RVA: 0x0032690E File Offset: 0x00324B0E
	public SignData SignData
	{
		get
		{
			return this.signData;
		}
	}

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x06008483 RID: 33923 RVA: 0x00326916 File Offset: 0x00324B16
	public string LibraryName
	{
		get
		{
			return this.libraryName;
		}
	}

	// Token: 0x06008484 RID: 33924 RVA: 0x0032691E File Offset: 0x00324B1E
	public XUiC_SignStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x06008485 RID: 33925 RVA: 0x0032694C File Offset: 0x00324B4C
	// (set) Token: 0x06008486 RID: 33926 RVA: 0x00326954 File Offset: 0x00324B54
	public GlobalSignId SignId
	{
		get
		{
			return this.signId;
		}
		set
		{
			if (this.signId != value)
			{
				this.signId = value;
				this.IsDirty = true;
				if (!this.signId.IsValid)
				{
					this.SetItemNameText("");
					this.libraryColor = XUiC_SignStack.backgroundColor;
				}
				else
				{
					this.libraryName = SignDataManager.GetLibraryNiceName(this.signId);
					if (!SignDataManager.Instance.TryGetSignData(this.signId, out this.signData))
					{
						this.SetItemNameText("");
						this.libraryColor = XUiC_SignStack.backgroundColor;
						Log.Error("Failed to retrieve sign data for global id: '" + this.signId.ToString() + "'.");
					}
					else
					{
						this.SetItemNameText(this.signData.name + " (" + this.libraryName + ")");
						string libraryId = this.signId.libraryId;
						if (!(libraryId == "[D]"))
						{
							if (!(libraryId == "[U]"))
							{
								this.libraryColor = XUiC_SignStack.libraryMarkerColorPrefab;
							}
							else
							{
								this.libraryColor = XUiC_SignStack.libraryMarkerColorUser;
							}
						}
						else
						{
							this.libraryColor = XUiC_SignStack.libraryMarkerColorDefault;
						}
					}
				}
			}
			this.signMaterial.IsVisible = this.signId.IsValid;
			base.ViewComponent.Enabled = this.signId.IsValid;
			this.SetColor(this.libraryColor);
			base.RefreshBindings();
		}
	}

	// Token: 0x06008487 RID: 33927 RVA: 0x00326ABF File Offset: 0x00324CBF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (!isSelected)
		{
			this.SetColor(this.libraryColor);
			return;
		}
		this.SetColor(this.selectColor);
		Action<XUiC_SignStack> onBecameSelected = this.OnBecameSelected;
		if (onBecameSelected == null)
		{
			return;
		}
		onBecameSelected(this);
	}

	// Token: 0x06008488 RID: 33928 RVA: 0x00326AEE File Offset: 0x00324CEE
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06008489 RID: 33929 RVA: 0x00326B04 File Offset: 0x00324D04
	public override void Init()
	{
		base.Init();
		this.highlightOverlay = base.GetChildById("highlightOverlay").ViewComponent as XUiV_Sprite;
		this.background = base.GetChildById("background").ViewComponent as XUiV_Sprite;
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		this.signMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
	}

	// Token: 0x0600848A RID: 33930 RVA: 0x00326BAF File Offset: 0x00324DAF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x0600848B RID: 33931 RVA: 0x00326BCC File Offset: 0x00324DCC
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
					if (guiactions.Submit.WasReleased)
					{
						GlobalSignId globalSignId = this.signId;
						base.IsSelected = true;
					}
				}
				else if (mouseButtonUp)
				{
					GlobalSignId globalSignId2 = this.signId;
					base.IsSelected = true;
				}
			}
			else
			{
				this.currentColor = this.libraryColor;
				if (this.highlightOverlay != null)
				{
					this.highlightOverlay.Color = XUiC_SignStack.backgroundColor;
				}
				if (!base.IsSelected)
				{
					this.background.Color = this.currentColor;
				}
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

	// Token: 0x0600848C RID: 33932 RVA: 0x00326D11 File Offset: 0x00324F11
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		XUiView viewComponent = this.viewComponent;
		GlobalSignId globalSignId = this.signId;
		viewComponent.ToolTip = name;
	}

	// Token: 0x0600848D RID: 33933 RVA: 0x00326D28 File Offset: 0x00324F28
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = XUiC_SignStack.highlightColor;
			}
			else
			{
				this.background.Color = this.libraryColor;
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x0600848E RID: 33934 RVA: 0x0008B69C File Offset: 0x0008989C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600848F RID: 33935 RVA: 0x00326D7C File Offset: 0x00324F7C
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "select_color"))
			{
				if (!(name == "background_color"))
				{
					if (!(name == "highlight_color"))
					{
						if (!(name == "lib_marker_color_default"))
						{
							if (!(name == "lib_marker_color_prefab"))
							{
								return false;
							}
							XUiC_SignStack.libraryMarkerColorPrefab = StringParsers.ParseColor32(value);
						}
						else
						{
							XUiC_SignStack.libraryMarkerColorDefault = StringParsers.ParseColor32(value);
						}
					}
					else
					{
						XUiC_SignStack.highlightColor = StringParsers.ParseColor32(value);
					}
				}
				else
				{
					XUiC_SignStack.backgroundColor = StringParsers.ParseColor32(value);
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

	// Token: 0x04006239 RID: 25145
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x0400623A RID: 25146
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x0400623B RID: 25147
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x0400623C RID: 25148
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x0400623D RID: 25149
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x0400623E RID: 25150
	public static Color32 libraryMarkerColorDefault = new Color32(120, 0, 200, byte.MaxValue);

	// Token: 0x0400623F RID: 25151
	public static Color32 libraryMarkerColorPrefab = new Color32(45, 220, 30, byte.MaxValue);

	// Token: 0x04006240 RID: 25152
	public static Color32 libraryMarkerColorUser = new Color32(0, 170, 190, byte.MaxValue);

	// Token: 0x04006241 RID: 25153
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04006242 RID: 25154
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006243 RID: 25155
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006244 RID: 25156
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 libraryColor;

	// Token: 0x04006245 RID: 25157
	public SignData signData;

	// Token: 0x04006246 RID: 25158
	public string libraryName;

	// Token: 0x04006247 RID: 25159
	public Action<XUiC_SignStack> OnBecameSelected;

	// Token: 0x04006248 RID: 25160
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
