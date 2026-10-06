using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200105E RID: 4190
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignStack : XUiC_SelectableEntry
{
	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x060084B8 RID: 33976 RVA: 0x0032A503 File Offset: 0x00328703
	// (set) Token: 0x060084B9 RID: 33977 RVA: 0x0032A50B File Offset: 0x0032870B
	[XuiXmlAttribute("select_color", false)]
	public Color32 SelectColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x060084BA RID: 33978 RVA: 0x0032A514 File Offset: 0x00328714
	// (set) Token: 0x060084BB RID: 33979 RVA: 0x0032A51C File Offset: 0x0032871C
	[XuiXmlAttribute("background_color", false)]
	public Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x060084BC RID: 33980 RVA: 0x0032A525 File Offset: 0x00328725
	// (set) Token: 0x060084BD RID: 33981 RVA: 0x0032A52D File Offset: 0x0032872D
	[XuiXmlAttribute("highlight_color", false)]
	public Color32 HighlightColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x060084BE RID: 33982 RVA: 0x0032A536 File Offset: 0x00328736
	// (set) Token: 0x060084BF RID: 33983 RVA: 0x0032A53E File Offset: 0x0032873E
	[XuiXmlAttribute("lib_marker_color_default", false)]
	public Color32 LibraryMarkerColorDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060084C0 RID: 33984 RVA: 0x0032A547 File Offset: 0x00328747
	// (set) Token: 0x060084C1 RID: 33985 RVA: 0x0032A54F File Offset: 0x0032874F
	[XuiXmlAttribute("lib_marker_color_own", false)]
	public Color32 LibraryMarkerColorOwn
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x060084C2 RID: 33986 RVA: 0x0032A558 File Offset: 0x00328758
	// (set) Token: 0x060084C3 RID: 33987 RVA: 0x0032A560 File Offset: 0x00328760
	[XuiXmlAttribute("lib_marker_color_allies", false)]
	public Color32 LibraryMarkerColorAllies
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x060084C4 RID: 33988 RVA: 0x0032A569 File Offset: 0x00328769
	// (set) Token: 0x060084C5 RID: 33989 RVA: 0x0032A571 File Offset: 0x00328771
	[XuiXmlAttribute("lib_marker_color_prefab", false)]
	public Color32 LibraryMarkerColorPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x060084C6 RID: 33990 RVA: 0x0032A57A File Offset: 0x0032877A
	// (set) Token: 0x060084C7 RID: 33991 RVA: 0x0032A582 File Offset: 0x00328782
	[XuiXmlAttribute("lib_marker_color_all_prefabs", false)]
	public Color32 LibraryMarkerColorAllPrefabs
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x060084C8 RID: 33992 RVA: 0x0032A58B File Offset: 0x0032878B
	public SignData SignData
	{
		get
		{
			return this.signData;
		}
	}

	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x060084C9 RID: 33993 RVA: 0x0032A593 File Offset: 0x00328793
	public string LibraryName
	{
		get
		{
			return this.libraryName;
		}
	}

	// Token: 0x060084CA RID: 33994 RVA: 0x0032A59C File Offset: 0x0032879C
	public XUiC_SignStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x060084CB RID: 33995 RVA: 0x0032A60A File Offset: 0x0032880A
	// (set) Token: 0x060084CC RID: 33996 RVA: 0x0032A614 File Offset: 0x00328814
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
					this.libraryColor = this.BackgroundColor;
				}
				else
				{
					this.libraryName = SignDataManager.GetLibraryNiceName(this.signId);
					if (!SignDataManager.Instance.TryGetSignData(this.signId, out this.signData))
					{
						this.SetItemNameText("");
						this.libraryColor = this.BackgroundColor;
						Log.Error("Failed to retrieve sign data for global id: '" + this.signId.ToString() + "'.");
					}
					else
					{
						this.SetItemNameText(this.signData.name + " (" + this.libraryName + ")");
						this.libraryColor = this.GetLibraryMarkerColor(this.signId.libraryId);
					}
				}
			}
			this.signMaterial.IsVisible = this.signId.IsValid;
			base.ViewComponent.Enabled = this.signId.IsValid;
			this.SetColor(this.libraryColor);
			base.RefreshBindings();
		}
	}

	// Token: 0x060084CD RID: 33997 RVA: 0x0032A74C File Offset: 0x0032894C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 GetLibraryMarkerColor(string libraryId)
	{
		if (libraryId == "[D]")
		{
			return this.LibraryMarkerColorDefault;
		}
		if (libraryId == "[U]" || libraryId == PlatformManager.InternalLocalUserIdentifier.CombinedString)
		{
			return this.LibraryMarkerColorOwn;
		}
		if (SignDataManager.IsPlayerLibraryId(libraryId))
		{
			return this.LibraryMarkerColorAllies;
		}
		if (!(libraryId == XUiC_SignStack.GetCurrentPrefabName()))
		{
			return this.LibraryMarkerColorAllPrefabs;
		}
		return this.LibraryMarkerColorPrefab;
	}

	// Token: 0x060084CE RID: 33998 RVA: 0x0032A7BC File Offset: 0x003289BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetCurrentPrefabName()
	{
		if (PrefabEditModeManager.Instance.IsActive())
		{
			return PrefabEditModeManager.Instance.LoadedPrefab.FileNameNoExtension;
		}
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return null;
		}
		EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return null;
		}
		PrefabInstance prefab = primaryPlayer.prefab;
		if (prefab == null)
		{
			return null;
		}
		return prefab.location.FileNameNoExtension;
	}

	// Token: 0x060084CF RID: 33999 RVA: 0x0032A815 File Offset: 0x00328A15
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (!isSelected)
		{
			this.SetColor(this.libraryColor);
			return;
		}
		this.SetColor(this.SelectColor);
		Action<XUiC_SignStack> onBecameSelected = this.OnBecameSelected;
		if (onBecameSelected == null)
		{
			return;
		}
		onBecameSelected(this);
	}

	// Token: 0x060084D0 RID: 34000 RVA: 0x0032A844 File Offset: 0x00328A44
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x060084D1 RID: 34001 RVA: 0x0032A858 File Offset: 0x00328A58
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

	// Token: 0x060084D2 RID: 34002 RVA: 0x0032A903 File Offset: 0x00328B03
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x060084D3 RID: 34003 RVA: 0x0032A920 File Offset: 0x00328B20
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
					this.highlightOverlay.Color = this.BackgroundColor;
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

	// Token: 0x060084D4 RID: 34004 RVA: 0x0032AA66 File Offset: 0x00328C66
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		XUiView viewComponent = this.viewComponent;
		GlobalSignId globalSignId = this.signId;
		viewComponent.ToolTip = name;
	}

	// Token: 0x060084D5 RID: 34005 RVA: 0x0032AA7C File Offset: 0x00328C7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isOver = _isOver;
		if (!base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = this.HighlightColor;
			}
			else
			{
				this.background.Color = this.libraryColor;
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x060084D6 RID: 34006 RVA: 0x0008B91C File Offset: 0x00089B1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0400627C RID: 25212
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x0400627D RID: 25213
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x04006286 RID: 25222
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x04006287 RID: 25223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006288 RID: 25224
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006289 RID: 25225
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 libraryColor;

	// Token: 0x0400628A RID: 25226
	public SignData signData;

	// Token: 0x0400628B RID: 25227
	public string libraryName;

	// Token: 0x0400628C RID: 25228
	public Action<XUiC_SignStack> OnBecameSelected;

	// Token: 0x0400628D RID: 25229
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
