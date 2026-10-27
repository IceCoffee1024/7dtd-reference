using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E8E RID: 3726
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_ComboBoxBase : XUiController
{
	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06007256 RID: 29270 RVA: 0x002BF714 File Offset: 0x002BD914
	// (remove) Token: 0x06007257 RID: 29271 RVA: 0x002BF74C File Offset: 0x002BD94C
	public event XUiC_ComboBoxBase.XUiEvent_GenericValueChanged OnValueChangedGeneric;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06007258 RID: 29272 RVA: 0x002BF784 File Offset: 0x002BD984
	// (remove) Token: 0x06007259 RID: 29273 RVA: 0x002BF7BC File Offset: 0x002BD9BC
	public event XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged OnHoveredStateChanged;

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x0600725A RID: 29274 RVA: 0x002BF7F1 File Offset: 0x002BD9F1
	// (set) Token: 0x0600725B RID: 29275 RVA: 0x002BF7F9 File Offset: 0x002BD9F9
	[XuiXmlAttribute("combo_enabled", false)]
	[XuiXmlBinding("combo_enabled")]
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (value == this.enabled)
			{
				return;
			}
			this.enabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x0600725C RID: 29276 RVA: 0x002BF813 File Offset: 0x002BDA13
	// (set) Token: 0x0600725D RID: 29277 RVA: 0x002BF81B File Offset: 0x002BDA1B
	[XuiXmlBinding("show_buttons")]
	public bool ShowButtons
	{
		get
		{
			return this.showButtons;
		}
		set
		{
			if (value == this.showButtons)
			{
				return;
			}
			this.showButtons = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x0600725E RID: 29278 RVA: 0x002BF835 File Offset: 0x002BDA35
	// (set) Token: 0x0600725F RID: 29279 RVA: 0x002BF83D File Offset: 0x002BDA3D
	public Color TextColor
	{
		get
		{
			return this.ColorEnabled;
		}
		set
		{
			if (this.ColorEnabled == value)
			{
				return;
			}
			this.ColorEnabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06007260 RID: 29280
	// (set) Token: 0x06007261 RID: 29281
	public abstract long ValueGeneric { get; set; }

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06007262 RID: 29282
	// (set) Token: 0x06007263 RID: 29283
	public abstract long ValueMinGeneric { get; set; }

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06007264 RID: 29284
	// (set) Token: 0x06007265 RID: 29285
	public abstract long ValueMaxGeneric { get; set; }

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x06007266 RID: 29286 RVA: 0x002BF85C File Offset: 0x002BDA5C
	// (set) Token: 0x06007267 RID: 29287 RVA: 0x002BF864 File Offset: 0x002BDA64
	[XuiXmlAttribute("scroll_by_increment", false)]
	public bool ScrollByIncrement
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x06007268 RID: 29288 RVA: 0x002BF86D File Offset: 0x002BDA6D
	// (set) Token: 0x06007269 RID: 29289 RVA: 0x002BF875 File Offset: 0x002BDA75
	[XuiXmlAttribute("enabled_color", false)]
	public Color ColorEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.colorEnabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.colorEnabled == value)
			{
				return;
			}
			this.colorEnabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x0600726A RID: 29290 RVA: 0x002BF894 File Offset: 0x002BDA94
	// (set) Token: 0x0600726B RID: 29291 RVA: 0x002BF89C File Offset: 0x002BDA9C
	[XuiXmlAttribute("disabled_color", false)]
	public Color ColorDisabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.colorDisabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.colorDisabled == value)
			{
				return;
			}
			this.colorDisabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x0600726C RID: 29292 RVA: 0x002BF8BB File Offset: 0x002BDABB
	// (set) Token: 0x0600726D RID: 29293 RVA: 0x002BF8C3 File Offset: 0x002BDAC3
	[XuiXmlAttribute("bg_color", false)]
	[XuiXmlBinding("bgcolor")]
	public Color ColorBg
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.colorBg;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.colorBg == value)
			{
				return;
			}
			this.colorBg = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x0600726E RID: 29294 RVA: 0x002BF8E2 File Offset: 0x002BDAE2
	// (set) Token: 0x0600726F RID: 29295 RVA: 0x002BF8EA File Offset: 0x002BDAEA
	[XuiXmlAttribute("enabled_fill_color", false)]
	public Color ColorFillEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.colorFillEnabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.colorFillEnabled == value)
			{
				return;
			}
			this.colorFillEnabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x06007270 RID: 29296 RVA: 0x002BF909 File Offset: 0x002BDB09
	// (set) Token: 0x06007271 RID: 29297 RVA: 0x002BF911 File Offset: 0x002BDB11
	[XuiXmlAttribute("disabled_fill_color", false)]
	public Color ColorFillDisabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.colorFillDisabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.colorFillDisabled == value)
			{
				return;
			}
			this.colorFillDisabled = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x06007272 RID: 29298 RVA: 0x002BF930 File Offset: 0x002BDB30
	// (set) Token: 0x06007273 RID: 29299 RVA: 0x002BF938 File Offset: 0x002BDB38
	[XuiXmlAttribute("index_markers", false)]
	public bool IndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06007274 RID: 29300
	public abstract int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x06007275 RID: 29301
	public abstract int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x06007276 RID: 29302 RVA: 0x002BF941 File Offset: 0x002BDB41
	// (set) Token: 0x06007277 RID: 29303 RVA: 0x002BF949 File Offset: 0x002BDB49
	[XuiXmlAttribute("index_marker_spacing", false)]
	public int IndexMarkerSpacing
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 5;

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x06007278 RID: 29304 RVA: 0x002BF952 File Offset: 0x002BDB52
	[XuiXmlBinding("usesmarkers")]
	public virtual bool UsesIndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.IndexMarkers && this.sprFill != null && this.IndexElementCount > 0 && this.IndexElementCount < this.indexMarkerSprites.Length;
		}
	}

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x06007279 RID: 29305 RVA: 0x002BF97F File Offset: 0x002BDB7F
	// (set) Token: 0x0600727A RID: 29306 RVA: 0x002BF987 File Offset: 0x002BDB87
	[XuiXmlAttribute("gamepad_default_hover_control", false)]
	public bool GamepadDefaultHoverControl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x0600727B RID: 29307 RVA: 0x002BF990 File Offset: 0x002BDB90
	// (set) Token: 0x0600727C RID: 29308 RVA: 0x002BF998 File Offset: 0x002BDB98
	[XuiXmlAttribute("gamepad_decrease", false)]
	public PlayerAction GamepadDecreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x0600727D RID: 29309 RVA: 0x002BF9A1 File Offset: 0x002BDBA1
	// (set) Token: 0x0600727E RID: 29310 RVA: 0x002BF9A9 File Offset: 0x002BDBA9
	[XuiXmlAttribute("gamepad_increase", false)]
	public PlayerAction GamepadIncreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x0600727F RID: 29311 RVA: 0x002BF9B2 File Offset: 0x002BDBB2
	// (set) Token: 0x06007280 RID: 29312 RVA: 0x002BF9BA File Offset: 0x002BDBBA
	[XuiXmlAttribute("gamepad_shortcuts_hoveronly", false)]
	public bool GamepadShortcutsHoverOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x06007281 RID: 29313 RVA: 0x002BF9C3 File Offset: 0x002BDBC3
	// (set) Token: 0x06007282 RID: 29314 RVA: 0x002BF9D5 File Offset: 0x002BDBD5
	[XuiXmlBinding("valuetext")]
	public string ValueText
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.valueTextOverride ?? this.valueText;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (this.valueText == value)
			{
				return;
			}
			this.valueText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x06007283 RID: 29315 RVA: 0x002BF9F4 File Offset: 0x002BDBF4
	// (set) Token: 0x06007284 RID: 29316 RVA: 0x002BF9FC File Offset: 0x002BDBFC
	public string ValueTextOverride
	{
		get
		{
			return this.valueTextOverride;
		}
		set
		{
			this.valueTextOverride = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x06007285 RID: 29317 RVA: 0x002BFA0C File Offset: 0x002BDC0C
	// (set) Token: 0x06007286 RID: 29318 RVA: 0x002BFA1A File Offset: 0x002BDC1A
	[XuiXmlAttribute("segment_count", false)]
	[XuiXmlBinding("segmentcount")]
	public int SegmentedFillCount
	{
		get
		{
			return Mathf.Max(this.segmentedFillCount, 1);
		}
		set
		{
			if (value == this.segmentedFillCount)
			{
				return;
			}
			this.segmentedFillCount = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x06007287 RID: 29319 RVA: 0x002BFA34 File Offset: 0x002BDC34
	// (set) Token: 0x06007288 RID: 29320 RVA: 0x002BFA3C File Offset: 0x002BDC3C
	[XuiXmlBinding("cbx_is_hovered")]
	public bool IsOver
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isOver;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.isOver == value)
			{
				return;
			}
			this.isOver = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x06007289 RID: 29321
	// (set) Token: 0x0600728A RID: 29322
	[XuiXmlBinding("fillvalue")]
	public abstract double RelativeValue { get; set; }

	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x0600728B RID: 29323 RVA: 0x002BFA56 File Offset: 0x002BDC56
	[XuiXmlBinding("valuecolor")]
	public Color ValueColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.Enabled)
			{
				return this.ColorDisabled;
			}
			return this.ColorEnabled;
		}
	}

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x0600728C RID: 29324 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isnumber")]
	public virtual bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x0600728D RID: 29325 RVA: 0x002BFA6D File Offset: 0x002BDC6D
	[XuiXmlBinding("fillcolor")]
	public Color FillColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.Enabled)
			{
				return this.ColorFillDisabled;
			}
			return this.ColorFillEnabled;
		}
	}

	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x0600728E RID: 29326 RVA: 0x002BFA84 File Offset: 0x002BDC84
	[XuiXmlBinding("hascontrollershortcuts")]
	public bool HasControllerShortcuts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.GamepadIncreaseShortcut != null && this.GamepadDecreaseShortcut != null;
		}
	}

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x0600728F RID: 29327 RVA: 0x002BFA99 File Offset: 0x002BDC99
	[XuiXmlBinding("can_backward")]
	public bool CanBackward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMin());
		}
	}

	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x06007290 RID: 29328 RVA: 0x002BFAC0 File Offset: 0x002BDCC0
	[XuiXmlBinding("can_forward")]
	public bool CanForward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMax());
		}
	}

	// Token: 0x06007291 RID: 29329 RVA: 0x002BFAE8 File Offset: 0x002BDCE8
	public override void Init()
	{
		base.Init();
		base.OnVisiblity += delegate(XUiController _, bool _, bool _visibleInScene)
		{
			if (!_visibleInScene)
			{
				XUiC_Paging.IsOverPagingOverrideElement = false;
			}
		};
		XUiController childById = base.GetChildById("indexMarkers");
		if (childById != null)
		{
			object obj;
			if (childById.CustomAttributes.TryGetValue("active_color", out obj))
			{
				this.colorIndexMarkerActive = StringParsers.ParseColor32(obj.ToString());
			}
			object obj2;
			if (childById.CustomAttributes.TryGetValue("inactive_color", out obj2))
			{
				this.colorIndexMarkerInactive = StringParsers.ParseColor32(obj2.ToString());
			}
			object obj3;
			if (childById.CustomAttributes.TryGetValue("disabled_active_color", out obj3))
			{
				this.colorIndexMarkerDisabledActive = StringParsers.ParseColor32(obj3.ToString());
			}
		}
		if (string.IsNullOrEmpty(this.viewComponent.ToolTip) && base.Parent != null)
		{
			foreach (XUiController xuiController in base.Parent.Children)
			{
				XUiV_Label xuiV_Label = xuiController.ViewComponent as XUiV_Label;
				if (xuiV_Label != null)
				{
					this.viewComponent.ToolTip = xuiV_Label.ToolTip;
					break;
				}
			}
		}
	}

	// Token: 0x06007292 RID: 29330 RVA: 0x002BFC20 File Offset: 0x002BDE20
	public override void OnOpen()
	{
		this.IsDirty = true;
		base.OnOpen();
		this.UpdateLabel();
		this.UpdateIndexMarkerPositions();
		this.UpdateIndexMarkerStates();
		base.RefreshBindings();
	}

	// Token: 0x06007293 RID: 29331 RVA: 0x002BFC47 File Offset: 0x002BDE47
	public override void OnClose()
	{
		base.OnClose();
		if (this.isOver)
		{
			XUiC_GamepadCalloutWindow calloutWindow = this.xui.CalloutWindow;
			if (calloutWindow == null)
			{
				return;
			}
			calloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
		}
	}

	// Token: 0x06007294 RID: 29332 RVA: 0x002BFC70 File Offset: 0x002BDE70
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.UsesIndexMarkers && this.indexMarkersSetupBefore != this.IndexElementCount)
		{
			this.UpdateIndexMarkerPositions();
		}
		if (this.Enabled)
		{
			bool flag = this.clickable != null && this.xui.playerUI.CursorController.CurrentTarget == this.clickable.ViewComponent && !this.xui.playerUI.CursorController.Locked && XUiUtils.HotkeysAllowedFor(this.viewComponent, false);
			if (flag && this.GamepadDefaultHoverControl)
			{
				XUi.HandlePaging(this.xui, new Func<bool>(this.PageUpAction), new Func<bool>(this.PageDownAction), false);
			}
			if (this.GamepadDecreaseShortcut != null && this.viewComponent.IsActiveInHierarchy && this.GamepadDecreaseShortcut.WasPressed && (!this.GamepadShortcutsHoverOnly || flag))
			{
				this.PageDownAction();
			}
			if (this.GamepadIncreaseShortcut != null && this.viewComponent.IsActiveInHierarchy && this.GamepadIncreaseShortcut.WasPressed && (!this.GamepadShortcutsHoverOnly || flag))
			{
				this.PageUpAction();
			}
		}
		if (base.handleDirtyUpdateDefault())
		{
			this.UpdateIndexMarkerStates();
		}
	}

	// Token: 0x06007295 RID: 29333 RVA: 0x002BFDA5 File Offset: 0x002BDFA5
	[XuiXmlAttribute("value_wrap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueWrap(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Wrap = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x06007296 RID: 29334
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void UpdateLabel();

	// Token: 0x06007297 RID: 29335 RVA: 0x002BFDC4 File Offset: 0x002BDFC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateIndexMarkerStates()
	{
		if (!this.UsesIndexMarkers)
		{
			return;
		}
		for (int i = 0; i < this.indexMarkerSprites.Length; i++)
		{
			this.indexMarkerSprites[i].Color = this.colorIndexMarkerInactive;
		}
		if (this.IndexMarkerIndex >= 0 && this.IndexMarkerIndex < this.indexMarkerSprites.Length)
		{
			this.indexMarkerSprites[this.IndexMarkerIndex].Color = (this.Enabled ? this.colorIndexMarkerActive : this.colorIndexMarkerDisabledActive);
		}
	}

	// Token: 0x06007298 RID: 29336 RVA: 0x002BFE44 File Offset: 0x002BE044
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateIndexMarkerPositions()
	{
		if (!this.UsesIndexMarkers)
		{
			return;
		}
		this.indexMarkersSetupBefore = this.IndexElementCount;
		float num = (float)(this.sprFill.Size.x + this.IndexMarkerSpacing) / (float)this.IndexElementCount;
		int num2 = Mathf.RoundToInt(num - (float)this.IndexMarkerSpacing);
		for (int i = 0; i < this.indexMarkerSprites.Length; i++)
		{
			if (i >= this.IndexElementCount)
			{
				this.indexMarkerSprites[i].IsVisible = false;
			}
			else
			{
				Vector2i position = this.indexMarkerSprites[i].Position;
				position.x = Mathf.RoundToInt((float)i * num);
				this.indexMarkerSprites[i].Position = position;
				Vector2i size = this.indexMarkerSprites[i].Size;
				size.x = num2;
				if (i == this.IndexElementCount - 1)
				{
					size.x = this.sprFill.Size.x - position.x;
				}
				this.indexMarkerSprites[i].Size = size;
				this.indexMarkerSprites[i].IsVisible = true;
			}
		}
	}

	// Token: 0x06007299 RID: 29337 RVA: 0x002BFF54 File Offset: 0x002BE154
	[XuiBindEvent("OnHover", "forwardButton")]
	[XuiBindEvent("OnHover", "backButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Button_OnHover(XUiController _sender, bool _isOver)
	{
		this.isOverAnyPart = _isOver;
		XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged onHoveredStateChanged = this.OnHoveredStateChanged;
		if (onHoveredStateChanged == null)
		{
			return;
		}
		onHoveredStateChanged(this, this.isOver, this.isOverAnyPart);
	}

	// Token: 0x0600729A RID: 29338 RVA: 0x002BFF7A File Offset: 0x002BE17A
	[XuiBindEvent("OnPress", "backButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageDown();
	}

	// Token: 0x0600729B RID: 29339 RVA: 0x002BFF83 File Offset: 0x002BE183
	[XuiBindEvent("OnPress", "forwardButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ForwardButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageUp();
	}

	// Token: 0x0600729C RID: 29340 RVA: 0x002BFF8C File Offset: 0x002BE18C
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool PageUpAction()
	{
		bool flag = this.TryPageUp();
		if (this.xui.playerUI.CursorController.CursorModeActive)
		{
			base.SelectCursorElement(false, true);
		}
		return flag;
	}

	// Token: 0x0600729D RID: 29341 RVA: 0x002BFFB4 File Offset: 0x002BE1B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool PageDownAction()
	{
		bool flag = this.TryPageDown();
		if (this.xui.playerUI.CursorController.CursorModeActive)
		{
			base.SelectCursorElement(false, true);
		}
		return flag;
	}

	// Token: 0x0600729E RID: 29342
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void BackPressed();

	// Token: 0x0600729F RID: 29343
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ForwardPressed();

	// Token: 0x060072A0 RID: 29344
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMax();

	// Token: 0x060072A1 RID: 29345
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMin();

	// Token: 0x060072A2 RID: 29346
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isEmpty();

	// Token: 0x060072A3 RID: 29347 RVA: 0x002BFFDC File Offset: 0x002BE1DC
	[XuiBindEvent("OnPress", "clickable")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void pressEvent(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			return;
		}
		if (!this.Enabled)
		{
			return;
		}
		ref Vector2i mouseXUiPosition = this.xui.GetMouseXUiPosition();
		Rect xuiRect = this.clickable.ViewComponent.GetXUiRect();
		float num = ((float)mouseXUiPosition.x - xuiRect.min.x) / xuiRect.width;
		this.setRelativeValue(num);
	}

	// Token: 0x060072A4 RID: 29348 RVA: 0x002C0044 File Offset: 0x002BE244
	[XuiBindEvent("OnScroll", "clickable")]
	public void ScrollEvent(XUiController _sender, float _delta)
	{
		if (!InputUtils.ShiftKeyPressed)
		{
			return;
		}
		if (this.Enabled && !this.isEmpty())
		{
			if (this.ScrollByIncrement)
			{
				if (_delta > 0f)
				{
					this.PageUpAction();
					return;
				}
				this.PageDownAction();
				return;
			}
			else
			{
				this.incrementalChangeValue((double)_delta);
			}
		}
	}

	// Token: 0x060072A5 RID: 29349 RVA: 0x002C0094 File Offset: 0x002BE294
	[XuiBindEvent("OnHover", "clickable")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void hoverEvent(XUiController _sender, bool _isOver)
	{
		this.IsOver = _isOver;
		this.isOverAnyPart = _isOver;
		XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged onHoveredStateChanged = this.OnHoveredStateChanged;
		if (onHoveredStateChanged != null)
		{
			onHoveredStateChanged(this, this.isOver, this.isOverAnyPart);
		}
		XUiC_Paging.IsOverPagingOverrideElement = this.isOver;
		if (this.isOver && this.GamepadDefaultHoverControl)
		{
			XUiC_GamepadCalloutWindow calloutWindow = this.xui.CalloutWindow;
			if (calloutWindow == null)
			{
				return;
			}
			calloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox, 0f);
			return;
		}
		else
		{
			XUiC_GamepadCalloutWindow calloutWindow2 = this.xui.CalloutWindow;
			if (calloutWindow2 == null)
			{
				return;
			}
			calloutWindow2.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuComboBox);
			return;
		}
	}

	// Token: 0x060072A6 RID: 29350 RVA: 0x002C011A File Offset: 0x002BE31A
	[XuiBindEvent("OnDrag", "clickable")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void dragEvent(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		if (!this.isOver)
		{
			return;
		}
		if (this.Enabled && !this.isEmpty())
		{
			this.pressEvent(_sender, -1);
		}
	}

	// Token: 0x060072A7 RID: 29351 RVA: 0x002C013D File Offset: 0x002BE33D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void invokeValueChangedGeneric()
	{
		XUiC_ComboBoxBase.XUiEvent_GenericValueChanged onValueChangedGeneric = this.OnValueChangedGeneric;
		if (onValueChangedGeneric == null)
		{
			return;
		}
		onValueChangedGeneric(this);
	}

	// Token: 0x060072A8 RID: 29352
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void incrementalChangeValue(double _value);

	// Token: 0x060072A9 RID: 29353
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageUp();

	// Token: 0x060072AA RID: 29354
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageDown();

	// Token: 0x060072AB RID: 29355
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void setRelativeValue(float _value);

	// Token: 0x060072AC RID: 29356 RVA: 0x002C0150 File Offset: 0x002BE350
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxBase()
	{
	}

	// Token: 0x0400564B RID: 22091
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x0400564C RID: 22092
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showButtons = true;

	// Token: 0x0400564D RID: 22093
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Wrap = true;

	// Token: 0x0400564F RID: 22095
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerActive = Color.white;

	// Token: 0x04005650 RID: 22096
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerInactive = Color.grey;

	// Token: 0x04005651 RID: 22097
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerDisabledActive = Color.white;

	// Token: 0x04005653 RID: 22099
	[XuiBindComponent("indexMarkers.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite[] indexMarkerSprites;

	// Token: 0x04005654 RID: 22100
	[PublicizedFrom(EAccessModifier.Private)]
	public int indexMarkersSetupBefore;

	// Token: 0x0400565A RID: 22106
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x0400565B RID: 22107
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueTextOverride;

	// Token: 0x0400565C RID: 22108
	[XuiBindComponent("fill", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite sprFill;

	// Token: 0x0400565D RID: 22109
	[PublicizedFrom(EAccessModifier.Private)]
	public int segmentedFillCount = 1;

	// Token: 0x0400565E RID: 22110
	[XuiBindComponent("directvalue", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController clickable;

	// Token: 0x0400565F RID: 22111
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005660 RID: 22112
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOverAnyPart;

	// Token: 0x04005661 RID: 22113
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x04005662 RID: 22114
	[XuiBindComponent("forward", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController forwardButton;

	// Token: 0x04005663 RID: 22115
	[XuiBindComponent("back", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController backButton;

	// Token: 0x04005664 RID: 22116
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorEnabled;

	// Token: 0x04005665 RID: 22117
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorDisabled;

	// Token: 0x04005666 RID: 22118
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorBg;

	// Token: 0x04005667 RID: 22119
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillEnabled;

	// Token: 0x04005668 RID: 22120
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillDisabled;

	// Token: 0x02000E8F RID: 3727
	// (Invoke) Token: 0x060072AE RID: 29358
	public delegate void XUiEvent_GenericValueChanged(XUiController _sender);

	// Token: 0x02000E90 RID: 3728
	// (Invoke) Token: 0x060072B2 RID: 29362
	public delegate void XUiEvent_HoveredStateChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart);
}
