using System;
using UnityEngine.Scripting;

// Token: 0x02000E58 RID: 3672
[Preserve]
public class XUiC_ButtonSelectable : XUiC_Button
{
	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x06007151 RID: 29009 RVA: 0x002B6C44 File Offset: 0x002B4E44
	// (remove) Token: 0x06007152 RID: 29010 RVA: 0x002B6C7C File Offset: 0x002B4E7C
	public event XUiC_ButtonSelectable.ButtonSelectedDelegate OnButtonSelected;

	// Token: 0x17000CE3 RID: 3299
	// (get) Token: 0x06007153 RID: 29011 RVA: 0x002B6CB1 File Offset: 0x002B4EB1
	// (set) Token: 0x06007154 RID: 29012 RVA: 0x002B6CB9 File Offset: 0x002B4EB9
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

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06007155 RID: 29013 RVA: 0x002B6CEA File Offset: 0x002B4EEA
	// (set) Token: 0x06007156 RID: 29014 RVA: 0x002B6CF2 File Offset: 0x002B4EF2
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

	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x06007157 RID: 29015 RVA: 0x002B6D0C File Offset: 0x002B4F0C
	// (set) Token: 0x06007158 RID: 29016 RVA: 0x002B6D14 File Offset: 0x002B4F14
	[XuiXmlAttribute("auto_toggle_selected", false)]
	public bool AutoToggleSelected { get; set; } = true;

	// Token: 0x06007159 RID: 29017 RVA: 0x002B6D1D File Offset: 0x002B4F1D
	[XuiBindEvent("OnEnabled", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnEnabled(XUiController _sender, bool _enabled)
	{
		if (!_enabled && this.deselectOnDisable)
		{
			this.IsSelected = false;
		}
	}

	// Token: 0x0600715A RID: 29018 RVA: 0x002B6D31 File Offset: 0x002B4F31
	public override void Pressed(int _mouseButton)
	{
		if (this.AutoToggleSelected)
		{
			this.IsSelected = !this.IsSelected;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x0400552F RID: 21807
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x04005530 RID: 21808
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deselectOnDisable;

	// Token: 0x02000E59 RID: 3673
	// (Invoke) Token: 0x0600715D RID: 29021
	public delegate void ButtonSelectedDelegate(XUiC_ButtonSelectable _sender, bool _isSelected);
}
