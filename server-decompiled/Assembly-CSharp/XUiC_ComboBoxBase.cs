using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E83 RID: 3715
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_ComboBoxBase : XUiController
{
	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06007231 RID: 29233 RVA: 0x002BE1A8 File Offset: 0x002BC3A8
	// (remove) Token: 0x06007232 RID: 29234 RVA: 0x002BE1E0 File Offset: 0x002BC3E0
	public event XUiC_ComboBoxBase.XUiEvent_GenericValueChanged OnValueChangedGeneric;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06007233 RID: 29235 RVA: 0x002BE218 File Offset: 0x002BC418
	// (remove) Token: 0x06007234 RID: 29236 RVA: 0x002BE250 File Offset: 0x002BC450
	public event XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged OnHoveredStateChanged;

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06007235 RID: 29237 RVA: 0x002BE285 File Offset: 0x002BC485
	// (set) Token: 0x06007236 RID: 29238 RVA: 0x002BE28D File Offset: 0x002BC48D
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

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06007237 RID: 29239 RVA: 0x002BE2A7 File Offset: 0x002BC4A7
	// (set) Token: 0x06007238 RID: 29240 RVA: 0x002BE2AF File Offset: 0x002BC4AF
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

	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x06007239 RID: 29241 RVA: 0x002BE2C9 File Offset: 0x002BC4C9
	// (set) Token: 0x0600723A RID: 29242 RVA: 0x002BE2D1 File Offset: 0x002BC4D1
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

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x0600723B RID: 29243
	// (set) Token: 0x0600723C RID: 29244
	public abstract long ValueGeneric { get; set; }

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x0600723D RID: 29245
	// (set) Token: 0x0600723E RID: 29246
	public abstract long ValueMinGeneric { get; set; }

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x0600723F RID: 29247
	// (set) Token: 0x06007240 RID: 29248
	public abstract long ValueMaxGeneric { get; set; }

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06007241 RID: 29249 RVA: 0x002BE2F0 File Offset: 0x002BC4F0
	// (set) Token: 0x06007242 RID: 29250 RVA: 0x002BE2F8 File Offset: 0x002BC4F8
	[XuiXmlAttribute("scroll_by_increment", false)]
	public bool ScrollByIncrement
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x06007243 RID: 29251 RVA: 0x002BE301 File Offset: 0x002BC501
	// (set) Token: 0x06007244 RID: 29252 RVA: 0x002BE309 File Offset: 0x002BC509
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

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06007245 RID: 29253 RVA: 0x002BE328 File Offset: 0x002BC528
	// (set) Token: 0x06007246 RID: 29254 RVA: 0x002BE330 File Offset: 0x002BC530
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

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06007247 RID: 29255 RVA: 0x002BE34F File Offset: 0x002BC54F
	// (set) Token: 0x06007248 RID: 29256 RVA: 0x002BE357 File Offset: 0x002BC557
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

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06007249 RID: 29257 RVA: 0x002BE376 File Offset: 0x002BC576
	// (set) Token: 0x0600724A RID: 29258 RVA: 0x002BE37E File Offset: 0x002BC57E
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

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x0600724B RID: 29259 RVA: 0x002BE39D File Offset: 0x002BC59D
	// (set) Token: 0x0600724C RID: 29260 RVA: 0x002BE3A5 File Offset: 0x002BC5A5
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

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x0600724D RID: 29261 RVA: 0x002BE3C4 File Offset: 0x002BC5C4
	// (set) Token: 0x0600724E RID: 29262 RVA: 0x002BE3CC File Offset: 0x002BC5CC
	[XuiXmlAttribute("index_markers", false)]
	public bool IndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x0600724F RID: 29263
	public abstract int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x06007250 RID: 29264
	public abstract int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06007251 RID: 29265 RVA: 0x002BE3D5 File Offset: 0x002BC5D5
	// (set) Token: 0x06007252 RID: 29266 RVA: 0x002BE3DD File Offset: 0x002BC5DD
	[XuiXmlAttribute("index_marker_spacing", false)]
	public int IndexMarkerSpacing
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 5;

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06007253 RID: 29267 RVA: 0x002BE3E6 File Offset: 0x002BC5E6
	[XuiXmlBinding("usesmarkers")]
	public virtual bool UsesIndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.IndexMarkers && this.sprFill != null && this.IndexElementCount > 0 && this.IndexElementCount < this.indexMarkerSprites.Length;
		}
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x06007254 RID: 29268 RVA: 0x002BE413 File Offset: 0x002BC613
	// (set) Token: 0x06007255 RID: 29269 RVA: 0x002BE41B File Offset: 0x002BC61B
	[XuiXmlAttribute("gamepad_default_hover_control", false)]
	public bool GamepadDefaultHoverControl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x06007256 RID: 29270 RVA: 0x002BE424 File Offset: 0x002BC624
	// (set) Token: 0x06007257 RID: 29271 RVA: 0x002BE42C File Offset: 0x002BC62C
	[XuiXmlAttribute("gamepad_decrease", false)]
	public PlayerAction GamepadDecreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06007258 RID: 29272 RVA: 0x002BE435 File Offset: 0x002BC635
	// (set) Token: 0x06007259 RID: 29273 RVA: 0x002BE43D File Offset: 0x002BC63D
	[XuiXmlAttribute("gamepad_increase", false)]
	public PlayerAction GamepadIncreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x0600725A RID: 29274 RVA: 0x002BE446 File Offset: 0x002BC646
	// (set) Token: 0x0600725B RID: 29275 RVA: 0x002BE44E File Offset: 0x002BC64E
	[XuiXmlAttribute("gamepad_shortcuts_hoveronly", false)]
	public bool GamepadShortcutsHoverOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x0600725C RID: 29276 RVA: 0x002BE457 File Offset: 0x002BC657
	// (set) Token: 0x0600725D RID: 29277 RVA: 0x002BE469 File Offset: 0x002BC669
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

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x0600725E RID: 29278 RVA: 0x002BE488 File Offset: 0x002BC688
	// (set) Token: 0x0600725F RID: 29279 RVA: 0x002BE490 File Offset: 0x002BC690
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

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x06007260 RID: 29280 RVA: 0x002BE4A0 File Offset: 0x002BC6A0
	// (set) Token: 0x06007261 RID: 29281 RVA: 0x002BE4AE File Offset: 0x002BC6AE
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

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x06007262 RID: 29282 RVA: 0x002BE4C8 File Offset: 0x002BC6C8
	// (set) Token: 0x06007263 RID: 29283 RVA: 0x002BE4D0 File Offset: 0x002BC6D0
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

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x06007264 RID: 29284
	// (set) Token: 0x06007265 RID: 29285
	[XuiXmlBinding("fillvalue")]
	public abstract double RelativeValue { get; set; }

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x06007266 RID: 29286 RVA: 0x002BE4EA File Offset: 0x002BC6EA
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

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x06007267 RID: 29287 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isnumber")]
	public virtual bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x06007268 RID: 29288 RVA: 0x002BE501 File Offset: 0x002BC701
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

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x06007269 RID: 29289 RVA: 0x002BE518 File Offset: 0x002BC718
	[XuiXmlBinding("hascontrollershortcuts")]
	public bool HasControllerShortcuts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.GamepadIncreaseShortcut != null && this.GamepadDecreaseShortcut != null;
		}
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x0600726A RID: 29290 RVA: 0x002BE52D File Offset: 0x002BC72D
	[XuiXmlBinding("can_backward")]
	public bool CanBackward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMin());
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x0600726B RID: 29291 RVA: 0x002BE554 File Offset: 0x002BC754
	[XuiXmlBinding("can_forward")]
	public bool CanForward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMax());
		}
	}

	// Token: 0x0600726C RID: 29292 RVA: 0x002BE57C File Offset: 0x002BC77C
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

	// Token: 0x0600726D RID: 29293 RVA: 0x002BE6B4 File Offset: 0x002BC8B4
	public override void OnOpen()
	{
		this.IsDirty = true;
		base.OnOpen();
		this.UpdateLabel();
		this.UpdateIndexMarkerPositions();
		this.UpdateIndexMarkerStates();
		base.RefreshBindings();
	}

	// Token: 0x0600726E RID: 29294 RVA: 0x002BE6DB File Offset: 0x002BC8DB
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

	// Token: 0x0600726F RID: 29295 RVA: 0x002BE704 File Offset: 0x002BC904
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

	// Token: 0x06007270 RID: 29296 RVA: 0x002BE839 File Offset: 0x002BCA39
	[XuiXmlAttribute("value_wrap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueWrap(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Wrap = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x06007271 RID: 29297
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void UpdateLabel();

	// Token: 0x06007272 RID: 29298 RVA: 0x002BE858 File Offset: 0x002BCA58
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

	// Token: 0x06007273 RID: 29299 RVA: 0x002BE8D8 File Offset: 0x002BCAD8
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

	// Token: 0x06007274 RID: 29300 RVA: 0x002BE9E8 File Offset: 0x002BCBE8
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

	// Token: 0x06007275 RID: 29301 RVA: 0x002BEA0E File Offset: 0x002BCC0E
	[XuiBindEvent("OnPress", "backButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageDown();
	}

	// Token: 0x06007276 RID: 29302 RVA: 0x002BEA17 File Offset: 0x002BCC17
	[XuiBindEvent("OnPress", "forwardButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ForwardButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageUp();
	}

	// Token: 0x06007277 RID: 29303 RVA: 0x002BEA20 File Offset: 0x002BCC20
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

	// Token: 0x06007278 RID: 29304 RVA: 0x002BEA48 File Offset: 0x002BCC48
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

	// Token: 0x06007279 RID: 29305
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void BackPressed();

	// Token: 0x0600727A RID: 29306
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ForwardPressed();

	// Token: 0x0600727B RID: 29307
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMax();

	// Token: 0x0600727C RID: 29308
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMin();

	// Token: 0x0600727D RID: 29309
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isEmpty();

	// Token: 0x0600727E RID: 29310 RVA: 0x002BEA70 File Offset: 0x002BCC70
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

	// Token: 0x0600727F RID: 29311 RVA: 0x002BEAD8 File Offset: 0x002BCCD8
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

	// Token: 0x06007280 RID: 29312 RVA: 0x002BEB28 File Offset: 0x002BCD28
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

	// Token: 0x06007281 RID: 29313 RVA: 0x002BEBAE File Offset: 0x002BCDAE
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

	// Token: 0x06007282 RID: 29314 RVA: 0x002BEBD1 File Offset: 0x002BCDD1
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

	// Token: 0x06007283 RID: 29315
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void incrementalChangeValue(double _value);

	// Token: 0x06007284 RID: 29316
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageUp();

	// Token: 0x06007285 RID: 29317
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageDown();

	// Token: 0x06007286 RID: 29318
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void setRelativeValue(float _value);

	// Token: 0x06007287 RID: 29319 RVA: 0x002BEBE4 File Offset: 0x002BCDE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxBase()
	{
	}

	// Token: 0x04005626 RID: 22054
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x04005627 RID: 22055
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showButtons = true;

	// Token: 0x04005628 RID: 22056
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Wrap = true;

	// Token: 0x0400562A RID: 22058
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerActive = Color.white;

	// Token: 0x0400562B RID: 22059
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerInactive = Color.grey;

	// Token: 0x0400562C RID: 22060
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerDisabledActive = Color.white;

	// Token: 0x0400562E RID: 22062
	[XuiBindComponent("indexMarkers.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite[] indexMarkerSprites;

	// Token: 0x0400562F RID: 22063
	[PublicizedFrom(EAccessModifier.Private)]
	public int indexMarkersSetupBefore;

	// Token: 0x04005635 RID: 22069
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x04005636 RID: 22070
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueTextOverride;

	// Token: 0x04005637 RID: 22071
	[XuiBindComponent("fill", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite sprFill;

	// Token: 0x04005638 RID: 22072
	[PublicizedFrom(EAccessModifier.Private)]
	public int segmentedFillCount = 1;

	// Token: 0x04005639 RID: 22073
	[XuiBindComponent("directvalue", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController clickable;

	// Token: 0x0400563A RID: 22074
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x0400563B RID: 22075
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOverAnyPart;

	// Token: 0x0400563C RID: 22076
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x0400563D RID: 22077
	[XuiBindComponent("forward", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController forwardButton;

	// Token: 0x0400563E RID: 22078
	[XuiBindComponent("back", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController backButton;

	// Token: 0x0400563F RID: 22079
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorEnabled;

	// Token: 0x04005640 RID: 22080
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorDisabled;

	// Token: 0x04005641 RID: 22081
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorBg;

	// Token: 0x04005642 RID: 22082
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillEnabled;

	// Token: 0x04005643 RID: 22083
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillDisabled;

	// Token: 0x02000E84 RID: 3716
	// (Invoke) Token: 0x06007289 RID: 29321
	public delegate void XUiEvent_GenericValueChanged(XUiController _sender);

	// Token: 0x02000E85 RID: 3717
	// (Invoke) Token: 0x0600728D RID: 29325
	public delegate void XUiEvent_HoveredStateChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart);
}
