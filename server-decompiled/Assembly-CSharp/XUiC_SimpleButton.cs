using System;
using UnityEngine.Scripting;

// Token: 0x02000EA3 RID: 3747
[Preserve]
public class XUiC_SimpleButton : XUiController
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x0600741D RID: 29725 RVA: 0x002C2824 File Offset: 0x002C0A24
	// (remove) Token: 0x0600741E RID: 29726 RVA: 0x002C285C File Offset: 0x002C0A5C
	public event XUiEvent_OnPressEventHandler OnPressed;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x0600741F RID: 29727 RVA: 0x002C2894 File Offset: 0x002C0A94
	// (remove) Token: 0x06007420 RID: 29728 RVA: 0x002C28CC File Offset: 0x002C0ACC
	public new event XUiEvent_OnHoverEventHandler OnHovered;

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06007421 RID: 29729 RVA: 0x002C2901 File Offset: 0x002C0B01
	// (set) Token: 0x06007422 RID: 29730 RVA: 0x002C2914 File Offset: 0x002C0B14
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

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06007423 RID: 29731 RVA: 0x002C292A File Offset: 0x002C0B2A
	// (set) Token: 0x06007424 RID: 29732 RVA: 0x002C2937 File Offset: 0x002C0B37
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

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06007425 RID: 29733 RVA: 0x002C2958 File Offset: 0x002C0B58
	// (set) Token: 0x06007426 RID: 29734 RVA: 0x002C2965 File Offset: 0x002C0B65
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

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06007427 RID: 29735 RVA: 0x002C2986 File Offset: 0x002C0B86
	public XUiV_Button Button
	{
		get
		{
			return this.button;
		}
	}

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x06007428 RID: 29736 RVA: 0x002C298E File Offset: 0x002C0B8E
	// (set) Token: 0x06007429 RID: 29737 RVA: 0x002C2998 File Offset: 0x002C0B98
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

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x0600742A RID: 29738 RVA: 0x002C29E7 File Offset: 0x002C0BE7
	[XuiXmlBinding("selected")]
	public bool ButtonSelected
	{
		get
		{
			XUiV_Button xuiV_Button = this.Button;
			return xuiV_Button != null && xuiV_Button.Selected;
		}
	}

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x0600742B RID: 29739 RVA: 0x002C29FA File Offset: 0x002C0BFA
	[XuiXmlBinding("hovered")]
	public bool ButtonHovered
	{
		get
		{
			return this.isOver && this.isEnabled;
		}
	}

	// Token: 0x0600742C RID: 29740 RVA: 0x002C2A0C File Offset: 0x002C0C0C
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

	// Token: 0x0600742D RID: 29741 RVA: 0x002C2ADD File Offset: 0x002C0CDD
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

	// Token: 0x0600742E RID: 29742 RVA: 0x002C2AFF File Offset: 0x002C0CFF
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

	// Token: 0x0600742F RID: 29743 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x040056B2 RID: 22194
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x040056B3 RID: 22195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;

	// Token: 0x040056B4 RID: 22196
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x040056B5 RID: 22197
	public string Tag;

	// Token: 0x040056B6 RID: 22198
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;
}
