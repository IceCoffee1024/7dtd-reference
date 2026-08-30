using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAA RID: 3754
[Preserve]
public class XUiC_ToggleButton : XUiController
{
	// Token: 0x140000BE RID: 190
	// (add) Token: 0x06007514 RID: 29972 RVA: 0x002C15C4 File Offset: 0x002BF7C4
	// (remove) Token: 0x06007515 RID: 29973 RVA: 0x002C15FC File Offset: 0x002BF7FC
	public event XUiEvent_ToggleButtonValueChanged OnValueChanged;

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x06007516 RID: 29974 RVA: 0x002C1631 File Offset: 0x002BF831
	// (set) Token: 0x06007517 RID: 29975 RVA: 0x002C1639 File Offset: 0x002BF839
	[XuiXmlAttribute("enabled_font_color", false)]
	public Color EnabledLabelColor { get; set; }

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06007518 RID: 29976 RVA: 0x002C1642 File Offset: 0x002BF842
	// (set) Token: 0x06007519 RID: 29977 RVA: 0x002C164A File Offset: 0x002BF84A
	[XuiXmlAttribute("disabled_font_color", false)]
	public Color DisabledLabelColor { get; set; }

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x0600751A RID: 29978 RVA: 0x002C1653 File Offset: 0x002BF853
	// (set) Token: 0x0600751B RID: 29979 RVA: 0x002C1670 File Offset: 0x002BF870
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

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x0600751C RID: 29980 RVA: 0x002C1686 File Offset: 0x002BF886
	// (set) Token: 0x0600751D RID: 29981 RVA: 0x002C16A3 File Offset: 0x002BF8A3
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

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x0600751E RID: 29982 RVA: 0x002C16B9 File Offset: 0x002BF8B9
	// (set) Token: 0x0600751F RID: 29983 RVA: 0x002C16C1 File Offset: 0x002BF8C1
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

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06007520 RID: 29984 RVA: 0x002C16DB File Offset: 0x002BF8DB
	// (set) Token: 0x06007521 RID: 29985 RVA: 0x002C16E4 File Offset: 0x002BF8E4
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

	// Token: 0x06007522 RID: 29986 RVA: 0x002C1757 File Offset: 0x002BF957
	public override void Init()
	{
		base.Init();
		this.button.Controller.OnPress += this.Btn_OnPress;
		if (this.label != null)
		{
			this.label.Color = this.EnabledLabelColor;
		}
	}

	// Token: 0x06007523 RID: 29987 RVA: 0x002C1794 File Offset: 0x002BF994
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

	// Token: 0x06007524 RID: 29988 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x040056DF RID: 22239
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040056E0 RID: 22240
	[XuiBindComponent("clickable", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Button button;

	// Token: 0x040056E1 RID: 22241
	[PublicizedFrom(EAccessModifier.Private)]
	public bool value;

	// Token: 0x040056E2 RID: 22242
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x040056E3 RID: 22243
	public string Tag;
}
