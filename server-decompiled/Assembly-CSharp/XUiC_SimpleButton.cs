using System;
using UnityEngine.Scripting;

// Token: 0x02000E94 RID: 3732
[Preserve]
public class XUiC_SimpleButton : XUiController
{
	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06007425 RID: 29733 RVA: 0x002BF474 File Offset: 0x002BD674
	// (remove) Token: 0x06007426 RID: 29734 RVA: 0x002BF4AC File Offset: 0x002BD6AC
	public event XUiEvent_OnPressEventHandler OnPressed;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06007427 RID: 29735 RVA: 0x002BF4E4 File Offset: 0x002BD6E4
	// (remove) Token: 0x06007428 RID: 29736 RVA: 0x002BF51C File Offset: 0x002BD71C
	public new event XUiEvent_OnHoverEventHandler OnHovered;

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06007429 RID: 29737 RVA: 0x002BF551 File Offset: 0x002BD751
	// (set) Token: 0x0600742A RID: 29738 RVA: 0x002BF564 File Offset: 0x002BD764
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

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x0600742B RID: 29739 RVA: 0x002BF57A File Offset: 0x002BD77A
	// (set) Token: 0x0600742C RID: 29740 RVA: 0x002BF587 File Offset: 0x002BD787
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

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x0600742D RID: 29741 RVA: 0x002BF5A8 File Offset: 0x002BD7A8
	// (set) Token: 0x0600742E RID: 29742 RVA: 0x002BF5B5 File Offset: 0x002BD7B5
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

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x0600742F RID: 29743 RVA: 0x002BF5D6 File Offset: 0x002BD7D6
	public XUiV_Button Button
	{
		get
		{
			return this.button;
		}
	}

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x06007430 RID: 29744 RVA: 0x002BF5DE File Offset: 0x002BD7DE
	// (set) Token: 0x06007431 RID: 29745 RVA: 0x002BF5E8 File Offset: 0x002BD7E8
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

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x06007432 RID: 29746 RVA: 0x002BF637 File Offset: 0x002BD837
	[XuiXmlBinding("selected")]
	public bool ButtonSelected
	{
		get
		{
			XUiV_Button xuiV_Button = this.Button;
			return xuiV_Button != null && xuiV_Button.Selected;
		}
	}

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x06007433 RID: 29747 RVA: 0x002BF64A File Offset: 0x002BD84A
	[XuiXmlBinding("hovered")]
	public bool ButtonHovered
	{
		get
		{
			return this.isOver && this.isEnabled;
		}
	}

	// Token: 0x06007434 RID: 29748 RVA: 0x002BF65C File Offset: 0x002BD85C
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

	// Token: 0x06007435 RID: 29749 RVA: 0x002BF72D File Offset: 0x002BD92D
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

	// Token: 0x06007436 RID: 29750 RVA: 0x002BF74F File Offset: 0x002BD94F
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

	// Token: 0x06007437 RID: 29751 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0400567B RID: 22139
	[XuiBindComponent("btnLabel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label label;

	// Token: 0x0400567C RID: 22140
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button button;

	// Token: 0x0400567D RID: 22141
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEnabled = true;

	// Token: 0x0400567E RID: 22142
	public string Tag;

	// Token: 0x0400567F RID: 22143
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;
}
