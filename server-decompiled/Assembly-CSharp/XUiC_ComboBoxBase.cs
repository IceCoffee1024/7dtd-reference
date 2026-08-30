using System;
using InControl;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E5F RID: 3679
[global::UnityEngine.Scripting.Preserve]
public abstract class XUiC_ComboBoxBase : XUiController
{
	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06007199 RID: 29081 RVA: 0x002B78D4 File Offset: 0x002B5AD4
	// (remove) Token: 0x0600719A RID: 29082 RVA: 0x002B790C File Offset: 0x002B5B0C
	public event XUiC_ComboBoxBase.XUiEvent_GenericValueChanged OnValueChangedGeneric;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x0600719B RID: 29083 RVA: 0x002B7944 File Offset: 0x002B5B44
	// (remove) Token: 0x0600719C RID: 29084 RVA: 0x002B797C File Offset: 0x002B5B7C
	public event XUiC_ComboBoxBase.XUiEvent_HoveredStateChanged OnHoveredStateChanged;

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x0600719D RID: 29085 RVA: 0x002B79B1 File Offset: 0x002B5BB1
	// (set) Token: 0x0600719E RID: 29086 RVA: 0x002B79B9 File Offset: 0x002B5BB9
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

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x0600719F RID: 29087 RVA: 0x002B79D3 File Offset: 0x002B5BD3
	// (set) Token: 0x060071A0 RID: 29088 RVA: 0x002B79DB File Offset: 0x002B5BDB
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

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x060071A1 RID: 29089 RVA: 0x002B79F5 File Offset: 0x002B5BF5
	// (set) Token: 0x060071A2 RID: 29090 RVA: 0x002B79FD File Offset: 0x002B5BFD
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

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x060071A3 RID: 29091
	// (set) Token: 0x060071A4 RID: 29092
	public abstract long ValueGeneric { get; set; }

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x060071A5 RID: 29093
	// (set) Token: 0x060071A6 RID: 29094
	public abstract long ValueMinGeneric { get; set; }

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x060071A7 RID: 29095
	// (set) Token: 0x060071A8 RID: 29096
	public abstract long ValueMaxGeneric { get; set; }

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x060071A9 RID: 29097 RVA: 0x002B7A1C File Offset: 0x002B5C1C
	// (set) Token: 0x060071AA RID: 29098 RVA: 0x002B7A24 File Offset: 0x002B5C24
	[XuiXmlAttribute("scroll_by_increment", false)]
	public bool ScrollByIncrement
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x060071AB RID: 29099 RVA: 0x002B7A2D File Offset: 0x002B5C2D
	// (set) Token: 0x060071AC RID: 29100 RVA: 0x002B7A35 File Offset: 0x002B5C35
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

	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x060071AD RID: 29101 RVA: 0x002B7A54 File Offset: 0x002B5C54
	// (set) Token: 0x060071AE RID: 29102 RVA: 0x002B7A5C File Offset: 0x002B5C5C
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

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x060071AF RID: 29103 RVA: 0x002B7A7B File Offset: 0x002B5C7B
	// (set) Token: 0x060071B0 RID: 29104 RVA: 0x002B7A83 File Offset: 0x002B5C83
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

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x060071B1 RID: 29105 RVA: 0x002B7AA2 File Offset: 0x002B5CA2
	// (set) Token: 0x060071B2 RID: 29106 RVA: 0x002B7AAA File Offset: 0x002B5CAA
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

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x060071B3 RID: 29107 RVA: 0x002B7AC9 File Offset: 0x002B5CC9
	// (set) Token: 0x060071B4 RID: 29108 RVA: 0x002B7AD1 File Offset: 0x002B5CD1
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

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x060071B5 RID: 29109 RVA: 0x002B7AF0 File Offset: 0x002B5CF0
	// (set) Token: 0x060071B6 RID: 29110 RVA: 0x002B7AF8 File Offset: 0x002B5CF8
	[XuiXmlAttribute("index_markers", false)]
	public bool IndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CFE RID: 3326
	// (get) Token: 0x060071B7 RID: 29111
	public abstract int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000CFF RID: 3327
	// (get) Token: 0x060071B8 RID: 29112
	public abstract int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x060071B9 RID: 29113 RVA: 0x002B7B01 File Offset: 0x002B5D01
	// (set) Token: 0x060071BA RID: 29114 RVA: 0x002B7B09 File Offset: 0x002B5D09
	[XuiXmlAttribute("index_marker_spacing", false)]
	public int IndexMarkerSpacing
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 5;

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x060071BB RID: 29115 RVA: 0x002B7B12 File Offset: 0x002B5D12
	[XuiXmlBinding("usesmarkers")]
	public virtual bool UsesIndexMarkers
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.IndexMarkers && this.sprFill != null && this.IndexElementCount > 0 && this.IndexElementCount < this.indexMarkerSprites.Length;
		}
	}

	// Token: 0x17000D02 RID: 3330
	// (get) Token: 0x060071BC RID: 29116 RVA: 0x002B7B3F File Offset: 0x002B5D3F
	// (set) Token: 0x060071BD RID: 29117 RVA: 0x002B7B47 File Offset: 0x002B5D47
	[XuiXmlAttribute("gamepad_default_hover_control", false)]
	public bool GamepadDefaultHoverControl
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D03 RID: 3331
	// (get) Token: 0x060071BE RID: 29118 RVA: 0x002B7B50 File Offset: 0x002B5D50
	// (set) Token: 0x060071BF RID: 29119 RVA: 0x002B7B58 File Offset: 0x002B5D58
	[XuiXmlAttribute("gamepad_decrease", false)]
	public PlayerAction GamepadDecreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x060071C0 RID: 29120 RVA: 0x002B7B61 File Offset: 0x002B5D61
	// (set) Token: 0x060071C1 RID: 29121 RVA: 0x002B7B69 File Offset: 0x002B5D69
	[XuiXmlAttribute("gamepad_increase", false)]
	public PlayerAction GamepadIncreaseShortcut
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x060071C2 RID: 29122 RVA: 0x002B7B72 File Offset: 0x002B5D72
	// (set) Token: 0x060071C3 RID: 29123 RVA: 0x002B7B7A File Offset: 0x002B5D7A
	[XuiXmlAttribute("gamepad_shortcuts_hoveronly", false)]
	public bool GamepadShortcutsHoverOnly
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x060071C4 RID: 29124 RVA: 0x002B7B83 File Offset: 0x002B5D83
	// (set) Token: 0x060071C5 RID: 29125 RVA: 0x002B7B95 File Offset: 0x002B5D95
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

	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x060071C6 RID: 29126 RVA: 0x002B7BB4 File Offset: 0x002B5DB4
	// (set) Token: 0x060071C7 RID: 29127 RVA: 0x002B7BBC File Offset: 0x002B5DBC
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

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x060071C8 RID: 29128 RVA: 0x002B7BCC File Offset: 0x002B5DCC
	// (set) Token: 0x060071C9 RID: 29129 RVA: 0x002B7BDA File Offset: 0x002B5DDA
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

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x060071CA RID: 29130 RVA: 0x002B7BF4 File Offset: 0x002B5DF4
	// (set) Token: 0x060071CB RID: 29131 RVA: 0x002B7BFC File Offset: 0x002B5DFC
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

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x060071CC RID: 29132
	// (set) Token: 0x060071CD RID: 29133
	[XuiXmlBinding("fillvalue")]
	public abstract double RelativeValue { get; set; }

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x060071CE RID: 29134 RVA: 0x002B7C16 File Offset: 0x002B5E16
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

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x060071CF RID: 29135 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("isnumber")]
	public virtual bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x060071D0 RID: 29136 RVA: 0x002B7C2D File Offset: 0x002B5E2D
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

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x060071D1 RID: 29137 RVA: 0x002B7C44 File Offset: 0x002B5E44
	[XuiXmlBinding("hascontrollershortcuts")]
	public bool HasControllerShortcuts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.GamepadIncreaseShortcut != null && this.GamepadDecreaseShortcut != null;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x060071D2 RID: 29138 RVA: 0x002B7C59 File Offset: 0x002B5E59
	[XuiXmlBinding("can_backward")]
	public bool CanBackward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMin());
		}
	}

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x060071D3 RID: 29139 RVA: 0x002B7C80 File Offset: 0x002B5E80
	[XuiXmlBinding("can_forward")]
	public bool CanForward
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.Enabled && !this.isEmpty() && (this.Wrap || !this.isMax());
		}
	}

	// Token: 0x060071D4 RID: 29140 RVA: 0x002B7CA8 File Offset: 0x002B5EA8
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

	// Token: 0x060071D5 RID: 29141 RVA: 0x002B7DE0 File Offset: 0x002B5FE0
	public override void OnOpen()
	{
		this.IsDirty = true;
		base.OnOpen();
		this.UpdateLabel();
		this.UpdateIndexMarkerPositions();
		this.UpdateIndexMarkerStates();
		base.RefreshBindings();
	}

	// Token: 0x060071D6 RID: 29142 RVA: 0x002B7E07 File Offset: 0x002B6007
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

	// Token: 0x060071D7 RID: 29143 RVA: 0x002B7E30 File Offset: 0x002B6030
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

	// Token: 0x060071D8 RID: 29144 RVA: 0x002B7F65 File Offset: 0x002B6165
	[XuiXmlAttribute("value_wrap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueWrap(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Wrap = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x060071D9 RID: 29145
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void UpdateLabel();

	// Token: 0x060071DA RID: 29146 RVA: 0x002B7F84 File Offset: 0x002B6184
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

	// Token: 0x060071DB RID: 29147 RVA: 0x002B8004 File Offset: 0x002B6204
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

	// Token: 0x060071DC RID: 29148 RVA: 0x002B8114 File Offset: 0x002B6314
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

	// Token: 0x060071DD RID: 29149 RVA: 0x002B813A File Offset: 0x002B633A
	[XuiBindEvent("OnPress", "backButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageDown();
	}

	// Token: 0x060071DE RID: 29150 RVA: 0x002B8143 File Offset: 0x002B6343
	[XuiBindEvent("OnPress", "forwardButton")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ForwardButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TryPageUp();
	}

	// Token: 0x060071DF RID: 29151 RVA: 0x002B814C File Offset: 0x002B634C
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

	// Token: 0x060071E0 RID: 29152 RVA: 0x002B8174 File Offset: 0x002B6374
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

	// Token: 0x060071E1 RID: 29153
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void BackPressed();

	// Token: 0x060071E2 RID: 29154
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ForwardPressed();

	// Token: 0x060071E3 RID: 29155
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMax();

	// Token: 0x060071E4 RID: 29156
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isMin();

	// Token: 0x060071E5 RID: 29157
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isEmpty();

	// Token: 0x060071E6 RID: 29158 RVA: 0x002B819C File Offset: 0x002B639C
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

	// Token: 0x060071E7 RID: 29159 RVA: 0x002B8204 File Offset: 0x002B6404
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

	// Token: 0x060071E8 RID: 29160 RVA: 0x002B8254 File Offset: 0x002B6454
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

	// Token: 0x060071E9 RID: 29161 RVA: 0x002B82DA File Offset: 0x002B64DA
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

	// Token: 0x060071EA RID: 29162 RVA: 0x002B82FD File Offset: 0x002B64FD
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

	// Token: 0x060071EB RID: 29163
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void incrementalChangeValue(double _value);

	// Token: 0x060071EC RID: 29164
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageUp();

	// Token: 0x060071ED RID: 29165
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool TryPageDown();

	// Token: 0x060071EE RID: 29166
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void setRelativeValue(float _value);

	// Token: 0x060071EF RID: 29167 RVA: 0x002B8310 File Offset: 0x002B6510
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxBase()
	{
	}

	// Token: 0x0400554B RID: 21835
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x0400554C RID: 21836
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showButtons = true;

	// Token: 0x0400554D RID: 21837
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Wrap = true;

	// Token: 0x0400554F RID: 21839
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerActive = Color.white;

	// Token: 0x04005550 RID: 21840
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerInactive = Color.grey;

	// Token: 0x04005551 RID: 21841
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorIndexMarkerDisabledActive = Color.white;

	// Token: 0x04005553 RID: 21843
	[XuiBindComponent("indexMarkers.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite[] indexMarkerSprites;

	// Token: 0x04005554 RID: 21844
	[PublicizedFrom(EAccessModifier.Private)]
	public int indexMarkersSetupBefore;

	// Token: 0x0400555A RID: 21850
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x0400555B RID: 21851
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueTextOverride;

	// Token: 0x0400555C RID: 21852
	[XuiBindComponent("fill", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite sprFill;

	// Token: 0x0400555D RID: 21853
	[PublicizedFrom(EAccessModifier.Private)]
	public int segmentedFillCount = 1;

	// Token: 0x0400555E RID: 21854
	[XuiBindComponent("directvalue", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController clickable;

	// Token: 0x0400555F RID: 21855
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005560 RID: 21856
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOverAnyPart;

	// Token: 0x04005561 RID: 21857
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDragging;

	// Token: 0x04005562 RID: 21858
	[XuiBindComponent("forward", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController forwardButton;

	// Token: 0x04005563 RID: 21859
	[XuiBindComponent("back", false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiController backButton;

	// Token: 0x04005564 RID: 21860
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorEnabled;

	// Token: 0x04005565 RID: 21861
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorDisabled;

	// Token: 0x04005566 RID: 21862
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorBg;

	// Token: 0x04005567 RID: 21863
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillEnabled;

	// Token: 0x04005568 RID: 21864
	[PublicizedFrom(EAccessModifier.Private)]
	public Color colorFillDisabled;

	// Token: 0x02000E60 RID: 3680
	// (Invoke) Token: 0x060071F1 RID: 29169
	public delegate void XUiEvent_GenericValueChanged(XUiController _sender);

	// Token: 0x02000E61 RID: 3681
	// (Invoke) Token: 0x060071F5 RID: 29173
	public delegate void XUiEvent_HoveredStateChanged(XUiController _sender, bool _isOverMainArea, bool _isOverAnyPart);
}
