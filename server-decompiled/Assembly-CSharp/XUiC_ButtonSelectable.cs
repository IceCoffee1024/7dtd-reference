using System;
using UnityEngine.Scripting;

// Token: 0x02000E7C RID: 3708
[Preserve]
public class XUiC_ButtonSelectable : XUiC_Button
{
	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x060071E9 RID: 29161 RVA: 0x002BD518 File Offset: 0x002BB718
	// (remove) Token: 0x060071EA RID: 29162 RVA: 0x002BD550 File Offset: 0x002BB750
	public event XUiC_ButtonSelectable.ButtonSelectedDelegate OnButtonSelected;

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x060071EB RID: 29163 RVA: 0x002BD585 File Offset: 0x002BB785
	// (set) Token: 0x060071EC RID: 29164 RVA: 0x002BD58D File Offset: 0x002BB78D
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

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x060071ED RID: 29165 RVA: 0x002BD5BE File Offset: 0x002BB7BE
	// (set) Token: 0x060071EE RID: 29166 RVA: 0x002BD5C6 File Offset: 0x002BB7C6
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

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x060071EF RID: 29167 RVA: 0x002BD5E0 File Offset: 0x002BB7E0
	// (set) Token: 0x060071F0 RID: 29168 RVA: 0x002BD5E8 File Offset: 0x002BB7E8
	[XuiXmlAttribute("auto_toggle_selected", false)]
	public bool AutoToggleSelected { get; set; } = true;

	// Token: 0x060071F1 RID: 29169 RVA: 0x002BD5F1 File Offset: 0x002BB7F1
	[XuiBindEvent("OnEnabled", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Event_OnEnabled(XUiController _sender, bool _enabled)
	{
		if (!_enabled && this.deselectOnDisable)
		{
			this.IsSelected = false;
		}
	}

	// Token: 0x060071F2 RID: 29170 RVA: 0x002BD605 File Offset: 0x002BB805
	public override void Pressed(int _mouseButton)
	{
		if (this.AutoToggleSelected)
		{
			this.IsSelected = !this.IsSelected;
		}
		base.Pressed(_mouseButton);
	}

	// Token: 0x0400560A RID: 22026
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selected;

	// Token: 0x0400560B RID: 22027
	[PublicizedFrom(EAccessModifier.Private)]
	public bool deselectOnDisable;

	// Token: 0x02000E7D RID: 3709
	// (Invoke) Token: 0x060071F5 RID: 29173
	public delegate void ButtonSelectedDelegate(XUiC_ButtonSelectable _sender, bool _isSelected);
}
