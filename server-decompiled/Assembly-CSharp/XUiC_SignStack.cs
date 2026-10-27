using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200106E RID: 4206
[global::UnityEngine.Scripting.Preserve]
public class XUiC_SignStack : XUiC_SelectableEntry
{
	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x0600850A RID: 34058 RVA: 0x0032C207 File Offset: 0x0032A407
	// (set) Token: 0x0600850B RID: 34059 RVA: 0x0032C20F File Offset: 0x0032A40F
	[XuiXmlAttribute("select_color", false)]
	public Color32 SelectColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x0600850C RID: 34060 RVA: 0x0032C218 File Offset: 0x0032A418
	// (set) Token: 0x0600850D RID: 34061 RVA: 0x0032C220 File Offset: 0x0032A420
	[XuiXmlAttribute("background_color", false)]
	public Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x0600850E RID: 34062 RVA: 0x0032C229 File Offset: 0x0032A429
	// (set) Token: 0x0600850F RID: 34063 RVA: 0x0032C231 File Offset: 0x0032A431
	[XuiXmlAttribute("highlight_color", false)]
	public Color32 HighlightColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x06008510 RID: 34064 RVA: 0x0032C23A File Offset: 0x0032A43A
	// (set) Token: 0x06008511 RID: 34065 RVA: 0x0032C242 File Offset: 0x0032A442
	[XuiXmlAttribute("lib_marker_color_default", false)]
	public Color32 LibraryMarkerColorDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x06008512 RID: 34066 RVA: 0x0032C24B File Offset: 0x0032A44B
	// (set) Token: 0x06008513 RID: 34067 RVA: 0x0032C253 File Offset: 0x0032A453
	[XuiXmlAttribute("lib_marker_color_own", false)]
	public Color32 LibraryMarkerColorOwn
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x06008514 RID: 34068 RVA: 0x0032C25C File Offset: 0x0032A45C
	// (set) Token: 0x06008515 RID: 34069 RVA: 0x0032C264 File Offset: 0x0032A464
	[XuiXmlAttribute("lib_marker_color_allies", false)]
	public Color32 LibraryMarkerColorAllies
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x06008516 RID: 34070 RVA: 0x0032C26D File Offset: 0x0032A46D
	// (set) Token: 0x06008517 RID: 34071 RVA: 0x0032C275 File Offset: 0x0032A475
	[XuiXmlAttribute("lib_marker_color_prefab", false)]
	public Color32 LibraryMarkerColorPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x06008518 RID: 34072 RVA: 0x0032C27E File Offset: 0x0032A47E
	// (set) Token: 0x06008519 RID: 34073 RVA: 0x0032C286 File Offset: 0x0032A486
	[XuiXmlAttribute("lib_marker_color_all_prefabs", false)]
	public Color32 LibraryMarkerColorAllPrefabs
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x0600851A RID: 34074 RVA: 0x0032C28F File Offset: 0x0032A48F
	public SignData SignData
	{
		get
		{
			return this.signData;
		}
	}

	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x0600851B RID: 34075 RVA: 0x0032C297 File Offset: 0x0032A497
	public string LibraryName
	{
		get
		{
			return this.libraryName;
		}
	}

	// Token: 0x0600851C RID: 34076 RVA: 0x0032C2A0 File Offset: 0x0032A4A0
	public XUiC_SignStack()
	{
		this.IsDirty = true;
	}

	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x0600851D RID: 34077 RVA: 0x0032C30E File Offset: 0x0032A50E
	// (set) Token: 0x0600851E RID: 34078 RVA: 0x0032C318 File Offset: 0x0032A518
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

	// Token: 0x0600851F RID: 34079 RVA: 0x0032C450 File Offset: 0x0032A650
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

	// Token: 0x06008520 RID: 34080 RVA: 0x0032C4C0 File Offset: 0x0032A6C0
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

	// Token: 0x06008521 RID: 34081 RVA: 0x0032C519 File Offset: 0x0032A719
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

	// Token: 0x06008522 RID: 34082 RVA: 0x0032C548 File Offset: 0x0032A748
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetColor(Color32 color)
	{
		this.background.Color = color;
	}

	// Token: 0x06008523 RID: 34083 RVA: 0x0032C55C File Offset: 0x0032A75C
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

	// Token: 0x06008524 RID: 34084 RVA: 0x0032C607 File Offset: 0x0032A807
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.Standard);
	}

	// Token: 0x06008525 RID: 34085 RVA: 0x0032C624 File Offset: 0x0032A824
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

	// Token: 0x06008526 RID: 34086 RVA: 0x0032C76A File Offset: 0x0032A96A
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		XUiView viewComponent = this.viewComponent;
		GlobalSignId globalSignId = this.signId;
		viewComponent.ToolTip = name;
	}

	// Token: 0x06008527 RID: 34087 RVA: 0x0032C780 File Offset: 0x0032A980
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

	// Token: 0x06008528 RID: 34088 RVA: 0x0008BB50 File Offset: 0x00089D50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x040062BA RID: 25274
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x040062BB RID: 25275
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 currentColor;

	// Token: 0x040062C4 RID: 25284
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite highlightOverlay;

	// Token: 0x040062C5 RID: 25285
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040062C6 RID: 25286
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040062C7 RID: 25287
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 libraryColor;

	// Token: 0x040062C8 RID: 25288
	public SignData signData;

	// Token: 0x040062C9 RID: 25289
	public string libraryName;

	// Token: 0x040062CA RID: 25290
	public Action<XUiC_SignStack> OnBecameSelected;

	// Token: 0x040062CB RID: 25291
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
