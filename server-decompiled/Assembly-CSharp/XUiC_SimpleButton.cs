using System;
using UnityEngine.Scripting;

// Token: 0x02000EAE RID: 3758
[Preserve]
public class XUiC_SimpleButton : XUiController
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06007442 RID: 29762 RVA: 0x002C3D90 File Offset: 0x002C1F90
	// (remove) Token: 0x06007443 RID: 29763 RVA: 0x002C3DC8 File Offset: 0x002C1FC8
	public event XUiEvent_OnPressEventHandler OnPressed;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06007444 RID: 29764 RVA: 0x002C3E00 File Offset: 0x002C2000
	// (remove) Token: 0x06007445 RID: 29765 RVA: 0x002C3E38 File Offset: 0x002C2038
	public new event XUiEvent_OnHoverEventHandler OnHovered;

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06007446 RID: 29766 RVA: 0x002C3E6D File Offset: 0x002C206D
	// (set) Token: 0x06007447 RID: 29767 RVA: 0x002C3E80 File Offset: 0x002C2080
	public string Text
	{
		get
		{
			XUiV_Label xuiV_Label = this.label;
			if (xuiV_Label == null)
			{
				return null;
			}
			return xuiV_Label.Text;
		}
		set
		{
			if (this.label != null)
			{
				this.label.Text = value;
			}
		}
	}

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06007448 RID: 29768 RVA: 0x002C3E96 File Offset: 0x002C2096
	// (set) Token: 0x06007449 RID: 29769 RVA: 0x002C3EA3 File Offset: 0x002C20A3
	public string Tooltip
	{
		get
		{
			return this.button.ToolTip;
		}
		set
		{
			if (this.button.ToolTip != value)
			{
				this.button.ToolTip = value;
			}
		}
	}

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x0600744A RID: 29770 RVA: 0x002C3EC4 File Offset: 0x002C20C4
	// (set) Token: 0x0600744B RID: 29771 RVA: 0x002C3ED1 File Offset: 0x002C20D1
	public string DisabledToolTip
	{
		get
		{
			return this.button.DisabledToolTip;
		}
		set
		{
			if (this.button.DisabledToolTip != value)
			{
				this.button.DisabledToolTip = value;
			}
		}
	}

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x0600744C RID: 29772 RVA: 0x002C3EF2 File Offset: 0x002C20F2
	public XUiV_Button Button
	{
		get
		{
			return this.button;
		}
	}

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x0600744D RID: 29773 RVA: 0x002C3EFA File Offset: 0x002C20FA
	// (set) Token: 0x0600744E RID: 29774 RVA: 0x002C3F04 File Offset: 0x002C2104
	[XuiXmlAttribute("button_enabled", false)]
	[XuiXmlBinding("enabled")]
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
			this.IsDirty = true;
		}
	}

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x0600744F RID: 29775 RVA: 0x002C3F53 File Offset: 0x002C2153
	[XuiXmlBinding("selected")]
	public bool ButtonSelected
	{
		get
		{
			XUiV_Button xuiV_Button = this.Button;
			return xuiV_Button != null && xuiV_Button.Selected;
		}
	}

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x06007450 RID: 29776 RVA: 0x002C3F66 File Offset: 0x002C2166
	[XuiXmlBinding("hovered")]
	public bool ButtonHovered
	{
		get
		{
			return this.isOver && this.isEnabled;
		}
	}

	// Token: 0x06007451 RID: 29777 RVA: 0x002C3F78 File Offset: 0x002C2178
	public override void Init()
	{
		base.Init();
		XUiV_Button xuiV_Button = base.ViewComponent as XUiV_Button;
		if (xuiV_Button != null)
		{
			this.button = xuiV_Button;
		}
		else
		{
			XUiController childById = base.GetChildById("clickable");
			XUiV_Button xuiV_Button2 = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
			if (xuiV_Button2 == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"SimpleButton requires a Button view (itself or as a child named 'clickable'). WindowGroup: '",
					base.WindowGroup.Id,
					"', element: '",
					base.ViewComponent.ID,
					"'"
				}));
			}
			this.button = xuiV_Button2;
		}
		this.button.Controller.OnPress += this.Btn_OnPress;
		this.button.Controller.OnHover += this.Btn_OnHover;
	}

	// Token: 0x06007452 RID: 29778 RVA: 0x002C4049 File Offset: 0x002C2249
	[PublicizedFrom(EAccessModifier.Private)]
	public void Btn_OnHover(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
		this.IsDirty = true;
		XUiEvent_OnHoverEventHandler onHovered = this.OnHovered;
		if (onHovered == null)
		{
			return;
		}
		onHovered(this, _isOver);
	}

	// Token: 0x06007453 RID: 29779 RVA: 0x002C406B File Offset: 0x002C226B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Btn_OnPress(XUiController _sender, int _mouseButton)
	{
		if (!this.isEnabled)
		{
			return;
		}
		XUiEvent_OnPressEventHandler onPressed = this.OnPressed;
		if (onPressed != null)
		{
			onPressed(this, _mouseButton);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007454 RID: 29780 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x040056D7 RID: 22231
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040056D8 RID: 22232
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;

	// Token: 0x040056D9 RID: 22233
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x040056DA RID: 22234
	public string Tag;

	// Token: 0x040056DB RID: 22235
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;
}
