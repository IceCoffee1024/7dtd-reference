using System;
using UnityEngine.Scripting;

// Token: 0x02000E87 RID: 3719
[Preserve]
public class XUiC_ButtonSelectable : XUiC_Button
{
	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x0600720E RID: 29198 RVA: 0x002BEA84 File Offset: 0x002BCC84
	// (remove) Token: 0x0600720F RID: 29199 RVA: 0x002BEABC File Offset: 0x002BCCBC
	public event XUiC_ButtonSelectable.ButtonSelectedDelegate OnButtonSelected;

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06007210 RID: 29200 RVA: 0x002BEAF1 File Offset: 0x002BCCF1
	// (set) Token: 0x06007211 RID: 29201 RVA: 0x002BEAF9 File Offset: 0x002BCCF9
	[XuiXmlBinding("btn_selected")]
	[XuiXmlAttribute("selected", false)]
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
		set
		{
			if (this.selected == value)
			{
				return;
			}
			this.selected = value;
			this.IsDirty = true;
			XUiC_ButtonSelectable.ButtonSelectedDelegate onButtonSelected = this.OnButtonSelected;
			if (onButtonSelected == null)
			{
				return;
			}
			onButtonSelected(this, this.selected);
		}
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06007212 RID: 29202 RVA: 0x002BEB2A File Offset: 0x002BCD2A
	// (set) Token: 0x06007213 RID: 29203 RVA: 0x002BEB32 File Offset: 0x002BCD32
	[XuiXmlAttribute("deselect_on_disable", false)]
	public bool DeselectOnDisable
	{
		get
		{
			return this.deselectOnDisable;
		}
		set
		{
			if (this.deselectOnDisable == value)
			{
				return;
			}
			this.deselectOnDisable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06007214 RID: 29204 RVA: 0x002BEB4C File Offset: 0x002BCD4C
	// (set) Token: 0x06007215 RID: 29205 RVA: 0x002BEB54 File Offset: 0x002BCD54
	[XuiXmlAttribute("auto_toggle_selected", false)]
	public bool AutoToggleSelected { get; set; } = true;

	// Token: 0x06007216 RID: 29206 RVA: 0x002BEB5D File Offset: 0x002BCD5D
	[XuiBindEvent("OnEnabled", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnEnabled(XUiController _sender, bool _enabled)
	{
		if (!_enabled && this.deselectOnDisable)
		{
			this.IsSelected = false;
		}
	}

	// Token: 0x06007217 RID: 29207 RVA: 0x002BEB71 File Offset: 0x002BCD71
	public override void Pressed(int _mouseButton)
	{
		if (this.AutoToggleSelected)
		{
			this.IsSelected = !this.IsSelected;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x0400562F RID: 22063
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005630 RID: 22064
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deselectOnDisable;

	// Token: 0x02000E88 RID: 3720
	// (Invoke) Token: 0x0600721A RID: 29210
	public delegate void ButtonSelectedDelegate(XUiC_ButtonSelectable _sender, bool _isSelected);
}
