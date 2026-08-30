using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E74 RID: 3700
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_ComboBoxBase : XUiController
{
	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06007239 RID: 29241 RVA: 0x002BADF8 File Offset: 0x002B8FF8
	// (remove) Token: 0x0600723A RID: 29242 RVA: 0x002BAE30 File Offset: 0x002B9030
	public event XUiC_ComboBoxBase.XUiEvent_GenericValueChanged OnValueChangedGeneric;

	// Token: 0x140000AA RID: 170
	// (add) Token: 0x0600723B RID: 29243 RVA: 0x002BAE68 File Offset: 0x002B9068
	// (remove) Token: 0x0600723C RID: 29244 RVA: 0x002BAEA0 File Offset: 0x002B90A0
	public event XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged OnHoveredStateChanged;

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x0600723D RID: 29245 RVA: 0x002BAED5 File Offset: 0x002B90D5
	// (set) Token: 0x0600723E RID: 29246 RVA: 0x002BAEDD File Offset: 0x002B90DD
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

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x0600723F RID: 29247 RVA: 0x002BAEF7 File Offset: 0x002B90F7
	// (set) Token: 0x06007240 RID: 29248 RVA: 0x002BAEFF File Offset: 0x002B90FF
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

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x06007241 RID: 29249 RVA: 0x002BAF19 File Offset: 0x002B9119
	// (set) Token: 0x06007242 RID: 29250 RVA: 0x002BAF21 File Offset: 0x002B9121
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

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x06007243 RID: 29251
	// (set) Token: 0x06007244 RID: 29252
	public abstract long ValueGeneric { get; set; }

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x06007245 RID: 29253
	// (set) Token: 0x06007246 RID: 29254
	public abstract long ValueMinGeneric { get; set; }

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x06007247 RID: 29255
	// (set) Token: 0x06007248 RID: 29256
	public abstract long ValueMaxGeneric { get; set; }

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x06007249 RID: 29257 RVA: 0x002BAF40 File Offset: 0x002B9140
	// (set) Token: 0x0600724A RID: 29258 RVA: 0x002BAF48 File Offset: 0x002B9148
	[XuiXmlAttribute("scroll_by_increment", false)]
	public bool ScrollByIncrement
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x0600724B RID: 29259 RVA: 0x002BAF51 File Offset: 0x002B9151
	// (set) Token: 0x0600724C RID: 29260 RVA: 0x002BAF59 File Offset: 0x002B9159
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

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x0600724D RID: 29261 RVA: 0x002BAF78 File Offset: 0x002B9178
	// (set) Token: 0x0600724E RID: 29262 RVA: 0x002BAF80 File Offset: 0x002B9180
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

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x0600724F RID: 29263 RVA: 0x002BAF9F File Offset: 0x002B919F
	// (set) Token: 0x06007250 RID: 29264 RVA: 0x002BAFA7 File Offset: 0x002B91A7
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

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x06007251 RID: 29265 RVA: 0x002BAFC6 File Offset: 0x002B91C6
	// (set) Token: 0x06007252 RID: 29266 RVA: 0x002BAFCE File Offset: 0x002B91CE
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

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x06007253 RID: 29267 RVA: 0x002BAFED File Offset: 0x002B91ED
	// (set) Token: 0x06007254 RID: 29268 RVA: 0x002BAFF5 File Offset: 0x002B91F5
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

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06007255 RID: 29269 RVA: 0x002BB014 File Offset: 0x002B9214
	// (set) Token: 0x06007256 RID: 29270 RVA: 0x002BB01C File Offset: 0x002B921C
	[XuiXmlAttribute("index_markers", false)]
	public bool IndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06007257 RID: 29271
	public abstract int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06007258 RID: 29272
	public abstract int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06007259 RID: 29273 RVA: 0x002BB025 File Offset: 0x002B9225
	// (set) Token: 0x0600725A RID: 29274 RVA: 0x002BB02D File Offset: 0x002B922D
	[XuiXmlAttribute("index_marker_spacing", false)]
	public int IndexMarkerSpacing
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 5;

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x0600725B RID: 29275 RVA: 0x002BB036 File Offset: 0x002B9236
	[XuiXmlBinding("usesmarkers")]
	public virtual bool UsesIndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.IndexMarkers && this.sprFill != null && this.IndexElementCount > 0 && this.IndexElementCount < this.indexMarkerSprites.Length;
		}
	}

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x0600725C RID: 29276 RVA: 0x002BB063 File Offset: 0x002B9263
	// (set) Token: 0x0600725D RID: 29277 RVA: 0x002BB06B File Offset: 0x002B926B
	[XuiXmlAttribute("gamepad_default_hover_control", false)]
	public bool GamepadDefaultHoverControl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x0600725E RID: 29278 RVA: 0x002BB074 File Offset: 0x002B9274
	// (set) Token: 0x0600725F RID: 29279 RVA: 0x002BB07C File Offset: 0x002B927C
	[XuiXmlAttribute("gamepad_decrease", false)]
	public PlayerAction GamepadDecreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06007260 RID: 29280 RVA: 0x002BB085 File Offset: 0x002B9285
	// (set) Token: 0x06007261 RID: 29281 RVA: 0x002BB08D File Offset: 0x002B928D
	[XuiXmlAttribute("gamepad_increase", false)]
	public PlayerAction GamepadIncreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06007262 RID: 29282 RVA: 0x002BB096 File Offset: 0x002B9296
	// (set) Token: 0x06007263 RID: 29283 RVA: 0x002BB09E File Offset: 0x002B929E
	[XuiXmlAttribute("gamepad_shortcuts_hoveronly", false)]
	public bool GamepadShortcutsHoverOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06007264 RID: 29284 RVA: 0x002BB0A7 File Offset: 0x002B92A7
	// (set) Token: 0x06007265 RID: 29285 RVA: 0x002BB0B9 File Offset: 0x002B92B9
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

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06007266 RID: 29286 RVA: 0x002BB0D8 File Offset: 0x002B92D8
	// (set) Token: 0x06007267 RID: 29287 RVA: 0x002BB0E0 File Offset: 0x002B92E0
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

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06007268 RID: 29288 RVA: 0x002BB0F0 File Offset: 0x002B92F0
	// (set) Token: 0x06007269 RID: 29289 RVA: 0x002BB0FE File Offset: 0x002B92FE
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

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x0600726A RID: 29290 RVA: 0x002BB118 File Offset: 0x002B9318
	// (set) Token: 0x0600726B RID: 29291 RVA: 0x002BB120 File Offset: 0x002B9320
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

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x0600726C RID: 29292
	// (set) Token: 0x0600726D RID: 29293
	[XuiXmlBinding("fillvalue")]
	public abstract double RelativeValue { get; set; }

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x0600726E RID: 29294 RVA: 0x002BB13A File Offset: 0x002B933A
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

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x0600726F RID: 29295 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isnumber")]
	public virtual bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06007270 RID: 29296 RVA: 0x002BB151 File Offset: 0x002B9351
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

	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x06007271 RID: 29297 RVA: 0x002BB168 File Offset: 0x002B9368
	[XuiXmlBinding("hascontrollershortcuts")]
	public bool HasControllerShortcuts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.GamepadIncreaseShortcut != null && this.GamepadDecreaseShortcut != null;
		}
	}

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x06007272 RID: 29298 RVA: 0x002BB17D File Offset: 0x002B937D
	[XuiXmlBinding("can_backward")]
	public bool CanBackward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMin());
		}
	}

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06007273 RID: 29299 RVA: 0x002BB1A4 File Offset: 0x002B93A4
	[XuiXmlBinding("can_forward")]
	public bool CanForward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMax());
		}
	}

	// Token: 0x06007274 RID: 29300 RVA: 0x002BB1CC File Offset: 0x002B93CC
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

	// Token: 0x06007275 RID: 29301 RVA: 0x002BB304 File Offset: 0x002B9504
	public override void OnOpen()
	{
		this.IsDirty = true;
		base.OnOpen();
		this.UpdateLabel();
		this.UpdateIndexMarkerPositions();
		this.UpdateIndexMarkerStates();
		base.RefreshBindings();
	}

	// Token: 0x06007276 RID: 29302 RVA: 0x002BB32B File Offset: 0x002B952B
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

	// Token: 0x06007277 RID: 29303 RVA: 0x002BB354 File Offset: 0x002B9554
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

	// Token: 0x06007278 RID: 29304 RVA: 0x002BB489 File Offset: 0x002B9689
	[XuiXmlAttribute("value_wrap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueWrap(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Wrap = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x06007279 RID: 29305
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void UpdateLabel();

	// Token: 0x0600727A RID: 29306 RVA: 0x002BB4A8 File Offset: 0x002B96A8
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

	// Token: 0x0600727B RID: 29307 RVA: 0x002BB528 File Offset: 0x002B9728
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

	// Token: 0x0600727C RID: 29308 RVA: 0x002BB638 File Offset: 0x002B9838
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

	// Token: 0x0600727D RID: 29309 RVA: 0x002BB65E File Offset: 0x002B985E
	[XuiBindEvent("OnPress", "backButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageDown();
	}

	// Token: 0x0600727E RID: 29310 RVA: 0x002BB667 File Offset: 0x002B9867
	[XuiBindEvent("OnPress", "forwardButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ForwardButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageUp();
	}

	// Token: 0x0600727F RID: 29311 RVA: 0x002BB670 File Offset: 0x002B9870
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

	// Token: 0x06007280 RID: 29312 RVA: 0x002BB698 File Offset: 0x002B9898
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

	// Token: 0x06007281 RID: 29313
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void BackPressed();

	// Token: 0x06007282 RID: 29314
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ForwardPressed();

	// Token: 0x06007283 RID: 29315
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMax();

	// Token: 0x06007284 RID: 29316
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMin();

	// Token: 0x06007285 RID: 29317
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isEmpty();

	// Token: 0x06007286 RID: 29318 RVA: 0x002BB6C0 File Offset: 0x002B98C0
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

	// Token: 0x06007287 RID: 29319 RVA: 0x002BB728 File Offset: 0x002B9928
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

	// Token: 0x06007288 RID: 29320 RVA: 0x002BB778 File Offset: 0x002B9978
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

	// Token: 0x06007289 RID: 29321 RVA: 0x002BB7FE File Offset: 0x002B99FE
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

	// Token: 0x0600728A RID: 29322 RVA: 0x002BB821 File Offset: 0x002B9A21
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

	// Token: 0x0600728B RID: 29323
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void incrementalChangeValue(double _value);

	// Token: 0x0600728C RID: 29324
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageUp();

	// Token: 0x0600728D RID: 29325
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageDown();

	// Token: 0x0600728E RID: 29326
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void setRelativeValue(float _value);

	// Token: 0x0600728F RID: 29327 RVA: 0x002BB834 File Offset: 0x002B9A34
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxBase()
	{
	}

	// Token: 0x040055EF RID: 21999
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x040055F0 RID: 22000
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showButtons = true;

	// Token: 0x040055F1 RID: 22001
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Wrap = true;

	// Token: 0x040055F3 RID: 22003
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerActive = Color.white;

	// Token: 0x040055F4 RID: 22004
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerInactive = Color.grey;

	// Token: 0x040055F5 RID: 22005
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerDisabledActive = Color.white;

	// Token: 0x040055F7 RID: 22007
	[XuiBindComponent("indexMarkers.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite[] indexMarkerSprites;

	// Token: 0x040055F8 RID: 22008
	[PublicizedFrom(EAccessModifier.Private)]
	public int indexMarkersSetupBefore;

	// Token: 0x040055FE RID: 22014
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x040055FF RID: 22015
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueTextOverride;

	// Token: 0x04005600 RID: 22016
	[XuiBindComponent("fill", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite sprFill;

	// Token: 0x04005601 RID: 22017
	[PublicizedFrom(EAccessModifier.Private)]
	public int segmentedFillCount = 1;

	// Token: 0x04005602 RID: 22018
	[XuiBindComponent("directvalue", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController clickable;

	// Token: 0x04005603 RID: 22019
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005604 RID: 22020
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOverAnyPart;

	// Token: 0x04005605 RID: 22021
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x04005606 RID: 22022
	[XuiBindComponent("forward", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController forwardButton;

	// Token: 0x04005607 RID: 22023
	[XuiBindComponent("back", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController backButton;

	// Token: 0x04005608 RID: 22024
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorEnabled;

	// Token: 0x04005609 RID: 22025
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorDisabled;

	// Token: 0x0400560A RID: 22026
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorBg;

	// Token: 0x0400560B RID: 22027
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillEnabled;

	// Token: 0x0400560C RID: 22028
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillDisabled;

	// Token: 0x02000E75 RID: 3701
	// (Invoke) Token: 0x06007291 RID: 29329
	public delegate void XUiEvent_GenericValueChanged(XUiController _sender);

	// Token: 0x02000E76 RID: 3702
	// (Invoke) Token: 0x06007295 RID: 29333
	public delegate void XUiEvent_HoveredStateChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart);
}
