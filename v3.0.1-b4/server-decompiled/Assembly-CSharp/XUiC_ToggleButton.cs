using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E95 RID: 3733
[Preserve]
public class XUiC_ToggleButton : XUiController
{
	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06007474 RID: 29812 RVA: 0x002BE098 File Offset: 0x002BC298
	// (remove) Token: 0x06007475 RID: 29813 RVA: 0x002BE0D0 File Offset: 0x002BC2D0
	public event XUiEvent_ToggleButtonValueChanged OnValueChanged;

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x06007476 RID: 29814 RVA: 0x002BE105 File Offset: 0x002BC305
	// (set) Token: 0x06007477 RID: 29815 RVA: 0x002BE10D File Offset: 0x002BC30D
	[XuiXmlAttribute("enabled_font_color", false)]
	public Color EnabledLabelColor { get; set; }

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x06007478 RID: 29816 RVA: 0x002BE116 File Offset: 0x002BC316
	// (set) Token: 0x06007479 RID: 29817 RVA: 0x002BE11E File Offset: 0x002BC31E
	[XuiXmlAttribute("disabled_font_color", false)]
	public Color DisabledLabelColor { get; set; }

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x0600747A RID: 29818 RVA: 0x002BE127 File Offset: 0x002BC327
	// (set) Token: 0x0600747B RID: 29819 RVA: 0x002BE144 File Offset: 0x002BC344
	public string Label
	{
		get
		{
			XUiV_Label xuiV_Label = this.label;
			return ((xuiV_Label != null) ? xuiV_Label.Text : null) ?? "";
		}
		set
		{
			if (this.label != null)
			{
				this.label.Text = value;
			}
		}
	}

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x0600747C RID: 29820 RVA: 0x002BE15A File Offset: 0x002BC35A
	// (set) Token: 0x0600747D RID: 29821 RVA: 0x002BE177 File Offset: 0x002BC377
	public string Tooltip
	{
		get
		{
			XUiV_Button xuiV_Button = this.button;
			return ((xuiV_Button != null) ? xuiV_Button.ToolTip : null) ?? "";
		}
		set
		{
			if (this.button != null)
			{
				this.button.ToolTip = value;
			}
		}
	}

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x0600747E RID: 29822 RVA: 0x002BE18D File Offset: 0x002BC38D
	// (set) Token: 0x0600747F RID: 29823 RVA: 0x002BE195 File Offset: 0x002BC395
	[XuiXmlAttribute("toggle_value", false)]
	[XuiXmlBinding("value")]
	public bool Value
	{
		get
		{
			return this.value;
		}
		set
		{
			if (value == this.value)
			{
				return;
			}
			this.value = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06007480 RID: 29824 RVA: 0x002BE1AF File Offset: 0x002BC3AF
	// (set) Token: 0x06007481 RID: 29825 RVA: 0x002BE1B8 File Offset: 0x002BC3B8
	public bool Enabled
	{
		get
		{
			return this.isEnabled;
		}
		set
		{
			if (value == this.isEnabled && (this.button == null || value == this.button.Enabled))
			{
				return;
			}
			this.isEnabled = value;
			if (this.button != null)
			{
				this.button.Enabled = value;
			}
			if (this.label != null)
			{
				this.label.Color = (value ? this.EnabledLabelColor : this.DisabledLabelColor);
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x06007482 RID: 29826 RVA: 0x002BE22B File Offset: 0x002BC42B
	public override void Init()
	{
		base.Init();
		this.button.Controller.OnPress += this.Btn_OnPress;
		if (this.label != null)
		{
			this.label.Color = this.EnabledLabelColor;
		}
	}

	// Token: 0x06007483 RID: 29827 RVA: 0x002BE268 File Offset: 0x002BC468
	[PublicizedFrom(EAccessModifier.Private)]
	public void Btn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.isEnabled)
		{
			return;
		}
		this.value = !this.value;
		this.IsDirty = true;
		XUiEvent_ToggleButtonValueChanged onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this, this.value);
	}

	// Token: 0x06007484 RID: 29828 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0400563B RID: 22075
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x0400563C RID: 22076
	[XuiBindComponent("clickable", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Button button;

	// Token: 0x0400563D RID: 22077
	[PublicizedFrom(EAccessModifier.Private)]
	public bool value;

	// Token: 0x0400563E RID: 22078
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x0400563F RID: 22079
	public string Tag;
}
