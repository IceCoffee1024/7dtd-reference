using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC4 RID: 3780
[Preserve]
public class XUiC_ToggleButton : XUiController
{
	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06007531 RID: 30001 RVA: 0x002C5EE8 File Offset: 0x002C40E8
	// (remove) Token: 0x06007532 RID: 30002 RVA: 0x002C5F20 File Offset: 0x002C4120
	public event XUiEvent_ToggleButtonValueChanged OnValueChanged;

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x06007533 RID: 30003 RVA: 0x002C5F55 File Offset: 0x002C4155
	// (set) Token: 0x06007534 RID: 30004 RVA: 0x002C5F5D File Offset: 0x002C415D
	[XuiXmlAttribute("enabled_font_color", false)]
	public Color EnabledLabelColor { get; set; }

	// Token: 0x17000DEC RID: 3564
	// (get) Token: 0x06007535 RID: 30005 RVA: 0x002C5F66 File Offset: 0x002C4166
	// (set) Token: 0x06007536 RID: 30006 RVA: 0x002C5F6E File Offset: 0x002C416E
	[XuiXmlAttribute("disabled_font_color", false)]
	public Color DisabledLabelColor { get; set; }

	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x06007537 RID: 30007 RVA: 0x002C5F77 File Offset: 0x002C4177
	// (set) Token: 0x06007538 RID: 30008 RVA: 0x002C5F94 File Offset: 0x002C4194
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

	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x06007539 RID: 30009 RVA: 0x002C5FAA File Offset: 0x002C41AA
	// (set) Token: 0x0600753A RID: 30010 RVA: 0x002C5FC7 File Offset: 0x002C41C7
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

	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x0600753B RID: 30011 RVA: 0x002C5FDD File Offset: 0x002C41DD
	// (set) Token: 0x0600753C RID: 30012 RVA: 0x002C5FE5 File Offset: 0x002C41E5
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

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x0600753D RID: 30013 RVA: 0x002C5FFF File Offset: 0x002C41FF
	// (set) Token: 0x0600753E RID: 30014 RVA: 0x002C6008 File Offset: 0x002C4208
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

	// Token: 0x0600753F RID: 30015 RVA: 0x002C607B File Offset: 0x002C427B
	public override void Init()
	{
		base.Init();
		this.button.Controller.OnPress += this.Btn_OnPress;
		if (this.label != null)
		{
			this.label.Color = this.EnabledLabelColor;
		}
	}

	// Token: 0x06007540 RID: 30016 RVA: 0x002C60B8 File Offset: 0x002C42B8
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

	// Token: 0x06007541 RID: 30017 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0400573B RID: 22331
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x0400573C RID: 22332
	[XuiBindComponent("clickable", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Button button;

	// Token: 0x0400573D RID: 22333
	[PublicizedFrom(EAccessModifier.Private)]
	public bool value;

	// Token: 0x0400573E RID: 22334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x0400573F RID: 22335
	public string Tag;
}
