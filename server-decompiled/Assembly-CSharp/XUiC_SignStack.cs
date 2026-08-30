using System;
using InControl;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001034 RID: 4148
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignStack : XUiC_SelectableEntry
{
	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x06008390 RID: 33680 RVA: 0x003220E6 File Offset: 0x003202E6
	public SignData SignData
	{
		get
		{
			return this.signData;
		}
	}

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x06008391 RID: 33681 RVA: 0x003220EE File Offset: 0x003202EE
	public string LibraryName
	{
		get
		{
			return this.libraryName;
		}
	}

	// Token: 0x06008392 RID: 33682 RVA: 0x003220F6 File Offset: 0x003202F6
	public XUiC_SignStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x06008393 RID: 33683 RVA: 0x00322124 File Offset: 0x00320324
	// (set) Token: 0x06008394 RID: 33684 RVA: 0x0032212C File Offset: 0x0032032C
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

	// Token: 0x06008395 RID: 33685 RVA: 0x00322297 File Offset: 0x00320497
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

	// Token: 0x06008396 RID: 33686 RVA: 0x003222C6 File Offset: 0x003204C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06008397 RID: 33687 RVA: 0x003222DC File Offset: 0x003204DC
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

	// Token: 0x06008398 RID: 33688 RVA: 0x00322387 File Offset: 0x00320587
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x06008399 RID: 33689 RVA: 0x003223A4 File Offset: 0x003205A4
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

	// Token: 0x0600839A RID: 33690 RVA: 0x003224E9 File Offset: 0x003206E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		XUiView viewComponent = this.viewComponent;
		GlobalSignId globalSignId = this.signId;
		viewComponent.ToolTip = name;
	}

	// Token: 0x0600839B RID: 33691 RVA: 0x00322500 File Offset: 0x00320700
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

	// Token: 0x0600839C RID: 33692 RVA: 0x0008AF5C File Offset: 0x0008915C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600839D RID: 33693 RVA: 0x00322554 File Offset: 0x00320754
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

	// Token: 0x04006172 RID: 24946
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04006173 RID: 24947
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04006174 RID: 24948
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04006175 RID: 24949
	public static Color32 backgroundColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04006176 RID: 24950
	public static Color32 highlightColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006177 RID: 24951
	public static Color32 libraryMarkerColorDefault = new Color32(120, 0, 200, byte.MaxValue);

	// Token: 0x04006178 RID: 24952
	public static Color32 libraryMarkerColorPrefab = new Color32(45, 220, 30, byte.MaxValue);

	// Token: 0x04006179 RID: 24953
	public static Color32 libraryMarkerColorUser = new Color32(0, 170, 190, byte.MaxValue);

	// Token: 0x0400617A RID: 24954
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x0400617B RID: 24955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400617C RID: 24956
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x0400617D RID: 24957
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 libraryColor;

	// Token: 0x0400617E RID: 24958
	public SignData signData;

	// Token: 0x0400617F RID: 24959
	public string libraryName;

	// Token: 0x04006180 RID: 24960
	public Action<XUiC_SignStack> OnBecameSelected;

	// Token: 0x04006181 RID: 24961
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
