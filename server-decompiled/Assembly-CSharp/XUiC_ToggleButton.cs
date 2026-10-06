using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB9 RID: 3769
[Preserve]
public class XUiC_ToggleButton : XUiController
{
	// Token: 0x140000BD RID: 189
	// (add) Token: 0x0600750C RID: 29964 RVA: 0x002C497C File Offset: 0x002C2B7C
	// (remove) Token: 0x0600750D RID: 29965 RVA: 0x002C49B4 File Offset: 0x002C2BB4
	public event XUiEvent_ToggleButtonValueChanged OnValueChanged;

	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x0600750E RID: 29966 RVA: 0x002C49E9 File Offset: 0x002C2BE9
	// (set) Token: 0x0600750F RID: 29967 RVA: 0x002C49F1 File Offset: 0x002C2BF1
	[XuiXmlAttribute("enabled_font_color", false)]
	public Color EnabledLabelColor { get; set; }

	// Token: 0x17000DE6 RID: 3558
	// (get) Token: 0x06007510 RID: 29968 RVA: 0x002C49FA File Offset: 0x002C2BFA
	// (set) Token: 0x06007511 RID: 29969 RVA: 0x002C4A02 File Offset: 0x002C2C02
	[XuiXmlAttribute("disabled_font_color", false)]
	public Color DisabledLabelColor { get; set; }

	// Token: 0x17000DE7 RID: 3559
	// (get) Token: 0x06007512 RID: 29970 RVA: 0x002C4A0B File Offset: 0x002C2C0B
	// (set) Token: 0x06007513 RID: 29971 RVA: 0x002C4A28 File Offset: 0x002C2C28
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

	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x06007514 RID: 29972 RVA: 0x002C4A3E File Offset: 0x002C2C3E
	// (set) Token: 0x06007515 RID: 29973 RVA: 0x002C4A5B File Offset: 0x002C2C5B
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

	// Token: 0x17000DE9 RID: 3561
	// (get) Token: 0x06007516 RID: 29974 RVA: 0x002C4A71 File Offset: 0x002C2C71
	// (set) Token: 0x06007517 RID: 29975 RVA: 0x002C4A79 File Offset: 0x002C2C79
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

	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x06007518 RID: 29976 RVA: 0x002C4A93 File Offset: 0x002C2C93
	// (set) Token: 0x06007519 RID: 29977 RVA: 0x002C4A9C File Offset: 0x002C2C9C
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

	// Token: 0x0600751A RID: 29978 RVA: 0x002C4B0F File Offset: 0x002C2D0F
	public override void Init()
	{
		base.Init();
		this.button.Controller.OnPress += this.Btn_OnPress;
		if (this.label != null)
		{
			this.label.Color = this.EnabledLabelColor;
		}
	}

	// Token: 0x0600751B RID: 29979 RVA: 0x002C4B4C File Offset: 0x002C2D4C
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

	// Token: 0x0600751C RID: 29980 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005716 RID: 22294
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x04005717 RID: 22295
	[XuiBindComponent("clickable", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Button button;

	// Token: 0x04005718 RID: 22296
	[PublicizedFrom(EAccessModifier.Private)]
	public bool value;

	// Token: 0x04005719 RID: 22297
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x0400571A RID: 22298
	public string Tag;
}
