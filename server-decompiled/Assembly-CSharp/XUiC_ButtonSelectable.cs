using System;
using UnityEngine.Scripting;

// Token: 0x02000E6D RID: 3693
[Preserve]
public class XUiC_ButtonSelectable : XUiC_Button
{
	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x060071F1 RID: 29169 RVA: 0x002BA168 File Offset: 0x002B8368
	// (remove) Token: 0x060071F2 RID: 29170 RVA: 0x002BA1A0 File Offset: 0x002B83A0
	public event XUiC_ButtonSelectable.ButtonSelectedDelegate OnButtonSelected;

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x060071F3 RID: 29171 RVA: 0x002BA1D5 File Offset: 0x002B83D5
	// (set) Token: 0x060071F4 RID: 29172 RVA: 0x002BA1DD File Offset: 0x002B83DD
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

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x060071F5 RID: 29173 RVA: 0x002BA20E File Offset: 0x002B840E
	// (set) Token: 0x060071F6 RID: 29174 RVA: 0x002BA216 File Offset: 0x002B8416
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

	// Token: 0x17000CFE RID: 3326
	// (get) Token: 0x060071F7 RID: 29175 RVA: 0x002BA230 File Offset: 0x002B8430
	// (set) Token: 0x060071F8 RID: 29176 RVA: 0x002BA238 File Offset: 0x002B8438
	[XuiXmlAttribute("auto_toggle_selected", false)]
	public bool AutoToggleSelected { get; set; } = true;

	// Token: 0x060071F9 RID: 29177 RVA: 0x002BA241 File Offset: 0x002B8441
	[XuiBindEvent("OnEnabled", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnEnabled(XUiController _sender, bool _enabled)
	{
		if (!_enabled && this.deselectOnDisable)
		{
			this.IsSelected = false;
		}
	}

	// Token: 0x060071FA RID: 29178 RVA: 0x002BA255 File Offset: 0x002B8455
	public override void Pressed(int _mouseButton)
	{
		if (this.AutoToggleSelected)
		{
			this.IsSelected = !this.IsSelected;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x040055D3 RID: 21971
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x040055D4 RID: 21972
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deselectOnDisable;

	// Token: 0x02000E6E RID: 3694
	// (Invoke) Token: 0x060071FD RID: 29181
	public delegate void ButtonSelectedDelegate(XUiC_ButtonSelectable _sender, bool _isSelected);
}
